// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.AppCenter.Unity.Analytics.Internal;
using Microsoft.AppCenter.Unity.Internal.Utility;

namespace Microsoft.AppCenter.Unity.Analytics
{
    public class TransmissionTargetInternal
    {
        public static void TrackEvent(AndroidJavaObject transmissionTarget, string eventName)
        {
            transmissionTarget.Call("trackEvent", eventName);
        }

        public static void TrackEvent(AndroidJavaObject transmissionTarget, string eventName, int flags)
        {
            transmissionTarget.Call("trackEvent", eventName, null, flags);
        }

        public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, IDictionary<string, string> properties)
        {
            var androidProperties = JavaStringMapHelper.ConvertToJava(properties);
            transmissionTarget.Call("trackEvent", eventName, androidProperties);
        }

        public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, EventProperties properties)
        {
            transmissionTarget.Call("trackEvent", eventName, properties.GetRawObject());
        }

        public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, IDictionary<string, string> properties, int flags)
        {
            var androidProperties = JavaStringMapHelper.ConvertToJava(properties);
            transmissionTarget.Call("trackEvent", eventName, androidProperties, flags);
        }

        public static void TrackEventWithProperties(AndroidJavaObject transmissionTarget, string eventName, EventProperties properties, int flags)
        {
            transmissionTarget.Call("trackEvent", eventName, properties.GetRawObject(), flags);
        }

        public static AppCenterTask SetEnabledAsync(AndroidJavaObject transmissionTarget, bool enabled)
        {
            var future = transmissionTarget.Call<AndroidJavaObject>("setEnabledAsync", enabled);
            return new AppCenterTask(future);
        }

        public static AppCenterTask<bool> IsEnabledAsync(AndroidJavaObject transmissionTarget)
        {
            var future = transmissionTarget.Call<AndroidJavaObject>("isEnabledAsync");
            return new AppCenterTask<bool>(future);
        }

        public static AndroidJavaObject GetTransmissionTarget(AndroidJavaObject transmissionTargetParent, string transmissionTargetToken, out bool success)
        {
            var target = transmissionTargetParent.Call<AndroidJavaObject>("getTransmissionTarget", transmissionTargetToken);
            success = target != null;
            return target;
        }

        public static AndroidJavaObject GetPropertyConfigurator(AndroidJavaObject transmissionTarget)
        {
            return transmissionTarget.Call<AndroidJavaObject>("getPropertyConfigurator");
        }

        public static void Pause(AndroidJavaObject transmissionTarget)
        {
            transmissionTarget.Call("pause");
        }

        public static void Resume(AndroidJavaObject transmissionTarget)
        {
            transmissionTarget.Call("resume");
        }
    }
}
#endif