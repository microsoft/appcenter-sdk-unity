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

public class MobileCenterPostBuild
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.WSAPlayer &&
            PlayerSettings.GetScriptingBackend(BuildTargetGroup.WSA) == ScriptingImplementation.IL2CPP)
        {
            // If UWP, need to add NuGet packages.
            var projectJson = pathToBuiltProject + "/" + PlayerSettings.productName + "/project.json";
            AddDependenciesToProjectJson(projectJson);

            var nuget = EditorApplication.applicationContentsPath + "/PlaybackEngines/MetroSupport/Tools/nuget.exe";
            ExecuteCommand(nuget, "restore \"" + projectJson + "\" -NonInteractive");
        }
        if (target == BuildTarget.iOS)
        {
            // Note: Need to use reflection for all of this because Windows editor might not have
            // the namespace required. Can't use #if UNITY_IOS due to a bug where building from 
            // command line in batch mode doesn't properly register the target.

            // Load Mobile Center settings.
            var settingsPath = MobileCenterSettingsEditor.SettingsPath;
            var settings = AssetDatabase.LoadAssetAtPath<MobileCenterSettings>(settingsPath);

            // Update project.
            var xcExtensionsAssembly = Assembly.Load("UnityEditor.iOS.Extensions.Xcode");
            var pbxProjType = xcExtensionsAssembly.GetType("UnityEditor.iOS.Xcode.PBXProject");
            var projectPath = pbxProjType.GetMethod("GetPBXProjectPath", BindingFlags.Public | BindingFlags.Static).Invoke(pbxProjType, new object[] { pathToBuiltProject });
            var targetName = pbxProjType.GetMethod("GetUnityTargetName", BindingFlags.Public | BindingFlags.Static).Invoke(pbxProjType, null);
            var project = pbxProjType.GetConstructor(Type.EmptyTypes).Invoke(null);
            project.GetType().GetMethod("ReadFromFile").Invoke(project, new[] { projectPath });
            OnPostprocessProject(project, settings);
            project.GetType().GetMethod("WriteToFile").Invoke(project, new[] { projectPath });

            // Update Info.plist.
            var plistType = xcExtensionsAssembly.GetType("UnityEditor.iOS.Xcode.PlistDocument");
            var infoPath = pathToBuiltProject + "/Info.plist";
            var info = plistType.GetConstructor(Type.EmptyTypes).Invoke(null);
            plistType.GetMethod("ReadFromFile").Invoke(info, new[] { infoPath });
            OnPostprocessInfo(info, settings);
            plistType.GetMethod("WriteToFile").Invoke(info, new[] { infoPath });

#if UNITY_2017_1_OR_NEWER
            // Update capabilities.
            var capabilityManager = xcExtensionsAssembly
                .GetType("UnityEditor.iOS.Xcode.ProjectCapabilityManager")
                .GetConstructor(new [] { typeof(string), typeof(string), typeof(string) })
                .Invoke(new object[] { projectPath, targetName + ".entitlements", targetName });
            OnPostprocessCapabilities(capabilityManager, settings);
            capabilityManager.GetType().GetMethod("WriteToFile").Invoke(capabilityManager, null);

#endif
		}
    }

    private static string GetApplicationId()
    {
#if UNITY_5_6_OR_NEWER
        return PlayerSettings.applicationIdentifier;
#else
        return PlayerSettings.bundleIdentifier;
#endif
    }

    #region UWP Methods
    public static void ProcessUwpIl2CppDependencies()
    {
        var binaries = AssetDatabase.FindAssets("*", new[] { "Assets/Plugins/WSA/IL2CPP" });
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

    public static void ProcessUwpMobileCenterBinaries()
    {
        var directories = Directory.GetDirectories("Assets/Plugins/WSA", "*", SearchOption.AllDirectories);
        var assemblies = AssetDatabase.FindAssets("Microsoft.Azure.Mobile", directories);
        foreach (var guid in assemblies)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var importer = AssetImporter.GetAtPath(assetPath) as PluginImporter;
            if (importer != null)
            {
                importer.SetPlatformData(BuildTarget.WSAPlayer, "DontProcess", "true");
                importer.SetPlatformData(BuildTarget.WSAPlayer, "SDK", "UWP");
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
        jsonString = AddDependencyToProjectJson(jsonString, "System.Collections.NonGeneric", "4.0.0");

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
    private static void OnPostprocessProject(object project, MobileCenterSettings settings)
    {
        // The target we want to add to is created by Unity.
        var targetName = project.GetType().GetMethod("GetUnityTargetName", BindingFlags.Public | BindingFlags.Static).Invoke(project.GetType(), null);
        var targetGuid = project.GetType().GetMethod("TargetGuidByName").Invoke(project, new object[] { targetName });
		// Need to add "-lsqlite3" linker flag to "Other linker flags" due to SQLite dependency.
        project.GetType().GetMethod("AddBuildProperty", new[] { typeof(string), typeof(string), typeof(string) })
               .Invoke(project, new [] { targetGuid, "OTHER_LDFLAGS", "-lsqlite3" });
	}

    private static void OnPostprocessInfo(object info, MobileCenterSettings settings)
    {
        if (settings.UseDistribute && MobileCenterSettings.Distribute != null)
        {
            // Add Mobile Center URL sceme.
            var root = info.GetType().GetField("root").GetValue(info);
			var urlTypes = root.GetType().GetMethod("CreateArray").Invoke(root, new object[] { "CFBundleURLTypes" });
            var urlType = urlTypes.GetType().GetMethod("AddDict").Invoke(urlTypes, null);
            var setStringMethod = urlType.GetType().GetMethod("SetString");
            setStringMethod.Invoke(urlType, new object[] { "CFBundleTypeRole", "None" });
            setStringMethod.Invoke(urlType, new object[] { "CFBundleURLName", GetApplicationId() });
            var urlSchemes = urlType.GetType().GetMethod("CreateArray").Invoke(urlType, new [] { "CFBundleURLSchemes" });
            urlSchemes.GetType().GetMethod("AddString").Invoke(urlSchemes, new [] { "mobilecenter-" + settings.iOSAppSecret });
        }
    }
#if UNITY_2017_1_OR_NEWER
    private static void OnPostprocessCapabilities(object capabilityManager, MobileCenterSettings settings)
    {
        if (settings.UsePush && MobileCenterSettings.Push != null)
        {
            capabilityManager.GetType().GetMethod("AddPushNotifications").Invoke(capabilityManager, new object[] { true });
            var backgroundModesEnumType = capabilityManager.GetType().Assembly.GetType("UnityEditor.iOS.Xcode.BackgroundModesOptions");
            var remoteNotifEnum = Enum.Parse(backgroundModesEnumType, "RemoteNotifications");
            capabilityManager.GetType().GetMethod("AddBackgroundModes").Invoke(capabilityManager, new object[] { remoteNotifEnum });
        }
    }
#endif
	#endregion
}
