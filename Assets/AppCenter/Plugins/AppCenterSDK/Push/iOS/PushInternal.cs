// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.AppCenter.Unity.Push.Internal
{
    class PushInternal
    {
#region Push delegate
#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        delegate void ReceivedPushNotificationDelegate(IntPtr notification);

        private static ReceivedPushNotificationDelegate _receivedPushDel;

        [MonoPInvokeCallback(typeof(ReceivedPushNotificationDelegate))]
        static void ReceivedPushNotificationFunc(IntPtr notification)
        {
            var eventArgs = PushNotificationHelper.PushConvert(notification);
            Push.NotifyPushNotificationReceived(eventArgs);
        }
#endregion
        private static bool IsAppCenterStart = false;
        private static bool IsWaitingToReply = false;

        public static void PrepareEventHandlers()
        {
            AppCenterBehavior.InitializingServices += Initialize;
        }

        private static void Initialize()
        {
            _receivedPushDel = ReceivedPushNotificationFunc;
            appcenter_unity_push_set_received_push_impl(_receivedPushDel);
            IsAppCenterStart = true;
            if (IsWaitingToReply)
            {
                PushInternal.ReplayUnprocessedPushNotifications();
                IsWaitingToReply = false;
            }
        }

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(appcenter_unity_push_get_type());
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            appcenter_unity_push_set_enabled(isEnabled);
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = appcenter_unity_push_is_enabled();
            return AppCenterTask<bool>.FromCompleted(isEnabled);
        }

        public static void EnableFirebaseAnalytics()
        {
        }

        public static void StartPush()
        {
            appcenter_unity_start_push();
        }

        internal static void ReplayUnprocessedPushNotifications()
        {
            if(!IsAppCenterStart)
            {
                IsWaitingToReply = true;
                return;
            }
            appcenter_unity_push_replay_unprocessed_notifications();
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_push_get_type();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_push_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_push_is_enabled();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_start_push();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_push_set_received_push_impl(ReceivedPushNotificationDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_push_replay_unprocessed_notifications();
#endregion
    }
}
#endif
