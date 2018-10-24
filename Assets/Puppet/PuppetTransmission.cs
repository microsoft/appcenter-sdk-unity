// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
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
    public Toggle CollectDeviceId;
    public Toggle CollectDeviceIdChild;
    public InputField EventName;
    public InputField AppName;
    public InputField AppVersion;
    public InputField AppLocale;
    public InputField TransmissionTarget;
    public InputField ChildTransmissionTarget;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;
    public Text TransmissionStatus;
    public Text ChildTransmissionStatus;
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
        CollectDeviceId.isOn = false;
        CollectDeviceIdChild.isOn = false;
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

    public void SetCollectDeviceID(bool enabled)
    {
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            return;
        }
        if (enabled)
        {
            transmissionTarget.GetPropertyConfigurator().CollectDeviceId();
            CollectDeviceId.enabled = false;
        }
    }

    public void SetCollectDeviceIDChild(bool enabled)
    {
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            return;
        }
        var childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
        if (enabled)
        {
            childTransmissionTarget.GetPropertyConfigurator().CollectDeviceId();
            CollectDeviceIdChild.enabled = false;
        }
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

    public void PauseParentTransmission()
    {
        GetTransmissionTarget(transmissionTarget =>
        {
            Debug.Log("Pausing the parent transmission...");
            transmissionTarget.Pause();
            TransmissionStatus.text = "Transmission paused.";
        });
    }

    public void ResumeParentTransmission()
    {
        GetTransmissionTarget(transmissionTarget =>
        {
            Debug.Log("Resuming the parent transmission...");
            transmissionTarget.Resume();
            TransmissionStatus.text = "Transmission resumed.";
        });
    }

    public void PauseChildTransmission()
    {
        GetChildTransmissionTarget(transmissionTarget =>
        {
            Debug.Log("Pausing the child transmission...");
            transmissionTarget.Pause();
            ChildTransmissionStatus.text = "Child transmission paused.";
        });
    }

    public void ResumeChildTransmission()
    {
        GetChildTransmissionTarget(transmissionTarget =>
        {
            Debug.Log("Resuming the child transmission...");
            transmissionTarget.Resume();
            ChildTransmissionStatus.text = "Child transmission resumed.";
        });
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

    private void GetTransmissionTarget(Action<TransmissionTarget> action)
    {
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
        }
        else
        {
            action(transmissionTarget);
        }
    }

    private void GetChildTransmissionTarget(Action<TransmissionTarget> action)
    {
        GetTransmissionTarget(transmissionTarget =>
        {
            var childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
            if (childTransmissionTarget == null)
            {
                Debug.Log("Child transmission target is null.");
            }
            else
            {
                action(childTransmissionTarget);
            }
        });
    }
}
