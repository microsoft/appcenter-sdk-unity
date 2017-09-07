// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR
using System;
using Microsoft.Azure.Mobile.Unity;

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
        public static void Configure(string appSecret)
        {
        }

        public static void Start(string appSecret, ServiceType[] services, int numServices)
        {
        }

        public static void StartServices(ServiceType[] services, int numServices)
        {
        }

        public static void SetLogLevel(int logLevel)
        {
        }

        public static int GetLogLevel()
        {
            return 0;
        }

        public static bool IsConfigured()
        {
            return false;
        }

        public static void SetLogUrl(string logUrl)
        {
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return MobileCenterTask<bool>.FromCompleted(false);
        }

        public static MobileCenterTask<string> GetInstallIdAsync()
        {
            return MobileCenterTask<string>.FromCompleted("");
        }

        public static void SetCustomProperties(RawType properties)
        {
        }

        public static void SetWrapperSdk(string wrapperSdkVersion,
                                         string wrapperSdkName,
                                         string wrapperRuntimeVersion,
                                         string liveUpdateReleaseLabel,
                                         string liveUpdateDeploymentKey,
                                         string liveUpdatePackageHash)
        {
        }
    }
}
#endif
