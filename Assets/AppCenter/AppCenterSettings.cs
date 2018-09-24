// Copyright (c) Microsoft Corporation. All rights reserved.
//
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
    public string iOSAppSecret = "ios-app-secret";
    [AppSecret]
    public string AndroidAppSecret = "android-app-secret";
    [AppSecret]
    public string UWPAppSecret = "uwp-app-secret";

    [Tooltip("App Center Analytics helps you understand user behavior and customer engagement to improve your app.")]
    public bool UseAnalytics = true;
    [Tooltip("App Center Crashes will automatically generate a crash log every time your app crashes.")]
    public bool UseCrashes = true;
    [Tooltip("App Center Distribute will let your users install a new version of the app when you distribute it via the App Center.")]
    public bool UseDistribute = true;
    public CustomUrlProperty CustomApiUrl = new CustomUrlProperty("API");
    public CustomUrlProperty CustomInstallUrl = new CustomUrlProperty("Install");

    [Tooltip("App Center Push enables you to send push notifications to users of your app from the App Center portal.")]
    public bool UsePush = true;
    [Tooltip("By default, App Center Push disables Firebase Analytics. Use this option to enable it. This only applies to Android applications.")]
    public bool EnableFirebaseAnalytics = false;
    public LogLevel InitialLogLevel = LogLevel.Info;
    public CustomUrlProperty CustomLogUrl = new CustomUrlProperty("Log");
    [Tooltip("To enable push for Android apps, you need to set the Sender ID found in the Firebase portal for your application.")]
    public string SenderId;

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
                services.Add(Analytics);
            }
            if (UseCrashes)
            {
                services.Add(Crashes);
            }
            if (UseDistribute)
            {
                services.Add(Distribute);
            }
            if (UsePush)
            {
                services.Add(Push);
            }
            return services.Where(i => i != null).ToArray();
        }
    }

    public static Type Analytics
    {
        get { return AppCenterAssembly.GetType("Microsoft.AppCenter.Unity.Analytics.Analytics"); }
    }

    public static Type Crashes
    {
        get { return AppCenterAssembly.GetType("Microsoft.AppCenter.Unity.Crashes.Crashes"); }
    }

    public static Type Distribute
    {
        get { return AppCenterAssembly.GetType("Microsoft.AppCenter.Unity.Distribute.Distribute"); }
    }

    public static Type Push
    {
        get { return AppCenterAssembly.GetType("Microsoft.AppCenter.Unity.Push.Push"); }
    }

    private static Assembly AppCenterAssembly
    {
        get
        {
#if !UNITY_EDITOR && UNITY_WSA_10_0
            return typeof(AppCenterSettings).GetTypeInfo().Assembly;
#else
            return Assembly.GetExecutingAssembly();
#endif
        }
    }
}
