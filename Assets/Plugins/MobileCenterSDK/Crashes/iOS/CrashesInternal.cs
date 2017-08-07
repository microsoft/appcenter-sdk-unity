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
        public static RawType GetNativeType()
        {
            return mobile_center_unity_crashes_get_type();
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            mobile_center_unity_crashes_set_enabled(isEnabled);
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = mobile_center_unity_crashes_is_enabled();
            return MobileCenterTask<bool>.FromCompleted(isEnabled);
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr mobile_center_unity_crashes_get_type();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_crashes_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_crashes_is_enabled();

#endregion
    }
}
#endif