// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AppCenterSettingsAdvanced))]
public class AppCenterSettingsEditorAdvanced : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("TransmissionTargetToken"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("AppCenterStartupType"));
        serializedObject.ApplyModifiedProperties();
    }
}
