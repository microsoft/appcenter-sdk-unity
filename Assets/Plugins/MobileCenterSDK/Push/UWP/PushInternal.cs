// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using Windows.ApplicationModel.Activation;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    using UWPPush = Microsoft.Azure.Mobile.Push.Push;

    class PushInternal
    {
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
                Push.NotifyPushNotificationReceived(eventArgs);
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

        public static void CheckLaunchedFromNotification(LaunchActivatedEventArgs e)
        {
            UWPPush.CheckLaunchedFromNotification(e);
        }

        public static void EnableFirebaseAnalytics()
        {
        }

        internal static void ReplayUnprocessedPushNotifications()
        {
            //TODO implement me
        }
    }
}
#endif
