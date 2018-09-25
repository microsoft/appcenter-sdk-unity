// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AppCenter.Unity.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class PuppetTransmission : MonoBehaviour
{
    public Toggle TransmissionEnabled;
    public Toggle ChildTransmissionEnabled;
    public InputField EventName;
    public InputField TransmissionTarget;
    public InputField ChildTransmissionTarget;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;
    private string TransmissionTargetToken = "2932e5d1bff54f62935e7352e98a7819-59ba431f-6bf7-44f5-a1d3-a24a6f6dd544-7075";
    private string ChildTransmissionTargetToken = "1f7bbd10a32c4f8e9d545a7a4b2f4b6a-e6f4d0c5-2fc2-4024-bfb6-56d2d676b5c0-8020";

    private void OnEnable()
    {
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        transmissionTarget.IsEnabledAsync().ContinueWith(task =>
        {
            TransmissionEnabled.isOn = task.Result;
        });
    }

    private string ResolveToken() {
        if (string.IsNullOrEmpty(TransmissionTarget.text)) {
            return TransmissionTargetToken;
        } else {
            return TransmissionTarget.text;
        }
    }

    private string ResolveChildToken()
    {
        if (string.IsNullOrEmpty(ChildTransmissionTarget.text))
        {
            return ChildTransmissionTargetToken;
        }
        else
        {
            return ChildTransmissionTarget.text;
        }
    }

    public void SetTransmissionEnabled(bool enabled)
    {
        StartCoroutine(SetTransmissionEnabledCoroutine(enabled));
    }

    public void SetChildTransmissionEnabled(bool enabled)
    {
        StartCoroutine(SetChildTransmissionEnabledCoroutine(enabled));
    }

    private IEnumerator SetTransmissionEnabledCoroutine(bool enabled)
    {
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        yield return transmissionTarget.SetEnabledAsync(enabled);
        var isEnabled = transmissionTarget.IsEnabledAsync();
        yield return isEnabled;
        TransmissionEnabled.isOn = isEnabled.Result;
    }

    private IEnumerator SetChildTransmissionEnabledCoroutine(bool enabled)
    {
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        TransmissionTarget childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
        yield return childTransmissionTarget.SetEnabledAsync(enabled);
        var isEnabled = childTransmissionTarget.IsEnabledAsync();
        yield return isEnabled;
        ChildTransmissionEnabled.isOn = isEnabled.Result;
    }

    public void AddProperty()
    {
        var property = Instantiate(EventProperty);
        property.transform.SetParent(EventPropertiesList, false);
    }

    public void TrackEventChildTransmission()
    {
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        TransmissionTarget childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
        Dictionary<string, string> properties = GetProperties();
        if (properties == null)
        {
            childTransmissionTarget.TrackEvent(EventName.text);
        }
        else
        {
            childTransmissionTarget.TrackEventWithProperties(EventName.text, GetProperties());
        }
    }

    public void TrackEventTransmission() 
    { 
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        Dictionary<string, string> properties = GetProperties();
        if (properties == null) 
        {
            transmissionTarget.TrackEvent(EventName.text);
        }
        else
        {
            transmissionTarget.TrackEventWithProperties(EventName.text, properties);
        }
    }

    private Dictionary<string, string> GetProperties()
    {
        var properties = EventPropertiesList.GetComponentsInChildren<PuppetEventProperty>();
        if (properties == null || properties.Length == 0)
        {
            return null;
        }
        return properties.ToDictionary(i => i.Key.text, i => i.Value.text);
    }
}
