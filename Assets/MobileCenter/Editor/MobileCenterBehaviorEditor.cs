// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEditor;

[CustomEditor(typeof(MobileCenterBehavior))]
public class MobileCenterBehaviorEditor : Editor
{
    private const string SettingsPath = "Assets/MobileCenter/MobileCenterSettings.asset";

    private Editor settingsEditor;

    public override void OnInspectorGUI()
    {
        // Load or create settings.
        var behaviour = (MobileCenterBehavior) target;
        if (behaviour.settings == null)
        {
            behaviour.settings = AssetDatabase.LoadAssetAtPath<MobileCenterSettings>(SettingsPath);
        }
        if (behaviour.settings == null)
        {
            behaviour.settings = CreateInstance<MobileCenterSettings>();
            AssetDatabase.CreateAsset(behaviour.settings, SettingsPath);
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
