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

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            var future = _distribute.CallStatic<AndroidJavaObject>("setEnabled", isEnabled);
            return new MobileCenterTask(future);
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            var future = _distribute.CallStatic<AndroidJavaObject>("isEnabled");
            return new MobileCenterTask<bool>(future);
        }

        public static IntPtr mobile_center_unity_distribute_get_type()
        {
            return AndroidJNI.FindClass("com/microsoft/azure/mobile/distribute/Distribute");
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
