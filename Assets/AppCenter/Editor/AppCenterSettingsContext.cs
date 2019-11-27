// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AppCenterSettingsContext : ScriptableObject
{
    private static readonly string SettingsPath = AppCenterPath + "/AppCenterSettings.asset";
    private static readonly string AdvancedSettingsPath = AppCenterPath + "/AppCenterSettingsAdvanced.asset";
    private static AppCenterSettingsContext Instance;

    public static AppCenterSettingsContext GetInstance()
    {
        if (Instance == null)
        {
            Instance = new AppCenterSettingsContext();
        }
        return Instance;
    }

    private AppCenterSettingsContext()
    {
    }

    public static string AppCenterPath
    {
        get
        {
            return GetInstance().GetAppCenterPath();
        }
    }

    public static AppCenterSettings SettingsInstance
    {
        get
        {
            // No need to lock because this can only be accessed from the main thread.
            var instance = AssetDatabase.LoadAssetAtPath<AppCenterSettings>(SettingsPath);
            if (instance == null)
            {
                instance = CreateInstance<AppCenterSettings>();
                AssetDatabase.CreateAsset(instance, SettingsPath);
                AssetDatabase.SaveAssets();
            }
            return instance;
        }
    }

    public static AppCenterSettingsAdvanced SettingsInstanceAdvanced
    {
        get
        {
            // No need to lock because this can only be accessed from the main thread.
            return AssetDatabase.LoadAssetAtPath<AppCenterSettingsAdvanced>(AdvancedSettingsPath);
        }
    }

    public string GetAppCenterPath()
    {
        var script = MonoScript.FromScriptableObject(this);
        var pathScript = AssetDatabase.GetAssetPath(script);
        var lastAppCenterIndex = pathScript.LastIndexOf("Editor", StringComparison.Ordinal);
        var directoryAppCenter = pathScript.Remove(lastAppCenterIndex).TrimEnd(Path.DirectorySeparatorChar);
        return directoryAppCenter;
    }

    public static AppCenterSettingsAdvanced CreateSettingsInstanceAdvanced()
    {
        var instance = AssetDatabase.LoadAssetAtPath<AppCenterSettingsAdvanced>(AdvancedSettingsPath);
        if (instance == null)
        {
            instance = CreateInstance<AppCenterSettingsAdvanced>();
            AssetDatabase.CreateAsset(instance, AdvancedSettingsPath);
            AssetDatabase.SaveAssets();
        }
        return instance;
    }

    public static void DeleteSettingsInstanceAdvanced()
    {
        AssetDatabase.DeleteAsset(AdvancedSettingsPath);
    }

}
