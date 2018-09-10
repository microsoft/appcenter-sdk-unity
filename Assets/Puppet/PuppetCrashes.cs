// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Crashes;
using Microsoft.AppCenter.Unity.Crashes.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuppetCrashes : MonoBehaviour
{
    public Toggle CrashesEnabled;
    public Toggle ReportUnhandledExceptions;
    public Text LastSessionCrashReport;

    void OnEnable()
    {
        Crashes.IsEnabledAsync().ContinueWith(task =>
        {
            CrashesEnabled.isOn = task.Result;
        });
        ReportUnhandledExceptions.isOn = Crashes.ReportUnhandledExceptions;
    }

    public void SetCrashesEnabled(bool enabled)
    {
        StartCoroutine(SetCrashesEnabledCoroutine(enabled));
    }

    public void SetReportUnhandledExceptions(bool enabled)
    {
        Crashes.ReportUnhandledExceptions = enabled;
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
        var errorReport = Crashes.LastSessionCrashReport();
        var info = new Dictionary<string, string>();
        if (errorReport != null)
        {
            info.Add("Type", errorReport.Exception.Type);
            info.Add("Message", errorReport.Exception.Message);
            info.Add("App Start Time", errorReport.AppStartTime.ToString());
            info.Add("App Error Time", errorReport.AppErrorTime.ToString());
            info.Add("Report Id", errorReport.Id);
            info.Add("Process Id", errorReport.ProcessId.ToString());
            info.Add("Reporter Key", errorReport.ReporterKey);
            info.Add("Reporter Signal", errorReport.ReporterSignal);
            info.Add("Is App Killed", errorReport.IsAppKill.ToString());
            info.Add("Stack Trace", errorReport.Exception.StackTrace);
        }
        else
        {
            info.Add("Result", "No crash in last session");
        }
        LastSessionCrashReport.text = string.Join("\n", info.Select(i => i.Key + " : " + i.Value).ToArray());
    }
}
