// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using Microsoft.AppCenter.Unity.Analytics.Internal;
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics
{
    public class TransmissionTargetInternal
    {
        public static void TrackEvent(object transmissionTarget, string eventName)
        {
        }

        public static void TrackEvent(object transmissionTarget, string eventName, int flags)
        {
        }

        public static void TrackEventWithProperties(object transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
        }

        public static void TrackEventWithProperties(object transmissionTarget, string eventName, EventProperties properties)
        {
        }

        public static void TrackEventWithProperties(object transmissionTarget, string eventName, IDictionary<string, string> properties, int flags)
        {
        }

        public static void TrackEventWithProperties(object transmissionTarget, string eventName, EventProperties properties, int flags)
        {
        }

        public static AppCenterTask SetEnabledAsync(object transmissionTarget, bool enabled)
        {
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync(object transmissionTarget)
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }

        public static TransmissionTargetInternal GetTransmissionTarget(object transmissionTargetParent, string transmissionTargetToken, out bool success)
        {
            success = false;
            return null;
        }

        public static PropertyConfiguratorInternal GetPropertyConfigurator(object transmissionTargetParent)
        {
            return null;
        }

        public static void Pause(object transmissionTarget)
        {
        }

        public static void Resume(object transmissionTarget)
        {
        }
    }
}
#endif