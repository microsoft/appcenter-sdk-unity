// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
	class CustomPropertiesInternal  {
        [DllImport("__Internal")]
        public static extern IntPtr mobile_center_unity_custom_properties_create();

		[DllImport("__Internal")]
        public static extern void mobile_center_unity_custom_properties_set_string(IntPtr properties, string key, string val);

		[DllImport("__Internal")]
		public static extern void mobile_center_unity_custom_properties_set_number(IntPtr properties, string key, IntPtr val);

		[DllImport("__Internal")]
		public static extern void mobile_center_unity_custom_properties_set_bool(IntPtr properties, string key, bool val);

		[DllImport("__Internal")]
		public static extern void mobile_center_unity_custom_properties_set_date(IntPtr properties, string key, IntPtr val);

		[DllImport("__Internal")]
		public static extern void mobile_center_unity_custom_properties_clear(IntPtr properties, string key);
	}
}
#endif
