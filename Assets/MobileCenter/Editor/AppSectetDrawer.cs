// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AppSectetAttribute))]
public class AppSectetDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var name = ((AppSectetAttribute) attribute).Name;
        if (!string.IsNullOrEmpty(name))
        {
            label.text = name;
        }
        EditorGUI.PropertyField(position, property, label);
    }
}
