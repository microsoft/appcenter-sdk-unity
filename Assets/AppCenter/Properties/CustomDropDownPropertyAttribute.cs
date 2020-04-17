// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class CustomDropDownPropertyAttribute : PropertyAttribute
{
    public CustomDropDownPropertyAttribute(string _key, int _value)
    {
        SelectedKey = _key;
        SelectedValue = _value;
    }

    public CustomDropDownPropertyAttribute()
    { }

    public string SelectedKey;
    public int SelectedValue;
}
