// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Crashes;
using Microsoft.AppCenter.Unity.Crashes.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PuppetCrashes : MonoBehaviour
{
    public Toggle CrashesEnabled;
    public PuppetPushDialog ErrorReportDialog;
    public Text LastSessionCrashReport;

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
            throw new System.Exception("Test error");
        }
        catch (System.Exception ex)
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

    public void LastCrashReport()
    {
        ErrorReport errorReport = Crashes.LastSessionCrashReport();
        if (errorReport != null)
        {
            IDictionary<string, string> info = new Dictionary<string, string>();
            info.Add("Type", errorReport.Exception.Type);
            info.Add("Message", errorReport.Exception.Message);
            info.Add("App Start Time", errorReport.AppStartTime.ToString());
            info.Add("App Error Time", errorReport.AppErrorTime.ToString());
            info.Add("Report Id", errorReport.Id);
            info.Add("Stack Trace", errorReport.Exception.StackTrace);
            LastSessionCrashReport.text = string.Join("\n", info.Select(i => i.Key + " : " + i.Value).ToArray());
        }
        else
        {
            ErrorReportDialog.Title = "No crash in last session";
            ErrorReportDialog.Message = "";
            ErrorReportDialog.CustomData = new Dictionary<string, string>();
            ErrorReportDialog.Show();
        }
    }
}
