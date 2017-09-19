// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.IO;

public class MobileCenterSettingsMakerIos
{
    private const string TemplateFilePath = "Assets/MobileCenter/Plugins/iOS/Core/MobileCenterStarter.original";
    private const string TargetFilePath = "Assets/MobileCenter/Plugins/iOS/Core/MobileCenterStarter.m";
    private const string AppSecretSearchText = "mobile-center-app-secret";
    private const string LogUrlSearchText = "custom-log-url";
    private const string LogUrlToken = "MOBILE_CENTER_UNITY_USE_CUSTOM_LOG_URL";
    private const string LogLevelSearchText = "0/*LOG_LEVEL*/";
    private const string UsePushToken = "MOBILE_CENTER_UNITY_USE_PUSH";
    private const string UseAnalyticsToken = "MOBILE_CENTER_UNITY_USE_ANALYTICS";
    private const string UseDistributeToken = "MOBILE_CENTER_UNITY_USE_DISTRIBUTE";
    private const string ApiUrlSearchText = "custom-api-url";
    private const string ApiUrlToken = "MOBILE_CENTER_UNITY_USE_CUSTOM_API_URL";
    private const string InstallUrlSearchText = "custom-install-url";
    private const string InstallUrlToken = "MOBILE_CENTER_UNITY_USE_CUSTOM_INSTALL_URL";

    private string _loaderFileText;

    public MobileCenterSettingsMakerIos()
    {
        _loaderFileText = File.ReadAllText(TemplateFilePath);
    }

    public void SetLogLevel(int logLevel)
    {
        _loaderFileText = _loaderFileText.Replace(LogLevelSearchText, logLevel.ToString());
    }

    public void SetLogUrl(string logUrl)
    {
        AddToken(LogUrlToken);
        _loaderFileText = _loaderFileText.Replace(LogUrlSearchText, logUrl);
    }

    public void SetAppSecret(string appSecret)
    {
        _loaderFileText = _loaderFileText.Replace(AppSecretSearchText, appSecret);
    }

    public void StartDistributeClass()
    {
        AddToken(UseDistributeToken);
    }

    public void StartAnalyticsClass()
    {
        AddToken(UseAnalyticsToken);
    }

    public void SetApiUrl(string apiUrl)
    {
        AddToken(ApiUrlToken);
        _loaderFileText = _loaderFileText.Replace(ApiUrlSearchText, apiUrl);
    }

    public void SetInstallUrl(string installUrl)
    {
        AddToken(InstallUrlToken);
        _loaderFileText = _loaderFileText.Replace(InstallUrlSearchText, installUrl);
    }

    public void StartPushClass()
    {
        AddToken(UsePushToken);
    }

    public void CommitSettings()
    {
        File.WriteAllText(TargetFilePath, _loaderFileText);
    }

    private void AddToken(string token)
    {
        var tokenText = "#define " + token + "\n";
        _loaderFileText = tokenText + _loaderFileText;
    }
}
