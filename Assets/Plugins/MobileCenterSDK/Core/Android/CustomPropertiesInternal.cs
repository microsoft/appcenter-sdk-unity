// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity.Internal
{
	class CustomPropertiesInternal  {
        
        public static AndroidJavaObject mobile_center_unity_custom_properties_create()
        {
            return new AndroidJavaObject("com.microsoft.azure.mobile.CustomProperties");
        }

        public static void mobile_center_unity_custom_properties_set_string(AndroidJavaObject properties, string key, string val)
        {
            properties.Call<AndroidJavaObject>("set", key, val);
        }

		public static void mobile_center_unity_custom_properties_set_number(AndroidJavaObject properties, string key, AndroidJavaObject val)
		{
			properties.Call<AndroidJavaObject>("set", key, val);
		}

		public static void mobile_center_unity_custom_properties_set_bool(AndroidJavaObject properties, string key, bool val)
		{
			properties.Call<AndroidJavaObject>("set", key, val);
		}

		public static void mobile_center_unity_custom_properties_set_date(AndroidJavaObject properties, string key, AndroidJavaObject val)
		{
			properties.Call<AndroidJavaObject>("set", key, val);
		}

		public static void mobile_center_unity_custom_properties_clear(AndroidJavaObject properties, string key)
		{
			properties.Call<AndroidJavaObject>("clear", key);
		}
	}
}
#endif
