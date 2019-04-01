// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR
using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Analytics.Internal;

namespace Microsoft.AppCenter.Unity.Analytics
{
#if UNITY_IOS
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = System.Object;
#endif

    public class PropertyConfiguratorInternal
    {
        public static void SetAppName(RawType propertyConfigurator, string appName)
        {
        }

        public static void SetUserId(RawType propertyConfigurator, string userId)
        {
        }

        public static void SetAppVersion(RawType propertyConfigurator, string appVersion)
        {
        }

        public static void SetAppLocale(RawType propertyConfigurator, string appLocale)
        {
        }

        public static void CollectDeviceId(RawType propertyConfigurator)
        {
        }

        public static void SetEventProperty(RawType propertyConfigurator, string key, string value)
        {
        }

        public static void SetEventProperty(RawType propertyConfigurator, string key, DateTime value)
        {
        }

        public static void SetEventProperty(RawType propertyConfigurator, string key, long value)
        {
        }

        public static void SetEventProperty(RawType propertyConfigurator, string key, double value)
        {
        }

        public static void SetEventProperty(RawType propertyConfigurator, string key, bool value)
        {
        }

        public static void RemoveEventProperty(RawType propertyConfigurator, string key)
        {
        }
    }
}
#endif