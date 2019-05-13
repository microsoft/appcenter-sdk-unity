// Copyright (c) Microsoft Corporation. All rights reserved.
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
    public Toggle DefaultTransmissionEnabled;
    public Toggle CollectDeviceId;
    public Toggle CollectDeviceIdChild;
    public Toggle CollectDeviceIdDefault;
    public Toggle UseParentPropertyConfigurator;
    public Toggle UseChildPropertyConfigurator;
    public Toggle UseDefaultPropertyConfigurator;
    public InputField EventName;
    public InputField AppName;
    public InputField AppVersion;
    public InputField AppLocale;
    public InputField TransmissionTarget;
    public InputField ChildTransmissionTarget;
    public InputField DefaultTransmissionTarget;
    public InputField ParentUserId;
    public InputField ChildUserId;
    public InputField DefaultUserId;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;
    public Text TransmissionStatus;
    public Text ChildTransmissionStatus;
    public Text DefaultTransmissionStatus;
    public Toggle IsCritical;
    private string _transmissionTargetToken = "";
    private string _childTransmissionTargetToken = "";
    private string _defaultTransmissionTargetToken = "";
    private bool _isCritical;

    private void OnEnable()
    {
        TransmissionTarget.text = _transmissionTargetToken;
        ChildTransmissionTarget.text = _childTransmissionTargetToken;
        DefaultTransmissionTarget.text = _defaultTransmissionTargetToken;

        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            TransmissionEnabled.isOn = false;
            ChildTransmissionEnabled.isOn = false;
            DefaultTransmissionEnabled.isOn = false;
            return;
        }

        StartCoroutine(IsEnabledCoroutine(transmissionTarget));
        TransmissionTarget childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
        if (childTransmissionTarget != null)
        {
            StartCoroutine(IsChildEnabledCoroutine(childTransmissionTarget));
        }
        CollectDeviceId.isOn = false;
        CollectDeviceIdChild.isOn = false;
        CollectDeviceIdDefault.isOn = false;
    }

    private IEnumerator IsChildEnabledCoroutine(TransmissionTarget childTransmissionTarget)
    {
        var task = childTransmissionTarget.IsEnabledAsync();
        yield return task;
        ChildTransmissionEnabled.isOn = task.Result;
    }

    private IEnumerator IsDefaultEnabledCoroutine(TransmissionTarget defaultTransmissionTarget)
    {
        var task = defaultTransmissionTarget.IsEnabledAsync();
        yield return task;
        DefaultTransmissionEnabled.isOn = task.Result;
    }

    private IEnumerator IsEnabledCoroutine(TransmissionTarget transmissionTarget)
    {
        var task = transmissionTarget.IsEnabledAsync();
        yield return task;
        TransmissionEnabled.isOn = task.Result;
    }

    public void SetIsCritical(bool critical)
    {
        _isCritical = IsCritical.isOn;
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

    private string ResolveDefaultToken()
    {
        if (string.IsNullOrEmpty(DefaultTransmissionTarget.text))
        {
            return _defaultTransmissionTargetToken;
        }
        else
        {
            return DefaultTransmissionTarget.text;
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

    public void SetCollectDeviceIDDefault(bool enabled)
    {
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            return;
        }
        var defaultTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveDefaultToken());
        if (enabled)
        {
            defaultTransmissionTarget.GetPropertyConfigurator().CollectDeviceId();
            CollectDeviceIdDefault.enabled = false;
        }
    }

    public void SetChildTransmissionEnabled(bool enabled)
    {
        StartCoroutine(SetChildTransmissionEnabledCoroutine(enabled));
    }

    public void SetDefaultTransmissionEnabled(bool enabled)
    {
        StartCoroutine(SetDefaultTransmissionEnabledCoroutine(enabled));
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

    private IEnumerator SetDefaultTransmissionEnabledCoroutine(bool enabled)
    {
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget == null)
        {
            Debug.Log("Transmission target is null.");
            yield break;
        }
        var defaultTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveDefaultToken());
        yield return defaultTransmissionTarget.SetEnabledAsync(enabled);
        var isEnabled = defaultTransmissionTarget.IsEnabledAsync();
        yield return isEnabled;
        DefaultTransmissionEnabled.isOn = isEnabled.Result;
    }

    public void AddProperty()
    {
        var property = Instantiate(EventProperty);
        property.transform.SetParent(EventPropertiesList, false);
    }

    public void TrackEventStringPropertiesChildTransmission()
    {
        var transmissionTarget = GetChildTransmissionTarget();
        if (transmissionTarget != null)
        {
            var properties = PropertiesHelper.GetStringProperties(EventPropertiesList);
            if (properties == null)
            {
                if (_isCritical)
                {
                    IDictionary<string, string> nullProps = null;
                    transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                }
                else
                {
                    transmissionTarget.TrackEvent(EventName.text);
                }
            }
            else
            {
                if (UseChildPropertyConfigurator.isOn)
                {
                    var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
                    foreach (var property in properties)
                    {
                        propertyConfigurator.SetEventProperty(property.Key, property.Value);
                    }
                    propertyConfigurator.SetEventProperty("extraEventProperty", "should be removed!");
                    propertyConfigurator.RemoveEventProperty("extraEventProperty");
                    if (_isCritical)
                    {
                        IDictionary<string, string> nullProps = null;
                        transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text);
                    }
                }
                else
                {
                    if (_isCritical)
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties);
                    }
                }
            }
        }
    }

    public void TrackEventTypedPropertiesChildTransmission()
    {
        var transmissionTarget = GetChildTransmissionTarget();
        if (transmissionTarget != null)
        {
            var properties = PropertiesHelper.GetTypedProperties(EventPropertiesList);
            if (properties == null)
            {
                if (_isCritical)
                {
                    EventProperties nullProps = null;
                    transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                }
                else
                {
                    transmissionTarget.TrackEvent(EventName.text);
                }
            }
            else
            {
                if (UseChildPropertyConfigurator.isOn)
                {
                    var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
                    PropertiesHelper.AddPropertiesToPropertyConfigurator(EventPropertiesList, propertyConfigurator);
                    propertyConfigurator.SetEventProperty("extraEventProperty", "should be removed!");
                    propertyConfigurator.RemoveEventProperty("extraEventProperty");
                    if (_isCritical)
                    {
                        EventProperties nullProps = null;
                        transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text);
                    }
                }
                else
                {
                    if (_isCritical)
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties);
                    }
                }
            }
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

    public void TrackEventStringPropertiesTransmission()
    {
        var transmissionTarget = GetTransmissionTarget();
        if (transmissionTarget != null)
        {
            OverrideProperties(transmissionTarget);
            var properties = PropertiesHelper.GetStringProperties(EventPropertiesList);
            if (properties == null)
            {
                if (_isCritical)
                {
                    IDictionary<string, string> nullProps = null;
                    transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                }
                else
                {
                    transmissionTarget.TrackEvent(EventName.text);
                }
            }
            else
            {
                if (UseParentPropertyConfigurator.isOn)
                {
                    var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
                    foreach (var property in properties)
                    {
                        propertyConfigurator.SetEventProperty(property.Key, property.Value);
                    }
                    propertyConfigurator.SetEventProperty("extraEventProperty", "should be removed!");
                    propertyConfigurator.RemoveEventProperty("extraEventProperty");
                    if (_isCritical)
                    {
                        IDictionary<string, string> nullProps = null;
                        transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text);
                    }
                }
                else
                {
                    if (_isCritical)
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties);
                    }
                }
            }
        }
    }

    public void TrackEventTypedPropertiesTransmission()
    {
        var transmissionTarget = GetTransmissionTarget();
        if (transmissionTarget != null)
        {
            OverrideProperties(transmissionTarget);
            var properties = PropertiesHelper.GetTypedProperties(EventPropertiesList);
            if (properties == null)
            {
                if (_isCritical)
                {
                    EventProperties nullProps = null;
                    transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                }
                else
                {
                    transmissionTarget.TrackEvent(EventName.text);
                }
            }
            else
            {
                if (UseParentPropertyConfigurator.isOn)
                {
                    var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
                    PropertiesHelper.AddPropertiesToPropertyConfigurator(EventPropertiesList, propertyConfigurator);
                    propertyConfigurator.SetEventProperty("extraEventProperty", "should be removed!");
                    propertyConfigurator.RemoveEventProperty("extraEventProperty");
                    if (_isCritical)
                    {
                        EventProperties nullProps = null;
                        transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text);
                    }
                }
                else
                {
                    if (_isCritical)
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties);
                    }
                }
            }
        }
    }

    public void TrackEventStringPropertiesDefaultTransmission()
    {
        var transmissionTarget = GetDefaultTransmissionTarget();
        if (transmissionTarget != null)
        {
            var properties = PropertiesHelper.GetStringProperties(EventPropertiesList);
            if (properties == null)
            {
                if (_isCritical)
                {
                    IDictionary<string, string> nullProps = null;
                    transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                }
                else
                {
                    transmissionTarget.TrackEvent(EventName.text);
                }
            }
            else
            {
                if (UseDefaultPropertyConfigurator.isOn)
                {
                    var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
                    foreach (var property in properties)
                    {
                        propertyConfigurator.SetEventProperty(property.Key, property.Value);
                    }
                    propertyConfigurator.SetEventProperty("extraEventProperty", "should be removed!");
                    propertyConfigurator.RemoveEventProperty("extraEventProperty");
                    if (_isCritical)
                    {
                        IDictionary<string, string> nullProps = null;
                        transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text);
                    }
                }
                else
                {
                    if (_isCritical)
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties);
                    }
                }
            }
        }
    }

    public void TrackEventTypedPropertiesDefaultTransmission()
    {
        var transmissionTarget = GetDefaultTransmissionTarget();
        if (transmissionTarget != null)
        {
            var properties = PropertiesHelper.GetTypedProperties(EventPropertiesList);
            if (properties == null)
            {
                if (_isCritical)
                {
                    EventProperties nullProps = null;
                    transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                }
                else
                {
                    transmissionTarget.TrackEvent(EventName.text);
                }
            }
            else
            {
                if (UseDefaultPropertyConfigurator.isOn)
                {
                    var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
                    PropertiesHelper.AddPropertiesToPropertyConfigurator(EventPropertiesList, propertyConfigurator);
                    propertyConfigurator.SetEventProperty("extraEventProperty", "should be removed!");
                    propertyConfigurator.RemoveEventProperty("extraEventProperty");
                    if (_isCritical)
                    {
                        EventProperties nullProps = null;
                        transmissionTarget.TrackEvent(EventName.text, nullProps, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text);
                    }
                }
                else
                {
                    if (_isCritical)
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties, Flags.PersistenceCritical);
                    }
                    else
                    {
                        transmissionTarget.TrackEvent(EventName.text, properties);
                    }
                }
            }
        }
    }

    public void OnParentUserIdChanged(string newUserId)
    {
        var transmissionTarget = GetTransmissionTarget();
        if (transmissionTarget != null)
        {
            var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
            propertyConfigurator.SetUserId(newUserId);
        }
    }

    public void OnChildUserIdChanged(string newUserId)
    {
        var transmissionTarget = GetChildTransmissionTarget();
        if (transmissionTarget != null)
        {
            var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
            propertyConfigurator.SetUserId(newUserId);
        }
    }

    public void OnDefaultUserIdChanged(string newUserId)
    {
        var transmissionTarget = GetDefaultTransmissionTarget();
        if (transmissionTarget != null)
        {
            var propertyConfigurator = transmissionTarget.GetPropertyConfigurator();
            propertyConfigurator.SetUserId(newUserId);
        }
    }

    public void PauseParentTransmission()
    {
        var transmissionTarget = GetTransmissionTarget();
        if (transmissionTarget != null)
        {
            Debug.Log("Pausing the parent transmission...");
            transmissionTarget.Pause();
            TransmissionStatus.text = "Transmission paused.";
        }
    }

    public void ResumeParentTransmission()
    {
        var transmissionTarget = GetTransmissionTarget();
        if (transmissionTarget != null)
        {
            Debug.Log("Resuming the parent transmission...");
            transmissionTarget.Resume();
            TransmissionStatus.text = "Transmission resumed.";
        }
    }

    public void PauseChildTransmission()
    {
        var transmissionTarget = GetChildTransmissionTarget();
        if (transmissionTarget != null)
        {
            Debug.Log("Pausing the child transmission...");
            transmissionTarget.Pause();
            ChildTransmissionStatus.text = "Child transmission paused.";
        }
    }

    public void ResumeChildTransmission()
    {
        var transmissionTarget = GetChildTransmissionTarget();
        if (transmissionTarget != null)
        {
            Debug.Log("Resuming the child transmission...");
            transmissionTarget.Resume();
            ChildTransmissionStatus.text = "Child transmission resumed.";
        }
    }

    public void ResumeDefaultTransmission()
    {
        var transmissionTarget = GetDefaultTransmissionTarget();
        if (transmissionTarget != null)
        {
            Debug.Log("Resuming the default transmission...");
            transmissionTarget.Resume();
            DefaultTransmissionStatus.text = "Default transmission resumed.";
        }
    }

    public void PauseDefaultTransmission()
    {
        var transmissionTarget = GetDefaultTransmissionTarget();
        if (transmissionTarget != null)
        {
            Debug.Log("Pausing the default transmission...");
            transmissionTarget.Pause();
            DefaultTransmissionStatus.text = "Default transmission paused.";
        }
    }

    private TransmissionTarget GetTransmissionTarget()
    {
        var transmissionTarget = Analytics.GetTransmissionTarget(ResolveToken());
        if (transmissionTarget != null)
        {
            OverrideProperties(transmissionTarget);
            return transmissionTarget;
        }
        Debug.Log("Transmission target is null.");
        return null;
    }

    private TransmissionTarget GetChildTransmissionTarget()
    {
        var transmissionTarget = GetTransmissionTarget();
        if (transmissionTarget != null)
        {
            var childTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveChildToken());
            if (childTransmissionTarget != null)
            {
                return childTransmissionTarget;
            }
            Debug.Log("Child transmission target is null.");
        }
        return null;
    }

    private TransmissionTarget GetDefaultTransmissionTarget()
    {
        var transmissionTarget = GetTransmissionTarget();
        if (transmissionTarget != null)
        {
            var defaultTransmissionTarget = transmissionTarget.GetTransmissionTarget(ResolveDefaultToken());
            if (defaultTransmissionTarget != null)
            {
                return defaultTransmissionTarget;
            }
            Debug.Log("Default transmission target is null.");
        }
        return null;
    }

    public void ClearParentUserId()
    {
        ParentUserId.text = "";
        OnParentUserIdChanged(null);
    }

    public void ClearChildUserId()
    {
        ChildUserId.text = "";
        OnChildUserIdChanged(null);
    }

    public void CleardefaultUserId()
    {
        DefaultUserId.text = "";
        OnDefaultUserIdChanged(null);
    }
}
