// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using System.Reflection;
namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    using UWPPush = Microsoft.Azure.Mobile.Push.Push;
    using WSAApplication = UnityEngine.WSA.Application;

    class PushInternal
    {
        public static List<PushNotificationReceivedEventArgs> _unprocessedPushNotifications = new List<PushNotificationReceivedEventArgs>();
        public static readonly object _lockObject = new object();
        
        public static void PrepareEventHandlers()
        {
            MobileCenterBehavior.InitializingServices += Initialize;
        }

        private static void Initialize()
        {
            Utils.ApplicationLifecycleHelper.Instance.ApplicationResuming += (s, e) =>
            {
                WSAApplication.InvokeOnAppThread(new UnityEngine.WSA.AppCallbackItem(() => {
                    //TODO make version correspond to wrapper sdk version
                    var starterTypeName = "MobileCenterUWPStarter.MobileCenterUWPStarter, MobileCenterUWPStarter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime";
                    var starterType = Type.GetType(starterTypeName);
                    if (starterType == null)
                    {
                        starterType = Type.GetType("MobileCenterUWPStarter.MobileCenterUWPStarter");
                    }
                    var launchArgs = starterType.GetMethod("GetLaunchArgs").Invoke(null, null) as string;
                    if (!string.IsNullOrEmpty(launchArgs))
                    {
                        UWPPush.Instance.InstanceCheckLaunchedFromNotification(launchArgs);
                    }
                }), false);
            };
            UWPPush.PushNotificationReceived += (sender, e) =>
            {
                var eventArgs = new PushNotificationReceivedEventArgs
                {
                    Message = e.Message,
                    Title = e.Title,
                    CustomData = e.CustomData
                };
                HandlePushNotification(eventArgs);
            };
        }

        public static Type GetNativeType()
        {
            return typeof(UWPPush);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            return new MobileCenterTask(UWPPush.SetEnabledAsync(isEnabled));
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return new MobileCenterTask<bool>(UWPPush.IsEnabledAsync());
        }

        public static void HandlePushNotification(PushNotificationReceivedEventArgs eventArgs)
        {
            lock (_lockObject)
            {
                if (_unprocessedPushNotifications != null)
                {
                    _unprocessedPushNotifications.Add(eventArgs);

                    // Don't want to invoke push callback inside lock.
                    eventArgs = null;
                }
            }

            // If eventArgs isn't null, it must not have been added to queue.
            if (eventArgs != null)
            {
                Push.NotifyPushNotificationReceived(eventArgs);
            }
        }

        public static void EnableFirebaseAnalytics()
        {
        }

        internal static void ReplayUnprocessedPushNotifications()
        {
            List<PushNotificationReceivedEventArgs> unprocessedPushNotificationsCopy = null;
            lock (_lockObject)
            {
                if (_unprocessedPushNotifications != null)
                {
                    // Don't want to invoke push callback inside lock, so make
                    // a copy.
                    unprocessedPushNotificationsCopy = new List<PushNotificationReceivedEventArgs>(_unprocessedPushNotifications);
                    _unprocessedPushNotifications = null;
                }
            }
            if (unprocessedPushNotificationsCopy != null)
            {
                foreach (var notification in unprocessedPushNotificationsCopy)
                {
                    Push.NotifyPushNotificationReceived(notification);
                }
            }
        }
    }
}
#endif
