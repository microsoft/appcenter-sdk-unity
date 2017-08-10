// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity.Distribute;
using Microsoft.Azure.Mobile.Unity.Push;
using UnityEngine;
using UnityEngine.UI;

public class PuppetOther : MonoBehaviour
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
    }


    public void SetPushEnabled(bool enabled)
    {
        Push.SetEnabledAsync(enabled).ContinueWith(task =>
        {
            PushEnabled.isOn = enabled;
        });
    }

    public void SetDistributeEnabled(bool enabled)
    {
        Distribute.SetEnabledAsync(enabled).ContinueWith(task =>
        {
            DistributeEnabled.isOn = enabled;
        });
    }
}
