// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Reflection;
using Microsoft.AppCenter.Unity.Internal;
using UnityEngine;

namespace Microsoft.AppCenter.Unity
{
#if UNITY_IOS || UNITY_ANDROID
    using ServiceType = System.IntPtr;
#else
    using ServiceType = System.Type;
#endif

    public class MobileCenter
    {
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
            stringTask.ContinueWith(t =>
            {
                var installId = !string.IsNullOrEmpty(t.Result) ? new Guid(t.Result) : (Guid?)null;
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

#if UNITY_IOS
        public static ServiceType[] ServicesToNativeTypes(Type[] services)
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
        public static ServiceType[] ServicesToNativeTypes(Type[] services)
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
        public static ServiceType[] ServicesToNativeTypes(Type[] services)
        {
            //TODO after all namespaces are changed to be in Microsoft.AppCenter.Unity,
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
        public static ServiceType[] ServicesToNativeTypes(Type[] services)
        {
            return null;
        }
#endif

        /// <summary>
        /// Set the custom properties.
        /// </summary>
        /// <param name="customProperties">Custom properties object.</param>
        public static void SetCustomProperties(Unity.CustomProperties customProperties)
        {
            var rawCustomProperties = customProperties.GetRawObject();
            MobileCenterInternal.SetCustomProperties(rawCustomProperties);
        }

        public static void SetWrapperSdk()
        {
            MobileCenterInternal.SetWrapperSdk(WrapperSdk.WrapperSdkVersion,
                                               WrapperSdk.Name,
                                               WrapperSdk.WrapperRuntimeVersion, null, null, null);
        }

        // Gets the first instance of an app secret corresponding to the given platform name, or returns the string
        // as-is if no identifier can be found.
        public static string GetSecretForPlatform(string secrets)
        {
            var platformIdentifier = GetPlatformIdentifier();
            if (platformIdentifier == null)
            {
                // Return as is for unsupported platform.
                return secrets;
            }
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

        private static string GetPlatformIdentifier()
        {
#if UNITY_IOS
            return "ios";
#elif UNITY_ANDROID
            return "android";
#elif UNITY_WSA_10_0
            return "uwp";
#else
            return null;
#endif
        }
    }
}
