// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


/// <summary>
/// This file is used to define dependencies, and pass them along to a system which can resolve dependencies.
/// </summary>
public class MsalDependency : BaseDependency 

{
    private const string MsalVersion = "0.3.1-alpha";
    private const string SupportLibVersion = "27.1.1";
    private const string GsonVersion = "2.8.4";
    private const string IdentityCommonVersion = "0.0.10-alpha";
    private const string NimbusVersion = "5.7";

    public override void SetupDependencies()
    {
        
#if UNITY_ANDROID
        // Setup the resolver using reflection as the module may not be available at compile time.

        base.SetupDependencies();

        object svcSupport = versionHandler.InvokeMember("InvokeStaticMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            playServicesSupport, "CreateInstance", new object[] { "Msal", EditorPrefs.GetString("AndroidSdkRoot"), "ProjectSettings" }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.microsoft.identity.client", "msal", MsalVersion }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn",
            new object[] { "com.android.support", "appcompat-v7", SupportLibVersion },
            new Dictionary<string, object>() {
                { "packageIds", new string[] { "extra-google-m2repository", "extra-android-m2repository" } },
                { "repositories", null }
            }
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn",
            new object[] { "com.android.support", "customtabs", SupportLibVersion },
            new Dictionary<string, object>() {
                { "packageIds", new string[] { "extra-google-m2repository", "extra-android-m2repository" } },
                { "repositories", null }
            }
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.google.code.gson", "gson", GsonVersion }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.microsoft.identity", "common", IdentityCommonVersion }, null
        });
        versionHandler.InvokeMember("InvokeInstanceMethod", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            svcSupport, "DependOn", new object[] { "com.nimbusds", "nimbus-jose-jwt", NimbusVersion }, null
        });
        // Update editor project view.
        AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// Handle delayed loading of the dependency resolvers.
    /// </summary>
    public static void SetupAuth()
    {
        string[] importedAssets = AssetDatabase.GetAllAssetPaths();
        foreach (string asset in importedAssets)
        {
            if (asset.Contains("JarResolver"))
            {
                (new MsalDependency()).SetupDependencies();
            }
        }
    }
}