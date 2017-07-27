// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using Microsoft.Azure.Mobile.Unity;

public class MobileCenterCoreBehavior : MonoBehaviour
{
    [Header("App Secrets")]
    [StringPropertyName("iOS App Secret")]
    public string iOSAppSecret = "ios-app-secret";
    public string AndroidAppSecret = "android-app-secret";
    public string UWPAppSecret = "uwp-app-secret";

    [Header("Other Setup")]
    public LogLevel InitialLogLevel = LogLevel.Info;
    [Space]
    public LogUrlProperty CustomLogUrl = new LogUrlProperty();

    public void Awake()
    {
        MobileCenter.LogLevel = InitialLogLevel;
        if (CustomLogUrl.UseCustomLogUrl)
        {
            MobileCenter.SetLogUrl(CustomLogUrl.LogUrl);
        }
        var appSecret = "ios=" + iOSAppSecret + 
                        ";android=" + AndroidAppSecret + 
                        "uwp=" + UWPAppSecret + ";";
        MobileCenter.Start(appSecret);
    }
}
