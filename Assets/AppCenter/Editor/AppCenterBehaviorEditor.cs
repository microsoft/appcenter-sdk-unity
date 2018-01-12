// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(AppCenterBehavior))]
public class AppCenterBehaviorEditor : Editor
{
    private Editor settingsEditor;

    public override void OnInspectorGUI()
    {
        // Load or create settings.
        var behaviour = (AppCenterBehavior) target;
        if (behaviour.settings == null)
        {
            behaviour.settings = AppCenterSettingsContext.SettingsInstance;
        }
        
        // Draw settings.
        if (settingsEditor == null)
        {
            settingsEditor = CreateEditor(behaviour.settings);
        }
        settingsEditor.OnInspectorGUI();
    }
}
#endif
