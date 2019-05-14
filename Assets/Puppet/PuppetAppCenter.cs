// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity;
using Microsoft.AppCenter.Unity.Distribute;
using Microsoft.AppCenter.Unity.Push;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuppetAppCenter : MonoBehaviour
{
    public static string TextAttachmentCached = "";
    public static string BinaryAttachmentCached = "";
    public Toggle Enabled;
    public Text StorageSizeLabel;
    public Text InstallIdLabel;
    public Text AppSecretLabel;
    public Text LogUrlLabel;
    public Text DeviceIdLabel;
    public Text SdkVersionLabel;
    public InputField UserId;
    public Dropdown LogLevel;
    public Dropdown StartupType;
    public PuppetConfirmationDialog userConfirmationDialog;
    public const string TextAttachmentKey = "text_attachment";
    public const string BinaryAttachmentKey = "binary_attachment";
    public const string UserIdKey = "user_id";
    private const string StartupModeValue = "STARTUP_MODE_VALUE";
    public GameObject CustomProperty;
    public RectTransform PropertiesList;
    public Toggle DistributeEnabled;
    public Toggle PushEnabled;

    private string _customUserId;

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
        var customUserId = PlayerPrefs.GetString(UserIdKey);
        if (customUserId != null && customUserId.Length > 0)
        {
            _customUserId = customUserId;
            AppCenter.SetUserId(customUserId);
        }

        // Caching this in Awake method because PlayerPrefs.GetString() can't be called from a background thread.
        TextAttachmentCached = PlayerPrefs.GetString(TextAttachmentKey);
        BinaryAttachmentCached = PlayerPrefs.GetString(BinaryAttachmentKey);
    }

    void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
        StartCoroutine(ShowCustomLogUrl());
    }

    private IEnumerator ShowCustomLogUrl()
    {
        var logUrl = AppCenter.GetLogUrl();
        yield return logUrl;
        LogUrlLabel.text = logUrl.Result;
    }

    private IEnumerator OnEnableCoroutine()
    {
        var isEnabled = AppCenter.IsEnabledAsync();
        yield return isEnabled;
        Enabled.isOn = isEnabled.Result;

        var installId = AppCenter.GetInstallIdAsync();
        yield return installId;
        if (installId.Result.HasValue)
        {
            InstallIdLabel.text = installId.Result.ToString();
        }

        var appSecret = AppCenter.GetSecretForPlatform();
        yield return appSecret;
        AppSecretLabel.text = appSecret.Result;

        var storageSize = AppCenter.GetStorageSize();
        yield return storageSize;
        StorageSizeLabel.text = storageSize.Result <= 0 ? "Unchanged" : storageSize.Result.ToString() + " bytes";

        DeviceIdLabel.text = SystemInfo.deviceUniqueIdentifier;
        SdkVersionLabel.text = AppCenter.GetSdkVersion();
        LogLevel.value = AppCenter.LogLevel - Microsoft.AppCenter.Unity.LogLevel.Verbose;
        StartupType.value = GetStartupMode();

        var isPushEnabled = Push.IsEnabledAsync();
        yield return isPushEnabled;
        PushEnabled.isOn = isPushEnabled.Result;

        var isDistributeEnabled = Distribute.IsEnabledAsync();
        yield return isDistributeEnabled;
        DistributeEnabled.isOn = isDistributeEnabled.Result;

        Distribute.ReleaseAvailable = (releaseDetails) => true;
        UserId.text = _customUserId;
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

    public void SetStartupMode(string startupMode)
    {
        foreach (var value in Enum.GetValues(typeof(StartupType)))
        {
            if (((StartupType)value).ToString() == startupMode)
            {
                PlayerPrefs.SetInt(StartupModeValue, (int)value);
                PlayerPrefs.Save();
                break;
            }
        }
    }

    public int GetStartupMode()
    {
        return PlayerPrefs.GetInt(StartupModeValue, 0);
    }

    public void OnUserIdChanged(string newUserId)
    {
        PlayerPrefs.SetString(UserIdKey, newUserId);
        AppCenter.SetUserId(newUserId);
    }

    public void ClearUserId()
    {
        PlayerPrefs.DeleteKey(UserIdKey);
        UserId.text = "";
        AppCenter.SetUserId(null);
    }
}
