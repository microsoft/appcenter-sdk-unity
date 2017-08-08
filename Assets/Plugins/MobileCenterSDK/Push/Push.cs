// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using Microsoft.Azure.Mobile.Unity.Push.Internal;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity.Push
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Push
    {
        public static event EventHandler<PushNotificationReceivedEventArgs> PushNotificationReceived;

        public static void Initialize()
        {
            PushInternal.Initialize();
        }

        public static void PostInitialize()
        {
            PushInternal.PostInitialize();
        }
        public static RawType GetNativeType()
        {
            return PushInternal.GetNativeType();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return PushInternal.IsEnabledAsync();
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return PushInternal.SetEnabledAsync(enabled);
        }

        public static void EnableFirebaseAnalytics()
        {
            PushInternal.EnableFirebaseAnalytics();
        }

        internal static void NotifyPushNotificationReceived(PushNotificationReceivedEventArgs e)
        {
            if (PushNotificationReceived != null)
            {
                Debug.Log("Pushnotification received callback not null");
                PushNotificationReceived.Invoke(null, e);
            }
        }
    }
}
