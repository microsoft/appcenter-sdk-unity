// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine; 

namespace Microsoft.Azure.Mobile.Unity.Analytics.Internal
{
	class AnalyticsInternal
    {
        private static AndroidJavaClass _analytics = new AndroidJavaClass("com.microsoft.azure.mobile.analytics.Analytics");

        public static IntPtr mobile_center_unity_analytics_get_type()
        {
			return AndroidJNI.FindClass("com/microsoft/azure/mobile/analytics/Analytics");
        }

        public static void mobile_center_unity_analytics_track_event(string eventName)
        {
            _analytics.CallStatic("trackEvent", eventName);
        }

        public static void mobile_center_unity_analytics_track_event_with_properties(string eventName, string[] keys, string[] values, int count)
        {
            var properties = new AndroidJavaObject("java.util.HashMap");
            for (int i = 0; i < count; ++i)
            {
                properties.Call<AndroidJavaObject>("put", keys[i], values[i]);
            }
            _analytics.CallStatic("trackEvent", eventName, properties);
        }

        public static void mobile_center_unity_analytics_set_enabled(bool isEnabled)
        {
            _analytics.CallStatic("setEnabled", isEnabled);
        }

        public static bool mobile_center_unity_analytics_is_enabled()
        {
            return _analytics.CallStatic<bool>("isEnabled");
        }
	}
}
#endif
