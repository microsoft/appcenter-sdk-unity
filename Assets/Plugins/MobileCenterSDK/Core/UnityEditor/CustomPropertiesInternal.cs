#if UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity.Internal
{
#if UNITY_IOS
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = System.Object;
#endif

    class CustomPropertiesInternal  {

		public static RawType mobile_center_unity_custom_properties_create()
        {
            return default(RawType);
        }

        public static void mobile_center_unity_custom_properties_set_string(RawType properties, string key, string val)
        {
        }

		public static void mobile_center_unity_custom_properties_set_number(RawType properties, string key, object val)
        {
        }

		public static void mobile_center_unity_custom_properties_set_bool(RawType properties, string key, bool val)
        {
        }

		public static void mobile_center_unity_custom_properties_set_date(RawType properties, string key, object val)
        {
        }

        public static void mobile_center_unity_custom_properties_clear(RawType properties, string key)
        {
        }
	}
}
#endif
