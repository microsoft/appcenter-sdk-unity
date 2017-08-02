// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
    using UWPCustomProperties = Microsoft.Azure.Mobile.CustomProperties;

    class CustomPropertiesInternal
    {
       public static object mobile_center_unity_custom_properties_create()
        {
            return new UWPCustomProperties();
        }

        public static void mobile_center_unity_custom_properties_set_string(object properties, string key, string val)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Set(key, val);
        }

        public static void mobile_center_unity_custom_properties_set_number(object properties, string key, int val)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Set(key, val);
        }

        public static void mobile_center_unity_custom_properties_set_number(object properties, string key, long val)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Set(key, val);
        }

        public static void mobile_center_unity_custom_properties_set_number(object properties, string key, float val)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Set(key, val);
        }

        public static void mobile_center_unity_custom_properties_set_number(object properties, string key, double val)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Set(key, val);
        }

        public static void mobile_center_unity_custom_properties_set_bool(object properties, string key, bool val)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Set(key, val);
        }

        public static void mobile_center_unity_custom_properties_set_date(object properties, string key, DateTime val)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Set(key, val);
        }

        public static void mobile_center_unity_custom_properties_clear(object properties, string key)
        {
            var uwpProperties = properties as UWPCustomProperties;
            uwpProperties.Clear(key);
        }
    }
}
#endif
