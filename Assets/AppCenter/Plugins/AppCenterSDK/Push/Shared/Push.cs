// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Push.Internal;

namespace Microsoft.AppCenter.Unity.Push
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Push
    {
        // Used by App Center Unity Editor Extensions: https://github.com/Microsoft/AppCenter-SDK-Unity-Extension
        public const string PushSDKVersion = "1.1.0";
        private static readonly object _lockObject = new object();
        private static bool _needsReplay = true;

        private static event EventHandler<PushNotificationReceivedEventArgs> _pushNotificationReceived;

        // Any notifications that were received before setting this handler for the first time
        // will be sent to the first handler that is attached to this.
        public static event EventHandler<PushNotificationReceivedEventArgs> PushNotificationReceived
        {
            add
            {
                _pushNotificationReceived += value;

                // This won't cause a race condition because even if it's true,
                // we will double check inside the lock, and if it's false, its
                // value will never change. Just check outside to avoid waiting
                // for the lock unnecessarily.
                if (_needsReplay)
                {
                    var replay = false;
                    lock (_lockObject)
                    {
                        replay = _needsReplay;
                        _needsReplay = false;
                    }
                    if (replay)
                    {
                        PushInternal.ReplayUnprocessedPushNotifications();
                    }
                }
            }
            remove
            {
                _pushNotificationReceived -= value;
            }
        }

        public static void PrepareEventHandlers()
        {
            PushInternal.PrepareEventHandlers();
        }

        public static void AddNativeType(List<RawType> nativeTypes)
        {
            PushInternal.AddNativeType(nativeTypes);
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return PushInternal.IsEnabledAsync();
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
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
        }
    }
}
