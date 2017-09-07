// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class MobileCenterSettingsMakerAndroid
{
    private static string MobileCenterResourcesFolderPath = "Assets/MobileCenter/Plugins/Android/mobile-center/res/values/";
    private static string MobileCenterResourcesPath = MobileCenterResourcesFolderPath + "mobile-center-settings.xml";
    private static string MobileCenterManifestPath = "Assets/MobileCenter/Plugins/Android/mobile-center/AndroidManifest.xml";
    private static readonly string ManifestAppIdPlaceholder = "${mobile-center-app-id-placeholder}";
    private static readonly string AppSecretKey = "mobile_center_app_secret";
    private static readonly string CustomLogUrlKey = "mobile_center_custom_log_url";
    private static readonly string UseCustomLogUrlKey = "mobile_center_use_custom_log_url";
    private static readonly string InitialLogLevelKey = "mobile_center_initial_log_level";
    private static readonly string UsePushKey = "mobile_center_use_push";
    private static readonly string UseAnalyticsKey = "mobile_center_use_analytics";
    private static readonly string UseDistributeKey = "mobile_center_use_distribute";
    private static readonly string CustomApiUrlKey = "mobile_center_custom_api_url";
    private static readonly string UseCustomApiUrlKey = "mobile_center_use_custom_api_url";
    private static readonly string CustomInstallUrlKey = "mobile_center_custom_install_url";
    private static readonly string UseCustomInstallUrlKey = "mobile_center_use_custom_install_url";

    private IDictionary<string, string> _resourceValues = new Dictionary<string, string>();

    static MobileCenterSettingsMakerAndroid()
    {
        if (!Directory.Exists(MobileCenterResourcesFolderPath))
        {
            Directory.CreateDirectory(MobileCenterResourcesFolderPath);
        }
    }

    public void SetLogLevel(int logLevel)
    {
        _resourceValues[InitialLogLevelKey] = logLevel.ToString();
    }

    public void SetLogUrl(string logUrl)
    {
        _resourceValues[CustomLogUrlKey] = logUrl;
        _resourceValues[UseCustomLogUrlKey] = true.ToString();
    }

    public void SetAppSecret(string appSecret)
    {
        _resourceValues[AppSecretKey] = appSecret;
    }

    public void StartPushClass()
    {
        _resourceValues[UsePushKey] = true.ToString();
    }

    public void StartAnalyticsClass()
    {
        _resourceValues[UseAnalyticsKey] = true.ToString();
    }

    public void StartDistributeClass()
    {
        _resourceValues[UseDistributeKey] = true.ToString();
    }

    public void SetApiUrl(string apiUrl)
    {
        _resourceValues[CustomApiUrlKey] = apiUrl;
        _resourceValues[UseCustomApiUrlKey] = true.ToString();
    }

    public void SetInstallUrl(string installUrl)
    {
        _resourceValues[CustomInstallUrlKey] = installUrl;
        _resourceValues[UseCustomInstallUrlKey] = true.ToString();
    }

    public void CommitSettings()
    {
        if (File.Exists(MobileCenterResourcesPath))
        {
            File.Delete(MobileCenterResourcesPath);
        }
        XmlResourceHelper.WriteXmlResource(MobileCenterResourcesPath, _resourceValues);
    }

    [InitializeOnLoadMethod]
    static void SetApplicationId()
    {
        string appId = ApplicationIdHelper.GetApplicationId();
        var manifestText = File.ReadAllText(MobileCenterManifestPath);
        if (manifestText.Contains(ManifestAppIdPlaceholder))
        {
            manifestText = manifestText.Replace(ManifestAppIdPlaceholder, appId);
            File.WriteAllText(MobileCenterManifestPath, manifestText);
        }
    }
}
