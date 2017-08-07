// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using Microsoft.Azure.Mobile.Unity.Crashes.Internal;
using Microsoft.Azure.Mobile.Unity.Crashes.Models;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity.Crashes
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Crashes
    {
        public static void Initialize()
        {
            CrashesDelegate.SetDelegate();
        }

        public static RawType GetNativeType()
        {
            return CrashesInternal.GetType();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return CrashesInternal.IsEnabledAsync();
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return CrashesInternal.SetEnabledAsync(enabled);
        }
    }
}
