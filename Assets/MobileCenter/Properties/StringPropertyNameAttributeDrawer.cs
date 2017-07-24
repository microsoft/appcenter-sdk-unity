// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringPropertyNameAttribute))]
public class StringPropertyNameDrawer : PropertyDrawer
{
	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
        EditorGUI.BeginProperty(position, label, property);
        StringPropertyNameAttribute stringNameAttribute = attribute as StringPropertyNameAttribute;
        label.text = stringNameAttribute.Name;
		EditorGUI.BeginChangeCheck();
		var newValue = EditorGUI.TextField(position, label, property.stringValue);
        var didChange = EditorGUI.EndChangeCheck();

		// Only assign the value back if it was actually changed by the user.
		// Otherwise a single value will be assigned to all objects when multi-object editing,
		// even when the user didn't touch the control.
        if (didChange)
        {
			property.stringValue = newValue;
		}
        EditorGUI.EndProperty();
	}
}
#endif
