// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
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

    public class WrapperTransmissionTargetInternal
    {

        public static void TrackEvent(IntPtr transmissionTarget, string eventName)
        {
            
        }

        public static void TrackEventWithProperties(IntPtr transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
            
        }

        public static AppCenterTask SetEnabledAsync(IntPtr transmissionTarget, bool enabled)
        {
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync(IntPtr transmissionTarget)
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }

        public static AppCenterTask<bool> IsEnabledAsync(IntPtr transmissionTarget)
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }

        public static Type GetTransmissionTarget(IntPtr transmissionTargetParent, string transmissionTargetToken) 
        {
            return null;
        }
    }
}
#endif