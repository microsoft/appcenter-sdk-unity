﻿#if UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
#if UNITY_IOS || UNITY_ANDROID
	using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif
    
	class DistributeInternal
    {
        public static void Initialize()
        {
        }

        public static RawType mobile_center_unity_distribute_get_type()
        {
            return default(RawType);
        }

        public static void mobile_center_unity_distribute_set_enabled(bool isEnabled)
        {
        }

		public static bool mobile_center_unity_distribute_is_enabled()
        {
            return false;
        }

		public static void mobile_center_unity_distribute_set_install_url(string installUrl)
        {
        }

		public static void mobile_center_unity_distribute_set_api_url(string apiUrl)
        {
        }

		public static void mobile_center_unity_distribute_notify_update_action(int updateAction)
        {
        }
	}
}
#endif
