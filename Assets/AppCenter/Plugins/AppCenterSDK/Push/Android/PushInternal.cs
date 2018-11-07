// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Push.Internal
{
    class PushInternal
    {
        private static AndroidJavaClass _push = new AndroidJavaClass("com.microsoft.appcenter.push.Push");
        private static AndroidJavaClass _unityListener = new AndroidJavaClass("com.microsoft.appcenter.loader.UnityAppCenterPushDelegate");

        public static void PrepareEventHandlers()
        {
            AppCenterBehavior.InitializingServices += Initialize;
            AppCenterBehavior.InitializedAppCenterAndServices += PostInitialize;
        }

        private static void Initialize()
        {
            _unityListener.CallStatic("setListener", new PushDelegate());
        }

        private static void PostInitialize()
        {
            var instance = _push.CallStatic<AndroidJavaObject>("getInstance");
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            instance.Call("onActivityResumed", activity);
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            var future = _push.CallStatic<AndroidJavaObject>("setEnabled", isEnabled);
            return new AppCenterTask(future);
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var future = _push.CallStatic<AndroidJavaObject>("isEnabled");
            return new AppCenterTask<bool>(future);
        }

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(AndroidJNI.FindClass("com/microsoftappcenter/push/Push"));
        }

        public static void EnableFirebaseAnalytics()
        {
            _push.CallStatic("enableFirebaseAnalytics");
        }

        internal static void ReplayUnprocessedPushNotifications()
        {
            _unityListener.CallStatic("replayPushNotifications");
        }
    }
}
#endif
