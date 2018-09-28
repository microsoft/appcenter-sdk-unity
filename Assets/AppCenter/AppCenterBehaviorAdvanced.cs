// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using UnityEngine;

[HelpURL("https://docs.microsoft.com/en-us/appcenter/sdk/crashes/unity")]
public class AppCenterBehaviorAdvanced : MonoBehaviour
{
    public AppCenterSettingsAdvanced SettingsAdvanced;

    private void Awake()
    {
        // Make sure that App Center have only one instance.
        if (gameObject.GetComponent<AppCenterBehavior>() == null)
        {
            Debug.LogError("App Center should have the AppCenterBehavior instance attached to the game object!");
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Initialize App Center.
        if (SettingsAdvanced == null)
        {
            Debug.LogError("App Center advanced instance isn't configured!");
            return;
        }
    }
}
