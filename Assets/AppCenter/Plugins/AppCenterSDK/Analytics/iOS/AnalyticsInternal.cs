// Copyright (c) Microsoft Corporation. All rights reserved.
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

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(appcenter_unity_analytics_get_type());
        }

        public static void TrackEvent(string eventName)
        {
            appcenter_unity_analytics_track_event(eventName, (int)Flags.PersistenceNormal);
        }

        public static void TrackEvent(string eventName, int flags)
        {
            appcenter_unity_analytics_track_event(eventName, flags);
        }

        public static void TrackEventWithProperties(string eventName, EventProperties properties)
        {
            appcenter_unity_analytics_track_event_with_typed_properties(eventName, properties.GetRawObject(), (int)Flags.PersistenceNormal);
        }

        public static void TrackEventWithProperties(string eventName, IDictionary<string, string> properties)
        {
            appcenter_unity_analytics_track_event_with_properties(eventName, properties.Keys.ToArray(), properties.Values.ToArray(), properties.Count, (int)Flags.PersistenceNormal);
        }

        public static void TrackEventWithProperties(string eventName, EventProperties properties, int flags)
        {
            appcenter_unity_analytics_track_event_with_typed_properties(eventName, properties.GetRawObject(), flags);
        }

        public static void TrackEventWithProperties(string eventName, IDictionary<string, string> properties, int flags)
        {
            appcenter_unity_analytics_track_event_with_properties(eventName, properties.Keys.ToArray(), properties.Values.ToArray(), properties.Count, flags);
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

        public static IntPtr GetTransmissionTarget(string transmissionTargetToken, out bool success)
        {
            var target = appcenter_unity_analytics_transmission_target_for_token(transmissionTargetToken);
            success = target != IntPtr.Zero;
            return target;
        }

        public static void Pause()
        {
            appcenter_unity_analytics_pause();
        }

        public static void Resume()
        {
            appcenter_unity_analytics_resume();
        }

        public static void EnableManualSessionTracker()
        {
            appcenter_unity_analytics_enable_manual_session_tracker();
        }

        public static void StartSession()
        {
            appcenter_unity_analytics_start_session();
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_analytics_get_type();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_track_event(string eventName, int flags);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_track_event_with_properties(string eventName, string[] keys, string[] values, int count, int flags);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_track_event_with_typed_properties(string eventName, IntPtr properties, int flags);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_analytics_is_enabled();

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_analytics_transmission_target_for_token(string transmissionTargetToken);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_pause();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_resume();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_enable_manual_session_tracker();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_start_session();

#endregion
    }
}
#endif
