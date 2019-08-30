// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public abstract class BaseDependency
{
    protected Type VersionHandler { get; set; }
    protected Type PlayServicesSupport { get; set; }
    public virtual void SetupDependencies() {

        VersionHandler = Type.GetType("Google.VersionHandler, Google.VersionHandler, Version=1.2.0.0, Culture=neutral, PublicKeyToken=null");
        if (VersionHandler == null)
        {
            Debug.LogError("Unable to set up Android dependencies, class `Google.VersionHandler` is not found");
            return;
        }
        PlayServicesSupport = (Type)VersionHandler.InvokeMember("FindClass", BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[]
        {
            "Google.JarResolver", "Google.JarResolver.PlayServicesSupport"
        });
        if (PlayServicesSupport == null)
        {
            Debug.LogError("Unable to set up Android dependencies, class `Google.JarResolver.PlayServicesSupport` is not found");
            return;
        }

    } 
}