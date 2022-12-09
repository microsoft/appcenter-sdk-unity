// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

// This class provides methods to build the puppet app in many different configurations.
// They are meant to be invoked from the build scripts in this repository.
public class BuildPuppet
{
    private static readonly string BuildFolder = "CAKE_SCRIPT_TEMPPuppetBuilds";
    private static readonly string AssetsFolder = "Assets";
    private static readonly string AppIdentifier = "com.microsoft.appcenter.unity.puppet";

    static BuildPuppet()
    {
        PlayerSettings.applicationIdentifier = AppIdentifier;
    }

    public static void BuildPuppetSceneAndroidMono()
    {
        BuildPuppetScene(BuildTarget.Android, BuildTargetGroup.Android, ScriptingImplementation.Mono2x, "AndroidMonoBuild.apk");
    }

    public static void BuildPuppetSceneAndroidIl2CPP()
    {

        // Set NDK location if provided
        var args = Environment.GetCommandLineArgs();
        bool next = false;
        foreach (var arg in args)
        {
            if (next)
            {
                var ndkLocation = arg;
                var subdir = System.IO.Directory.GetDirectories(ndkLocation).Single();

#if UNITY_2021_0_OR_NEWER
                Debug.Log($"Setting NDK location to {subdir}, via AndroidExternalToolsSettings");
                UnityEditor.Android.AndroidExternalToolsSettings.ndkRootPath = subdir;
#else
                Debug.Log($"Setting NDK location to {subdir}, via EditorPrefs");
                EditorPrefs.SetString("AndroidNdkRoot", subdir);
                EditorPrefs.SetString("AndroidNdkRootR16b", subdir);
#endif
                break;
            }
            if (arg == "-NdkLocation")
            {
                next = true;
            }
        }
        BuildPuppetScene(BuildTarget.Android, BuildTargetGroup.Android, ScriptingImplementation.IL2CPP, "AndroidIL2CPPBuild.apk");
    }

    public static void BuildPuppetSceneIosMono()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
        BuildPuppetScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.Mono2x, "iOSMonoBuild");
    }

    public static void BuildPuppetSceneIosIl2CPP()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
        BuildPuppetScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP, "iOSIL2CPPBuild");
    }

    public static void BuildPuppetSceneIosMonoDeviceSdk()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildPuppetScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.Mono2x, "iOSMonoBuild");
    }

    public static void BuildPuppetSceneIosIl2CPPDeviceSdk()
    {
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildPuppetScene(BuildTarget.iOS, BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP, "iOSIL2CPPBuild");
    }

    public static void BuildPuppetSceneWsaIl2CPPXaml()
    {
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.XAML;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        BuildPuppetScene(BuildTarget.WSAPlayer, BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP, "WSAIL2CPPBuildXaml");
    }

    public static void BuildPuppetSceneWsaIl2CPPD3D()
    {
        EditorUserBuildSettings.wsaUWPBuildType = WSAUWPBuildType.D3D;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.WSA, ApiCompatibilityLevel.NET_4_6);
        BuildPuppetScene(BuildTarget.WSAPlayer, BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP, "WSAIL2CPPBuildD3D");
    }

    private static void BuildPuppetScene(BuildTarget target, BuildTargetGroup targetGroup, ScriptingImplementation scriptingImplementation, string outputPath)
    {
        PlayerSettings.SetScriptingBackend(targetGroup, scriptingImplementation);
        string[] puppetScene = { AssetsFolder + "/Puppet/PuppetScene.unity" };
        var options = new BuildPlayerOptions
        {
            scenes = puppetScene,
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

    // Sets version number for puppet app
    public static void SetVersionNumber()
    {
        PlayerSettings.bundleVersion = Microsoft.AppCenter.Unity.WrapperSdk.WrapperSdkVersion;
        PlayerSettings.Android.bundleVersionCode++;
    }
}
