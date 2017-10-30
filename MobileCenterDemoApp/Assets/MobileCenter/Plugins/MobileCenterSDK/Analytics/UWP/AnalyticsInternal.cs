// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    using UWPAnalytics = Microsoft.Azure.Mobile.Analytics.Analytics;

    class AnalyticsInternal
    {
        public static void PrepareEventHandlers()
        {
        }

        public static Type GetNativeType()
        {
            return typeof(Microsoft.Azure.Mobile.Analytics.Analytics);
        }

        public static void TrackEvent(string eventName)
        {
            UWPAnalytics.TrackEvent(eventName);
        }

        public static void TrackEventWithProperties(string eventName, string[] keys, string[] values, int count)
        {
            var properties = new Dictionary<string, string>();
            for (var i = 0; i < count; ++i)
            {
                properties[keys[i]] = values[i];
            }
            UWPAnalytics.TrackEvent(eventName, properties);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            return new MobileCenterTask(UWPAnalytics.SetEnabledAsync(isEnabled));
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return new MobileCenterTask<bool>(UWPAnalytics.IsEnabledAsync());
        }
    }
}
#endif