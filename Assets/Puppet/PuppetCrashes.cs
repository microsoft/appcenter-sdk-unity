// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Crashes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuppetCrashes : MonoBehaviour
{
    public Toggle CrashesEnabled;

    void OnEnable()
    {
        Crashes.IsEnabledAsync().ContinueWith(task =>
        {
            CrashesEnabled.isOn = task.Result;
        });
    }

    public void SetCrashesEnabled(bool enabled)
    {
        StartCoroutine(SetCrashesEnabledCoroutine(enabled));
    }

    private IEnumerator SetCrashesEnabledCoroutine(bool enabled)
    {
        yield return Crashes.SetEnabledAsync(enabled);
        var isEnabled = Crashes.IsEnabledAsync();
        yield return isEnabled;
        CrashesEnabled.isOn = isEnabled.Result;
    }

    public void TestHandledError()
    {
        try
        {
            throw new Exception("Test error");
        }
        catch (Exception ex)
        {
            var properties = new Dictionary<string, string> { { "Category", "Music" }, { "Wifi", "On" } };
            Crashes.TrackError(ex, properties);
        }
    }

    public void TestCrash()
    {
        Crashes.GenerateTestCrash();
    }

    public void DivideByZero()
    {
        Debug.Log(42 / int.Parse("0"));
    }

    public void NullReferenceException()
    {
        string str = null;
        Debug.Log(str.Length);
    }
}
