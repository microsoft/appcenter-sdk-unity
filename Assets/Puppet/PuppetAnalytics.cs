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
    public InputField EventName;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;
    public Text StatusText;

    void OnEnable()
    {
        Analytics.IsEnabledAsync().ContinueWith(task =>
        {
            Enabled.isOn = task.Result;
        });
    }

    public void SetEnabled(bool enabled)
    {
        StartCoroutine(SetEnabledCoroutine(enabled));
    }

    public void Pause()
    {
        Debug.Log("Pausing the app analytics...");
        Analytics.Pause();
        StatusText.text = "Analytics paused.";
    }

    public void Resume()
    {
        Debug.Log("Resuming the app analytics...");
        Analytics.Resume();
        StatusText.text = "Analytics resumed.";
    }

    private IEnumerator SetEnabledCoroutine(bool enabled)
    {
        yield return Analytics.SetEnabledAsync(enabled);
        var isEnabled = Analytics.IsEnabledAsync();
        yield return isEnabled;
        Enabled.isOn = isEnabled.Result;
    }

    public void AddProperty()
    {
        var property = Instantiate(EventProperty);
        property.transform.SetParent(EventPropertiesList, false);
    }

    public void TrackEventStringProperties()
    {
        Analytics.TrackEvent(EventName.text, GetProperties());
    }

    public void TrackEventTypedProperties()
    {
        Analytics.TrackEvent(EventName.text, GetProperties());
    }

    private EventProperties GetProperties()
    {
        var properties = EventPropertiesList.GetComponentsInChildren<PuppetEventProperty>();
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
}