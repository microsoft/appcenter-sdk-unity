// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections.Generic;
using System.IO;

public class MobileCenterSettingsMakerAndroid
{
    private const string MobileCenterResourcesFolderPath = "Assets/Plugins/Android/mobile-center/res/values/";
    private const string MobileCenterResourcesPath = MobileCenterResourcesFolderPath + "mobile-center-settings.xml";
    private const string MobileCenterManifestPath = "Assets/Plugins/Android/mobile-center/AndroidManifest.xml";
    private const string MobileCenterManifestPlaceholderPath = "Assets/MobileCenter/Plugins/Android/AndroidManifestPlaceholder.xml";
    private const string ManifestAppIdPlaceholder = "${mobile-center-app-id-placeholder}";
    private const string AppSecretKey = "mobile_center_app_secret";
    private const string CustomLogUrlKey = "mobile_center_custom_log_url";
    private const string UseCustomLogUrlKey = "mobile_center_use_custom_log_url";
    private const string InitialLogLevelKey = "mobile_center_initial_log_level";
    private const string UsePushKey = "mobile_center_use_push";
    private const string UseAnalyticsKey = "mobile_center_use_analytics";
    private const string UseDistributeKey = "mobile_center_use_distribute";
    private const string CustomApiUrlKey = "mobile_center_custom_api_url";
    private const string UseCustomApiUrlKey = "mobile_center_use_custom_api_url";
    private const string CustomInstallUrlKey = "mobile_center_custom_install_url";
    private const string UseCustomInstallUrlKey = "mobile_center_use_custom_install_url";
    private readonly IDictionary<string, string> _resourceValues = new Dictionary<string, string>();

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
        var appId = ApplicationIdHelper.GetApplicationId();
        var manifestText = File.ReadAllText(MobileCenterManifestPlaceholderPath);
        if (manifestText.Contains(ManifestAppIdPlaceholder))
        {
            manifestText = manifestText.Replace(ManifestAppIdPlaceholder, appId);
            File.WriteAllText(MobileCenterManifestPath, manifestText);
        }
        if (File.Exists(MobileCenterResourcesPath))
        {
            File.Delete(MobileCenterResourcesPath);
        }
        XmlResourceHelper.WriteXmlResource(MobileCenterResourcesPath, _resourceValues);
    }
}
