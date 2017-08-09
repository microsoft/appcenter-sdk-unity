// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Mobile.Unity;
using Microsoft.Azure.Mobile.Unity.Analytics;
using Microsoft.Azure.Mobile.Unity.Crashes;
using Microsoft.Azure.Mobile.Unity.Distribute;
using Microsoft.Azure.Mobile.Unity.Push;
using UnityEngine;

[Serializable]
public class MobileCenterSettings : ScriptableObject
{
    [Header("App Secrets")]
    [StringPropertyName("iOS App Secret")]
    public string iOSAppSecret = "ios-app-secret";
    public string AndroidAppSecret = "android-app-secret";
    public string UWPAppSecret = "uwp-app-secret";

    [Header("Modules")]
    public bool UseAnalytics = true;
    public bool UseCrashes = true;
    public bool UseDistribute = true;
    public bool UsePush = true;

    [Header("Other Setup")]
    public LogLevel InitialLogLevel = LogLevel.Info;
    [Space]
    public LogUrlProperty CustomLogUrl = new LogUrlProperty();

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
                services.Add(typeof(Analytics));
            }
            if (UseCrashes)
            {
                services.Add(typeof(Crashes));
            }
            if (UseDistribute)
            {
                services.Add(typeof(Distribute));
            }
            if (UsePush)
            {
                services.Add(typeof(Push));
            }
            return services.ToArray();
        }
    }
}
