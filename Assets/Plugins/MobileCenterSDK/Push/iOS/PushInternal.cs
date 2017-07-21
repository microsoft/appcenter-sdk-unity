// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    class PushInternal  {
        public static void Initialize()
        {
            PushDelegate.mobile_center_unity_push_set_delegate();
        }

        [DllImport("__Internal")]
        public static extern IntPtr mobile_center_unity_push_get_type();

        [DllImport("__Internal")]
        public static extern void mobile_center_unity_push_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        public static extern bool mobile_center_unity_push_is_enabled();

        public static void mobile_center_unity_push_enable_firebase_analytics()
        {
        }
    }
}
#endif
