// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Linq;
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
            Debug.LogError("App Center Behavior Advanced should have the App Center Behavior instance attached to the same game object.");
        }
    }

    public void Reset()
    {
        if (GetAllInstances().Length > 1)
        {
            Debug.LogError("Only one game object with App Center Behaviour Advanced should exist.");
            DestroyImmediate(this);
        }
    }

    public static AppCenterBehaviorAdvanced[] GetAllInstances()
    {
        var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        return rootObjects.Select(obj => obj.GetComponentsInChildren<AppCenterBehaviorAdvanced>())
            .Aggregate((first, next) => first.Concat(next).ToArray());
    }
}
