// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AppCenter.Unity.Analytics.Internal;
using Microsoft.AppCenter.Unity.Internal.Utility;

namespace Microsoft.AppCenter.Unity.Analytics
{
    public class WrapperPropertyConfiguratorInternal
    {
        public static void SetAppName(UnityEngine.AndroidJavaObject propertyConfigurator, string appName)
        {
            propertyConfigurator.Call("setAppName", appName);
        }

        public static void SetAppVersion(UnityEngine.AndroidJavaObject propertyConfigurator, string appVersion)
        {
            propertyConfigurator.Call("setAppVersion", appVersion);
        }

        public static void SetAppLocale(UnityEngine.AndroidJavaObject propertyConfigurator, string appLocale)
        {
            propertyConfigurator.Call("setAppLocale", appLocale);
        }

        public static void SetEventProperty(UnityEngine.AndroidJavaObject propertyConfigurator, string key, string value)
        {
            propertyConfigurator.Call("setEventProperty", key, value);
        }

        public static void RemoveEventProperty(UnityEngine.AndroidJavaObject propertyConfigurator, string key)
        {
            propertyConfigurator.Call("removeEventProperty", key);
        }

        public static void CollectDeviceId(UnityEngine.AndroidJavaObject propertyConfigurator)
        {
            propertyConfigurator.Call("collectDeviceId");
        }
    }
}
#endif