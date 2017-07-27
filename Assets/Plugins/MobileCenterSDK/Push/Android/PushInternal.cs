// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    class PushInternal
    {
        private static AndroidJavaClass _push = new AndroidJavaClass("com.microsoft.azure.mobile.push.Push");
        public static void Initialize()
        {
            PushDelegate.mobile_center_unity_push_set_delegate();
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent");
            _push.CallStatic("checkLaunchedFromNotification", activity, intent);           
        }

        public static IntPtr mobile_center_unity_push_get_type()
        {
            return AndroidJNI.FindClass("com/microsoft/azure/mobile/push/Push");
        }

        public static void mobile_center_unity_push_set_enabled(bool isEnabled)
        {
            _push.CallStatic("setEnabled", isEnabled);
        }

        public static bool mobile_center_unity_push_is_enabled()
        {
            return _push.CallStatic<bool>("getEnabled");
        }

        public static void mobile_center_unity_push_enable_firebase_analytics()
        {
           _push.CallStatic("enableFirebaseAnalytics");
        }
    }
}
#endif
