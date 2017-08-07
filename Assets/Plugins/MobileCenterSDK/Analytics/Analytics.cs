// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Mobile.Unity.Analytics.Internal;

namespace Microsoft.Azure.Mobile.Unity.Analytics
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Analytics
    {
        public static void PostInitialize()
        {
            AnalyticsInternal.PostInitialize();
        }

        public static RawType GetNativeType()
        {
            return AnalyticsInternal.GetType();
        }

        public static void TrackEvent(string eventName, IDictionary<string, string> properties = null)
        {
            if (properties == null)
            {
                AnalyticsInternal.TrackEvent(eventName);
            }
            else
            {
                AnalyticsInternal.TrackEventWithProperties(eventName, properties.Keys.ToArray(), properties.Values.ToArray(), properties.Count);
            }
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return AnalyticsInternal.IsEnabledAsync();
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return AnalyticsInternal.SetEnabledAsync(enabled);
        }
    }
}
