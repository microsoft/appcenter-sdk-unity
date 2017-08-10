// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MobileCenterSettings))]
public class MobileCenterSettingsEditor : Editor
{
    public const string SettingsPath = "Assets/MobileCenter/MobileCenterSettings.asset";

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw app secrets.
        Header("App Secrets");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("iOSAppSecret"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AndroidAppSecret"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("UWPAppSecret"));

        // Draw modules.
        Header("Modules");
        if (MobileCenterSettings.Analytics != null)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UseAnalytics"));
        }
        if (MobileCenterSettings.Crashes != null)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UseCrashes"));
        }
        if (MobileCenterSettings.Distribute != null)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UseDistribute"));
        }
        if (MobileCenterSettings.Push != null)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UsePush"));
        }

        // Draw other.
        Header("Other Setup");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("InitialLogLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomLogUrl"));
        
        serializedObject.ApplyModifiedProperties();
    }

    private static void Header(string label)
    {
        GUILayout.Label(label, EditorStyles.boldLabel);
        GUILayout.Space(-4);
    }
}
