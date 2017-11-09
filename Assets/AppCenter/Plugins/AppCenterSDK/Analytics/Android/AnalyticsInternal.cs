// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class AnalyticsInternal
    {
        private static AndroidJavaClass _analytics = new AndroidJavaClass("com.microsoft.appcanter.analytics.Analytics");

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

        public static IntPtr GetNativeType()
        {
            return AndroidJNI.FindClass("com/microsoft/appcenter/analytics/Analytics");
        }

        public static void TrackEvent(string eventName)
        {
            _analytics.CallStatic("trackEvent", eventName);
        }

        public static void TrackEventWithProperties(string eventName, string[] keys, string[] values, int count)
        {
            var properties = new AndroidJavaObject("java.util.HashMap");
            for (int i = 0; i < count; ++i)
            {
                properties.Call<AndroidJavaObject>("put", keys[i], values[i]);
            }
            _analytics.CallStatic("trackEvent", eventName, properties);
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
    }
}
#endif
