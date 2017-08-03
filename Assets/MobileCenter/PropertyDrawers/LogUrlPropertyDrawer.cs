// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LogUrlProperty))]
public class LogUrlPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Though the property may have double height, each child should have
        // half that height
        position.height = EditorGUIUtility.singleLineHeight;
        property.Next(true);
        DrawToggle(position, property, label);
        if (property.boolValue)
        {
            property.Next(true);
            DrawTextField(position, property, label);  
        }
    }
    private void DrawToggle(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        label.text = "Use Custom Log URL";
        var prefixRect = EditorGUI.PrefixLabel(position, label);
        EditorGUI.BeginChangeCheck();
        var newBool = EditorGUI.Toggle(prefixRect, property.boolValue);
        var didChange = EditorGUI.EndChangeCheck();

        // Only assign the value back if it was actually changed by the user.
        // Otherwise a single value will be assigned to all objects when multi-object editing,
        // even when the user didn't touch the control.
        if (didChange)
        {
            property.boolValue = newBool;
        }
        EditorGUI.EndProperty();
    }

    private void DrawTextField(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position.y += EditorGUIUtility.singleLineHeight;
        label.text = "Custom Log URL";
        EditorGUI.BeginChangeCheck();
        var newString = EditorGUI.TextField(position, label, property.stringValue);
        var didChange = EditorGUI.EndChangeCheck();

        // Only assign the value back if it was actually changed by the user.
        if (didChange)
        {
            property.stringValue = newString;
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // If "set custom log url" is true, need to make room for the text field
        property.Next(true);
        var defaultHeight = base.GetPropertyHeight(property, label);
        return property.boolValue ? 2.0f * defaultHeight : defaultHeight;
    }
}

#endif
