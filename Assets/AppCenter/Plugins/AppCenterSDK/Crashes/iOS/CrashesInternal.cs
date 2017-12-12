// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using AOT;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    class CrashesInternal
    {

        public static IntPtr GetNativeType()
        {
            return appcenter_unity_crashes_get_type();
        }

        public static void TrackException(IntPtr exception)
        {
            appcenter_unity_crashes_track_model_exception(exception);
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            appcenter_unity_crashes_set_enabled(isEnabled);
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = appcenter_unity_crashes_is_enabled();
            return AppCenterTask<bool>.FromCompleted(isEnabled);
        }

        public static void GenerateTestCrash()
        {
            appcenter_unity_crashes_generate_test_crash();
        }

        public static AppCenterTask<bool> HasCrashedInLastSession()
        {
            var hasCrashedInLastSession = appcenter_unity_crashes_has_crashed_in_last_session();
            return AppCenterTask<bool>.FromCompleted(hasCrashedInLastSession);
        }

        public static void DisableMachExceptionHandler()
        {
            appcenter_unity_crashes_disable_mach_exception_handler();
        }

        #region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_crashes_get_type();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_crashes_track_model_exception(IntPtr exception);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_crashes_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_crashes_is_enabled();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_crashes_generate_test_crash();

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_crashes_has_crashed_in_last_session();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_crashes_disable_mach_exception_handler();

        #endregion
    }
}
#endif