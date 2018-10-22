// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class AnalyticsInternal
    {
        public static void PrepareEventHandlers()
        {
        }

        public static IntPtr GetNativeType()
        {
            return appcenter_unity_analytics_get_type();
        }

        public static void TrackEvent(string eventName)
        {
            appcenter_unity_analytics_track_event(eventName);
        }

        public static void TrackEventWithProperties(string eventName, IDictionary<string, string> properties)
        {
            appcenter_unity_analytics_track_event_with_properties(eventName, properties.Keys.ToArray(), properties.Values.ToArray(), properties.Count);
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            appcenter_unity_analytics_set_enabled(isEnabled);
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = appcenter_unity_analytics_is_enabled();
            return AppCenterTask<bool>.FromCompleted(isEnabled);
        }

        public static IntPtr GetTransmissionTarget(string transmissionTargetToken) 
        {
            return appcenter_unity_analytics_transmission_target_for_token(transmissionTargetToken);
        }

        public static void Pause()
        {
        }

        public static void Resume()
        {
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_analytics_get_type();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_track_event(string eventName);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_track_event_with_properties(string eventName, string[] keys, string[] values, int count);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_analytics_is_enabled();

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_analytics_transmission_target_for_token(string transmissionTargetToken);

#endregion
    }
}
#endif
