// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    using UWPPush = Microsoft.Azure.Mobile.Push.Push;
    using WSAApplication = UnityEngine.WSA.Application;

    class PushInternal
    {
        public static List<PushNotificationReceivedEventArgs> _unprocessedPushNotifications = new List<PushNotificationReceivedEventArgs>();
        public static readonly object _lockObject = new object();

        public static void Initialize()
        {
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

        public static void PostInitialize()
        {
            UWPPush.CheckLaunchedFromNotification(WSAApplication.arguments);
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

        public static void CheckLaunchedFromNotification(LaunchActivatedEventArgs e)
        {
            UWPPush.CheckLaunchedFromNotification(e);
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
