// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using UnityEngine;

[HelpURL("https://docs.microsoft.com/en-us/appcenter/sdk/crashes/unity")]
public class AppCenterBehaviorAdvanced : MonoBehaviour
{
    public static event Action Started;

    private static AppCenterBehaviorAdvanced _instance;

    public AppCenterSettingsAdvanced settingsAdvanced;

    private void Awake()
    {
        // Make sure that App Center have only one instance.
        if (gameObject.GetComponent("AppCenterBehavior") == null)
        {
            Debug.LogError("App Center should have the AppCenterBahvior instance attached to the game object!");
            DestroyImmediate(gameObject);
            return;
        }
        _instance = this;
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
