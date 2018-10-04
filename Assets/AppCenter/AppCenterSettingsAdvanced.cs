// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Reflection;
using Microsoft.AppCenter.Unity;
using UnityEngine;

[Serializable]
public class AppCenterSettingsAdvanced : ScriptableObject
{
    [AppSecret("Transmission target token")]
    public string TransmissionTargetToken = "transmission-target-token";

    [Tooltip("Configure the way App Center is started. For more info on startup types refer to the documentation.")]
    public StartupType AppCenterStartupType = StartupType.AppCenter;

    [Tooltip("Start Android and iOS native SDKs from AppCenterBehavior script instead of the native plugins. This option does not affect UWP apps.")]
    public bool StartFromAppCenterBehavior = false;

    private static Assembly AppCenterAssembly
    {
        get
        {
#if !UNITY_EDITOR && UNITY_WSA_10_0
            return typeof(AppCenterSettingsAdvanced).GetTypeInfo().Assembly;
#else
            return Assembly.GetExecutingAssembly();
#endif
        }
    }
}
