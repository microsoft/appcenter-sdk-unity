// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Analytics.Internal;

namespace Microsoft.AppCenter.Unity.Analytics
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Analytics
    {
        // An event can be lost due to low bandwidth or disk space constraints.     
        public const int PERSISTENCE_NORMAL = 0x01;
        
        // Used for events that should be prioritized over non-critical events.        
        public const int PERSISTENCE_CRITICAL = 0x02;

        // Used by App Center Unity Editor Extensions: https://github.com/Microsoft/AppCenter-SDK-Unity-Extension
        public const string AnalyticsSDKVersion = "1.0.0";

        public static void PrepareEventHandlers()
        {
            AnalyticsInternal.PrepareEventHandlers();
        }

        public static void AddNativeType(List<RawType> nativeTypes)
        {
            AnalyticsInternal.AddNativeType(nativeTypes);
        }

        public static void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            if (properties == null)
            {
                AnalyticsInternal.TrackEvent(eventName);
            }
            else
            {
                AnalyticsInternal.TrackEventWithProperties(eventName, properties);
            }
        }

        public static void TrackEvent(string eventName, IDictionary<string, string> properties, int flags)
        {
            AnalyticsInternal.TrackEventWithProperties(eventName, properties, flags);            
        }

        public static void TrackEvent(string eventName, EventProperties properties)
        {
            if (properties == null)
            {
                AnalyticsInternal.TrackEvent(eventName);
            }
            else
            {
                AnalyticsInternal.TrackEventWithProperties(eventName, properties);
            }
        }

        public static void TrackEvent(string eventName, EventProperties properties, int flags)
        {
            if (properties == null)
            {
                AnalyticsInternal.TrackEvent(eventName);
            }
            else
            {
                AnalyticsInternal.TrackEventWithProperties(eventName, properties, flags);
            }
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return AnalyticsInternal.IsEnabledAsync();
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
        {
            return AnalyticsInternal.SetEnabledAsync(enabled);
        }

        public static TransmissionTarget GetTransmissionTarget(string transmissionTargetToken)
        {
            if (string.IsNullOrEmpty(transmissionTargetToken))
            {
                return null;
            }
            var internalObject = AnalyticsInternal.GetTransmissionTarget(transmissionTargetToken);
            if (internalObject == null)
            {
                return null;
            }
            return new TransmissionTarget(internalObject);
        }

        public static void Pause()
        {
            AnalyticsInternal.Pause();
        }

        public static void Resume()
        {
            AnalyticsInternal.Resume();
        }
    }
}
