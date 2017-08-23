// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildPuppet
{
    public static void BuildPuppetSceneAndroidMono()
    {
        CreateGoogleServicesJsonIfNotPresent();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        BuildPuppetScene(BuildTarget.Android, "PuppetBuilds/AndroidMonoBuild");
    }

    public static void BuildPuppetSceneAndroidIl2CPP()
    {
        CreateGoogleServicesJsonIfNotPresent();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.Android, "PuppetBuilds/AndroidIL2CPPBuild");
    }

    public static void BuildPuppetSceneIosMono()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.Mono2x);
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildPuppetScene(BuildTarget.iOS, "PuppetBuilds/iOSMonoBuild");
    }

    public static void BuildPuppetSceneIosIl2CPP()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        BuildPuppetScene(BuildTarget.iOS, "PuppetBuilds/iOSIL2CPPBuild");
    }

    public static void BuildPuppetSceneWsaNet()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.WinRTDotNET);
        BuildPuppetScene(BuildTarget.WSAPlayer, "PuppetBuilds/WSANetBuild");
    }

    public static void BuildPuppetSceneWsaIl2CPP()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.WSAPlayer, "PuppetBuilds/WSAIL2CPPBuild");
    }

    private static void BuildPuppetScene(BuildTarget target, string outputDir)
    {
        string[] puppetScene = { "Assets/Puppet/PuppetScene.unity" };
        var options = new BuildPlayerOptions
        {
            scenes = puppetScene,
            options = BuildOptions.None,
            locationPathName = outputDir,
            target = target
        };
        BuildPipeline.BuildPlayer(options);
    }

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
        AssetDatabase.ImportAsset(actualFile, ImportAssetOptions.Default);
    }
}
