// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
    class DistributeInternal
    {
        private static AndroidJavaClass _distribute = new AndroidJavaClass("com.microsoft.azure.mobile.distribute.Distribute");

        public static void Initialize()
        {
            DistributeDelegate.mobile_center_unity_distribute_set_delegate();
        }

        public static IntPtr mobile_center_unity_distribute_get_type()
        {
            return AndroidJNI.FindClass("com/microsoft/azure/mobile/distribute/Distribute");
        }

        public static void mobile_center_unity_distribute_set_enabled(bool isEnabled)
        {
            _distribute.CallStatic("setEnabled", isEnabled);
        }

        public static bool mobile_center_unity_distribute_is_enabled()
        {
            return _distribute.CallStatic<bool>("getEnabled");
        }

        public static void mobile_center_unity_distribute_set_install_url(string installUrl)
        {
            _distribute.CallStatic("setInstallUrl", installUrl);
        }

        public static void mobile_center_unity_distribute_set_api_url(string apiUrl)
        {
            _distribute.CallStatic("setApiUrl", apiUrl);
        }

        public static void mobile_center_unity_distribute_notify_update_action(int updateAction)
        {
            _distribute.CallStatic("notifyUpdateAction", updateAction);
        }
    }
}
#endif
