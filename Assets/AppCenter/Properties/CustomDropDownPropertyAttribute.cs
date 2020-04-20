// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class CustomDropDownPropertyAttribute : PropertyAttribute
{
    public CustomDropDownPropertyAttribute(string key, int value)
    {
        SelectedKey = key;
        SelectedValue = value;
    }

    public CustomDropDownPropertyAttribute()
    {
    }

    public string SelectedKey { get; private set; }
    public int SelectedValue { get; private set; }
}
