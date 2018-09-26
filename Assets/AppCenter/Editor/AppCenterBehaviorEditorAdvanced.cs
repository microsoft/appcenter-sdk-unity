// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEditor;

[CustomEditor(typeof(AppCenterBehaviorAdvanced))]
public class AppCenterBehaviorEditorAdvanced : Editor
{
    private Editor settingsEditorAdvanced;

    public override void OnInspectorGUI()
    {
        // Load or create settings.
        var behaviour = (AppCenterBehaviorAdvanced) target;
        if (behaviour.settingsAdvanced == null)
        {
            behaviour.settingsAdvanced = AppCenterSettingsContext.SettingsInstanceAdvanced;
        }
        
        // Draw settings.
        if (settingsEditorAdvanced == null)
        {
            settingsEditorAdvanced = CreateEditor(behaviour.settingsAdvanced);
        }
        settingsEditorAdvanced.OnInspectorGUI();
    }
}
