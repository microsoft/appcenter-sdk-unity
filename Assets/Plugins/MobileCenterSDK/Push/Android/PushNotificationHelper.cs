// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Microsoft.Azure.Mobile.Unity.Internal.Utility;

namespace Microsoft.Azure.Mobile.Unity.Push.Internal
{
    class PushNotificationHelper
    {
        public static PushNotificationReceivedEventArgs PushConvert(AndroidJavaObject javaPush)
        {
            var customDataMap = javaPush.Call<AndroidJavaObject>("getCustomData");
            var customData = JavaStringMapHelper.JavaMapConvert(customDataMap);
            var title = javaPush.Call<string>("getTitle");
            var message = javaPush.Call<string>("getMessage");
            return new PushNotificationReceivedEventArgs
            {
                CustomData = customData,
                Message = message,
                Title = title
            };
        }
	}
}
#endif
