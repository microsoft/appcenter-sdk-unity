// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using UnityEngine;
using UnityEditor;
using Microsoft.AppCenter.Unity;

[CustomEditor(typeof(AppCenterSettings))]
public class AppCenterSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Draw app secrets.
        Header("App Secrets");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("iOSAppSecret"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AndroidAppSecret"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("UWPAppSecret"));

        // Draw modules.
        if (AppCenter.Analytics != null)
        {
            Header("Analytics");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UseAnalytics"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxStorageSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EnableManualSessionTracker"));
        }
        if (AppCenter.Crashes != null)
        {
            Header("Crashes");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UseCrashes"));
        }
        if (AppCenter.Distribute != null)
        {
            Header("Distribute");
            var serializedProperty = serializedObject.FindProperty("UseDistribute");
            EditorGUILayout.PropertyField(serializedProperty);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdateTrack"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AutomaticCheckForUpdate"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("EnableDistributeForDebuggableBuild"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomApiUrl"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomInstallUrl"));
        }

        // Draw other.
        Header("Other Setup");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("InitialLogLevel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("CustomLogUrl"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AllowNetworkRequests"));
        serializedObject.ApplyModifiedProperties();
    }

    private static void Header(string label)
    {
        GUILayout.Label(label, EditorStyles.boldLabel);
        GUILayout.Space(-4);
    }
}
