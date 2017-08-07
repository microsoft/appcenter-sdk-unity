// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

using Microsoft.Azure.Mobile.Internal.Utility.NSDateHelper;
using Microsoft.Azure.Mobile.Internal.Utility.NSNumberHelper;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
    class CustomPropertiesInternal
    {
        public static IntPtr Create()
        {
            return mobile_center_unity_custom_properties_create();
        }

        public static void SetString(IntPtr properties, string key, string val)
        {
            mobile_center_unity_custom_properties_set_string(properties, key, val);
        }

        public static void SetNumber(IntPtr properties, string key, object val)
        {
            mobile_center_unity_custom_properties_set_number(properties, key, NSNumberHelper.Convert(val));
        }

        public static void SetBool(IntPtr properties, string key, bool val)
        {
            mobile_center_unity_custom_properties_set_bool(properties, key, val);
        }

        public static void SetDate(IntPtr properties, string key, DateTime val)
        {
            mobile_center_unity_custom_properties_set_date(properties, key, NSDateHelper.DateTimeConvert(val));
        }

        public static void Clear(IntPtr properties, string key)
        {
            mobile_center_unity_custom_properties_clear(properties, key);
        }

##region External

        [DllImport("__Internal")]
        private static extern IntPtr mobile_center_unity_custom_properties_create();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_custom_properties_set_string(IntPtr properties, string key, string val);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_custom_properties_set_number(IntPtr properties, string key, IntPtr val);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_custom_properties_set_bool(IntPtr properties, string key, bool val);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_custom_properties_set_date(IntPtr properties, string key, IntPtr val);

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_custom_properties_clear(IntPtr properties, string key);

#endregion
    }
}
#endif
