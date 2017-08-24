// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    class PushDelegate
    {
#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        delegate void ReceivedPushNotificationDelegate(IntPtr notification);

        static ReceivedPushNotificationDelegate del;
        static IntPtr ptr;


        static PushDelegate()
        {
            del = ReceivedPushNotificationFunc;
            mobile_center_unity_push_delegate_provide_received_push_impl(del);
        }

        public static void SetDelegate()
        {
            mobile_center_unity_push_set_delegate();
        }

        [MonoPInvokeCallback(typeof(ReceivedPushNotificationDelegate))]
        static void ReceivedPushNotificationFunc(IntPtr notification)
        {
            var eventArgs = PushNotificationHelper.PushConvert(notification);
            Push.NotifyPushNotificationReceived(eventArgs);
        }

#region External

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_push_set_delegate();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_push_delegate_provide_received_push_impl(ReceivedPushNotificationDelegate functionPtr);

#endregion
    }
}
#endif
