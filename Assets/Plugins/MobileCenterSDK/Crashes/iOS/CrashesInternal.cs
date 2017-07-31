// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
    class CrashesInternal
    {
        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            mobile_center_unity_crashes_set_enabled(isEnabled);
            return new MobileCenterTask();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return new MobileCenterTask<bool>(mobile_center_unity_crashes_is_enabled());
        }

        [DllImport("__Internal")]
        public static extern IntPtr mobile_center_unity_crashes_get_type();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_crashes_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_crashes_is_enabled();
    }
}
#endif