// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Unity.Analytics.Internal
{
    class AnalyticsInternal
    {
        public static void PostInitialize()
        {
        }

        public static IntPtr GetType()
        {
            return mobile_center_unity_analytics_get_type();
        }

        public static void TrackEvent(string eventName)
        {
            mobile_center_unity_analytics_track_event(eventName);
        }

        public static void TrackEventWithProperties(string eventName, string[] keys, string[] values, int count)
        {
            mobile_center_unity_analytics_track_event_with_properties(eventName, keys, values, count);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            mobile_center_unity_analytics_set_enabled(isEnabled);
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = mobile_center_unity_analytics_is_enabled();
            return MobileCenterTask<bool>.FromCompleted(isEnabled);
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr mobile_center_unity_analytics_get_type();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_analytics_track_event(string eventName);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_analytics_track_event_with_properties(string eventName, string[] keys, string[] values, int count);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_analytics_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_analytics_is_enabled();

#endregion
    }
}
#endif