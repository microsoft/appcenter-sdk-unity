// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

using UnityEngine;
/// <summary>
/// This file is used to define dependencies, and pass them along to a system
/// which can resolve dependencies.
/// </summary>
[InitializeOnLoad]
public class FirebaseDependency : AssetPostprocessor
{
    private const string GoogleServicesFileBasename = "google-services";
    private const string GoogleServicesInputFile = GoogleServicesFileBasename + ".json";
    private const string GoogleServicesOutputFile = GoogleServicesFileBasename + ".xml";
    private const string GoogleServicesOutputDirectory = "Assets/Plugins/Android/appcenter/res/values";
    private const string GoogleServicesOutputPath = GoogleServicesOutputDirectory + "/" + GoogleServicesOutputFile;
    /// <summary>
    /// This is the entry point for "InitializeOnLoad". It will register the
    /// dependencies with the dependency tracking system.
    /// </summary>
    static FirebaseDependency()
    {
        SetupDependencies();
    }
    static void SetupDependencies()
    {
#if UNITY_ANDROID
        // Setup the resolver using reflection as the module may not be
        // available at compile time.
        Type playServicesSupport = Google.VersionHandler.FindClass(
            "Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
        if (playServicesSupport == null)
        {
            return;
        }
        object svcSupport = Google.VersionHandler.InvokeStaticMethod(
           playServicesSupport, "CreateInstance",
           new object[] {
                "FirebaseMessaging",
                EditorPrefs.GetString("AndroidSdkRoot"),
                "ProjectSettings"
           });
        Google.VersionHandler.InvokeInstanceMethod(
           svcSupport, "DependOn",
           new object[] {
                "com.google.firebase",
                "firebase-messaging",
                "11.0.0"
           },
           namedArgs: new Dictionary<string, object>() {
                { "packageIds",
                    new string[] {
                        "extra-google-m2repository",
                        "extra-android-m2repository"
                    }
                },
                { "repositories",
                    null
                }
           });
        Google.VersionHandler.InvokeInstanceMethod(
            svcSupport, "DependOn",
            new object[] {
                "com.google.firebase",
                "firebase-core",
                "11.0.0"
            },
            namedArgs: new Dictionary<string, object>() {
                { "packageIds",
                    new string[] {
                        "extra-google-m2repository",
                        "extra-android-m2repository"
                    }
                },
                { "repositories",
                    null
                }
            });
        // Update editor project view.
        AssetDatabase.Refresh();
#endif
    }
    static string[] FindGoogleServicesFiles()
    {
        var googleServicesFiles = new List<string>();
        foreach (var asset in AssetDatabase.FindAssets(GoogleServicesFileBasename))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(asset);
            if (Path.GetFileName(assetPath) == GoogleServicesInputFile)
            {
                googleServicesFiles.Add(assetPath);
            }
        }
        return googleServicesFiles.Count > 0 ? googleServicesFiles.ToArray() : null;
    }
    static void UpdateJson()
    {
#if UNITY_ANDROID
        var bundleId = ApplicationIdHelper.GetApplicationId();
        var projectDir = Path.Combine(Application.dataPath, "..");
        var googleServicesFiles = FindGoogleServicesFiles();
        if (googleServicesFiles == null)
        {
            return;
        }
        if (googleServicesFiles.Length > 1)
        {
            Debug.LogWarning("More than one " + GoogleServicesInputFile + " file found, using first one.");
        }
        var inputPath = Path.Combine(projectDir, googleServicesFiles[0]);
        var outputPath = Path.Combine(projectDir, GoogleServicesOutputPath);
        var outputDir = Path.Combine(projectDir, GoogleServicesOutputDirectory);
        if (!Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return;
            }
        }
        if (File.Exists(outputPath) &&
            File.GetLastWriteTime(outputPath).CompareTo(File.GetLastWriteTime(inputPath)) >= 0)
        {
            return;
        }
        var json = File.ReadAllText(inputPath);
        var googleServices = JsonUtility.FromJson<GoogleServices>(json);
        var resolvedClientInfo = googleServices.GetClient(bundleId);
        if (resolvedClientInfo == null)
        {
            Debug.LogWarning("Failed to find client_info in " + GoogleServicesInputFile + " matching package name: " + bundleId);
        }
        var valuesItems = new Dictionary<string, string> {
            { "default_web_client_id", googleServices.GetDefaultWebClientId(bundleId) },
            { "firebase_database_url", googleServices.GetFirebaseDatabaseUrl() },
            { "ga_trackingId", googleServices.GetGATrackingId(bundleId) },
            { "gcm_defaultSenderId", googleServices.GetDefaultGcmSenderId() },
            { "google_api_key", googleServices.GetGoogleApiKey(bundleId) },
            { "google_app_id", googleServices.GetGoogleAppId(bundleId) },
            { "google_crash_reporting_api_key", googleServices.GetCrashReportingApiKey(bundleId) },
            { "google_storage_bucket", googleServices.GetStorageBucket(bundleId) },
            { "project_id", googleServices.GetProjectId() },
        };
        XmlResourceHelper.WriteXmlResource(outputPath, valuesItems);
        // Update editor project view.
        AssetDatabase.Refresh();
#endif
    }
    /// <summary>
    /// Handle delayed loading of the dependency resolvers.
    /// </summary>
    static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets,
        string[] movedAssets, string[] movedFromPath)
    {
        Debug.Log("fsfdsgsdgsdfgds");
        foreach (string asset in importedAssets)
        {
            if (asset.Contains("JarResolver"))
            {
                SetupDependencies();
            }
            else if (Path.GetFileName(asset) == GoogleServicesInputFile)
            {
                UpdateJson();
            }
        }
    }
    #region Models
    [Serializable]
    public class ProjectInfo
    {
        public string project_id;
        public string project_number;
        public string name;
        public string firebase_url;
        public string storage_bucket;
    }
    [Serializable]
    public class AndroidClientInfo
    {
        public string package_name;
        public string[] certificate_hash;
    }
    [Serializable]
    public class ClientInfo
    {
        public string mobilesdk_app_id;
        public string client_id;
        public int client_type;
        public AndroidClientInfo android_client_info;
    }
    [Serializable]
    public class AndroidInfo
    {
        public string package_name;
        public string certificate_hash;
    }
    [Serializable]
    public class OauthClient
    {
        public string client_id;
        public int client_type;
        public AndroidInfo android_info;
    }
    [Serializable]
    public class AnalyticsProperty
    {
        public string tracking_id;
    }
    [Serializable]
    public class AnalyticsService
    {
        public int status;
        public AnalyticsProperty analytics_property;
    }
    [Serializable]
    public class Services
    {
        public AnalyticsService analytics_service;
    }
    [Serializable]
    public class Client
    {
        public ClientInfo client_info;
        public OauthClient[] oauth_client;
        public ApiKey[] api_key;
        public Services services;
    }
    [Serializable]
    public class ApiKey
    {
        public string current_key;
    }
    [Serializable]
    public class GoogleServices
    {
        public ProjectInfo project_info;
        public Client[] client;
        public object[] client_info;
        public string ARTIFACT_VERSION;
        public Client GetClient(string packageName)
        {
            if (client == null || !client.Any())
                return null;
            return client.FirstOrDefault(c => c.client_info.android_client_info.package_name == packageName);
        }
        public string GetGATrackingId(string packageName)
        {
            // {YOUR_CLIENT}/services/analytics-service/analytics_property/tracking_id
            var client = GetClient(packageName);
            if (client == null)
                return null;
            if (client.services != null &&
               client.services.analytics_service != null &&
               client.services.analytics_service.analytics_property != null)
                return client.services.analytics_service.analytics_property.tracking_id;
            return null;
        }
        public string GetProjectId()
        {
            // project_info/project_id
            if (project_info != null)
                return project_info.project_id;
            return null;
        }
        public string GetDefaultGcmSenderId()
        {
            // project_info/project_number
            if (project_info != null)
                return project_info.project_number;
            return null;
        }
        public string GetGoogleAppId(string packageName)
        {
            // {YOUR_CLIENT}/client_info/mobilesdk_app_id
            var client = GetClient(packageName);
            if (client == null)
                return null;
            if (client.client_info != null)
                return client.client_info.mobilesdk_app_id;
            return null;
        }
        public string GetDefaultWebClientId(string packageName)
        {
            // default_web_client_id:
            // {YOUR_CLIENT}/oauth_client/client_id(client_type == 3)
            var client = GetClient(packageName);
            if (client == null)
                return null;
            if (client.oauth_client != null && client.oauth_client.Any())
            {
                var oauthClient = client.oauth_client.FirstOrDefault(c => c.client_type == 3);
                if (oauthClient != null)
                    return oauthClient.client_id;
            }
            return null;
        }
        public string GetGoogleApiKey(string packageName)
        {
            // google_api_key:
            // {YOUR_CLIENT}/api_key/current_key
            var client = GetClient(packageName);
            if (client == null)
                return null;
            if (client.api_key != null && client.api_key.Any())
                return client.api_key.FirstOrDefault().current_key;
            return null;
        }
        public string GetFirebaseDatabaseUrl()
        {
            // firebase_database_url:
            // project_info/firebase_url
            if (project_info != null)
                return project_info.firebase_url;
            return null;
        }
        public string GetCrashReportingApiKey(string packageName)
        {
            // google_crash_reporting_api_key:
            // {YOUR_CLIENT}/api_key/current_key
            var client = GetClient(packageName);
            if (client == null)
                return null;
            if (client.api_key != null && client.api_key.Any())
                return client.api_key.FirstOrDefault().current_key;
            return null;
        }
        public string GetStorageBucket(string packageName)
        {
            // google_storage_bucket:
            // project_info/storage_bucket
            if (project_info != null)
                return project_info.storage_bucket;
            return null;
        }
    }
    #endregion
}