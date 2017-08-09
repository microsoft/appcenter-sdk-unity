// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

[CustomPropertyDrawer(typeof(AppSectetAttribute))]
public class AppSectetDrawer : PropertyDrawer
{
    private const string Pattern = "^[A-Za-z0-9-]*$";
    private const int HelpHeight = 38;
    private const int TextHeight = 16;
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var height = base.GetPropertyHeight(property, label);
        if (!IsValid(property))
        {
            height += HelpHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        return height;
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var textFieldPosition = position;
        textFieldPosition.height = TextHeight;
        var name = ((AppSectetAttribute) attribute).Name;
        if (!string.IsNullOrEmpty(name))
        {
            label.text = name;
        }
        else
        {
            name = label.text;
        }
        EditorGUI.PropertyField(textFieldPosition, property, label);

        if (!IsValid(property))
        {
            var helpPosition = EditorGUI.IndentedRect(position);
            helpPosition.y += TextHeight;
            helpPosition.height = HelpHeight;
            EditorGUI.HelpBox(helpPosition, name + " is invalid!", MessageType.Warning);
        }
    }

    // Test if the propertys string value matches the regex pattern.
    private bool IsValid(SerializedProperty property)
    {
        return Regex.IsMatch(property.stringValue, Pattern);
    }
}
