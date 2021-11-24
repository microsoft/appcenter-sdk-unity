// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AppCenter.Unity;
using UnityEngine;

[Serializable]
public class AppCenterSettings : ScriptableObject
{
    [AppSecret("iOS App Secret")]
    public string iOSAppSecret = "";

    [AppSecret]
    public string AndroidAppSecret = "";

    [AppSecret]
    public string UWPAppSecret = "";

    [Tooltip("App Center Analytics helps you understand user behavior and customer engagement to improve your app.")]
    public bool UseAnalytics = true;

    [Tooltip("App Center Crashes will automatically generate a crash log every time your app crashes.")]
    public bool UseCrashes = true;

    [Tooltip("App Center Distribute will let your users install a new version of the app when you distribute it via the App Center.")]
    public bool UseDistribute = true;

    public CustomUrlProperty CustomApiUrl = new CustomUrlProperty("API");

    public CustomUrlProperty CustomInstallUrl = new CustomUrlProperty("Install");

    [Tooltip("By default, App Center Distribute is disabled for debuggable builds. Check this to enable it.")]
    public bool EnableDistributeForDebuggableBuild = false;

    [Tooltip("By default, App Center Distribute checks for update at application startup automatically. Uncheck this to check for updates manually instead.")]
    public bool AutomaticCheckForUpdate = true;

    public LogLevel InitialLogLevel = LogLevel.Info;

    [Tooltip("By default, the network requests is allowed. Uncheck this to disallow network requests.")]
    public bool AllowNetworkRequests = true;
    
    [Tooltip("By default, the manual session tracker is disabled.")]
    public bool EnableManualSessionTracker = false;

    [CustomDropDownProperty("Public", 1)]
    [CustomDropDownProperty("Private", 2)]
    public int UpdateTrack;

    public CustomUrlProperty CustomLogUrl = new CustomUrlProperty("Log");

    public MaxStorageSizeProperty MaxStorageSize = new MaxStorageSizeProperty();

    public string AppSecret
    {
        get
        {
            var appSecrets = new Dictionary<string, string>
            {
                { "ios", iOSAppSecret },
                { "android", AndroidAppSecret },
                { "uwp", UWPAppSecret }
            };
            return string.Concat(appSecrets
                .Where(i => !string.IsNullOrEmpty(i.Value))
                .Select(i => string.Format("{0}={1};", i.Key, i.Value))
                .ToArray());
        }
    }

    public Type[] Services
    {
        get
        {
            var services = new List<Type>();
            if (UseAnalytics)
            {
                services.Add(AppCenter.Analytics);
            }
            if (UseCrashes)
            {
                services.Add(AppCenter.Crashes);
            }
            if (UseDistribute)
            {
                services.Add(AppCenter.Distribute);
            }
            return services.Where(i => i != null).ToArray();
        }
    }
}
