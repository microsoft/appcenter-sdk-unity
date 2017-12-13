using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class AppCenterPreBuild : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }

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
        if (settings.UsePush)
        {
            settingsMaker.StartPushClass();
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
        settingsMaker.CommitSettings();
    }
}
