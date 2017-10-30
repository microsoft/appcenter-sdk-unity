// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using Microsoft.Azure.Mobile.Utils;
using Microsoft.AppCenter.Unity.Internal.Utils;

namespace Microsoft.AppCenter.Unity.Internal
{
    using UWPMobileCenter = Microsoft.Azure.Mobile.MobileCenter;

    class MobileCenterInternal
    {
        private static bool _prepared = false;
        private static object _lockObject = new object();

        public static void Configure(string appSecret)
        {
            Prepare();
            UWPMobileCenter.Configure(appSecret);
        }

        public static void Start(string appSecret, Type[] services, int numServices)
        {
            Prepare();
            UWPMobileCenter.Start(appSecret, services);
        }

        public static void StartServices(Type[] services, int numServices)
        {
            Prepare();
            UWPMobileCenter.Start(services);
        }

        public static void SetLogLevel(int logLevel)
        {
            Prepare();
            UWPMobileCenter.LogLevel = (Microsoft.Azure.Mobile.LogLevel)LogLevelFromUnity(logLevel);
        }

        public static int GetLogLevel()
        {
            Prepare();
            return (int)LogLevelFromUnity((int)UWPMobileCenter.LogLevel);
        }

        public static bool IsConfigured()
        {
            Prepare();
            return UWPMobileCenter.Configured;
        }

        public static void SetLogUrl(string logUrl)
        {
            Prepare();
            UWPMobileCenter.SetLogUrl(logUrl);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            Prepare();
            return new MobileCenterTask(UWPMobileCenter.SetEnabledAsync(isEnabled));
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            Prepare();
            return new MobileCenterTask<bool>(UWPMobileCenter.IsEnabledAsync());
        }

        public static MobileCenterTask<string> GetInstallIdAsync()
        {
            Prepare();
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
            Prepare();
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
                    return (int)Microsoft.AppCenter.Unity.LogLevel.Verbose;
                case Microsoft.Azure.Mobile.LogLevel.Debug:
                    return (int)Microsoft.AppCenter.Unity.LogLevel.Debug;
                case Microsoft.Azure.Mobile.LogLevel.Info:
                    return (int)Microsoft.AppCenter.Unity.LogLevel.Info;
                case Microsoft.Azure.Mobile.LogLevel.Warn:
                    return (int)Microsoft.AppCenter.Unity.LogLevel.Warn;
                case Microsoft.Azure.Mobile.LogLevel.Error:
                    return (int)Microsoft.AppCenter.Unity.LogLevel.Error;
                case Microsoft.Azure.Mobile.LogLevel.Assert:
                    return (int)Microsoft.AppCenter.Unity.LogLevel.Assert;
                case Microsoft.Azure.Mobile.LogLevel.None:
                    return (int)Microsoft.AppCenter.Unity.LogLevel.None;
                default:
                    return logLevel;
            }
        }

        private static Microsoft.Azure.Mobile.LogLevel LogLevelFromUnity(int logLevel)
        {
            switch ((Microsoft.AppCenter.Unity.LogLevel)logLevel)
            {
                case Microsoft.AppCenter.Unity.LogLevel.Verbose:
                    return Microsoft.Azure.Mobile.LogLevel.Verbose;
                case Microsoft.AppCenter.Unity.LogLevel.Debug:
                    return Microsoft.Azure.Mobile.LogLevel.Debug;
                case Microsoft.AppCenter.Unity.LogLevel.Info:
                    return Microsoft.Azure.Mobile.LogLevel.Info;
                case Microsoft.AppCenter.Unity.LogLevel.Warn:
                    return Microsoft.Azure.Mobile.LogLevel.Warn;
                case Microsoft.AppCenter.Unity.LogLevel.Error:
                    return Microsoft.Azure.Mobile.LogLevel.Error;
                case Microsoft.AppCenter.Unity.LogLevel.Assert:
                    return Microsoft.Azure.Mobile.LogLevel.Assert;
                case Microsoft.AppCenter.Unity.LogLevel.None:
                    return Microsoft.Azure.Mobile.LogLevel.None;
                default:
                    return (Microsoft.Azure.Mobile.LogLevel)logLevel;
            }
        }

        private static void Prepare()
        {
            lock (_lockObject)
            {
                if (!_prepared)
                {
                    UnityScreenSizeProvider.Initialize();
                    DeviceInformationHelper.SetScreenSizeProviderFactory(new UnityScreenSizeProviderFactory());

#if ENABLE_IL2CPP
#pragma warning disable 612
                    /**
                     * Workaround for known IL2CPP issue.
                     * See https://issuetracker.unity3d.com/issues/il2cpp-use-of-windows-dot-foundation-dot-collections-dot-propertyset-throws-a-notsupportedexception-on-uwp
                     *
                     * NotSupportedException: Cannot call method
                     * 'System.Boolean System.Runtime.InteropServices.WindowsRuntime.IMapToIDictionaryAdapter`2::System.Collections.Generic.IDictionary`2.TryGetValue(TKey,TValue&)'.
                     * IL2CPP does not yet support calling this projected method.
                     */
                    UnityApplicationSettings.Initialize();
                    UWPMobileCenter.SetApplicationSettingsFactory(new UnityApplicationSettingsFactory());

                    /**
                     * Workaround for another IL2CPP issue.
                     * System.Net.Http.HttpClient doesn't work properly so, replace to unity specific implementation.
                     */
                    UWPMobileCenter.SetChannelGroupFactory(new UnityChannelGroupFactory());
#pragma warning restore 612
#endif
                    _prepared = true;
                }
            }
        }
    }
}
#endif
