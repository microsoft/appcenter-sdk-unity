// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Analytics.Internal;

namespace Microsoft.AppCenter.Unity.Analytics
{
    public class PropertyConfiguratorInternal
    {
        public static void SetAppName(object propertyConfigurator, string appName)
        {
        }

        public static void SetUserId(object propertyConfigurator, string userId)
        {
        }

        public static void SetAppVersion(object propertyConfigurator, string appVersion)
        {
        }

        public static void SetAppLocale(object propertyConfigurator, string appLocale)
        {
        }

        public static void SetEventProperty(object propertyConfigurator, string key, string value)
        {
        }

        public static void SetEventProperty(object propertyConfigurator, string key, DateTime value)
        {
        }

        public static void SetEventProperty(object propertyConfigurator, string key, long value)
        {
        }

        public static void SetEventProperty(object propertyConfigurator, string key, double value)
        {
        }

        public static void SetEventProperty(object propertyConfigurator, string key, bool value)
        {
        }

        public static void RemoveEventProperty(object propertyConfigurator, string key)
        {
        }

        public static void CollectDeviceId(object propertyConfigurator)
        {
        }
    }
}
#endif