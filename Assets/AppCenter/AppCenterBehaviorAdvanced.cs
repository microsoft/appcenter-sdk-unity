// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity;
using UnityEngine;
using System;
using System.Reflection;
using Microsoft.AppCenter.Unity.Internal;
using System.Linq;

[HelpURL("https://docs.microsoft.com/en-us/appcenter/sdk/crashes/unity")]
public class AppCenterBehaviorAdvanced : MonoBehaviour
{
    public static event Action InitializingServices;
    public static event Action InitializedAppCenterAndServices;
    public static event Action Started;

    private static AppCenterBehaviorAdvanced instance;

    public AppCenterSettingsAdvanced settingsAdvanced;

    private void Awake()
    {
        // Make sure that App Center have only one instance.
        if (instance != null)
        {
            Debug.LogError("App Center should have only one advanced instance!");
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initialize App Center.
        if (settingsAdvanced == null)
        {
            Debug.LogError("App Center advanced instance isn't configured!");
            return;
        }
        if (Started != null)
        {
            Started.Invoke();
        }
    }
}
