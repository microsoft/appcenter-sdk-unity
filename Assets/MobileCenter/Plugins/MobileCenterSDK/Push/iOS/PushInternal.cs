// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    class PushInternal
    {
#region Push delegate
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ReceivedPushNotificationDelegate(IntPtr notification);

        private static ReceivedPushNotificationDelegate _receivedPushDel;

        [MonoPInvokeCallback(typeof(ReceivedPushNotificationDelegate))]
        static void ReceivedPushNotificationFunc(IntPtr notification)
        {
            var eventArgs = PushNotificationHelper.PushConvert(notification);
            Push.NotifyPushNotificationReceived(eventArgs);
        }
#endregion
        public static void PrepareEventHandlers()
        {
            MobileCenterBehavior.InitializingServices += Initialize;
        }

        private static void Initialize()
        {
            _receivedPushDel = ReceivedPushNotificationFunc;
            mobile_center_unity_push_set_received_push_impl(_receivedPushDel);
        }

        public static IntPtr GetNativeType()
        {
            return mobile_center_unity_push_get_type();
        }
        
        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            mobile_center_unity_push_set_enabled(isEnabled);
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = mobile_center_unity_push_is_enabled();
            return MobileCenterTask<bool>.FromCompleted(isEnabled);
        }

        public static void EnableFirebaseAnalytics()
        {
        }

        internal static void ReplayUnprocessedPushNotifications()
        {
            mobile_center_unity_push_replay_unprocessed_notifications();
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr mobile_center_unity_push_get_type();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_push_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_push_is_enabled();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_push_set_received_push_impl(ReceivedPushNotificationDelegate functionPtr);
        
        [DllImport("__Internal")]
        private static extern void mobile_center_unity_push_replay_unprocessed_notifications();
#endregion
    }
}
#endif
