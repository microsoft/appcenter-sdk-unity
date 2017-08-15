﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class MobileCenterPostBuild
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // Load Mobile Center settings.
        var settingsPath = MobileCenterSettingsEditor.SettingsPath;
        var settings = AssetDatabase.LoadAssetAtPath<MobileCenterSettings>(settingsPath);

#if UNITY_WSA_10_0 && !ENABLE_IL2CPP
        if (target == BuildTarget.WSAPlayer)
        {
            // If UWP, need to add NuGet packages.
            var projectJson = pathToBuiltProject + "/" + PlayerSettings.productName + "/project.json";
            AddDependenciesToProjectJson(projectJson);

            var nuget = EditorApplication.applicationContentsPath + "/PlaybackEngines/MetroSupport/Tools/nuget.exe";
            ExecuteCommand(nuget, "restore \"" + projectJson + "\" -NonInteractive");
        }
#endif
#if UNITY_IOS
        if (target == BuildTarget.iOS)
        {
            // Update project.
            var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var targetName = PBXProject.GetUnityTargetName();
            var project = new PBXProject();
            project.ReadFromFile(projectPath);
            OnPostprocessProject(project, settings);
            project.WriteToFile(projectPath);

            // Update Info.plist.
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
#endif
        }
#endif
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
    private static void AddDependenciesToProjectJson(string projectJsonPath)
    {
        if (!File.Exists(projectJsonPath))
        {
            Debug.LogWarning(projectJsonPath + " not found!");
            return;
        }
        var jsonString = File.ReadAllText(projectJsonPath);
        jsonString = AddDependencyToProjectJson(jsonString, "Microsoft.NETCore.UniversalWindowsPlatform", "5.3.3");
        jsonString = AddDependencyToProjectJson(jsonString, "sqlite-net-pcl", "1.3.3");
        jsonString = AddDependencyToProjectJson(jsonString, "Newtonsoft.Json", "10.0.2");
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
    private static void OnPostprocessProject(PBXProject project, MobileCenterSettings settings)
    {
        // The target we want to add to is created by Unity.
        var targetName = PBXProject.GetUnityTargetName();
        var targetGuid = project.TargetGuidByName(targetName);

        // Need to add "-lsqlite3" linker flag to "Other linker flags" due to
        // SQLite dependency.
        project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-lsqlite3");
    }

    private static void OnPostprocessInfo(PlistDocument info, MobileCenterSettings settings)
    {
        if (settings.UseDistribute && MobileCenterSettings.Distribute != null)
        {
            // Add Mobile Center URL sceme.
            var urlTypes = info.root.CreateArray("CFBundleURLTypes");
            var urlType = urlTypes.AddDict();
            urlType.SetString("CFBundleTypeRole", "None");
            urlType.SetString("CFBundleURLName", GetApplicationId());
            var urlSchemes = urlType.CreateArray("CFBundleURLSchemes");
            urlSchemes.AddString("mobilecenter-" + settings.iOSAppSecret);
        }
    }

#if UNITY_2017_1_OR_NEWER
    private static void OnPostprocessCapabilities(ProjectCapabilityManager capabilityManager, MobileCenterSettings settings)
    {
        if (settings.UsePush && MobileCenterSettings.Push != null)
        {
            capabilityManager.AddPushNotifications(true);
            capabilityManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);
        }
    }
#endif
#endif
#endregion
}
