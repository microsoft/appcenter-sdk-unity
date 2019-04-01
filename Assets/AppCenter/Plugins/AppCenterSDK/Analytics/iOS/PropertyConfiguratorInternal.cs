// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using AOT;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.AppCenter.Unity.Internal.Utility;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    public class PropertyConfiguratorInternal
    {
        public static void SetAppName(IntPtr propertyConfigurator, string appName)
        {
            appcenter_unity_property_configurator_set_app_name(propertyConfigurator, appName);
        }

        public static void SetUserId(IntPtr propertyConfigurator, string userId)
        {
            appcenter_unity_property_configurator_set_user_id(propertyConfigurator, userId);
        }

        public static void SetAppVersion(IntPtr propertyConfigurator, string appVersion)
        {
            appcenter_unity_property_configurator_set_app_version(propertyConfigurator, appVersion);
        }

        public static void SetAppLocale(IntPtr propertyConfigurator, string appLocale)
        {
            appcenter_unity_property_configurator_set_app_locale(propertyConfigurator, appLocale);
        }

        public static void SetEventProperty(IntPtr propertyConfigurator, string key, string value)
        {
            appcenter_unity_property_configurator_set_event_property(propertyConfigurator, key, value);
        }

        public static void SetEventProperty(IntPtr propertyConfigurator, string key, DateTime value)
        {
            appcenter_unity_property_configurator_set_event_datetime_property(propertyConfigurator, key, NSDateHelper.DateTimeConvert(value));
        }

        public static void SetEventProperty(IntPtr propertyConfigurator, string key, long value)
        {
            appcenter_unity_property_configurator_set_event_long_property(propertyConfigurator, key, value);
        }

        public static void SetEventProperty(IntPtr propertyConfigurator, string key, double value)
        {
            appcenter_unity_property_configurator_set_event_double_property(propertyConfigurator, key, value);
        }

        public static void SetEventProperty(IntPtr propertyConfigurator, string key, bool value)
        {            
            appcenter_unity_property_configurator_set_event_bool_property(propertyConfigurator, key, value);
        }

        public static void CollectDeviceId(IntPtr propertyConfigurator)
        {
            appcenter_unity_property_configurator_collect_device_id(propertyConfigurator);
        }

        public static void RemoveEventProperty(IntPtr propertyConfigurator, string key)
        {
            appcenter_unity_property_configurator_remove_event_property(propertyConfigurator, key);
        }

#region External

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_app_name(IntPtr propertyConfigurator, string appName);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_user_id(IntPtr propertyConfigurator, string userId);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_app_version(IntPtr propertyConfigurator, string appVersion);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_app_locale(IntPtr propertyConfigurator, string appLocale);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_event_property(IntPtr propertyConfigurator, string key, string value);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_event_datetime_property(IntPtr propertyConfigurator, string key, IntPtr value);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_event_long_property(IntPtr propertyConfigurator, string key, long value);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_event_double_property(IntPtr propertyConfigurator, string key, double value);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_set_event_bool_property(IntPtr propertyConfigurator, string key, bool value);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_collect_device_id(IntPtr propertyConfigurator);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_property_configurator_remove_event_property(IntPtr propertyConfigurator, string key);

#endregion
    }
}
#endif