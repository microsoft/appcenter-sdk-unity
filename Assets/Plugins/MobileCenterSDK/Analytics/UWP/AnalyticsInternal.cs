// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace Microsoft.Azure.Mobile.Unity.Analytics.Internal
{
	using UWPAnalytics = Microsoft.Azure.Mobile.Analytics.Analytics;

	class AnalyticsInternal
    {
        public static Type mobile_center_unity_analytics_get_type()
        {
            return typeof(Microsoft.Azure.Mobile.Analytics.Analytics);
        }

        public static void mobile_center_unity_analytics_track_event(string eventName)
        {
            UWPAnalytics.TrackEvent(eventName);
        }

        public static void mobile_center_unity_analytics_track_event_with_properties(string eventName, string[] keys, string[] values, int count)
        {
            var properties = new Dictionary<string, string>();
            for (var i = 0; i < count; ++i)
            {
                properties[keys[i]] = values[i];
            }
            UWPAnalytics.TrackEvent(eventName, properties);
		}

        public static void mobile_center_unity_analytics_set_enabled(bool isEnabled)
        {
            UWPAnalytics.Enabled = isEnabled;
        }

        public static bool mobile_center_unity_analytics_is_enabled()
        {
            return UWPAnalytics.Enabled;
        }
	}
}
#endif