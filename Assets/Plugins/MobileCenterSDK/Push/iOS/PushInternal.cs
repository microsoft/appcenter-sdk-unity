// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    class PushInternal
    {
        public static void Initialize()
        {
            PushDelegate.mobile_center_unity_push_set_delegate();
        }

        public static void PostInitialize()
        {
        }
        
        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            mobile_center_unity_push_set_enabled(isEnabled);
            return new MobileCenterTask();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return new MobileCenterTask<bool>(mobile_center_unity_push_is_enabled());
        }

        [DllImport("__Internal")]
        public static extern IntPtr mobile_center_unity_push_get_type();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_push_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_push_is_enabled();

        public static void mobile_center_unity_push_enable_firebase_analytics()
        {
        }
    }
}
#endif
