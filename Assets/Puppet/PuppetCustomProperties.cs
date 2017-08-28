// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity;
using UnityEngine;

public class PuppetCustomProperties : MonoBehaviour
{
    public GameObject CustomProperty;
    public RectTransform PropertiesList;
    
    public void AddProperty()
    {
        var property = Instantiate(CustomProperty);
        property.transform.SetParent(PropertiesList, false);
    }

    public void Send()
    {
        MobileCenter.SetCustomProperties(GetProperties());
    }

    private CustomProperties GetProperties()
    {
        var properties = PropertiesList.GetComponentsInChildren<PuppetCustomProperty>();
        if (properties == null || properties.Length == 0)
        {
            return null;
        }
        var result = new CustomProperties();
        foreach (var i in properties)
        {
            i.Set(result);
        }
        return result;
    }
}
