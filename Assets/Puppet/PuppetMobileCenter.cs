// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections;
using Microsoft.Azure.Mobile.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PuppetMobileCenter : MonoBehaviour
{
    public Toggle Enabled;
    public Dropdown LogLevel;
    string installId = null;

    void OnEnable()
    {
        MobileCenter.IsEnabledAsync().ContinueWith(task =>
        {
            Enabled.isOn = task.Result;
        });
        MobileCenter.GetInstallIdAsync().ContinueWith(task =>
        {
            if (task.Result.HasValue)
            {
                installId = task.Result.ToString();
            }
        });
        LogLevel.value = MobileCenter.LogLevel - Microsoft.Azure.Mobile.Unity.LogLevel.Verbose;
    }

    private void Update()
    {
        if (installId != null)
        {
            print("Install ID = " + installId);
            installId = null;
        }
    }

    public void SetEnabled(bool enabled)
    {
        StartCoroutine(SetEnabledCoroutine(enabled));
    }

    private IEnumerator SetEnabledCoroutine(bool enabled)
    {
        yield return MobileCenter.SetEnabledAsync(enabled);
        var isEnabled = MobileCenter.IsEnabledAsync();
        yield return isEnabled;
        Enabled.isOn = isEnabled.Result;
    }

    public void SetLogLevel(int logLevel)
    {
        MobileCenter.LogLevel = Microsoft.Azure.Mobile.Unity.LogLevel.Verbose + logLevel;
    }
}
