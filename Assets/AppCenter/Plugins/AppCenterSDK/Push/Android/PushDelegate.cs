// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Push.Internal
{
    class PushDelegate : AndroidJavaProxy
    {
        public PushDelegate() : base("com.microsoft.appcenter.push.PushListener")
        {
        }

        void onPushNotificationReceived(AndroidJavaObject activity, AndroidJavaObject pushNotification)
        {
            var eventArgs = PushNotificationHelper.PushConvert(pushNotification);
            Push.NotifyPushNotificationReceived(eventArgs);
        }
    }
}
#endif
