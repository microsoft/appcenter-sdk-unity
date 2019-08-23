// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using AOT;
using Microsoft.AppCenter.Unity.Crashes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Exception = Microsoft.AppCenter.Unity.Crashes.Models.Exception;

public class PuppetCrashes : MonoBehaviour
{
    public Toggle CrashesEnabled;
    public Toggle ReportUnhandledExceptions;
    public Text LastSessionCrashReport;
    public InputField TextAttachment;
    public InputField BinaryAttachment;
    public Text LowMemoryLabel;
    private static bool _crashesNativeCallbackRegistered;

    void OnEnable()
    {
        ReportUnhandledExceptions.isOn = Crashes.IsReportingUnhandledExceptions();
        TextAttachment.text = PuppetAppCenter.TextAttachmentCached;
        BinaryAttachment.text = PuppetAppCenter.BinaryAttachmentCached;

        StartCoroutine(OnEnableCoroutine());
    }

    private IEnumerator OnEnableCoroutine()
    {
        var isEnabled = Crashes.IsEnabledAsync();
        yield return isEnabled;
        CrashesEnabled.isOn = isEnabled.Result;
        var hasLowMemoryWarning = Crashes.HasReceivedMemoryWarningInLastSessionAsync();
        yield return hasLowMemoryWarning;
        LowMemoryLabel.text = hasLowMemoryWarning.Result ? "Yes" : "No";
#if UNITY_ANDROID
        if (!_crashesNativeCallbackRegistered)
        {
            var minidumpDir = Crashes.GetMinidumpDirectoryAsync();
            yield return minidumpDir;
            setupNativeCrashesListener(minidumpDir.Result);
            _crashesNativeCallbackRegistered = true;
        }
#endif
    }

    public void TestCrash()
    {
        Crashes.GenerateTestCrash();
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
            throw new System.Exception("Test error");
        }
        catch (System.Exception ex)
        {
            var properties = new Dictionary<string, string> { { "Category", "Music" }, { "Wifi", "On" } };
            Crashes.TrackError(ex, properties);
        }
    }

    public void TriggerLowMemoryWarning()
    {
        StartCoroutine(LowMemoryTrigger());
    }

    private IEnumerator LowMemoryTrigger()
    {
        var list = new List<byte[]>();
        while (true)
        {
            list.Add(new byte[1024 * 1024 * 128]);
            yield return null;
        }
    }

    public void DivideByZero()
    {
        Debug.Log(42 / int.Parse("0"));
    }

    public void NullReferenceException()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        nativeCrashNullPointer();       
#else
        string str = null;
        Debug.Log(str.Length);
#endif
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
        StartCoroutine(LastCrashReportCoroutine());
    }

    public static ErrorAttachmentLog[] GetErrorAttachmentstHandler(ErrorReport errorReport)
    {
        return new ErrorAttachmentLog[]
        {
            ErrorAttachmentLog.AttachmentWithText(PuppetAppCenter.TextAttachmentCached, "hello.txt"),
            ErrorAttachmentLog.AttachmentWithBinary(ParseBytes(PuppetAppCenter.BinaryAttachmentCached), "fake_image.jpeg", "image/jpeg")
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
    public static void FailedToSendErrorReportHandler(ErrorReport errorReport, Exception exception)
    {
        Debug.LogFormat("Puppet FailedToSendErrorReportHandler, exception message: {0}", exception.Message);
    }

    private IEnumerator LastCrashReportCoroutine()
    {
        var hasCrashed = Crashes.HasCrashedInLastSessionAsync();
        yield return hasCrashed;
        if (hasCrashed.Result)
        {
            var lastSessionReport = Crashes.GetLastSessionCrashReportAsync();
            yield return lastSessionReport;
            var errorReport = lastSessionReport.Result;
            if (errorReport != null)
            {
                var status = new StringBuilder();
                status.AppendLine("Message: " + errorReport.Exception.Message);
                status.AppendLine("App Start Time: " + errorReport.AppStartTime);
                status.AppendLine("App Error Time: " + errorReport.AppErrorTime);
                status.AppendLine("Report Id: " + errorReport.Id);
                status.AppendLine("Process Id: " + errorReport.ProcessId);
                status.AppendLine("Reporter Key: " + errorReport.ReporterKey);
                status.AppendLine("Reporter Signal: " + errorReport.ReporterSignal);
                status.AppendLine("Is App Killed: " + errorReport.IsAppKill);
                status.AppendLine("Thread Name: " + errorReport.ThreadName);
                status.AppendLine("Stack Trace: " + errorReport.Exception.StackTrace);
                if (errorReport.Device != null)
                {
                    status.AppendLine("Device.SdkName: " + errorReport.Device.SdkName);
                    status.AppendLine("Device.SdkVersion: " + errorReport.Device.SdkVersion);
                    status.AppendLine("Device.Model: " + errorReport.Device.Model);
                    status.AppendLine("Device.OemName: " + errorReport.Device.OemName);
                    status.AppendLine("Device.OsName: " + errorReport.Device.OsName);
                    status.AppendLine("Device.OsVersion: " + errorReport.Device.OsVersion);
                    status.AppendLine("Device.OsBuild: " + errorReport.Device.OsBuild);
                    status.AppendLine("Device.OsApiLevel: " + errorReport.Device.OsApiLevel);
                    status.AppendLine("Device.Locale: " + errorReport.Device.Locale);
                    status.AppendLine("Device.TimeZoneOffset: " + errorReport.Device.TimeZoneOffset);
                    status.AppendLine("Device.ScreenSize: " + errorReport.Device.ScreenSize);
                    status.AppendLine("Device.AppVersion: " + errorReport.Device.AppVersion);
                    status.AppendLine("Device.CarrierName: " + errorReport.Device.CarrierName);
                    status.AppendLine("Device.CarrierCountry: " + errorReport.Device.CarrierCountry);
                    status.AppendLine("Device.AppBuild: " + errorReport.Device.AppBuild);
                    status.AppendLine("Device.AppNamespace: " + errorReport.Device.AppNamespace);
                }
                LastSessionCrashReport.text = status.ToString();
            }
            else
            {
                LastSessionCrashReport.text = "App has crashed during the last session but no error report has been found";
            }
        }
        else
        {
            LastSessionCrashReport.text = "App has not crashed during the last session";
        }
    }

    [DllImport("PuppetBreakpad")]
    private static extern void nativeCrashNullPointer();

    [DllImport("PuppetBreakpad")]
    private static extern void setupNativeCrashesListener(string path);
}
