// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using Microsoft.AppCenter.Unity.Analytics.Internal;
using AOT;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Analytics
{

    public class WrapperTransmissionTargetInternal
    {

        public static void TrackEvent(UnityEngine.AndroidJavaObject transmissionTarget, string eventName)
        {
            transmissionTarget.Call("trackEvent", eventName);
        }

        public static void TrackEventWithProperties(UnityEngine.AndroidJavaObject transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
            string[] keys = properties.Keys.ToArray();
            string[] values = properties.Values.ToArray();
            var androidProperties = new AndroidJavaObject("java.util.HashMap");
            for (int i = 0; i < properties.Count; ++i)
            {
                androidProperties.Call<AndroidJavaObject>("put", keys[i], values[i]);
            }
            transmissionTarget.Call("trackEvent", eventName, androidProperties);
            
        }

        public static AppCenterTask SetEnabledAsync(UnityEngine.AndroidJavaObject transmissionTarget, bool enabled)
        {
            var future = transmissionTarget.Call<AndroidJavaObject>("setEnabledAsync", enabled);
            return new AppCenterTask(future);
        }

        public static AppCenterTask<bool> IsEnabledAsync(UnityEngine.AndroidJavaObject transmissionTarget)
        {
            var future = transmissionTarget.Call<AndroidJavaObject>("isEnabledAsync");
            return new AppCenterTask<bool>(future);
        }
    }
}
#endif