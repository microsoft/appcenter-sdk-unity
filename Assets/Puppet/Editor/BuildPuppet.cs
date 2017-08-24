// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

// This class provides methods to build the puppet app in many different configurations.
// They are meant to be invoked from the build scripts in this repository.
public class BuildPuppet
{
    private static readonly string BuildFolder = "PuppetBuilds";

    public static void BuildPuppetSceneAndroidMono()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Unknown, "MOBILECENTERFORCERECOMPILE");
        CreateGoogleServicesJsonIfNotPresent();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        BuildPuppetScene(BuildTarget.Android, "AndroidMonoBuild.apk");
    }

    public static void BuildPuppetSceneAndroidIl2CPP()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "MOBILECENTERFORCERECOMPILE");
        CreateGoogleServicesJsonIfNotPresent();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.Android, "AndroidIL2CPPBuild.apk");
    }

    public static void BuildPuppetSceneIosMono()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "HELLO_FROM_VSMC");
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.Mono2x);
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildPuppetScene(BuildTarget.iOS, "iOSMonoBuild");
        
    }

    public static void BuildPuppetSceneIosIl2CPP()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "HELLO_FROM_VSMC");
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildPuppetScene(BuildTarget.iOS, "iOSIL2CPPBuild");
    }

    public static void BuildPuppetSceneWsaNetXaml()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WSA, BuildTarget.WSAPlayer);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Unknown, "MOBILECENTERFORCERECOMPILE");
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.XAML;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.WinRTDotNET);
        BuildPuppetScene(BuildTarget.WSAPlayer, "WSANetBuildXaml");
    }

    public static void BuildPuppetSceneWsaIl2CPPXaml()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WSA, BuildTarget.WSAPlayer);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Unknown, "MOBILECENTERFORCERECOMPILE");
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.XAML;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.WSAPlayer, "WSAIL2CPPBuildXaml");
    }
    
    public static void BuildPuppetSceneWsaNetD3D()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WSA, BuildTarget.WSAPlayer);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Unknown, "MOBILECENTERFORCERECOMPILE");
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.D3D;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.WinRTDotNET);
        BuildPuppetScene(BuildTarget.WSAPlayer, "WSANetBuildD3D");
    }

    public static void BuildPuppetSceneWsaIl2CPPD3D()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WSA, BuildTarget.WSAPlayer);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Unknown, "MOBILECENTERFORCERECOMPILE");
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.D3D;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.WSAPlayer, "WSAIL2CPPBuildD3D");
    }

    private static void BuildPuppetScene(BuildTarget target, string outputPath)
    {
        string[] puppetScene = { "Assets/Puppet/PuppetScene.unity" };
        var outputPlayer = Path.Combine(BuildFolder, outputPath);
        var options = new BuildPlayerOptions
        {
            scenes = puppetScene,
            options = BuildOptions.None,
            locationPathName = outputPlayer,
            target = target
        };
        
        BuildPipeline.BuildPlayer(options);
        //AssetImporter.GetAtPath("Assets/MobileCenter/Editor/MobileCenterPostBuild.cs").SaveAndReimport();
        //MobileCenterPostBuild.OnPostprocessBuild(target, outputPlayer);
    }

    // Detects whether there exists a "google-services.json" file, and if not,
    // copies the "google-services-placeholder.json" and imports it as a
    // "google-services.json" file. The resulting file contains only
    // placeholders for keys, not actual keys.
    private static void CreateGoogleServicesJsonIfNotPresent()
    {
        var actualFile = "Assets/google-services.json";
        if (File.Exists(actualFile))
        {
            return;
        }
        var placeholderFile = "Assets/google-services-placeholder.json";
        if (!File.Exists(placeholderFile))
        {
            System.Console.WriteLine("Could not find google services placeholder.");
        }
        File.Copy(placeholderFile, actualFile);
                Debug.Log("will post process");

        AssetDatabase.ImportAsset(actualFile, ImportAssetOptions.Default);
    }
}
