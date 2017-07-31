// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
    class DistributeInternal
    {
        public static void Initialize()
        {
        }

        public static Type mobile_center_unity_distribute_get_type()
        {
            return typeof(Microsoft.Azure.Mobile.Distribute.Distribute);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            return new MobileCenterTask(Task.FromResult(0));
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return new MobileCenterTask(Task.FromResult(false));
        }

        public static void mobile_center_unity_distribute_set_install_url(string installUrl)
        {
        }

        public static void mobile_center_unity_distribute_set_api_url(string apiUrl)
        {
        }

        public static void mobile_center_unity_distribute_notify_update_action(int updateAction)
        {
        }
    }
}
#endif
