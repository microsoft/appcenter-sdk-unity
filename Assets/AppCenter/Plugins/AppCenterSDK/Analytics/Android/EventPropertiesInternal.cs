using System;
using UnityEngine;
using Microsoft.AppCenter.Unity.Internal.Utility;

#if UNITY_ANDROID && !UNITY_EDITOR
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
            properties.Call<AndroidJavaObject>("set", key, JavaNumberHelper.Convert(val));
        }

        public static void SetNumber(AndroidJavaObject properties, string key, double val)
        {
            properties.Call<AndroidJavaObject>("set", key, JavaNumberHelper.Convert(val));
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