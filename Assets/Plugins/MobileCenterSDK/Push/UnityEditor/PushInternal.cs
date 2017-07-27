// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    class PushInternal
    {
        public static void Initialize()
        {
        }

        public static RawType mobile_center_unity_push_get_type()
        {
            return default(RawType);
        }

        public static void mobile_center_unity_push_set_enabled(bool isEnabled)
        {
        }

        public static bool mobile_center_unity_push_is_enabled()
        {
            return false;
        }

        public static void mobile_center_unity_push_enable_firebase_analytics()
        {
        }
    }
}
#endif
