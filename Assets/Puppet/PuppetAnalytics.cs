// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections;
using Microsoft.AppCenter.Unity.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class PuppetAnalytics : MonoBehaviour
{
    public Toggle Enabled;
    public Toggle IsCritical;
    public InputField EventName;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;
    public Text StatusText;
    private bool _isCritical;

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

    public void SetIsCritical(bool critical)
    {
        _isCritical = IsCritical.isOn;
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
        var properties = PropertiesHelper.GetStringProperties(EventPropertiesList);

        // We need to verify all the TrackEvent overloads, hence the full condition here instead of a ternary operator.
        if (_isCritical)
        {
            Analytics.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
        }
        else
        {
            Analytics.TrackEvent(EventName.text, properties);
        }
    }

    public void TrackEventTypedProperties()
    {
        var properties = PropertiesHelper.GetTypedProperties(EventPropertiesList);

        // We need to verify all the TrackEvent overloads, hence the full condition here instead of a ternary operator.
        if (_isCritical)
        {
            Analytics.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
        }
        else
        {
            Analytics.TrackEvent(EventName.text, properties);
        }
    }
}