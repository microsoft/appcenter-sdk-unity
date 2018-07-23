// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

// This class provides methods to build the demo app in many different configurations.
// They are meant to be invoked from the build scripts in this repository.
public class BuildDemo
{
    private static readonly string BuildFolder = "CAKE_SCRIPT_TEMPDemoBuilds";
    private static readonly string AppIdentifier = "com.microsoft.appcenter.unity.demo";

    static BuildDemo()
    {
#if UNITY_5_6_OR_NEWER
        PlayerSettings.applicationIdentifier = AppIdentifier;
#else
        PlayerSettings.bundleIdentifier = AppIdentifier;
#endif
    }

    public static void BuildDemoSceneAndroidMono()
    {
        BuildDemoScene(BuildTarget.Android, BuildTargetGroup.Android, ScriptingImplementation.Mono2x, "AndroidMonoBuild.apk");
    }

    public static void BuildDemoSceneAndroidIl2CPP()
    {
        // Set NDK location if provided
        var args = Environment.GetCommandLineArgs();
        bool next = false;
        foreach (var arg in args)
        {
            if (next)
            {
                var ndkLocation = arg;
                Debug.Log("Setting NDK location to " + ndkLocation);
                EditorPrefs.SetString("AndroidNdkRoot", ndkLocation);
                Debug.Log("NDK Location is now '" + EditorPrefs.GetString("AndroidNdkRoot") + "'");
                break;
            }
            if (arg == "-NdkLocation")
            {
                next = true;
            }
        }
        BuildDemoScene(BuildTarget.Android, BuildTargetGroup.Android, ScriptingImplementation.IL2CPP, "AndroidIL2CPPBuild.apk");
    }

    public static void BuildDemoSceneIosMono()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
        BuildDemoScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.Mono2x, "iOSMonoBuild");
    }

    public static void BuildDemoSceneIosIl2CPP()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
        BuildDemoScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP, "iOSIL2CPPBuild");
    }

    public static void BuildDemoSceneIosMonoDeviceSdk()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildDemoScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.Mono2x, "iOSMonoBuild");
    }

    public static void BuildDemoSceneIosIl2CPPDeviceSdk()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildDemoScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP, "iOSIL2CPPBuild");
    }

    public static void BuildDemoSceneWsaNetXaml()
    {
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.XAML;
        PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Legacy;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        BuildDemoScene(BuildTarget.WSAPlayer, BuildTargetGroup.WSA, ScriptingImplementation.WinRTDotNET, "WSANetBuildXaml");
    }

    public static void BuildDemoSceneWsaIl2CPPXaml()
    {
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.XAML;
        PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Legacy;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        BuildDemoScene(BuildTarget.WSAPlayer, BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP, "WSAIL2CPPBuildXaml");
    }

    public static void BuildDemoSceneWsaNetD3D()
    {
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.D3D;
        PlayerSettings.WSA.compilationOverrides = PlayerSettings.WSACompilationOverrides.UseNetCore;
        PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Legacy;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        BuildDemoScene(BuildTarget.WSAPlayer, BuildTargetGroup.WSA, ScriptingImplementation.WinRTDotNET, "WSANetBuildD3D");
    }

    public static void BuildDemoSceneWsaIl2CPPD3D()
    {
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.D3D;
        PlayerSettings.scriptingRuntimeVersion = ScriptingRuntimeVersion.Legacy;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        BuildDemoScene(BuildTarget.WSAPlayer, BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP, "WSAIL2CPPBuildD3D");
    }

    private static void BuildDemoScene(BuildTarget target, BuildTargetGroup targetGroup, ScriptingImplementation scriptingImplementation, string outputPath)
    {
        PlayerSettings.SetScriptingBackend(targetGroup, scriptingImplementation);
        string[] demoScene = { "Assets/Demo/DemoScene.unity" };
        var options = new BuildPlayerOptions
        {
            scenes = demoScene,
            options = BuildOptions.None,
            locationPathName = Path.Combine(BuildFolder, outputPath),
            target = target
        };
        BuildPipeline.BuildPlayer(options);
    }

    // Increments build version for all platforms
    public static void IncrementVersionNumber()
    {
        var currentVersion = PlayerSettings.bundleVersion;
        Debug.Log("current version: " + currentVersion);
        var minorVersion = int.Parse(currentVersion.Substring(currentVersion.LastIndexOf(".") + 1)) + 1;
        var newVersion = currentVersion.Substring(0, currentVersion.LastIndexOf(".") + 1) + minorVersion;
        Debug.Log("new version: " + newVersion);
        PlayerSettings.bundleVersion = newVersion;
        PlayerSettings.Android.bundleVersionCode++;
    }

    // Sets version number for demo app
    public static void SetVersionNumber()
    {
        var currentVersion = PlayerSettings.bundleVersion;
        var demoVersion = Microsoft.AppCenter.Unity.WrapperSdk.WrapperSdkVersion;
        PlayerSettings.bundleVersion = demoVersion;
        PlayerSettings.Android.bundleVersionCode++;
    }
}
