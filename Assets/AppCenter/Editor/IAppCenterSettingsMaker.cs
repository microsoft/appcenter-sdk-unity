// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

public interface IAppCenterSettingsMaker
{
    bool IsAnalyticsAvailable();
    bool IsCrashesAvailable();
    void IsAllowNetworkRequests(bool isAllowed);
    bool IsDistributeAvailable();
    void StartAnalyticsClass();
    void StartCrashesClass();
    void StartDistributeClass();
    void SetAppSecret(AppCenterSettings settings);
    void SetTransmissionTargetToken(string transmissionTargetToken);
    void SetLogLevel(int logLevel);
    bool IsStartFromAppCenterBehavior(AppCenterSettingsAdvanced advancedSettings);
    void SetStartupType(int startupType);
    void SetLogUrl(string logUrl);
    void SetApiUrl(string apiUrl);
    void SetInstallUrl(string installUrl);
    void SetMaxStorageSize(long size);
    void CommitSettings();
    void SetShouldEnableDistributeForDebuggableBuild();
    void SetDistributeDisableAutomaticCheckForUpdate();
    void SetUpdateTrack(int updateTrack);
    void EnableManualSessionTracker(bool isEnabled);
}
