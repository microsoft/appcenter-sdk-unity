// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEditor;

[CustomEditor(typeof(MobileCenterBehavior))]
public class MobileCenterBehaviorEditor : Editor
{
    private Editor settingsEditor;

    public override void OnInspectorGUI()
    {
        // Load or create settings.
        var settingsPath = MobileCenterSettingsEditor.SettingsPath;
        var behaviour = (MobileCenterBehavior) target;
        if (behaviour.settings == null)
        {
            behaviour.settings = AssetDatabase.LoadAssetAtPath<MobileCenterSettings>(settingsPath);
        }
        if (behaviour.settings == null)
        {
            behaviour.settings = CreateInstance<MobileCenterSettings>();
            AssetDatabase.CreateAsset(behaviour.settings, settingsPath);
            AssetDatabase.SaveAssets();
        }

        // Draw settings.
        if (settingsEditor == null)
        {
            settingsEditor = CreateEditor(behaviour.settings);
        }
        settingsEditor.OnInspectorGUI();
    }
}
