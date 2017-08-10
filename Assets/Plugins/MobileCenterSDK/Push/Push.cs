// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
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
        private static IList<PushNotificationReceivedEventArgs> _unprocessedPushNotifications = new List<PushNotificationReceivedEventArgs>();
        private static readonly object _lockObject = new object();
        private static event EventHandler<PushNotificationReceivedEventArgs> _pushNotificationReceived;

        // Any notifications that were received before setting this handler for the first time 
        // will be sent to the first handler that is attached to this.
        public static event EventHandler<PushNotificationReceivedEventArgs> PushNotificationReceived
        {
            add
            {
                var replay = false;
                IList<PushNotificationReceivedEventArgs> unprocessedNotificationsCopy = null;
                lock (_lockObject)
                {
                    unprocessedNotificationsCopy = new List<PushNotificationReceivedEventArgs>(_unprocessedPushNotifications);
                    if (unprocessedNotificationsCopy != null)
                    {
                        replay = true;
                        _unprocessedPushNotifications = null;
                    }
                }
                _pushNotificationReceived += value;
                var eventCopy = _pushNotificationReceived;
                if (eventCopy != null && replay && unprocessedNotificationsCopy != null)
                {
                    foreach (var push in unprocessedNotificationsCopy)
                    {
                        eventCopy.Invoke(null, push);
                    }
                }
            }
            remove
            {
                _pushNotificationReceived -= value;
            }
        }

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
            // Make a copy of the event so that it isn't suddenly null at the time
            // of invoking
            var eventCopy = _pushNotificationReceived;
            if (eventCopy != null)
            {
                eventCopy.Invoke(null, e);
            }
            else
            {
                lock (_lockObject)
                {
                    if (_unprocessedPushNotifications != null)
                    {
                        _unprocessedPushNotifications.Add(e);
                    }
                }
            }
        }
    }
}
