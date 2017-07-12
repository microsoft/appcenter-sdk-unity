#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
    class DistributeInternal
    {
        public static void Initialize()
        {
        }

        public static Type mobile_center_unity_distribute_get_type()
        {
            return typeof(Microsoft.Azure.Mobile.Distribute.Distribute);
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
