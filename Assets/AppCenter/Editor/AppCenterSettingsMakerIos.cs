// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.IO;
using UnityEditor;

public class AppCenterSettingsMakerIos : IAppCenterSettingsMaker
{
    private static readonly string TemplateFilePath = AppCenterSettingsContext.AppCenterPath + "/Plugins/iOS/Core/AppCenterStarter.original";
    private static readonly string TargetFilePath = AppCenterSettingsContext.AppCenterPath + "/Plugins/iOS/Core/AppCenterStarter.m";
    private const string AppSecretSearchText = "appcenter-app-secret";
    private const string AllowNetworkRequestsText = "allow-network-requests";
    private const string TransmissionTargetTokenSearchText = "appcenter-transmission-target-token";
    private const string LogUrlSearchText = "custom-log-url";
    private const string LogUrlToken = "APPCENTER_UNITY_USE_CUSTOM_LOG_URL";
    private const string LogLevelSearchText = "0/*LOG_LEVEL*/";
    private const string StartupTypeSearchText = "0/*STARTUP_TYPE*/";
    private const string UseCrashesToken = "APPCENTER_UNITY_USE_CRASHES";
    private const string UseAnalyticsToken = "APPCENTER_UNITY_USE_ANALYTICS";
    private const string UseDistributeToken = "APPCENTER_UNITY_USE_DISTRIBUTE";
    private const string DistributeDisableAutomaticCheckForUpdateToken = "APPCENTER_DISTRIBUTE_DISABLE_AUTOMATIC_CHECK_FOR_UPDATE";
    private const string ApiUrlSearchText = "custom-api-url";
    private const string ApiUrlToken = "APPCENTER_UNITY_USE_CUSTOM_API_URL";
    private const string InstallUrlSearchText = "custom-install-url";
    private const string InstallUrlToken = "APPCENTER_UNITY_USE_CUSTOM_INSTALL_URL";
    private const string UseCustomMaxStorageSize = "APPCENTER_USE_CUSTOM_MAX_STORAGE_SIZE";
    private const string MaxStorageSize = "APPCENTER_MAX_STORAGE_SIZE";
    private const string UpdateTrackSearchText = "1 /*UPDATE_TRACK*/";
    private const string EnableManualSessionTrackerText = "enable-manual-session-tracker";
    
    private string _loaderFileText;
    private bool _enableDistributeForDebuggableBuild;

    public AppCenterSettingsMakerIos()
    {
        _loaderFileText = File.ReadAllText(TemplateFilePath);
    }

    public void EnableManualSessionTracker(bool isEnabled) 
    {
        _loaderFileText = _loaderFileText.Replace(EnableManualSessionTrackerText, isEnabled ? "YES" : "NO");
    }

    public void SetLogLevel(int logLevel)
    {
        _loaderFileText = _loaderFileText.Replace(LogLevelSearchText, logLevel.ToString());
    }

    public void IsAllowNetworkRequests(bool isAllowed) 
    {
        _loaderFileText = _loaderFileText.Replace(AllowNetworkRequestsText, isAllowed ? "YES" : "NO");
    }

    public void SetStartupType(int startupType)
    {
        _loaderFileText = _loaderFileText.Replace(StartupTypeSearchText, startupType.ToString());
    }

    public void SetLogUrl(string logUrl)
    {
        AddToken(LogUrlToken);
        _loaderFileText = _loaderFileText.Replace(LogUrlSearchText, logUrl);
    }

    public void SetAppSecret(AppCenterSettings settings)
    {
        _loaderFileText = _loaderFileText.Replace(AppSecretSearchText, settings.iOSAppSecret);
    }

    public void SetTransmissionTargetToken(string transmissionTargetToken)
    {
        _loaderFileText = _loaderFileText.Replace(TransmissionTargetTokenSearchText, transmissionTargetToken);
    }

    public void StartCrashesClass()
    {
        AddToken(UseCrashesToken);
    }

    public void StartDistributeClass()
    {
        if (_enableDistributeForDebuggableBuild || !EditorUserBuildSettings.development)
        {
            AddToken(UseDistributeToken);
        }
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

    public void CommitSettings()
    {
        File.WriteAllText(TargetFilePath, _loaderFileText);
    }

    public void SetMaxStorageSize(long size)
    {
        AddToken(UseCustomMaxStorageSize);
        _loaderFileText = _loaderFileText.Replace(MaxStorageSize, size.ToString());
    }

    private void AddToken(string token)
    {
        var tokenText = "#define " + token + "\n";
        _loaderFileText = tokenText + _loaderFileText;
    }

    public bool IsStartFromAppCenterBehavior(AppCenterSettingsAdvanced advancedSettings)
    {
        return advancedSettings.StartIOSNativeSDKFromAppCenterBehavior;
    }

    public bool IsAnalyticsAvailable()
    {
        return Directory.Exists(AppCenterSettingsContext.AppCenterPath + "/Plugins/iOS/Analytics");
    }

    public bool IsCrashesAvailable()
    {
        return Directory.Exists(AppCenterSettingsContext.AppCenterPath + "/Plugins/iOS/Crashes");
    }

    public bool IsDistributeAvailable()
    {
        return Directory.Exists(AppCenterSettingsContext.AppCenterPath + "/Plugins/iOS/Distribute");
    }

    public void SetShouldEnableDistributeForDebuggableBuild()
    {
        _enableDistributeForDebuggableBuild = true;
    }

    public void SetDistributeDisableAutomaticCheckForUpdate()
    {
        AddToken(DistributeDisableAutomaticCheckForUpdateToken);
    }

    public void SetUpdateTrack(int updateTrack)
    {
        _loaderFileText = _loaderFileText.Replace(UpdateTrackSearchText, updateTrack.ToString());
    }
}
