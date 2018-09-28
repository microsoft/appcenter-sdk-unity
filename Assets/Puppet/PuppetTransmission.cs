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
    private string _transmissionTargetToken = "";
    private string _childTransmissionTargetToken = "";
    private TransmissionTarget _transmissionTarget;
    private TransmissionTarget _childTransmissionTarget;

    private void OnEnable()
    {
        TransmissionTarget.text = _transmissionTargetToken;
        ChildTransmissionTarget.text = _childTransmissionTargetToken;
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            TransmissionEnabled.isOn = false;
            ChildTransmissionEnabled.isOn = false;
            return;
        }
        transmissionTarget.IsEnabledAsync().ContinueWith(task =>
        {
            TransmissionEnabled.isOn = task.Result;
        });
        TransmissionTarget childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
        childTransmissionTarget.IsEnabledAsync().ContinueWith(task =>
        {
            ChildTransmissionEnabled.isOn = task.Result;
        });
    }

    private string ResolveToken() 
    {
        if (string.IsNullOrEmpty(TransmissionTarget.text)) 
        {
            return _transmissionTargetToken;
        } 
        else 
        {
            return TransmissionTarget.text;
        }
    }

    private string ResolveChildToken()
    {
        if (string.IsNullOrEmpty(ChildTransmissionTarget.text))
        {
            return _childTransmissionTargetToken;
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
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            yield break;
        }
        yield return transmissionTarget.SetEnabledAsync(enabled);
        var isEnabled = transmissionTarget.IsEnabledAsync();
        yield return isEnabled;
        TransmissionEnabled.isOn = isEnabled.Result;
    }

    private IEnumerator SetChildTransmissionEnabledCoroutine(bool enabled)
    {
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            yield break;
        }
        var childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
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
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            return;
        }
        OverrideProperties(transmissionTarget);
        var childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
        Dictionary<string, string> properties = GetProperties();
        if (properties == null)
        {
            childTransmissionTarget.TrackEvent(EventName.text);
        }
        else
        {
            childTransmissionTarget.TrackEventWithProperties(EventName.text, properties);
        }
    }

    private void OverrideProperties(TransmissionTarget transmissionTarget) 
    {
        var overridenAppName = AppName.text;
        var overridenAppVersion = AppVersion.text;
        var overridenAppLocale = AppLocale.text;
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
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            return;
        }
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
