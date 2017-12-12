// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections.Generic;
using System.IO;

public class AppCenterSettingsMakerAndroid
{
    private const string AppCenterResourcesFolderPath = "Assets/Plugins/Android/res/values/";
    private const string AppCenterResourcesPath = AppCenterResourcesFolderPath + "appcenter-settings.xml";
    private const string AppSecretKey = "appcenter_app_secret";
    private const string CustomLogUrlKey = "appcenter_custom_log_url";
    private const string UseCustomLogUrlKey = "appcenter_use_custom_log_url";
    private const string InitialLogLevelKey = "appcenter_initial_log_level";
    private const string UsePushKey = "appcenter_use_push";
    private const string SenderIdKey = "appcenter_sender_id";
    private const string UseAnalyticsKey = "appcenter_use_analytics";
    private const string UseDistributeKey = "appcenter_use_distribute";
    private const string CustomApiUrlKey = "appcenter_custom_api_url";
    private const string UseCustomApiUrlKey = "appcenter_use_custom_api_url";
    private const string CustomInstallUrlKey = "appcenter_custom_install_url";
    private const string UseCustomInstallUrlKey = "appcenter_use_custom_install_url";
    private const string EnableFirebaseAnalyticsKey = "appcenter_enable_firebase_analytics";

    private readonly IDictionary<string, string> _resourceValues = new Dictionary<string, string>();

    static AppCenterSettingsMakerAndroid()
    {
        if (!Directory.Exists(AppCenterResourcesFolderPath))
        {
            Directory.CreateDirectory(AppCenterResourcesFolderPath);
        }
    }

    public void SetLogLevel(int logLevel)
    {
        _resourceValues[InitialLogLevelKey] = logLevel.ToString();
    }

    public void SetLogUrl(string logUrl)
    {
        _resourceValues[CustomLogUrlKey] = logUrl;
        _resourceValues[UseCustomLogUrlKey] = true.ToString();
    }

    public void SetAppSecret(string appSecret)
    {
        _resourceValues[AppSecretKey] = appSecret;
    }

    public void SetSenderId(string senderId)
    {
        _resourceValues[SenderIdKey] = senderId;
    }

    public void EnableFirebaseAnalytics()
    {
        _resourceValues[EnableFirebaseAnalyticsKey] = true.ToString();
    }

    public void StartPushClass()
    {
        _resourceValues[UsePushKey] = true.ToString();
    }

    public void StartAnalyticsClass()
    {
        _resourceValues[UseAnalyticsKey] = true.ToString();
    }

    public void StartDistributeClass()
    {
        _resourceValues[UseDistributeKey] = true.ToString();
    }

    public void SetApiUrl(string apiUrl)
    {
        _resourceValues[CustomApiUrlKey] = apiUrl;
        _resourceValues[UseCustomApiUrlKey] = true.ToString();
    }

    public void SetInstallUrl(string installUrl)
    {
        _resourceValues[CustomInstallUrlKey] = installUrl;
        _resourceValues[UseCustomInstallUrlKey] = true.ToString();
    }

    public void CommitSettings()
    {
        if (File.Exists(AppCenterResourcesPath))
        {
            File.Delete(AppCenterResourcesPath);
        }
        XmlResourceHelper.WriteXmlResource(AppCenterResourcesPath, _resourceValues);
    }
}
