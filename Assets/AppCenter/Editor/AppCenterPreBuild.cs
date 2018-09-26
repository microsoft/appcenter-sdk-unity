using System.IO;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
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
            AddStartupCodeToAndroid();
        }
        else if (target == BuildTarget.iOS)
        {
            AddStartupCodeToiOS();
        }
    }

    void AddStartupCodeToAndroid()
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
        var advancedSettings = AppCenterSettingsContext.SettingsInstanceAdvanced;
        if (settings == null)
        {
            return;
        }
        var settingsMaker = new AppCenterSettingsMakerAndroid();
        settingsMaker.SetAppSecret(settings.AndroidAppSecret);
        if (settings.CustomLogUrl.UseCustomUrl)
        {
            settingsMaker.SetLogUrl(settings.CustomLogUrl.Url);
        }
        if (settings.UsePush && IsAndroidPushAvailable())
        {
            settingsMaker.StartPushClass();
            if (settings.EnableFirebaseAnalytics)
            {
                settingsMaker.EnableFirebaseAnalytics();
            }
        }
        if (settings.UseAnalytics && IsAndroidAnalyticsAvailable())
        {
            settingsMaker.StartAnalyticsClass();
        }
        if (settings.UseCrashes && IsAndroidCrashesAvailable())
        {
            settingsMaker.StartCrashesClass();
        }
        if (settings.UseDistribute && IsAndroidDistributeAvailable())
        {
            if (settings.CustomApiUrl.UseCustomUrl)
            {
                settingsMaker.SetApiUrl(settings.CustomApiUrl.Url);
            }
            if (settings.CustomInstallUrl.UseCustomUrl)
            {
                settingsMaker.SetInstallUrl(settings.CustomInstallUrl.Url);
            }
            settingsMaker.StartDistributeClass();
        }
        settingsMaker.SetLogLevel((int)settings.InitialLogLevel);
        settingsMaker.SetStartupType((int)advancedSettings.AppCenterStartupType);
        settingsMaker.SetTransmissionTargetToken(advancedSettings.TransmissionTargetToken);
        settingsMaker.CommitSettings();
    }

    static void AddStartupCodeToiOS()
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
        var advancedSettings = AppCenterSettingsContext.SettingsInstanceAdvanced;
        if (settings == null)
        {
            return;
        }
        var settingsMaker = new AppCenterSettingsMakerIos();
        if (settings.CustomLogUrl.UseCustomUrl)
        {
            settingsMaker.SetLogUrl(settings.CustomLogUrl.Url);
        }
        settingsMaker.SetLogLevel((int)settings.InitialLogLevel);
        settingsMaker.SetAppSecret(settings.iOSAppSecret);
        if (settings.UseCrashes && IsIOSCrashesAvailable())
        {
            settingsMaker.StartCrashesClass();
        }
        if (settings.UsePush && IsIOSPushAvailable())
        {
            settingsMaker.StartPushClass();
        }
        if (settings.UseAnalytics && IsIOSAnalyticsAvailable())
        {
            settingsMaker.StartAnalyticsClass();
        }
        if (settings.UseDistribute && IsIOSDistributeAvailable())
        {
            if (settings.CustomApiUrl.UseCustomUrl)
            {
                settingsMaker.SetApiUrl(settings.CustomApiUrl.Url);
            }
            if (settings.CustomInstallUrl.UseCustomUrl)
            {
                settingsMaker.SetInstallUrl(settings.CustomInstallUrl.Url);
            }
            settingsMaker.StartDistributeClass();
        }
        settingsMaker.SetStartupType((int)advancedSettings.AppCenterStartupType);
        settingsMaker.SetTransmissionTargetToken(advancedSettings.TransmissionTargetToken);
        settingsMaker.CommitSettings();
    }

    static bool IsAndroidDistributeAvailable()
    {
        return File.Exists("Assets/AppCenter/Plugins/Android/appcenter-distribute-release.aar");
    }

    static bool IsAndroidPushAvailable()
    {
        return File.Exists("Assets/AppCenter/Plugins/Android/appcenter-push-release.aar");
    }

    static bool IsAndroidAnalyticsAvailable()
    {
        return File.Exists("Assets/AppCenter/Plugins/Android/appcenter-analytics-release.aar");
    }

    static bool IsAndroidCrashesAvailable()
    {
        return File.Exists("Assets/AppCenter/Plugins/Android/appcenter-crashes-release.aar");
    }

    static bool IsIOSDistributeAvailable()
    {
        return Directory.Exists("Assets/AppCenter/Plugins/iOS/Distribute");
    }

    static bool IsIOSPushAvailable()
    {
        return Directory.Exists("Assets/AppCenter/Plugins/iOS/Push");
    }

    static bool IsIOSAnalyticsAvailable()
    {
        return Directory.Exists("Assets/AppCenter/Plugins/iOS/Analytics");
    }

    static bool IsIOSCrashesAvailable()
    {
        return Directory.Exists("Assets/AppCenter/Plugins/iOS/Crashes");
    }
}
