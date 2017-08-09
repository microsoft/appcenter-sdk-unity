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
        // Create settings.
        var behaviour = (MobileCenterBehavior) target;
        if (behaviour.settings == null)
        {
            behaviour.settings = CreateInstance<MobileCenterSettings>();
            AssetDatabase.CreateAsset(behaviour.settings, "Assets/MobileCenter/MobileCenterSettings.asset");
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
