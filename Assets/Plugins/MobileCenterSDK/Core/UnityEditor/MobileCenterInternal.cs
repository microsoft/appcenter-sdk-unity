﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
#if UNITY_IOS
    using RawType = System.IntPtr;
    using ServiceType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
	using ServiceType = System.IntPtr;
#else
    using RawType = System.Object;
    using ServiceType = System.Type;
#endif

	class MobileCenterInternal
    {
	    public static void mobile_center_unity_configure(string appSecret)
	    {
	    }

        public static void mobile_center_unity_start(string appSecret, ServiceType[] services, int numServices)
        {
        }

		public static void mobile_center_unity_start_services(ServiceType[] services, int numServices)
        {
        }

		public static void mobile_center_unity_set_log_level(int logLevel)
        {
        }

        public static int mobile_center_unity_get_log_level()
        {
            return 0;
        }

		public static bool mobile_center_unity_is_configured()
        {
            return false;
        }

		public static void mobile_center_unity_set_log_url(string logUrl)
        {
        }

		public static void mobile_center_unity_set_enabled(bool isEnabled)
        {
        }

        public static bool mobile_center_unity_is_enabled()
        {
            return false;
        }

		public static string mobile_center_unity_get_install_id()
        {
            return null;
        }

		public static void mobile_center_unity_set_custom_properties(RawType properties)
        {
            
        }

		public static void mobile_center_unity_set_wrapper_sdk(string wrapperSdkVersion,
															    string wrapperSdkName,
															    string wrapperRuntimeVersion,
															    string liveUpdateReleaseLabel,
															    string liveUpdateDeploymentKey,
															    string liveUpdatePackageHash)
        {
        }

        public static void PostConfigure()
        {
        }
	}
}
#endif
