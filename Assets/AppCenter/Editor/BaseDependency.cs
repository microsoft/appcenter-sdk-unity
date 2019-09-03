// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Reflection;
using UnityEngine;

public abstract class BaseDependency
{
    protected static Type versionHandler;
    protected static Type playServicesSupport;

    public static void InitializePlayServices()
    {
        // Setup the resolver using reflection as the module may not be available at compile time.
        versionHandler = Type.GetType("Google.VersionHandler, Google.VersionHandler, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null");
        if (versionHandler == null)
        {
            Debug.LogError("Unable to set up Android dependencies, class `Google.VersionHandler` is not found");
            return;
        }
        playServicesSupport = (Type)versionHandler.InvokeMember("FindClass", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            "Google.JarResolver", "Google.JarResolver.PlayServicesSupport"
        });
        if (playServicesSupport == null)
        {
            Debug.LogError("Unable to set up Android dependencies, class `Google.JarResolver.PlayServicesSupport` is not found");
            return;
        }
    }
}
