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
