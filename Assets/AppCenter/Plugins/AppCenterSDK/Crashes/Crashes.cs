// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using Microsoft.AppCenter.Unity.Crashes.Internal;
using Microsoft.AppCenter.Unity.Crashes.Models;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes
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
            return CrashesInternal.GetNativeType();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return CrashesInternal.IsEnabledAsync();
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
        {
            return CrashesInternal.SetEnabledAsync(enabled);
        }
    }
}
