// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections;
using Microsoft.AppCenter.Unity.Distribute;
using Microsoft.AppCenter.Unity.Push;
using UnityEngine;
using UnityEngine.UI;

public class DemoOther : MonoBehaviour
{
    public Toggle DistributeEnabled;
    public Toggle PushEnabled;

    void OnEnable()
    {
        Push.IsEnabledAsync().ContinueWith(task =>
        {
            PushEnabled.isOn = task.Result;
        });
        Distribute.IsEnabledAsync().ContinueWith(task =>
        {
            DistributeEnabled.isOn = task.Result;
        });

        Distribute.ReleaseAvailable = (releaseDetails) =>
        {
            return true;
        };
    }

    public void SetPushEnabled(bool enabled)
    {
        StartCoroutine(SetPushEnabledCoroutine(enabled));
    }

    private IEnumerator SetPushEnabledCoroutine(bool enabled)
    {
        yield return Push.SetEnabledAsync(enabled);
        var isEnabled = Push.IsEnabledAsync();
        yield return isEnabled;
        PushEnabled.isOn = isEnabled.Result;
    }

    public void SetDistributeEnabled(bool enabled)
    {
        StartCoroutine(SetDistributeEnabledCoroutine(enabled));
    }

    private IEnumerator SetDistributeEnabledCoroutine(bool enabled)
    {
        yield return Distribute.SetEnabledAsync(enabled);
        var isEnabled = Distribute.IsEnabledAsync();
        yield return isEnabled;
        DistributeEnabled.isOn = isEnabled.Result;
    }
}
