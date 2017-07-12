// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
    using UWPMobileCenter = Microsoft.Azure.Mobile.MobileCenter;

	class MobileCenterInternal  {
		public static void mobile_center_unity_configure (string appSecret)
        {
            UWPMobileCenter.Configure(appSecret);
        }

		public static void mobile_center_unity_start(string appSecret, Type[] services, int numServices)
        {
            UWPMobileCenter.Start(appSecret, services);
        }

        public static void mobile_center_unity_start_services(Type[] services, int numServices)
        {
            UWPMobileCenter.Start(services);
        }

        public static void mobile_center_unity_set_log_level(int logLevel)
        {
            UWPMobileCenter.LogLevel = (Microsoft.Azure.Mobile.LogLevel)LogLevelFromUnity(logLevel);
        }

        public static int mobile_center_unity_get_log_level()
        {
            return (int)LogLevelFromUnity((int)UWPMobileCenter.LogLevel);
        }

        public static bool mobile_center_unity_is_configured()
        {
            return UWPMobileCenter.Configured;
        }

        public static void mobile_center_unity_set_log_url(string logUrl)
        {
            UWPMobileCenter.SetLogUrl(logUrl);
        }

        public static void mobile_center_unity_set_enabled(bool isEnabled)
        {
            UWPMobileCenter.Enabled = isEnabled;
        }

        public static bool mobile_center_unity_is_enabled()
        {
            return UWPMobileCenter.Enabled;
        }

        public static string mobile_center_unity_get_install_id()
        {
            return UWPMobileCenter.InstallId?.ToString();
        }

        public static void mobile_center_unity_set_custom_properties(object properties)
        {
            var uwpProperties = properties as Microsoft.Azure.Mobile.CustomProperties;
            UWPMobileCenter.SetCustomProperties(uwpProperties);
        }

        public static void mobile_center_unity_set_wrapper_sdk(string wrapperSdkVersion,
                                                               string wrapperSdkName, 
                                                               string wrapperRuntimeVersion, 
                                                               string liveUpdateReleaseLabel, 
                                                               string liveUpdateDeploymentKey, 
                                                               string liveUpdatePackageHash)
        {
            //TODO once wrapper sdk exists for uwp, set it here
        }

        private static int LogLevelToUnity(int logLevel)
        {
            switch ((Microsoft.Azure.Mobile.LogLevel)logLevel)
            {
                case Microsoft.Azure.Mobile.LogLevel.Verbose:
                    return (int)Microsoft.Azure.Mobile.Unity.LogLevel.Verbose;
                case Microsoft.Azure.Mobile.LogLevel.Debug:
                    return (int)Microsoft.Azure.Mobile.Unity.LogLevel.Debug;
                case Microsoft.Azure.Mobile.LogLevel.Info:
                    return (int)Microsoft.Azure.Mobile.Unity.LogLevel.Info;
                case Microsoft.Azure.Mobile.LogLevel.Warn:
                    return (int)Microsoft.Azure.Mobile.Unity.LogLevel.Warn;
                case Microsoft.Azure.Mobile.LogLevel.Error:
                    return (int)Microsoft.Azure.Mobile.Unity.LogLevel.Error;
                case Microsoft.Azure.Mobile.LogLevel.Assert:
                    return (int)Microsoft.Azure.Mobile.Unity.LogLevel.Assert;
                case Microsoft.Azure.Mobile.LogLevel.None:
                    return (int)Microsoft.Azure.Mobile.Unity.LogLevel.None;
                default:
                    return logLevel;
            }
        }

        private static Microsoft.Azure.Mobile.LogLevel LogLevelFromUnity(int logLevel)
        {
            switch ((Microsoft.Azure.Mobile.Unity.LogLevel)logLevel)
            {
                case Microsoft.Azure.Mobile.Unity.LogLevel.Verbose:
                    return Microsoft.Azure.Mobile.LogLevel.Verbose;
                case Microsoft.Azure.Mobile.Unity.LogLevel.Debug:
                    return Microsoft.Azure.Mobile.LogLevel.Debug;
                case Microsoft.Azure.Mobile.Unity.LogLevel.Info:
                    return Microsoft.Azure.Mobile.LogLevel.Info;
                case Microsoft.Azure.Mobile.Unity.LogLevel.Warn:
                    return Microsoft.Azure.Mobile.LogLevel.Warn;
                case Microsoft.Azure.Mobile.Unity.LogLevel.Error:
                    return Microsoft.Azure.Mobile.LogLevel.Error;
                case Microsoft.Azure.Mobile.Unity.LogLevel.Assert:
                    return Microsoft.Azure.Mobile.LogLevel.Assert;
                case Microsoft.Azure.Mobile.Unity.LogLevel.None:
                    return Microsoft.Azure.Mobile.LogLevel.None;
                default:
                    return (Microsoft.Azure.Mobile.LogLevel)logLevel;
            }
        }
    }
}
#endif
