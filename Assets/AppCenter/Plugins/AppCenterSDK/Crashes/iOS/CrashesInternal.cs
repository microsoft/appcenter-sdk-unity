// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    class CrashesInternal
    {
        public static IntPtr GetNativeType()
        {
            return app_center_unity_crashes_get_type();
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            app_center_unity_crashes_set_enabled(isEnabled);
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = app_center_unity_crashes_is_enabled();
            return AppCenterTask<bool>.FromCompleted(isEnabled);
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_crashes_get_type();

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool app_center_unity_crashes_is_enabled();

#endregion
    }
}
#endif