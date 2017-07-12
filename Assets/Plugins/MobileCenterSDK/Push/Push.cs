// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if !UNITY_WSA_10_0 || UNITY_EDITOR

using System;
using Microsoft.Azure.Mobile.Push.Internal;

namespace Microsoft.Azure.Mobile.Push
{
	public class Push
	{        
        public static event EventHandler<PushNotificationReceivedEventArgs> PushNotificationReceived;

        public static void Initialize()
        {
            PushInternal.Initialize();
        }

        public static IntPtr GetNativeType()
        {
            return PushInternal.mobile_center_unity_push_get_type();
        }

        public static bool Enabled {
            get
            {
                return PushInternal.mobile_center_unity_push_is_enabled();
            }
            set
            {
                PushInternal.mobile_center_unity_push_set_enabled(value);
            }
        }

        public static void EnableFirebaseAnalytics()
        {
            PushInternal.mobile_center_unity_push_enable_firebase_analytics();
        }

        internal static void NotifyPushNotificationReceived(PushNotificationReceivedEventArgs e)
        {
            if (PushNotificationReceived != null)
            {
                PushNotificationReceived.Invoke(null, e);
            }
        }
	}
}
#endif
