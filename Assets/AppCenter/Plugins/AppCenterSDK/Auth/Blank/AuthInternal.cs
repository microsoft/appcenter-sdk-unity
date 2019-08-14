// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Auth.Internal
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

#if UNITY_IOS
    using TransmissionTargetType = System.IntPtr;
#elif UNITY_ANDROID
    using TransmissionTargetType = UnityEngine.AndroidJavaObject;
#else
    using TransmissionTargetType = System.Object;
#endif

    class AuthInternal
    {
        public static void PrepareEventHandlers()
        {
        }

        public static void AddNativeType(List<RawType> nativeTypes)
        {
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
        {
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }
    }
}
#endif
