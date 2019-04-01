// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using Microsoft.AppCenter.Unity.Internal.Utility;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class EventPropertiesInternal
    {
        public static IntPtr Create()
        {
            return appcenter_unity_analytics_create_event_properties();
        }

        public static void SetString(IntPtr properties, string key, string val)
        {
            appcenter_unity_analytics_event_properties_set_string(properties, key, val);
        }

        public static void SetNumber(IntPtr properties, string key, long val)
        {
            appcenter_unity_analytics_event_properties_set_long(properties, key, val);
        }

        public static void SetNumber(IntPtr properties, string key, double val)
        {
            appcenter_unity_analytics_event_properties_set_double(properties, key, val);
        }

        public static void SetBool(IntPtr properties, string key, bool val)
        {
            appcenter_unity_analytics_event_properties_set_bool(properties, key, val);
        }

        public static void SetDate(IntPtr properties, string key, DateTime val)
        {
            appcenter_unity_analytics_event_properties_set_date(properties, key, NSDateHelper.DateTimeConvert(val));
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_analytics_create_event_properties();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_event_properties_set_string(IntPtr properties, string key, string val);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_event_properties_set_long(IntPtr properties, string key, long val);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_event_properties_set_double(IntPtr properties, string key, double val);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_event_properties_set_bool(IntPtr properties, string key, bool val);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_analytics_event_properties_set_date(IntPtr properties, string key, IntPtr val);
#endregion
    }
}
#endif
