// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Reflection;
using Microsoft.Azure.Mobile.Unity.Internal;
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
            MobileCenterInternal.Configure(appSecret);
        }

        public static LogLevel LogLevel
        {
            get { return (LogLevel)MobileCenterInternal.GetLogLevel(); }
            set { MobileCenterInternal.SetLogLevel((int)value); }
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return MobileCenterInternal.SetEnabledAsync(enabled);
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return MobileCenterInternal.IsEnabledAsync();
        }

        /// <summary>
        /// Get the unique installation identifier for this application installation on this device.
        /// </summary>
        /// <remarks>
        /// The identifier is lost if clearing application data or uninstalling application.
        /// </remarks>
        public static MobileCenterTask<Guid?> GetInstallIdAsync()
        {
            var stringTask = MobileCenterInternal.GetInstallIdAsync();
            var guidTask = new MobileCenterTask<Guid?>();
            stringTask.ContinueWith(t => {
                var installId = !string.IsNullOrEmpty(t.Result) ? new Guid(t.Result) : (Guid?) null;
                guidTask.SetResult(installId);
            });
            return guidTask;
        }

        /// <summary>
        /// Change the base URL (scheme + authority + port only) used to communicate with the backend.
        /// </summary>
        /// <param name="logUrl">Base URL to use for server communication.</param>
        public static void SetLogUrl(string logUrl)
        {
            MobileCenterInternal.SetLogUrl(logUrl);
        }

        /// <summary>
        /// Check whether SDK has already been configured or not.
        /// </summary>
        public static bool Configured
        {
            get { return MobileCenterInternal.IsConfigured(); }
        }

        /// <summary>
        /// Start services.
        /// This may be called only once per service per application process lifetime.
        /// </summary>
        /// <param name="services">List of services to use.</param>
        public static void Start(params Type[] services)
        {
            InitializeServices(services);
            SetWrapperSdk();
            var nativeServiceTypes = ServicesToNativeTypes(services);
            MobileCenterInternal.StartServices(nativeServiceTypes, services.Length);
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
            InitializeServices(services);
            SetWrapperSdk();
            appSecret = GetSecretForPlatform(appSecret);
            var nativeServiceTypes = ServicesToNativeTypes(services);
            MobileCenterInternal.Start(appSecret, nativeServiceTypes, services.Length);
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

        internal static void InitializeServices(params Type[] services)
        {
            foreach (var serviceType in services)
            {
                var method = serviceType.GetMethod("Initialize");
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }

        internal static void PostInitializeServices(params Type[] services)
        {
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
            MobileCenterInternal.SetCustomProperties(rawCustomProperties);
        }

        private static void SetWrapperSdk()
        {
            MobileCenterInternal.SetWrapperSdk(WrapperSdk.WrapperSdkVersion,
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
#else
            var platformIdentifier = "uwp";
#endif
            if (secrets == null)
            {
                // If "secrets" is null, return that and let the error be dealt
                // with downstream.
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
                // If the platform indicator can't be found, return the original
                // string and let the error be dealt with downstream.
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
