// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using Microsoft.Azure.Mobile.Unity.Internal;
using System.Reflection;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
#if UNITY_IOS || UNITY_ANDROID
    using ServiceType = System.IntPtr;
#else
    using ServiceType = System.Type;
#endif

    public class MobileCenter
    {
        public static void Configure(string appSecret)
        {
            SetWrapperSdk();
            appSecret = GetSecretForPlatform(appSecret);
            MobileCenterInternal.mobile_center_unity_configure(appSecret);
        }

        public static LogLevel LogLevel
        {
            get { return (LogLevel)MobileCenterInternal.mobile_center_unity_get_log_level(); }
            set { MobileCenterInternal.mobile_center_unity_set_log_level((int)value); }
        }

        /// <summary>
        ///     Enable or disable the SDK as a whole. Updating the property propagates the value to all services that have been
        ///     started.
        /// </summary>
        /// <remarks>
        ///     The default state is <c>true</c> and updating the state is persisted into local application storage.
        /// </remarks>
        public static bool Enabled
        {
            get { return MobileCenterInternal.mobile_center_unity_is_enabled(); }
            set { MobileCenterInternal.mobile_center_unity_set_enabled(value); }
        }

        /// <summary>
        /// Get the unique installation identifier for this application installation on this device.
        /// </summary>
        /// <remarks>
        /// The identifier is lost if clearing application data or uninstalling application.
        /// </remarks>
        public static Guid? InstallId
        {
            get
            {
                var installIdString = MobileCenterInternal.mobile_center_unity_get_install_id();
                return new Guid(installIdString);
            }
        }

        /// <summary>
        /// Change the base URL (scheme + authority + port only) used to communicate with the backend.
        /// </summary>
        /// <param name="logUrl">Base URL to use for server communication.</param>
        public static void SetLogUrl(string logUrl)
        {
            MobileCenterInternal.mobile_center_unity_set_log_url(logUrl);
        }

        /// <summary>
        /// Check whether SDK has already been configured or not.
        /// </summary>
        public static bool Configured
        {
            get { return MobileCenterInternal.mobile_center_unity_is_configured(); }
        }

        /// <summary>
        /// Start services.
        /// This may be called only once per service per application process lifetime.
        /// </summary>
        /// <param name="services">List of services to use.</param>
        public static void Start(params Type[] services)
        {
            SetWrapperSdk();
            var nativeServiceTypes = ServicesToNativeTypes(services);
            InitializeServices(services);
            MobileCenterInternal.mobile_center_unity_start_services(nativeServiceTypes, services.Length);
            PostInitializeServices(services);
        }

        /// <summary>
        ///     Initialize the SDK with the list of services to start.
        ///     This may be called only once per application process lifetime.
        /// </summary>
        /// <param name="appSecret">A unique and secret key used to identify the application.</param>
        /// <param name="services">List of services to use.</param>
        public static void Start(string appSecret, params Type[] services)
        {
            SetWrapperSdk();
            var nativeServiceTypes = ServicesToNativeTypes(services);
            InitializeServices(services);
            appSecret = GetSecretForPlatform(appSecret);
            MobileCenterInternal.mobile_center_unity_start(appSecret, nativeServiceTypes, services.Length);
            PostInitializeServices(services);
        }

#if UNITY_IOS
        private static ServiceType[] ServicesToNativeTypes(Type[] services)
        {
            IntPtr[] classPointers = new IntPtr[services.Length];
            int currentIdx = 0;
            foreach (var serviceType in services)
            {
                IntPtr nativeType = (IntPtr)serviceType.GetMethod("GetNativeType").Invoke(null, null);
                classPointers[currentIdx++] = nativeType;
            }
            return classPointers;
        }
#elif UNITY_ANDROID
        private static ServiceType[] ServicesToNativeTypes(Type[] services)
        {
            var classClass = AndroidJNI.FindClass("java/lang/Class");
            var array = AndroidJNI.NewObjectArray(services.Length, classClass, classClass);
            int currentIdx = 0;
            foreach (var serviceType in services)
            {
                ServiceType nativeType = (ServiceType)serviceType.GetMethod("GetNativeType").Invoke(null, null);
                AndroidJNI.SetObjectArrayElement(array, currentIdx++, nativeType);
            }
            return new ServiceType[] { array };
        }
#elif UNITY_WSA_10_0
#pragma warning disable IDE0001
        private static ServiceType[] ServicesToNativeTypes(Type[] services)
#pragma warning restore IDE0001
        {
            //TODO after all namespaces are changed to be in Microsoft.Azure.Mobile.Unity,
            //TODO remove case where 'method == null'
            var nativeTypes = new ServiceType[services.Length];
            for (var i = 0; i < services.Length; ++i)
            {
                var method = services[i].GetMethod("GetNativeType");
                if (method == null)
                {
                    nativeTypes[i] = services[i];
                }
                else
                {
                    nativeTypes[i] = (ServiceType)method.Invoke(null, null);
                }
            }

            return nativeTypes;
        }
#else
        private static ServiceType[] ServicesToNativeTypes(Type[] services)
        {
            return null;
        }
#endif

        private static void InitializeServices(params Type[] services)
        {
            // TODO handle case where initialization has already occurred
            foreach (var serviceType in services)
            {
                var method = serviceType.GetMethod("Initialize");
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }

        private static void PostInitializeServices(params Type[] services)
        {
            // TODO handle case where post initialization has already occurred
            foreach (var serviceType in services)
            {
                var method = serviceType.GetMethod("PostInitialize");
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }

        /// <summary>
        /// Set the custom properties.
        /// </summary>
        /// <param name="customProperties">Custom properties object.</param>
        public static void SetCustomProperties(Unity.CustomProperties customProperties)
        {
            var rawCustomProperties = customProperties.GetRawObject();
            MobileCenterInternal.mobile_center_unity_set_custom_properties(rawCustomProperties);
        }

        private static void SetWrapperSdk()
        {
            MobileCenterInternal.mobile_center_unity_set_wrapper_sdk(WrapperSdk.WrapperSdkVersion,
                                                                     WrapperSdk.Name,
                                                                     WrapperSdk.WrapperRuntimeVersion, null, null, null);
        }


        // Gets the first instance of an app secret corresponding to the given platform name, or returns the string 
        // as-is if no identifier can be found.
        internal static string GetSecretForPlatform(string secrets)
        {
#if UNITY_IOS
            var platformIdentifier = "ios";
#elif UNITY_ANDROID
            var platformIdentifier = "android";
#elif UNITY_WSA_10_0
            var platformIdentifier = "uwp";
#else
            var platformIdentifier = "default";
#endif

            if (string.IsNullOrEmpty(secrets))
            {
                // error
                return secrets;
            }

            // If there are no equals signs, then there are no named identifiers
            if (!secrets.Contains("="))
            {
                return secrets;
            }

            var platformIndicator = platformIdentifier + "=";
            var secretIdx = secrets.IndexOf(platformIndicator, StringComparison.Ordinal);
            if (secretIdx == -1)
            {
                // error
                return secrets;
            }
            secretIdx += platformIndicator.Length;
            var platformSecret = string.Empty;

            while (secretIdx < secrets.Length)
            {
                var nextChar = secrets[secretIdx++];
                if (nextChar == ';')
                {
                    break;
                }

                platformSecret += nextChar;
            }

            return platformSecret;
        }
    }
}
