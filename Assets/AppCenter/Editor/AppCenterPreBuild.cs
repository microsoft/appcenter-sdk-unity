// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
using UnityEngine;
#endif

#if UNITY_2018_1_OR_NEWER
public class AppCenterPreBuild : IPreprocessBuildWithReport
#else
public class AppCenterPreBuild : IPreprocessBuild
#endif
{
    public int callbackOrder { get { return 0; } }

#if UNITY_2018_1_OR_NEWER
    public void OnPreprocessBuild(BuildReport report)
    {
        OnPreprocessBuild(report.summary.platform, report.summary.outputPath);
    }
#endif

    public void OnPreprocessBuild(BuildTarget target, string path)
    {
        if (target == BuildTarget.Android)
        {
            var settings = AppCenterSettingsContext.SettingsInstance;
            if (settings.UsePush && AppCenter.Push != null)
            {
                FirebaseDependency.SetupPush();
            }
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
        } else if (target == BuildTarget.WSAPlayer) {
            CleanOldDependencies();
        }
    }

    public void CleanOldDependencies() {

        //We don't delete Newtonsoft.Json, SQLitePCLRaw.provider.e_sqlite3, 
        //SQLitePCLRaw.core, SQLitePCLRaw.batteries_green and SQLitePCLRaw.batteries_v2
        //as they have the same name and will be replaced automatically upon import.
        var filesToDelete = new[] {"SQLite-net"};
        foreach(var file in filesToDelete) {
            var dllPath = AppCenterSettingsContext.AppCenterPath + "/Plugins/WSA/IL2CPP/" + file + ".dll";
            var metaFilePath = dllPath + ".meta";
            if (File.Exists(dllPath))
            {
                File.Delete(dllPath);
            }
            if (File.Exists(metaFilePath))
            {
                File.Delete(metaFilePath);
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
            Debug.LogWarning("Failed to load dependency file appcenter-loader-release.aar");
            return;
        }

        // Delete unzipped directory if it already exists.
        if (Directory.Exists(loaderFolder))
        {
            Directory.Delete(loaderFolder, true);
        }

        AndroidLibraryHelper.UnzipFile(loaderZipFile, loaderFolder);
        if (!Directory.Exists(loaderFolder))
        {
            Debug.LogWarning("Unzipping loader folder failed.");
            return;
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
        if (settings.CustomLogUrl.UseCustomUrl)
        {
            settingsMaker.SetLogUrl(settings.CustomLogUrl.Url);
        }
        if (settings.MaxStorageSize.UseCustomMaxStorageSize && settings.MaxStorageSize.Size > 0)
        {
            settingsMaker.SetMaxStorageSize(settings.MaxStorageSize.Size);
        }
        if (settings.UsePush && settingsMaker.IsPushAvailable())
        {
            settingsMaker.StartPushClass();
            if (settings.EnableFirebaseAnalytics)
            {
                settingsMaker.EnableFirebaseAnalytics();
            }
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
            settingsMaker.SetUpdateTrack((int)settings.UpdateTrack);
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
#if !UNITY_ANDROID
        settingsMaker.CommitSettings();
#endif
    }
}
