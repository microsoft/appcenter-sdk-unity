using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class AppCenterPreBuild : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        if (report.summary.platform == BuildTarget.Android)
        {
            AddStartupCodeToAndroid();
        }
        else if (report.summary.platform == BuildTarget.iOS)
        {
            AddStartupCodeToiOS();
        }
    }

    void AddStartupCodeToAndroid()
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
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
        if (settings.UsePush)
        {
            settingsMaker.StartPushClass();
            if (settings.EnableFirebaseAnalytics)
            {
                settingsMaker.EnableFirebaseAnalytics();
            }
        }
        if (settings.UseAnalytics)
        {
            settingsMaker.StartAnalyticsClass();
        }
        if (settings.UseDistribute)
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
        settingsMaker.CommitSettings();
    }

    static void AddStartupCodeToiOS()
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
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
        if (settings.UsePush && IsPushAvailable())
        {
            settingsMaker.StartPushClass();
        }
        if (settings.UseAnalytics && IsAnalyticsAvailable())
        {
            settingsMaker.StartAnalyticsClass();
        }
        if (settings.UseDistribute && IsDistributeAvailable())
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
        settingsMaker.CommitSettings();
    }

    static bool IsDistributeAvailable()
    {
        return Directory.Exists("Assets/AppCenter/Plugins/iOS/Distribute");
    }

    static bool IsPushAvailable()
    {
        return Directory.Exists("Assets/AppCenter/Plugins/iOS/Push");
    }

    static bool IsAnalyticsAvailable()
    {
        return Directory.Exists("Assets/AppCenter/Plugins/iOS/Analytics");
    }
}
