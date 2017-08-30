// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity;
using UnityEngine;
using System;

[HelpURL("https://docs.microsoft.com/en-us/mobile-center/sdk/")]
public class MobileCenterBehavior : MonoBehaviour
{
    public static event Action InitializingServices;
    public static event Action InitializedMobileCenterAndServices;
    public static event Action Started;

    private static MobileCenterBehavior instance;

    public MobileCenterSettings settings;

    private void Awake()
    {
        foreach (var service in settings.Services)
        {
            var method = service.GetMethod("PrepareEventHandlers");
            if (method != null)
            {
                method.Invoke(null, null);
            }
        }

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
#if UNITY_IOS || UNITY_ANDROID
        InvokeInitializingServices();
        InvokeInitializedServices();
#else
        InitializeMobileCenter();
#endif
    }

    private void Start()
    {
        if (Started != null)
        {
            Started.Invoke();
        }
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

    public static void InvokeInitializingServices()
    {
        if (InitializingServices != null)
        {
            InitializingServices.Invoke();
        }
    }

    public static void InvokeInitializedServices()
    {
        if (InitializedMobileCenterAndServices != null)
        {
            InitializedMobileCenterAndServices.Invoke();
        }
    }
}
