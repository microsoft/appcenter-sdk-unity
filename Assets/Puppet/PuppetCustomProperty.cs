// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Globalization;
using Microsoft.AppCenter.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PuppetCustomProperty : MonoBehaviour
{
    public InputField Key;
    public Dropdown Type;
    public InputField Value;
    public Toggle Boolean;

    public void Set(CustomProperties properties)
    {
        switch (Type.value)
        {
            case 0: // Clear
                properties.Clear(Key.text);
                break;
            case 1: // Boolean
                properties.Set(Key.text, Boolean.isOn);
                break;
            case 2: // Number
                if (Value.text.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                {
                    properties.Set(Key.text, float.Parse(Value.text));
                }
                else
                {
                    properties.Set(Key.text, int.Parse(Value.text));
                }
                break;
            case 3: // DateTime
                properties.Set(Key.text, DateTime.Parse(Value.text));
                break;
            case 4: // String
                properties.Set(Key.text, Value.text);
                break;
        }
    }

    public void SetType(int type)
    {
        switch (type)
        {
            case 0: // Clear
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(false);
                break;
            case 1: // Boolean
                Boolean.gameObject.SetActive(true);
                Value.gameObject.SetActive(false);
                break;
            case 2: // Number
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(true);
                Value.contentType = InputField.ContentType.DecimalNumber;
                break;
            case 3: // DateTime
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(true);
                Value.contentType = InputField.ContentType.Standard;
                break;
            case 4: // String
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(true);
                Value.contentType = InputField.ContentType.Alphanumeric;
                break;
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
