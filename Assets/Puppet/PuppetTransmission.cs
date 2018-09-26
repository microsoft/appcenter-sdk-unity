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
    public InputField AppName;
    public InputField AppVersion;
    public InputField AppLocale;
    public InputField TransmissionTarget;
    public InputField ChildTransmissionTarget;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;
    private string TransmissionTargetToken = "";
    private string ChildTransmissionTargetToken = "";
    private TransmissionTarget _transmissionTarget;
    private TransmissionTarget _childTransmissionTarget;

    private void OnEnable()
    {
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        transmissionTarget.IsEnabledAsync().ContinueWith(task =>
        {
            TransmissionEnabled.isOn = task.Result;
        });
        TransmissionTarget childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
        childTransmissionTarget.IsEnabledAsync().ContinueWith(task =>
        {
            ChildTransmissionEnabled.isOn = task.Result;
        });
        TransmissionTarget.text = TransmissionTargetToken;
        ChildTransmissionTarget.text = ChildTransmissionTargetToken;
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
        OverrideProperties(childTransmissionTarget);
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

    private void OverrideProperties(TransmissionTarget transmissionTarget) 
    {
        string overridenAppName = AppName.text;
        string overridenAppVersion = AppVersion.text;
        string overridenAppLocale = AppLocale.text;
        if (!string.IsNullOrEmpty(overridenAppName))
        {
            transmissionTarget.GetPropertyConfigurator().SetAppName(overridenAppName);
        }
        if (!string.IsNullOrEmpty(overridenAppVersion))
        {
            transmissionTarget.GetPropertyConfigurator().SetAppVersion(overridenAppVersion);
        }
        if (!string.IsNullOrEmpty(overridenAppLocale))
        {
            transmissionTarget.GetPropertyConfigurator().SetAppLocale(overridenAppLocale);
        }
    }

    public void TrackEventTransmission() 
    { 
        TransmissionTarget transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        OverrideProperties(transmissionTarget);
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
