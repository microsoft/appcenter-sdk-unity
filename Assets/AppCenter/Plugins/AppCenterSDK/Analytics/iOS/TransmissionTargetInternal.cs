// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using AOT;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    public class TransmissionTargetInternal
    {
        public static void TrackEvent(IntPtr transmissionTarget, string eventName)
        {
            appcenter_unity_transmission_target_track_event(transmissionTarget, eventName, (int)Flags.PersistenceNormal);
        }

        public static void TrackEvent(IntPtr transmissionTarget, string eventName, int flags)
        {
            appcenter_unity_transmission_target_track_event(transmissionTarget, eventName, flags);
        }

        public static void TrackEventWithProperties(IntPtr transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
            appcenter_unity_transmission_target_track_event_with_props(transmissionTarget, eventName, properties.Keys.ToArray(), properties.Values.ToArray(), properties.Count, (int)Flags.PersistenceNormal);
        }

        public static void TrackEventWithProperties(IntPtr transmissionTarget, string eventName, EventProperties properties)
        {
            appcenter_unity_transmission_target_track_event_with_typed_props(transmissionTarget, eventName, properties.GetRawObject(), (int)Flags.PersistenceNormal);
        }

        public static void TrackEventWithProperties(IntPtr transmissionTarget, string eventName, IDictionary<string, string> properties, int flags)
        {
            appcenter_unity_transmission_target_track_event_with_props(transmissionTarget, eventName, properties.Keys.ToArray(), properties.Values.ToArray(), properties.Count, flags);
        }

        public static void TrackEventWithProperties(IntPtr transmissionTarget, string eventName, EventProperties properties, int flags)
        {
            appcenter_unity_transmission_target_track_event_with_typed_props(transmissionTarget, eventName, properties.GetRawObject(), flags);
        }

        public static AppCenterTask SetEnabledAsync(IntPtr transmissionTarget, bool enabled)
        {
            appcenter_unity_transmission_target_set_enabled(transmissionTarget, enabled);
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync(IntPtr transmissionTarget)
        {
            bool isEnabled = appcenter_unity_transmission_target_is_enabled(transmissionTarget);
            return AppCenterTask<bool>.FromCompleted(isEnabled);
        }

        public static IntPtr GetTransmissionTarget(IntPtr transmissionTargetParent, string transmissionTargetToken, out bool success)
        {
            var target = appcenter_unity_transmission_transmission_target_for_token(transmissionTargetParent, transmissionTargetToken);
            success = target != IntPtr.Zero;
            return target;
        }

        public static IntPtr GetPropertyConfigurator(IntPtr transmissionTarget)
        {
            return appcenter_unity_transmission_get_property_configurator(transmissionTarget);
        }

        public static void Pause(IntPtr transmissionTarget)
        {
            appcenter_unity_transmission_pause(transmissionTarget);
        }

        public static void Resume(IntPtr transmissionTarget)
        {
            appcenter_unity_transmission_resume(transmissionTarget);
        }

#region External

        [DllImport("__Internal")]
        private static extern void appcenter_unity_transmission_target_track_event(IntPtr transmissionTarget, string eventName, int flags);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_transmission_target_track_event_with_props(IntPtr transmissionTarget, string eventName, string[] keys, string[] values, int count, int flags);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_transmission_target_track_event_with_typed_props(IntPtr transmissionTarget, string eventName, IntPtr properties, int flags);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_transmission_target_set_enabled(IntPtr transmissionTarget, bool enabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_transmission_target_is_enabled(IntPtr transmissionTarget);

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_transmission_transmission_target_for_token(IntPtr transmissionTargetParent, string transmissionTargetToken);

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_transmission_get_property_configurator(IntPtr transmissionTarget);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_transmission_pause(IntPtr transmissionTarget);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_transmission_resume(IntPtr transmissionTarget);
#endregion
    }
}
#endif