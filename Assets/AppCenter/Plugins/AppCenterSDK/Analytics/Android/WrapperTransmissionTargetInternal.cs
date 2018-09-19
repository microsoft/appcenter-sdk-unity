// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using Microsoft.AppCenter.Unity.Analytics.Internal;
using System;

namespace Microsoft.AppCenter.Unity.Analytics
{

    public class WrapperTransmissionTargetInternal
    {

        public static UnityEngine.AndroidJavaObject Create()
        {
            return default(RawType);
        }

        public static void TrackEvent(UnityEngine.AndroidJavaObject transmissionTarget, string eventName)
        {
            
        }

        public static void TrackEventWithProperties(UnityEngine.AndroidJavaObject transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
            
        }

        public static void SetEnabled(UnityEngine.AndroidJavaObject transmissionTarget, bool enabled)
        {
            
        }

        public static bool IsEnabled(UnityEngine.AndroidJavaObject transmissionTarget)
        {
            return false;
        }
    }
}
#endif