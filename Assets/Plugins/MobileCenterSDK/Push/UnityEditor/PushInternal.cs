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

        public static RawType GetNativeType()
        {
            return default(RawType);
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return null;
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return null;
        }

        public static void EnableFirebaseAnalytics()
        {
        }
    }
}
#endif
