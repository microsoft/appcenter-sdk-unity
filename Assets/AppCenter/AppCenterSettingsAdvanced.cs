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
public class AppCenterSettingsAdvanced : ScriptableObject
{
    [AppSecret("Transmission target token")]
    public string TransmissionTargetToken = "";

    [Tooltip("Configure the way App Center is started. For more info on startup types refer to the documentation.")]
    public StartupType AppCenterStartupType = StartupType.Both;

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
