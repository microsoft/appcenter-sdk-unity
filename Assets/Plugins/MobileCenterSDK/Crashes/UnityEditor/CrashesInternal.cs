// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    class CrashesInternal
    {
        public static RawType mobile_center_unity_crashes_get_type()
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
    }
}
#endif
