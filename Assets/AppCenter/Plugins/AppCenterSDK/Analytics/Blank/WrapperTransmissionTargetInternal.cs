// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR
using Microsoft.AppCenter.Unity.Analytics.Internal;
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics
{

    public class WrapperTransmissionTargetInternal
    {

        public static void TrackEvent(IntPtr transmissionTarget, string eventName)
        {
            
        }

        public static void TrackEventWithProperties(IntPtr transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
            
        }

        public static void SetEnabled(IntPtr transmissionTarget, bool enabled)
        {
            
        }

        public static bool IsEnabled(IntPtr transmissionTarget)
        {
            return false;
        }
    }
}
#endif