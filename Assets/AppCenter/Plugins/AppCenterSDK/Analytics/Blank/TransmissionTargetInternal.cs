// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR
using Microsoft.AppCenter.Unity.Analytics.Internal;
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics
{
#if UNITY_IOS
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = System.Object;
#endif

    public class TransmissionTargetInternal
    {
        public static void TrackEvent(RawType transmissionTarget, string eventName)
        {
        }

        public static void TrackEvent(RawType transmissionTarget, string eventName, int flags)
        {
        }

        public static void TrackEventWithProperties(RawType transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
        }

        public static void TrackEventWithProperties(RawType transmissionTarget, string eventName, EventProperties properties)
        {
        }

        public static void TrackEventWithProperties(RawType transmissionTarget, string eventName, IDictionary<string, string> properties, int flags)
        {
        }

        public static void TrackEventWithProperties(RawType transmissionTarget, string eventName, EventProperties properties, int flags)
        {
        }

        public static AppCenterTask SetEnabledAsync(RawType transmissionTarget, bool enabled)
        {
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync(RawType transmissionTarget)
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }

        public static RawType GetTransmissionTarget(RawType transmissionTargetParent, string transmissionTargetToken, out bool success)
        {
            success = false;
            return default(RawType);
        }

        public static RawType GetPropertyConfigurator(RawType transmissionTargetParent)
        {
            return default(RawType);
        }

        public static void Pause(RawType transmissionTargetParent)
        {
        }

        public static void Resume(RawType transmissionTargetParent)
        {
        }
    }
}
#endif