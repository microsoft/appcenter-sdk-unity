// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Internal.Utility;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class AnalyticsInternal
    {
        private static AndroidJavaClass _analytics = new AndroidJavaClass("com.microsoft.appcenter.analytics.Analytics");

        public static void PrepareEventHandlers()
        {
            AppCenterBehavior.InitializedAppCenterAndServices += PostInitialize;
        }

        private static void PostInitialize()
        {
            var instance = _analytics.CallStatic<AndroidJavaObject>("getInstance");
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            instance.Call("onActivityResumed", activity);
        }

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(AndroidJNI.FindClass("com/microsoft/appcenter/analytics/Analytics"));
        }

        public static void TrackEvent(string eventName)
        {
            _analytics.CallStatic("trackEvent", eventName);
        }

        public static void TrackEvent(string eventName, int flags)
        {
            _analytics.CallStatic("trackEvent", eventName, null, flags);
        }

        public static void TrackEventWithProperties(string eventName, IDictionary<string, string> properties)
        {
            var androidProperties = JavaStringMapHelper.ConvertToJava(properties);
            _analytics.CallStatic("trackEvent", eventName, androidProperties);
        }

        public static void TrackEventWithProperties(string eventName, EventProperties properties)
        {
            _analytics.CallStatic("trackEvent", eventName, properties.GetRawObject());
        }

        public static void TrackEventWithProperties(string eventName, IDictionary<string, string> properties, int flags)
        {
            var androidProperties = JavaStringMapHelper.ConvertToJava(properties);
            _analytics.CallStatic("trackEvent", eventName, androidProperties, flags);
        }

        public static void TrackEventWithProperties(string eventName, EventProperties properties, int flags)
        {
            _analytics.CallStatic("trackEvent", eventName, properties.GetRawObject(), flags);
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            var future = _analytics.CallStatic<AndroidJavaObject>("setEnabled", isEnabled);
            return new AppCenterTask(future);
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var future = _analytics.CallStatic<AndroidJavaObject>("isEnabled");
            return new AppCenterTask<bool>(future);
        }

        public static AndroidJavaObject GetTransmissionTarget(string transmissionTargetToken, out bool success)
        {
            var target = _analytics.CallStatic<AndroidJavaObject>("getTransmissionTarget", transmissionTargetToken);
            success = target != null;
            return target;
        }

        public static void Pause()
        {
            _analytics.CallStatic("pause");
        }

        public static void Resume()
        {
            _analytics.CallStatic("resume");
        }

        public static void EnableManualSessionTracker()
        {
            _analytics.CallStatic("enableManualSessionTracker");
        }

        public static void StartSession()
        {
            _analytics.CallStatic("startSession");
        }
    }
}
#endif
