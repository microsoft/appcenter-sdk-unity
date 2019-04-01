// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using Microsoft.AppCenter.Unity.Internal.Utility;
using System;

namespace Microsoft.AppCenter.Unity.Analytics
{
    public class PropertyConfiguratorInternal
    {
        public static void SetAppName(AndroidJavaObject propertyConfigurator, string appName)
        {
            propertyConfigurator.Call("setAppName", appName);
        }

        public static void SetUserId(AndroidJavaObject propertyConfigurator, string userId)
        {
            propertyConfigurator.Call("setUserId", userId);
        }

        public static void SetAppVersion(AndroidJavaObject propertyConfigurator, string appVersion)
        {
            propertyConfigurator.Call("setAppVersion", appVersion);
        }

        public static void SetAppLocale(AndroidJavaObject propertyConfigurator, string appLocale)
        {
            propertyConfigurator.Call("setAppLocale", appLocale);
        }

        public static void CollectDeviceId(AndroidJavaObject propertyConfigurator)
        {
            propertyConfigurator.Call("collectDeviceId");
        }

        public static void SetEventProperty(AndroidJavaObject propertyConfigurator, string key, string value)
        {
            propertyConfigurator.Call("setEventProperty", key, value);
        }

        public static void SetEventProperty(AndroidJavaObject propertyConfigurator, string key, DateTime value)
        {
            var javaDate = JavaDateHelper.DateTimeConvert(value);
            propertyConfigurator.Call("setEventProperty", key, javaDate);
        }

        public static void SetEventProperty(AndroidJavaObject propertyConfigurator, string key, long value)
        {
            propertyConfigurator.Call("setEventProperty", key, value);
        }

        public static void SetEventProperty(AndroidJavaObject propertyConfigurator, string key, double value)
        {
            propertyConfigurator.Call("setEventProperty", key, value);
        }

        public static void SetEventProperty(AndroidJavaObject propertyConfigurator, string key, bool value)
        {
            propertyConfigurator.Call("setEventProperty", key, value);
        }

        public static void RemoveEventProperty(AndroidJavaObject propertyConfigurator, string key)
        {
            propertyConfigurator.Call("removeEventProperty", key);
        }
    }
}
#endif