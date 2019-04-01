// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEngine;

#if UNITY_ANDROID && !UNITY_EDITOR
using Microsoft.AppCenter.Unity.Internal.Utility;
namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class EventPropertiesInternal
    {
        public static AndroidJavaObject Create()
        {
            return new AndroidJavaObject("com.microsoft.appcenter.analytics.EventProperties");
        }

        public static void SetString(AndroidJavaObject properties, string key, string val)
        {
            properties.Call<AndroidJavaObject>("set", key, val);
        }

        public static void SetNumber(AndroidJavaObject properties, string key, long val)
        {
            properties.Call<AndroidJavaObject>("set", key, val);
        }

        public static void SetNumber(AndroidJavaObject properties, string key, double val)
        {
            properties.Call<AndroidJavaObject>("set", key, val);
        }

        public static void SetBool(AndroidJavaObject properties, string key, bool val)
        {
            properties.Call<AndroidJavaObject>("set", key, val);
        }

        public static void SetDate(AndroidJavaObject properties, string key, DateTime val)
        {
            properties.Call<AndroidJavaObject>("set", key, JavaDateHelper.DateTimeConvert(val));
        }
    }
}
#endif