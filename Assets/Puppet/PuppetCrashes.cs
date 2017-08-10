// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity.Crashes;
using UnityEngine;
using UnityEngine.UI;

public class PuppetCrashes : MonoBehaviour
{
    public Toggle Enabled;

    void OnEnable()
    {
        Crashes.IsEnabledAsync().ContinueWith(task =>
        {
            Enabled.isOn = task.Result;
        });
    }

    public void SetEnabled(bool enabled)
    {
        Crashes.SetEnabledAsync(enabled).ContinueWith(task => 
        {
            Enabled.isOn = enabled;
        });
    }

    public void TestCrash()
    {
        //Crashes.GenerateTestCrash();
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
