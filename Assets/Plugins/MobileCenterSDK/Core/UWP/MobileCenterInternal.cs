// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using Microsoft.Azure.Mobile.Utils;
using Microsoft.Azure.Mobile.Unity.Internal.Utils;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
    using UWPMobileCenter = Microsoft.Azure.Mobile.MobileCenter;

    class MobileCenterInternal 
    {
        private static bool _needsSetScreenProvider = true;
        private static object _lockObject = new object();

        public static void Configure(string appSecret)
        {
            PrepareScreenSizeProvider();
            UWPMobileCenter.Configure(appSecret);
        }

        public static void Start(string appSecret, Type[] services, int numServices)
        {
            PrepareScreenSizeProvider();
            UWPMobileCenter.Start(appSecret, services);
        }

        public static void StartServices(Type[] services, int numServices)
        {
            PrepareScreenSizeProvider();
            UWPMobileCenter.Start(services);
        }

        public static void SetLogLevel(int logLevel)
        {
            UWPMobileCenter.LogLevel = (Microsoft.Azure.Mobile.LogLevel)LogLevelFromUnity(logLevel);
        }

        public static int GetLogLevel()
        {
            return (int)LogLevelFromUnity((int)UWPMobileCenter.LogLevel);
        }

        public static bool IsConfigured()
        {
            return UWPMobileCenter.Configured;
        }

        public static void SetLogUrl(string logUrl)
        {
            UWPMobileCenter.SetLogUrl(logUrl);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            return new MobileCenterTask(UWPMobileCenter.SetEnabledAsync(isEnabled));
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return new MobileCenterTask<bool>(UWPMobileCenter.IsEnabledAsync());
        }

        public static MobileCenterTask<string> GetInstallIdAsync()
        {
            var installIdTask = UWPMobileCenter.GetInstallIdAsync();
            var stringTask = new MobileCenterTask<string>();
            installIdTask.ContinueWith(t => {
                var installId = t.Result?.ToString();
                stringTask.SetResult(installId);
            });
            return stringTask;
        }

        public static void SetCustomProperties(object properties)
        {
            var uwpProperties = properties as Microsoft.Azure.Mobile.CustomProperties;
            UWPMobileCenter.SetCustomProperties(uwpProperties);
        }

        public static void SetWrapperSdk(string wrapperSdkVersion,
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

        private static void PrepareScreenSizeProvider()
        {
            lock (_lockObject)
            {
                if (_needsSetScreenProvider)
                {
                    UnityScreenSizeProvider.Initialize();
                    DeviceInformationHelper.SetScreenSizeProviderFactory(new UnityScreenSizeProviderFactory());
                    _needsSetScreenProvider = false;
                }
            }
        }
    }
}
#endif
