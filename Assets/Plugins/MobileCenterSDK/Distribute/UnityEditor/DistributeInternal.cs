﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif
    
    class DistributeInternal
    {
        public static void Initialize()
        {
        }

        public static void PostInitialize()
        {
        }
        
        public static RawType mobile_center_unity_distribute_get_type()
        {
            return default(RawType);
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return MobileCenterTask<bool>.FromCompleted(false);
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
