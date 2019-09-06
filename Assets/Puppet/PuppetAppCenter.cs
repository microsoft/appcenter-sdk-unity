// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Assets.AppCenter.Plugins.Android.Utility;
using Microsoft.AppCenter.Unity;
using Microsoft.AppCenter.Unity.Crashes;
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
    public static string AppSecretCached;
    public static string LogUrlCached;
    public static string MaxSizeCached;
    public static int StartupTypeCached = 2;
    public Toggle Enabled;
    public Text InstallIdLabel;
    public Text DeviceIdLabel;
    public Text SdkVersionLabel;
    public InputField UserId;
    public InputField AppSecret;
    public InputField LogUrl;
    public InputField MaxStorageSize;
    public Dropdown LogLevel;
    public Dropdown StartupType;
    public PuppetConfirmationDialog userConfirmationDialog;
    public const string TextAttachmentKey = "text_attachment";
    public const string BinaryAttachmentKey = "binary_attachment";
    public const string UserIdKey = "user_id";
    private const string StartupModeAndroidKey = "AppCenter.Unity.StartTargetKey";
    private const string StartupModeKey = "MSAppCenterStartTargetUnityKey";
    private const string MaxStorageSizeKey = "MSAppCenterMaxStorageSizeUnityKey";
    private const string MaxStorageSizeAndroidKey = "AppCenter.Unity.MaxStorageSizeKey";
    private const string LogUrlKey = "MSAppCenterLogUrlUnityKey";
    private const string LogUrlAndroidKey = "AppCenter.Unity.LogUrlKey";
    private const string AppSecretKey = "MSAppCenterAppSecretUnityKey";
    private const string AppSecretAndroidKey = "AppCenter.Unity.AppSecretKey";
    public GameObject CustomProperty;
    public RectTransform PropertiesList;
    public Toggle DistributeEnabled;
    public Toggle PushEnabled;
    public Toggle CustomDialog;
    public static string FlagCustomDialog = "FlagCustomDialog";
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
        AppSecretCached = PlayerPrefs.GetString(AppSecretKey, null);
        LogUrlCached = PlayerPrefs.GetString(LogUrlKey, null);
        MaxSizeCached = PlayerPrefs.GetString(MaxStorageSizeKey, null);
        StartupTypeCached = PlayerPrefs.GetInt(StartupModeKey, (int) Microsoft.AppCenter.Unity.StartupType.Both);
        CustomDialog.isOn = PlayerPrefs.GetInt(FlagCustomDialog, 0) == 1;
    }

    void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
        StartCoroutine(ShowCustomLogUrl());
    }

    private IEnumerator ShowCustomLogUrl()
    {
        if (LogUrlCached != null && LogUrlCached.Length > 0)
        {
            LogUrl.text = LogUrlCached;
        }
        else
        {
            var logUrl = AppCenter.GetLogUrlAsync();
            yield return logUrl;
            LogUrl.text = logUrl.Result;
        }
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

        if (AppSecretCached != null && AppSecretCached.Length > 0)
        {
            AppSecret.text = AppSecretCached;
        }
        else
        {
            var appSecret = AppCenter.GetSecretForPlatformAsync();
            yield return appSecret;
            AppSecret.text = appSecret.Result;
        }

        if (MaxSizeCached != null && MaxSizeCached.Length > 0)
        {
            MaxStorageSize.text = MaxSizeCached;
        }
        else
        {
            var storageSize = AppCenter.GetStorageSizeAsync();
            yield return storageSize;
            MaxStorageSize.text = storageSize.Result <= 0 ? "Unchanged" : storageSize.Result.ToString();
        }

        DeviceIdLabel.text = SystemInfo.deviceUniqueIdentifier;
        SdkVersionLabel.text = AppCenter.GetSdkVersion();
        LogLevel.value = AppCenter.LogLevel - Microsoft.AppCenter.Unity.LogLevel.Verbose;
        StartupType.value = StartupTypeCached;

        var isPushEnabled = Push.IsEnabledAsync();
        yield return isPushEnabled;
        PushEnabled.isOn = isPushEnabled.Result;

        var isDistributeEnabled = Distribute.IsEnabledAsync();
        yield return isDistributeEnabled;
        DistributeEnabled.isOn = isDistributeEnabled.Result;
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
        var isPushEnabled = Push.IsEnabledAsync();
        yield return isPushEnabled;
        PushEnabled.isOn = isPushEnabled.Result;
        var isDistributeEnabled = Distribute.IsEnabledAsync();
        yield return isDistributeEnabled;
        DistributeEnabled.isOn = isDistributeEnabled.Result;
    }

    public void SetLogLevel(int logLevel)
    {
        AppCenter.LogLevel = Microsoft.AppCenter.Unity.LogLevel.Verbose + logLevel;
    }

    public void SetCustomDialog(bool enable)
    {
        int isEnable = enable ? 1 : 0;
        PlayerPrefs.SetInt(FlagCustomDialog, isEnable);
        PlayerPrefs.Save();
    }

    public void SetStartupMode(int startupMode)
    {
        PlayerPrefs.SetInt(StartupModeKey, startupMode);
        PlayerPrefs.Save();
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtility.SetPreferenceInt(StartupModeAndroidKey, startupMode);
#endif
    }

    public void SetMaxStorageSize(string maxStorageSize)
    {
        long result;
        if (!long.TryParse(maxStorageSize, out result))
        {
            MaxStorageSize.text = "Invalid";
            return;
        }
        PlayerPrefs.SetString(MaxStorageSizeKey, maxStorageSize);
        PlayerPrefs.Save();
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtility.SetPreferenceString(MaxStorageSizeAndroidKey, maxStorageSize);
#endif
    }

    public void SetLogUrl(string logUrl)
    {
        PlayerPrefs.SetString(LogUrlKey, logUrl);
        PlayerPrefs.Save();
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtility.SetPreferenceString(LogUrlAndroidKey, logUrl);
#endif
    }

    public void ResetLogUrl(string logUrl)
    {
        PlayerPrefs.DeleteKey(LogUrlKey);
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtility.SetPreferenceString(LogUrlAndroidKey, logUrl);
#endif
    }

    public void SetAppSecret(string appSecret)
    {
        PlayerPrefs.SetString(AppSecretKey, appSecret);
        PlayerPrefs.Save();
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtility.SetPreferenceString(AppSecretAndroidKey, appSecret);
#endif
    }

    public void OnUserIdChanged(string newUserId)
    {
        _customUserId = newUserId;
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
