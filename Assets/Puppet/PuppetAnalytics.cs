// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AppCenter.Unity.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class PuppetAnalytics : MonoBehaviour
{
    public Toggle Enabled;
    public Toggle TransmissionEnabled;
    public InputField EventName;
    public InputField TransmissionTarget;
    public InputField ChildTransmissionTarget;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;
    private string TransmissionTargetToken = "";
    private string ChildTransmissionTargetToken = "";

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

    void OnEnable()
    {
        Analytics.IsEnabledAsync().ContinueWith(task =>
        {
            Enabled.isOn = task.Result;
        });

        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        transmissionTarget.IsEnabledAsync().ContinueWith(task => 
        {
            TransmissionEnabled.isOn = task.Result;
        });
    }

    public void SetEnabled(bool enabled)
    {
        StartCoroutine(SetEnabledCoroutine(enabled));
    }

    public void SetTransmissionEnabled(bool enabled)
    {
        StartCoroutine(SetTransmissionEnabledCoroutine(enabled));
    }

    private IEnumerator SetEnabledCoroutine(bool enabled)
    {
        yield return Analytics.SetEnabledAsync(enabled);
        var isEnabled = Analytics.IsEnabledAsync();
        yield return isEnabled;
        Enabled.isOn = isEnabled.Result;
    }

    private IEnumerator SetTransmissionEnabledCoroutine(bool enabled)
    {
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        yield return transmissionTarget.SetEnabledAsync(enabled);
        var isEnabled = transmissionTarget.IsEnabledAsync();
        yield return isEnabled;
        TransmissionEnabled.isOn = isEnabled.Result;
    }

    public void AddProperty()
    {
        var property = Instantiate(EventProperty);
        property.transform.SetParent(EventPropertiesList, false);
    }

    public void TrackEvent()
    {
        Analytics.TrackEvent(EventName.text, GetProperties());
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
