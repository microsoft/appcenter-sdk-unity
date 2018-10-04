// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections;
using Microsoft.AppCenter.Unity;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AppCenter.Unity.Crashes;
using Microsoft.AppCenter.Unity.Distribute;
using Microsoft.AppCenter.Unity.Push;
using AOT;

public class PuppetAppCenter : MonoBehaviour
{
    public Toggle Enabled;
    public Text InstallIdLabel;
    public Text AppSecretLabel;
    public Text LogUrlLabel;
    public Text DeviceIdLabel;
    public Text SdkVersionLabel;
    public Dropdown LogLevel;
    public PuppetConfirmationDialog userConfirmationDialog;
    public const string TextAttachmentKey = "text_attachment";
    public const string BinaryAttachmentKey = "binary_attachment";

    static PuppetAppCenter instance;

    public GameObject CustomProperty;
    public RectTransform PropertiesList;
    public Toggle DistributeEnabled;
    public Toggle PushEnabled;

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

    public void AddProperty()
    {
        var property = Instantiate(CustomProperty);
        property.transform.SetParent(PropertiesList, false);
    }

    public void Send()
    {
        AppCenter.SetCustomProperties(GetProperties());
    }

    private CustomProperties GetProperties()
    {
        var properties = PropertiesList.GetComponentsInChildren<PuppetCustomProperty>();
        if (properties == null || properties.Length == 0)
        {
            return null;
        }
        var result = new CustomProperties();
        foreach (var i in properties)
        {
            i.Set(result);
        }
        return result;
    }

    private void Awake()
    {
        Crashes.ShouldProcessErrorReport = ShouldProcessErrorReportHandler;
        Crashes.ShouldAwaitUserConfirmation = UserConfirmationHandler;
        Crashes.GetErrorAttachments = GetErrorAttachmentstHandler;
        Crashes.SendingErrorReport = SendingErrorReportHandler;
        Crashes.SentErrorReport = SentErrorReportHandler;
        Crashes.FailedToSendErrorReport = FailedToSendErrorReportHandler;
        instance = this;
    }

    [MonoPInvokeCallback(typeof(Crashes.GetErrorAttachmentsHandler))]
    public static ErrorAttachmentLog[] GetErrorAttachmentstHandler(ErrorReport errorReport)
    {
        return new ErrorAttachmentLog[]
        {
            ErrorAttachmentLog.AttachmentWithText(PlayerPrefs.GetString(TextAttachmentKey), "hello.txt"),
            ErrorAttachmentLog.AttachmentWithBinary(ParseBytes(PlayerPrefs.GetString(BinaryAttachmentKey)), "fake_image.jpeg", "image/jpeg")
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

    [MonoPInvokeCallback(typeof(Crashes.UserConfirmationHandler))]
    public static bool UserConfirmationHandler()
    {
        instance.userConfirmationDialog.Show();
        return true;
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

    void OnEnable()
    {
        AppCenter.IsEnabledAsync().ContinueWith(task =>
        {
            Enabled.isOn = task.Result;
        });
        AppCenter.GetInstallIdAsync().ContinueWith(task =>
        {
            if (task.Result.HasValue)
            {
                InstallIdLabel.text = task.Result.ToString();
            }
        });
        AppCenter.GetSecretForPlatform().ContinueWith(task =>
        {
            AppSecretLabel.text = task.Result;
        });
        AppCenter.GetLogUrl().ContinueWith(task =>
        {
            LogUrlLabel.text = task.Result;
        });
        DeviceIdLabel.text = SystemInfo.deviceUniqueIdentifier;
        SdkVersionLabel.text = AppCenter.GetSdkVersion();
        LogLevel.value = AppCenter.LogLevel - Microsoft.AppCenter.Unity.LogLevel.Verbose;
        Push.IsEnabledAsync().ContinueWith(task =>
        {
            PushEnabled.isOn = task.Result;
        });
        Distribute.IsEnabledAsync().ContinueWith(task =>
        {
            DistributeEnabled.isOn = task.Result;
        });

        Distribute.ReleaseAvailable = (releaseDetails) => true;
    }

    public void SetEnabled(bool enabled)
    {
        StartCoroutine(SetEnabledCoroutine(enabled));
    }

    private IEnumerator SetEnabledCoroutine(bool enabled)
    {
        yield return AppCenter.SetEnabledAsync(enabled);
        var isEnabled = AppCenter.IsEnabledAsync();
        yield return isEnabled;
        Enabled.isOn = isEnabled.Result;
    }

    public void SetLogLevel(int logLevel)
    {
        AppCenter.LogLevel = Microsoft.AppCenter.Unity.LogLevel.Verbose + logLevel;
    }
}
