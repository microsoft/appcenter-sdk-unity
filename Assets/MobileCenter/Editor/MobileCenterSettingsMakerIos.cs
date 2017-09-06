// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using UnityEditor;
using System.IO;

public class MobileCenterSettingsMakerIos
{
    public MobileCenterSettingsMakerIos(string pathToBuiltProject)
    {
        _pathToLoaderFile = pathToBuiltProject + LoaderPathSuffix;
        _loaderFileText = File.ReadAllText(_pathToLoaderFile);
    }

    private static string LoaderPathSuffix = "/Libraries/MobileCenter/Plugins/iOS/Core/MobileCenterStarter.m";
    private static string AppSecretSearchText = "mobile-center-app-secret";
    private static string LogUrlSearchText = "custom-log-url";
    private static string LogLevelSearchText = "0/*LOG_LEVEL*/";
    private static string LogUrlToken = "MOBILE_CENTER_UNITY_USE_CUSTOM_LOG_URL";
    private static string UsePushToken = "MOBILE_CENTER_UNITY_USE_PUSH";
    private static string UseAnalyticsToken = "MOBILE_CENTER_UNITY_USE_ANALYTICS";
    private static string UseDistributeToken = "MOBILE_CENTER_UNITY_USE_DISTRIBUTE";
    private static string UseCrashesToken = "MOBILE_CENTER_UNITY_USE_CRASHES";

    private string _loaderFileText;
    private string _pathToLoaderFile;

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

    public void StartCrashesClass()
    {
        AddToken(UseCrashesToken);
    }

    public void StartPushClass()
    {
        AddToken(UsePushToken);
    }

    public void CommitSettings()
    {
        Debug.Log("Committing settings");
        File.WriteAllText(_pathToLoaderFile, _loaderFileText);
    }

    private void AddToken(string token)
    {
        var tokenText = "#define " + token + "\n";
        _loaderFileText = tokenText + _loaderFileText;
    }
}
