// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class AppSectetAttribute : PropertyAttribute
{
    public string Name { get; set; }
    
    public AppSectetAttribute()
    {
    }

    public AppSectetAttribute(string name)
    {
        Name = name;
    }
}
