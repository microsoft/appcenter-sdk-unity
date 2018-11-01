// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Analytics;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PuppetEventProperty : MonoBehaviour
{
    public InputField Key;
    public Dropdown Type;
    public InputField Value;
    public Toggle Boolean;

    public void Set(EventProperties properties)
    {
        switch (Type.value)
        {
            case 0: // String
                properties.Set(Key.text, Value.text);
                break;
            case 1: // Long
                properties.Set(Key.text, long.Parse(Value.text));
                break;
            case 2: // Double
                properties.Set(Key.text, double.Parse(Value.text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
                break;
            case 3: // Boolean
                properties.Set(Key.text, Boolean.isOn);
                break;
            case 4: // DateTime
                properties.Set(Key.text, DateTime.Parse(Value.text));
                break;
        }
    }

    public void Set(PropertyConfigurator propertyConfigurator)
    {
        switch (Type.value)
        {
            case 0: // String
                propertyConfigurator.SetEventProperty(Key.text, Value.text);
                break;
            case 1: // Long
                propertyConfigurator.SetEventProperty(Key.text, long.Parse(Value.text));
                break;
            case 2: // Double
                propertyConfigurator.SetEventProperty(Key.text, double.Parse(Value.text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
                break;
            case 3: // Boolean
                propertyConfigurator.SetEventProperty(Key.text, Boolean.isOn);
                break;
            case 4: // DateTime
                propertyConfigurator.SetEventProperty(Key.text, DateTime.Parse(Value.text));
                break;
        }
    }

    public void Set(Dictionary<string, string> properties)
    {
        switch (Type.value)
        {
            case 0: // String
            case 1: // Long
            case 2: // Double
            case 4: // DateTime
                properties[Key.text] = Value.text;
                break;
            case 3: // Boolean
                properties[Key.text] = Boolean.isOn.ToString();
                break;
        }
    }

    public void SetType(int type)
    {
        switch (type)
        {
            case 0: // String
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(true);
                Value.contentType = InputField.ContentType.Alphanumeric;
                break;
            case 1: // Long
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(true);
                Value.contentType = InputField.ContentType.IntegerNumber;
                break;
            case 2: // Double
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(true);
                Value.contentType = InputField.ContentType.DecimalNumber;
                break;
            case 3: // Boolean
                Boolean.gameObject.SetActive(true);
                Value.gameObject.SetActive(false);
                break;
            case 4: // DateTime
                Boolean.gameObject.SetActive(false);
                Value.gameObject.SetActive(true);
                Value.contentType = InputField.ContentType.Standard;
                break;
        }
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
