// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity;
using UnityEngine;

[HelpURL("https://docs.microsoft.com/en-us/mobile-center/sdk/")]
public class MobileCenterBehavior : MonoBehaviour
{
    private static MobileCenterBehavior instance;

    public MobileCenterSettings settings;

    private void Awake()
    {
        // Make sure that Mobile Center have only one instance.
        if (instance != null)
        {
            Debug.LogError("Mobile Center should have only one instance!");
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize Mobile Center.
        if (settings == null)
        {
            Debug.LogError("Mobile Center isn't configured!");
            return;
        }
        InitializeMobileCenter();
    }

    private void InitializeMobileCenter()
    {
        MobileCenter.LogLevel = settings.InitialLogLevel;
        if (settings.CustomLogUrl.UseCustomLogUrl)
        {
            MobileCenter.SetLogUrl(settings.CustomLogUrl.LogUrl);
        }
        MobileCenter.Start(settings.AppSecret, settings.Services);
    }
}

