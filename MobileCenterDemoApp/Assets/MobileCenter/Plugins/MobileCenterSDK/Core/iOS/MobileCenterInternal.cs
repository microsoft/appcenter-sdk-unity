// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
    class MobileCenterInternal
    {
        public static void SetLogLevel(int logLevel)
        {
            mobile_center_unity_set_log_level(logLevel);
        }

        public static int GetLogLevel()
        {
            return mobile_center_unity_get_log_level();
        }

        public static bool IsConfigured()
        {
            return mobile_center_unity_is_configured();
        }

        public static void SetLogUrl(string logUrl)
        {
            mobile_center_unity_set_log_url(logUrl);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            mobile_center_unity_set_enabled(isEnabled);
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = mobile_center_unity_is_enabled();
            return MobileCenterTask<bool>.FromCompleted(isEnabled);
        }

        public static MobileCenterTask<string> GetInstallIdAsync()
        {
            var installId = mobile_center_unity_get_install_id();
            return MobileCenterTask<string>.FromCompleted(installId);
        }

        public static void SetCustomProperties(IntPtr properties)
        {
            mobile_center_unity_set_custom_properties(properties);
        }

        public static void SetWrapperSdk(string wrapperSdkVersion,
                                         string wrapperSdkName,
                                         string wrapperRuntimeVersion,
                                         string liveUpdateReleaseLabel,
                                         string liveUpdateDeploymentKey,
                                         string liveUpdatePackageHash)
        {
            mobile_center_unity_set_wrapper_sdk(wrapperSdkVersion,
                                                wrapperSdkName,
                                                wrapperRuntimeVersion,
                                                liveUpdateReleaseLabel,
                                                liveUpdateDeploymentKey,
                                                liveUpdatePackageHash);
        }

#region External

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_set_log_level(int logLevel);

        [DllImport("__Internal")]
        private static extern int mobile_center_unity_get_log_level();

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_is_configured();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_set_log_url(string logUrl);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_is_enabled();

        [DllImport("__Internal")]
        private static extern string mobile_center_unity_get_install_id();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_set_custom_properties(IntPtr properties);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_set_wrapper_sdk(string wrapperSdkVersion,
                                                                       string wrapperSdkName,
                                                                       string wrapperRuntimeVersion,
                                                                       string liveUpdateReleaseLabel,
                                                                       string liveUpdateDeploymentKey,
                                                                       string liveUpdatePackageHash);

#endregion
    }
}
#endif
