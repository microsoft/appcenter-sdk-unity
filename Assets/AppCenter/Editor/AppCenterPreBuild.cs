// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity;
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class AppCenterPreBuild : IPreprocessBuildWithReport
{
    private const string AarFilePattern = "appcenter-{0}-release";
    public int callbackOrder { get { return 0; } }
#if UNITY_WSA
    private readonly Version RequiredMinimalUWPVersion = new Version("10.0.16299.0");
#endif

    public void OnPreprocessBuild(BuildReport report)
    {
        OnPreprocessBuild(report.summary.platform, report.summary.outputPath);
    }

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.Android)
        {
#if !APPCENTER_DONT_USE_NATIVE_STARTER
            var settingsMaker = new AppCenterSettingsMakerAndroid();
            AddStartupCode(settingsMaker);
#if UNITY_ANDROID
            AddSettingsFileToLoader(settingsMaker);
#endif
#endif
        }
        else if (target == BuildTarget.iOS)
        {
#if !APPCENTER_DONT_USE_NATIVE_STARTER
            AddStartupCode(new AppCenterSettingsMakerIos());
#endif
        }
        else if (target == BuildTarget.WSAPlayer)
        {
#if UNITY_WSA
            var currentMinimalPlatformVersion = new Version(EditorUserBuildSettings.wsaMinUWPSDK);
            if (currentMinimalPlatformVersion < RequiredMinimalUWPVersion)
            {
                Debug.LogWarning($"Minimum platform version should be set to {RequiredMinimalUWPVersion} or higher. App Center does not support lower versions but it is set to {currentMinimalPlatformVersion}");
            }
#endif
        }
        if (target == BuildTarget.Android)
        {
            // No linking/unlinking in case module isn't added.
            if (AppCenter.Distribute != null) 
            {
                LinkModule(AppCenterSettingsContext.SettingsInstance.UseDistribute, "distribute");
            }
            if (AppCenter.Analytics != null) 
            {
                LinkModule(AppCenterSettingsContext.SettingsInstance.UseAnalytics, "analytics");
            }
            if (AppCenter.Crashes != null) 
            {
                LinkModule(AppCenterSettingsContext.SettingsInstance.UseCrashes, "crashes");
            }
        }
    }

#if UNITY_ANDROID
    public static void AddSettingsFileToLoader(AppCenterSettingsMakerAndroid settingsMaker)
    {
        var loaderZipFile = AppCenterSettingsContext.AppCenterPath + "/Plugins/Android/appcenter-loader-release.aar";
        const string loaderFolder = "appcenter-loader-release";
        const string settingsFilePath = loaderFolder + "/res/values/appcenter-settings.xml";
        const string settingsMetaFilePath = loaderFolder + "/res/values/appcenter-settings.xml.meta";

        if (!File.Exists(loaderZipFile))
        {
            throw new IOException("Failed to load dependency file appcenter-loader-release.aar");
        }

        // Delete unzipped directory if it already exists.
        if (Directory.Exists(loaderFolder))
        {
            Directory.Delete(loaderFolder, true);
        }

        AndroidLibraryHelper.UnzipFile(loaderZipFile, loaderFolder);
        if (!Directory.Exists(loaderFolder))
        {
            throw new IOException("Unzipping loader folder failed.");
        }

        settingsMaker.CommitSettings(settingsFilePath);

        // Delete the appcenter-settings.xml.meta file if generated.
        if (File.Exists(settingsMetaFilePath))
        {
            File.Delete(settingsMetaFilePath);
        }

        // Delete the original aar file and zipped the extracted folder to generate a new one.
        File.Delete(loaderZipFile);
        AndroidLibraryHelper.ZipFile(loaderFolder, loaderZipFile);
        Directory.Delete(loaderFolder, true);
    }
#endif

    private void AddStartupCode(IAppCenterSettingsMaker settingsMaker)
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
        var advancedSettings = AppCenterSettingsContext.SettingsInstanceAdvanced;
        settingsMaker.SetAppSecret(settings);
        settingsMaker.SetLogLevel((int)settings.InitialLogLevel);
        settingsMaker.IsAllowNetworkRequests((bool)settings.AllowNetworkRequests);
        settingsMaker.EnableManualSessionTracker((bool)settings.EnableManualSessionTracker);
        if (settings.CustomLogUrl.UseCustomUrl)
        {
            settingsMaker.SetLogUrl(settings.CustomLogUrl.Url);
        }
        if (settings.MaxStorageSize.UseCustomMaxStorageSize && settings.MaxStorageSize.Size > 0)
        {
            settingsMaker.SetMaxStorageSize(settings.MaxStorageSize.Size);
        }
        if (settings.UseAnalytics && settingsMaker.IsAnalyticsAvailable())
        {
            settingsMaker.StartAnalyticsClass();
        }
        if (settings.UseCrashes && settingsMaker.IsCrashesAvailable())
        {
            settingsMaker.StartCrashesClass();
        }
        if (settings.UseDistribute && settingsMaker.IsDistributeAvailable())
        {
            if (settings.CustomApiUrl.UseCustomUrl)
            {
                settingsMaker.SetApiUrl(settings.CustomApiUrl.Url);
            }
            if (settings.CustomInstallUrl.UseCustomUrl)
            {
                settingsMaker.SetInstallUrl(settings.CustomInstallUrl.Url);
            }
            if (settings.EnableDistributeForDebuggableBuild)
            {
                settingsMaker.SetShouldEnableDistributeForDebuggableBuild();
            }
            if (!settings.AutomaticCheckForUpdate)
            {
                settingsMaker.SetDistributeDisableAutomaticCheckForUpdate();
            }
            settingsMaker.SetUpdateTrack(settings.UpdateTrack);
            settingsMaker.StartDistributeClass();
        }
        if (advancedSettings != null)
        {
            var startupType = settingsMaker.IsStartFromAppCenterBehavior(advancedSettings) ? StartupType.Skip : advancedSettings.GetStartupType();
            settingsMaker.SetStartupType((int)startupType);
            settingsMaker.SetTransmissionTargetToken(advancedSettings.TransmissionTargetToken);
        }
        else
        {
            settingsMaker.SetStartupType((int)StartupType.AppCenter);
        }
        settingsMaker.CommitSettings();
    }

    #region Android Methods
    
    private static void LinkModule(bool isEnabled, string moduleName) 
    {
        var aarName = string.Format(AarFilePattern, moduleName);
        var aarFileAsset = AssetDatabase.FindAssets(aarName, new[] { AppCenterSettingsContext.AppCenterPath + "/Plugins/Android" });
        if (aarFileAsset.Length == 0)
        {
            Debug.LogWarning("Failed to link " + moduleName + ", file `" + aarName + "` is not found");
            return;
        }
        var assetPath = AssetDatabase.GUIDToAssetPath(aarFileAsset[0]);
        var importer = AssetImporter.GetAtPath(assetPath) as PluginImporter;
        if (importer != null)
        {
            Debug.Log (moduleName + " is " + (isEnabled ? "" : "not ") + "enabled. " +
                (isEnabled ? "Linking " : "Unlinking ") + aarName);
            importer.SetCompatibleWithPlatform(BuildTarget.Android, isEnabled);
            importer.SaveAndReimport();
        }
    }
    #endregion
}
