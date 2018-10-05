// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;

[HelpURL("https://docs.microsoft.com/en-us/appcenter/sdk/crashes/unity")]
public class AppCenterBehaviorAdvanced : MonoBehaviour
{
    public AppCenterSettingsAdvanced SettingsAdvanced;

    private void Awake()
    {
        // Make sure that App Center have the default behavior attached.
        if (gameObject.GetComponent<AppCenterBehavior>() == null)
        {
            Debug.LogError("App Center should have the AppCenterBehavior instance attached to the game object!");
        }
    }
}
