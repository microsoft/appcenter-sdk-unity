// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using Microsoft.Azure.Mobile.Unity;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public class MobileCenterPostBuild
{
    private static string SettingsPath = "Assets/MobileCenter/MobileCenterSettings.asset";
    private static readonly MobileCenterSettings Settings = AssetDatabase.LoadAssetAtPath<MobileCenterSettings>(SettingsPath);

    [PostProcessBuild(0)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.WSAPlayer)
        {
            // If UWP, need to add NuGet packages
            var projectJson = pathToBuiltProject + "/" + PlayerSettings.productName + "/project.json";
            AddDependenciesToProjectJson(projectJson);

            var nuget = EditorApplication.applicationContentsPath + "/PlaybackEngines/MetroSupport/Tools/nuget.exe";
            ExecuteCommand(nuget, "restore \"" + projectJson + "\" -NonInteractive");
        }
        else if (target == BuildTarget.iOS)
        {
            // For iOS, need to add "-lsqlite3" linker flag to PBXProject due to
            // SQLite dependency
#if UNITY_IOS
            AddLinkerFlagToXcodeProject("-lsqlite3", pathToBuiltProject);
            AddStartupCodeToIos(pathToBuiltProject);
#endif
        }
    }

    private static void AddStartupCodeToIos(string pathToBuiltProject)
    {
        var settingsMaker = new MobileCenterSettingsMakerIos(pathToBuiltProject);
        settingsMaker.SetAppSecret(GetAppSecret());
        if (Settings.CustomLogUrl.UseCustomLogUrl)
        {
            settingsMaker.SetLogUrl(Settings.CustomLogUrl.LogUrl);
        }
        if (Settings.UsePush)
        {
            settingsMaker.StartPushClass();
        }
        settingsMaker.SetLogLevel((int)Settings.InitialLogLevel);
        settingsMaker.CommitSettings();
    }

    private static string GetAppSecret()
    {
#if UNITY_IOS
        return Settings.iOSAppSecret;
#elif UNITY_ANDROID
        return Settings.AndroidAppSecret;
#elif UNITY_WSA_10_0
        return Settings.UWPAppSecret;
#else
        return Settings.AppSecret;
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

    private static void AddLinkerFlagToXcodeProject(string linkerFlag, string pathToBuiltProject)
    {
        // Linker flags are added to the setting "Other linker flags" which has
        // an ID of "OTHER_LDFLAGS"
        var setting = "OTHER_LDFLAGS";

        // Find the .pbxproj file and read into memory
        var pbxPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        var pbxProject = new PBXProject();
        pbxProject.ReadFromFile(pbxPath);

        // The target we want to add to is created by Unity
        var targetGuid = pbxProject.TargetGuidByName("Unity-iPhone");

        pbxProject.UpdateBuildProperty(targetGuid, setting, new List<string> { linkerFlag }, null);
        pbxProject.WriteToFile(pbxPath);
    }
#endif
    #endregion
}
