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
    [AppSectet("iOS App Secret")]
    public string iOSAppSecret = "ios-app-secret";
    [AppSectet]
    public string AndroidAppSecret = "android-app-secret";
    [AppSectet]
    public string UWPAppSecret = "uwp-app-secret";

    [Header("Modules")]
    [Tooltip("Mobile Center Analytics helps you understand user behavior and customer engagement to improve your app. " +
             "The SDK automatically captures session count, device properties like model, OS version, etc. " +
             "You can define your own custom events to measure things that matter to you. " +
             "All the information captured is available in the Mobile Center portal for you to analyze the data.")]
    public bool UseAnalytics = true;
    [Tooltip("Mobile Center Crashes will automatically generate a crash log every time your app crashes. " +
             "The log is first written to the device's storage and when the user starts the app again, " +
             "the crash report will be sent to Mobile Center. Collecting crashes works for both beta and live apps, " +
             "i.e. those submitted to the App Store. Crash logs contain valuable information for you to help fix the crash.")]
    public bool UseCrashes = true;
    [Tooltip("Mobile Center Distribute will let your users install a new version of the app when you distribute it via the Mobile Center. " +
             "With a new version of the app available, the SDK will present an update dialog to the users to either download or postpone the new version. " +
             "Once they choose to update, the SDK will start to update your application.\nThis feature will NOT work if your app is deployed to the app store.")]
    public bool UseDistribute = true;
    [Tooltip("Mobile Center Push enables you to send push notifications to users of your app from the Mobile Center portal. " +
             "To do that, the Mobile Center SDK and portal integrate with Firebase Cloud Messaging. " +
             "You can also segment your user base based on a set of properties and send them targeted notifications.")]
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
