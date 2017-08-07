// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    class CrashesInternal
    {
        public static RawType GetType()
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
    }
}
#endif
