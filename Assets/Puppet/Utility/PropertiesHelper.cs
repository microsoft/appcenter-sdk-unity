// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Analytics;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesHelper
{
    public static Dictionary<string, string> GetStringProperties(RectTransform propertiesContainer)
    {
        var properties = propertiesContainer.GetComponentsInChildren<PuppetEventProperty>();
        if (properties == null || properties.Length == 0)
        {
            return null;
        }
        var eventProperties = new Dictionary<string, string>();
        foreach (var prop in properties)
        {
            prop.Set(eventProperties);
        }
        return eventProperties;
    }

    public static EventProperties GetTypedProperties(RectTransform propertiesContainer)
    {
        var properties = propertiesContainer.GetComponentsInChildren<PuppetEventProperty>();
        if (properties == null || properties.Length == 0)
        {
            return null;
        }
        var eventProperties = new EventProperties();
        foreach (var prop in properties)
        {
            prop.Set(eventProperties);
        }
        return eventProperties;
    }

    public static void AddPropertiesToPropertyConfigurator(RectTransform propertiesContainer, PropertyConfigurator configurator)
    {
        var properties = propertiesContainer.GetComponentsInChildren<PuppetEventProperty>();
        if (properties == null || properties.Length == 0)
        {
            return;
        }
        foreach (var prop in properties)
        {
            prop.Set(configurator);
        }
    }
}
