// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomDropDownPropertyAttribute))]
public class CustomDropDownPropertyDrawer : PropertyDrawer
{
    bool initialized = false;
    object[] attributes = null;
    Dictionary<string, int> optionsDictionary = new Dictionary<string, int>();
    string[] options = null;
    int selectedIndex = 0;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!initialized)
        {
            attributes = fieldInfo.GetCustomAttributes(typeof(CustomDropDownPropertyAttribute), false);
            foreach (var itemAttribute in attributes)
            {
                var customPropertyAttribute = itemAttribute as CustomDropDownPropertyAttribute;
                optionsDictionary.Add(customPropertyAttribute.SelectedKey, customPropertyAttribute.SelectedValue);
                if (customPropertyAttribute.SelectedValue == AppCenterSettingsContext.SettingsInstance.UpdateTrack)
                {
                    selectedIndex = ArrayUtility.IndexOf(attributes, itemAttribute);
                }
            }
            options = optionsDictionary.Keys.ToArray();
            initialized = true;
        }

        selectedIndex = EditorGUI.Popup(position, property.displayName, selectedIndex, options);
        property.intValue = optionsDictionary[options[selectedIndex]];
    }
}
