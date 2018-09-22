// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
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
    public class WrapperPropertyConfiguratorInternal
    {
        public static void SetAppName(IntPtr propertyConfigurator, string appName)
        {
            
        }

        public static void SetAppVersion(IntPtr propertyConfigurator, string appVersion)
        {
            
        }

        public static void SetAppLocale(IntPtr propertyConfigurator, string appLocale)
        {
            
        }

        public static void CollectDeviceId(IntPtr propertyConfigurator)
        {

        }

        public static void SetEventProperty(IntPtr propertyConfigurator, string key, string value)
        {

        }

        public static void RemoveEventProperty(IntPtr propertyConfigurator, string key)
        {

        }
    }
}
#endif