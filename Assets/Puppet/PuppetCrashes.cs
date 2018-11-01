// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AppCenter.Unity.Crashes;
using AOT;

public class PuppetCrashes : MonoBehaviour
{
    public Toggle CrashesEnabled;
    public Toggle ReportUnhandledExceptions;
    public Text LastSessionCrashReport;
    public InputField TextAttachment;
    public InputField BinaryAttachment;

    void OnEnable()
    {
        Crashes.IsEnabledAsync().ContinueWith(task =>
        {
            CrashesEnabled.isOn = task.Result;
        });
        ReportUnhandledExceptions.isOn = Crashes.IsReportingUnhandledExceptions();
    }

    public void OnValueChanged()
    {
        PlayerPrefs.SetString(PuppetAppCenter.TextAttachmentKey, TextAttachment.text);
    }

    public void OnBinaryValueChanged()
    {
        PlayerPrefs.SetString(PuppetAppCenter.BinaryAttachmentKey, BinaryAttachment.text);
    }

    public void SetCrashesEnabled(bool enabled)
    {
        StartCoroutine(SetCrashesEnabledCoroutine(enabled));
    }

    public void SetReportUnhandledExceptions(bool enabled)
    {
        Crashes.ReportUnhandledExceptions(enabled);
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

    public void ExceptionInNewThread()
    {
#if !UNITY_WSA_10_0
        new Thread(() =>
        {
            Thread.Sleep(3000);
            object obj = null;
            obj.ToString();
        }).Start();
#endif
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

    public static ErrorAttachmentLog[] GetErrorAttachmentstHandler(ErrorReport errorReport)
    {
        return new ErrorAttachmentLog[]
        {
            ErrorAttachmentLog.AttachmentWithText(PlayerPrefs.GetString(PuppetAppCenter.TextAttachmentKey), "hello.txt"),
            ErrorAttachmentLog.AttachmentWithBinary(ParseBytes(PlayerPrefs.GetString(PuppetAppCenter.BinaryAttachmentKey)), "fake_image.jpeg", "image/jpeg")
        };
    }

    private static byte[] ParseBytes(string bytesString)
    {
        string[] bytesArray = bytesString.Split(' ');
        if (bytesArray.Length == 0)
        {
            return new byte[] { 100, 101, 102, 103 };
        }
        byte[] result = new byte[bytesArray.Length];
        int i = 0;
        foreach (string byteString in bytesArray)
        {
            byte parsed;
            bool isParsed = Byte.TryParse(bytesString, out parsed);
            if (isParsed)
            {
                result[i] = parsed;
            }
            else
            {
                result[i] = 0;
            }
        }
        return result;
    }

    [MonoPInvokeCallback(typeof(Crashes.ShouldProcessErrorReportHandler))]
    public static bool ShouldProcessErrorReportHandler(ErrorReport errorReport)
    {
        return true;
    }

    [MonoPInvokeCallback(typeof(Crashes.SendingErrorReportHandler))]
    public static void SendingErrorReportHandler(ErrorReport errorReport)
    {
        Debug.Log("Puppet SendingErrorReportHandler");
    }

    [MonoPInvokeCallback(typeof(Crashes.SentErrorReportHandler))]
    public static void SentErrorReportHandler(ErrorReport errorReport)
    {
        Debug.Log("Puppet SentErrorReportHandler");
    }

    [MonoPInvokeCallback(typeof(Crashes.FailedToSendErrorReportHandler))]
    public static void FailedToSendErrorReportHandler(ErrorReport errorReport)
    {
        Debug.Log("Puppet FailedToSendErrorReportHandler");
    }
}
