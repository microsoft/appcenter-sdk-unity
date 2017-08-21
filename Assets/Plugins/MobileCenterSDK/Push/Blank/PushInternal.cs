﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR

using System.Collections.Generic;
using System.Threading;

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

        public static void PostInitialize()
        {
        }

        public static RawType GetNativeType()
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

        public static void EnableFirebaseAnalytics()
        {
        }

        internal static void ReplayUnprocessedPushNotifications()
        {
        }
    }
}
#endif
