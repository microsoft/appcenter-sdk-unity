// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.


using UnityEngine;

public class StringPropertyNameAttribute : PropertyAttribute
{
    public string Name { get; set; }

	public StringPropertyNameAttribute(string name)
    {
        Name = name;
    }
}
