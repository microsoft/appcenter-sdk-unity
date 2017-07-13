// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using Windows.ApplicationModel.Activation;

namespace Microsoft.Azure.Mobile.Push.Unity.Internal
{
    using UWPPush = Microsoft.Azure.Mobile.Push.Push;

    class PushInternal
    {
        public static Type mobile_center_unity_push_get_type()
        {
            return typeof(UWPPush);
        }

        public static void mobile_center_unity_push_set_enabled(bool isEnabled)
        {
            UWPPush.Enabled = isEnabled;
        }

        public static bool mobile_center_unity_push_is_enabled()
        {
            return UWPPush.Enabled;
        }

        public static void mobile_center_unity_push_check_launched_from_notification(LaunchActivatedEventArgs e)
        {
            UWPPush.CheckLaunchedFromNotification(e)
        }

        public static void mobile_center_unity_push_enable_firebase_analytics()
        {
        }
    }
}
#endif
