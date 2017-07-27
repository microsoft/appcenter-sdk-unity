// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
    class DistributeInternal
    {
        public static void Initialize()
        {
            DistributeDelegate.mobile_center_unity_distribute_set_delegate();
        }

        public static void PostInitialize()
        {
        }

        [DllImport("__Internal")]
        public static extern IntPtr mobile_center_unity_distribute_get_type();

        [DllImport("__Internal")]
        public static extern void mobile_center_unity_distribute_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        public static extern bool mobile_center_unity_distribute_is_enabled();

        [DllImport("__Internal")]
        public static extern void mobile_center_unity_distribute_set_install_url(string installUrl);

        [DllImport("__Internal")]
        public static extern void mobile_center_unity_distribute_set_api_url(string apiUrl);

        [DllImport("__Internal")]
        public static extern void mobile_center_unity_distribute_notify_update_action(int updateAction);
    }
}
#endif
