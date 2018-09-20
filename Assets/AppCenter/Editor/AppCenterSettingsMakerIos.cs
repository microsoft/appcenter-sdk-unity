// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.IO;

public class AppCenterSettingsMakerIos
{
    private const string TemplateFilePath = "Assets/AppCenter/Plugins/iOS/Core/AppCenterStarter.original";
    private const string TargetFilePath = "Assets/AppCenter/Plugins/iOS/Core/AppCenterStarter.m";
    private const string AppSecretSearchText = "appcenter-app-secret";
    private const string TransmissionTargetSearchText = "appcenter-transmission-target";
    private const string ChildTransmissionTargetSearchText = "appcenter-child-transmission-target";
    private const string LogUrlSearchText = "custom-log-url";
    private const string LogUrlToken = "APPCENTER_UNITY_USE_CUSTOM_LOG_URL";
    private const string LogLevelSearchText = "0/*LOG_LEVEL*/";
    private const string UseCrashesToken = "APPCENTER_UNITY_USE_CRASHES";
    private const string UsePushToken = "APPCENTER_UNITY_USE_PUSH";
    private const string UseAnalyticsToken = "APPCENTER_UNITY_USE_ANALYTICS";
    private const string UseDistributeToken = "APPCENTER_UNITY_USE_DISTRIBUTE";
    private const string ApiUrlSearchText = "custom-api-url";
    private const string ApiUrlToken = "APPCENTER_UNITY_USE_CUSTOM_API_URL";
    private const string InstallUrlSearchText = "custom-install-url";
    private const string InstallUrlToken = "APPCENTER_UNITY_USE_CUSTOM_INSTALL_URL";

    private string _loaderFileText;

    public AppCenterSettingsMakerIos()
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

    public void SetTransmissionTarget(string transmissionTarget)
    {
        _loaderFileText = _loaderFileText.Replace(TransmissionTargetSearchText, transmissionTarget);
    }

    public void SetChildTransmissionTarget(string childTransmissionTarget)
    {
        _loaderFileText = _loaderFileText.Replace(ChildTransmissionTargetSearchText, childTransmissionTarget);
    }

    public void StartCrashesClass()
    {
        AddToken(UseCrashesToken);
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
