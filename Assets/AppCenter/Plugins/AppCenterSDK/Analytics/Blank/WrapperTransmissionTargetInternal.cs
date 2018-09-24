// Copyright (c) Microsoft Corporation. All rights reserved.
//
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

    public class WrapperTransmissionTargetInternal
    {

        public static void TrackEvent(RawType transmissionTarget, string eventName)
        {
            
        }

        public static void TrackEventWithProperties(RawType transmissionTarget, string eventName, IDictionary<string, string> properties)
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

        public static RawType GetTransmissionTarget(RawType transmissionTargetParent, string transmissionTargetToken)
        {
            return default(RawType);
        }
    }
}
#endif