// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class AppCenterPostBuild
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.WSAPlayer)
        {
#if UNITY_WSA_10_0
            AddHelperCodeToUWPProject(pathToBuiltProject);
            if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.WSA) != ScriptingImplementation.IL2CPP)
            {
                // If UWP with .NET scripting backend, need to add NuGet packages.
                var projectJson = pathToBuiltProject + "/" + PlayerSettings.productName + "/project.json";
                AddDependenciesToProjectJson(projectJson);

                var nuget = EditorApplication.applicationContentsPath + "/PlaybackEngines/MetroSupport/Tools/nuget.exe";
                ExecuteCommand(nuget, "restore \"" + projectJson + "\" -NonInteractive");
            }
            else
            {
                // Fix System.Diagnostics.Debug IL2CPP implementation.
                FixIl2CppLogging(pathToBuiltProject);
            }
#endif
        }
        if (target == BuildTarget.iOS)
        {
#if UNITY_IOS

            // Update project.
            var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var targetName = PBXProject.GetUnityTargetName();
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            OnPostprocessProject(project);
            project.WriteToFile(projectPath);

            // Update Info.plist.
            var settings = AppCenterSettingsContext.SettingsInstance;
            var infoPath = pathToBuiltProject + "/Info.plist";
            var info = new PlistDocument();
            info.ReadFromFile(infoPath);
            OnPostprocessInfo(info, settings);
            info.WriteToFile(infoPath);

#if UNITY_2017_1_OR_NEWER
            // Update capabilities.
            var capabilityManager = new ProjectCapabilityManager(
                projectPath, targetName + ".entitlements",
                PBXProject.GetUnityTargetName());
            OnPostprocessCapabilities(capabilityManager, settings);
            capabilityManager.WriteToFile();
#endif //UNITY_2017_1_OR_NEWER
#endif //UNITY_IOS
        }
    }

    #region UWP Methods
    public static void AddHelperCodeToUWPProject(string pathToBuiltProject)
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
        if (!settings.UsePush)
        {
            return;
        }

        // .NET, D3D
        if (EditorUserBuildSettings.wsaUWPBuildType == WSAUWPBuildType.D3D &&
            PlayerSettings.GetScriptingBackend(BuildTargetGroup.WSA) == ScriptingImplementation.WinRTDotNET)
        {
            var appFilePath = GetAppFilePath(pathToBuiltProject, "App.cs");
            var regexPattern = "private void ApplicationView_Activated \\( CoreApplicationView [a-zA-Z0-9_]*, IActivatedEventArgs args \\) {".Replace(" ", "[\\s]*");
            InjectCodeToFile(appFilePath, regexPattern, "d3ddotnet.txt");
        }
        // .NET, XAML
        else if (EditorUserBuildSettings.wsaUWPBuildType == WSAUWPBuildType.XAML &&
                PlayerSettings.GetScriptingBackend(BuildTargetGroup.WSA) == ScriptingImplementation.WinRTDotNET)
        {
            var appFilePath = GetAppFilePath(pathToBuiltProject, "App.xaml.cs");
            var regexPattern = "InitializeUnity\\(args.Arguments\\);";
            InjectCodeToFile(appFilePath, regexPattern, "xamldotnet.txt", false);
        }
        // IL2CPP, XAML
        else if (EditorUserBuildSettings.wsaUWPBuildType == WSAUWPBuildType.XAML &&
                PlayerSettings.GetScriptingBackend(BuildTargetGroup.WSA) == ScriptingImplementation.IL2CPP)
        {
            var appFilePath = GetAppFilePath(pathToBuiltProject, "App.xaml.cpp");
            var regexPattern = "InitializeUnity\\(e->Arguments\\);";
            InjectCodeToFile(appFilePath, regexPattern, "xamlil2cpp.txt", false);
        }
        // IL2CPP, D3D
        else if (EditorUserBuildSettings.wsaUWPBuildType == WSAUWPBuildType.D3D &&
                PlayerSettings.GetScriptingBackend(BuildTargetGroup.WSA) == ScriptingImplementation.IL2CPP)
        {
            var appFilePath = GetAppFilePath(pathToBuiltProject, "App.cpp");
            var regexPattern = "void App::OnActivated\\(CoreApplicationView\\s*\\^ [a-zA-Z0-9_]+, IActivatedEventArgs\\s*\\^ [a-zA-Z0-9_]+\\) {".Replace(" ", "[\\s]*");
            InjectCodeToFile(appFilePath, regexPattern, "d3dil2cpp.txt");
        }
    }

    public static void InjectCodeToFile(string appFilePath, string searchRegex, string codeToInsertFileName, bool includeSearchText = true)
    {
        var appAdditionsFolder = "Assets/AppCenter/Plugins/WSA/Push/AppAdditions";
        var codeToInsert = File.ReadAllText(Path.Combine(appAdditionsFolder, codeToInsertFileName));
        var commentText = "App Center Push code:";
        codeToInsert = "\n            // " + commentText + "\n" + codeToInsert;
        var fileText = File.ReadAllText(appFilePath);
        var regex = new Regex(searchRegex);
        var matches = regex.Match(fileText);
        if (matches.Success)
        {
            var codeToReplace = matches.ToString();
            if (!fileText.Contains(commentText))
            {
                if (includeSearchText)
                {
                    codeToInsert = codeToReplace + codeToInsert;
                }
                fileText = fileText.Replace(codeToReplace, codeToInsert);
            }
            File.WriteAllText(appFilePath, fileText);
        }
        else
        {
            // TODO Update documentation link
            Debug.LogError("Unable to automatically modify file '" + appFilePath + "'. For App Center Push to work properly, " +
                           "please follow troubleshooting instructions at https://docs.microsoft.com/en-us/mobile-center/sdk/troubleshooting/unity");
        }
    }

    public static void FixIl2CppLogging(string pathToBuiltProject)
    {
        var sourceDebuggerPath = "Assets\\AppCenter\\Plugins\\WSA\\IL2CPP\\Debugger.cpp.txt";
        var destDebuggerPath = Path.Combine(pathToBuiltProject,
            "Il2CppOutputProject\\IL2CPP\\libil2cpp\\icalls\\mscorlib\\System.Diagnostics\\Debugger.cpp");
        File.Copy(sourceDebuggerPath, destDebuggerPath, true);
    }

    public static string GetAppFilePath(string pathToBuiltProject, string filename)
    {
        var candidate = Path.Combine(pathToBuiltProject, PlayerSettings.WSA.tileShortName);
        candidate = Path.Combine(candidate, filename);
        return File.Exists(candidate) ? candidate : null;
    }

    public static void ProcessUwpIl2CppDependencies()
    {
        var binaries = AssetDatabase.FindAssets("*", new[] { "Assets/AppCenter/Plugins/WSA/IL2CPP" });
        foreach (var guid in binaries)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var importer = AssetImporter.GetAtPath(assetPath) as PluginImporter;
            if (importer != null)
            {
                importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
                importer.SetPlatformData(BuildTarget.WSAPlayer, "ScriptingBackend", "Il2Cpp");
                importer.SaveAndReimport();
            }
        }
    }

    private static void AddDependenciesToProjectJson(string projectJsonPath)
    {
        if (!File.Exists(projectJsonPath))
        {
            Debug.LogWarning(projectJsonPath + " not found!");
            return;
        }
        var jsonString = File.ReadAllText(projectJsonPath);
        jsonString = AddDependencyToProjectJson(jsonString, "Microsoft.NETCore.UniversalWindowsPlatform", "5.2.2");
        jsonString = AddDependencyToProjectJson(jsonString, "Newtonsoft.Json", "10.0.3");
        jsonString = AddDependencyToProjectJson(jsonString, "sqlite-net-pcl", "1.3.1");
        jsonString = AddDependencyToProjectJson(jsonString, "System.Collections.NonGeneric", "4.0.1");
        File.WriteAllText(projectJsonPath, jsonString);
    }

    private static string AddDependencyToProjectJson(string projectJson, string packageId, string packageVersion)
    {
        const string quote = @"\" + "\"";
        var dependencyString = "\"" + packageId + "\": \"" + packageVersion + "\"";
        var pattern = quote + packageId + quote + @":[\s]+" + quote + "[^" + quote + "]*" + quote;
        var regex = new Regex(pattern);
        var match = regex.Match(projectJson);
        if (match.Success)
        {
            return projectJson.Replace(match.Value, dependencyString);
        }
        pattern = quote + "dependencies" + quote + @":[\s]+{";
        regex = new Regex(pattern);
        match = regex.Match(projectJson);
        var idx = projectJson.IndexOf(match.Value, StringComparison.Ordinal) + match.Value.Length;
        return projectJson.Insert(idx, "\n" + dependencyString + ",");
    }

    private static void ExecuteCommand(string command, string arguments, int timeout = 600)
    {
        try
        {
            var buildProcess = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments
                }
            };
            buildProcess.Start();
            buildProcess.WaitForExit(timeout * 1000);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
        }
    }
    #endregion

    #region iOS Methods
#if UNITY_IOS
    private static void OnPostprocessProject(PBXProject project)
    {
        // The target we want to add to is created by Unity.
        var targetName = PBXProject.GetUnityTargetName();
        var targetGuid = project.TargetGuidByName(targetName);

        // Need to add "-lsqlite3" linker flag to "Other linker flags" due to
        // SQLite dependency.
        project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-lsqlite3");
        project.AddBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");
    }

    private static void OnPostprocessInfo(PlistDocument info, AppCenterSettings settings)
    {
        if (settings.UseDistribute && AppCenterSettings.Distribute != null)
        {
            // Add App Center URL sceme.
            var urlTypes = info.root.CreateArray("CFBundleURLTypes");
            var urlType = urlTypes.AddDict();
            urlType.SetString("CFBundleTypeRole", "None");
            urlType.SetString("CFBundleURLName", ApplicationIdHelper.GetApplicationId());
            var urlSchemes = urlType.CreateArray("CFBundleURLSchemes");
            urlSchemes.AddString("appcenter-" + settings.iOSAppSecret);
        }
    }

#if UNITY_2017_1_OR_NEWER
    private static void OnPostprocessCapabilities(ProjectCapabilityManager capabilityManager, AppCenterSettings settings)
    {
        if (settings.UsePush && AppCenterSettings.Push != null)
        {
            capabilityManager.AddPushNotifications(true);
            capabilityManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
        }
    }
#endif
#endif
    #endregion
}
