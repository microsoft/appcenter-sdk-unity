#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using Windows.ApplicationModel.Activation;

namespace Microsoft.Azure.Mobile.Push.Internal
{
    using UWPMobileCenter = Microsoft.Azure.Mobile.MobileCenter;

    class PushInternal
    {
		public static IntPtr mobile_center_unity_push_get_type()
        {
			return (IntPtr)UWPMobileCenter.GetType();
		}

        public static void mobile_center_unity_push_set_enabled(bool isEnabled)
        {
            UWPMobileCenter.Enabled = isEnabled;
        }

        public static bool mobile_center_unity_push_is_enabled()
        {
			return UWPMobileCenter.Enabled;
		}

        public static void mobile_center_unity_push_check_launched_from_notification(LaunchActivatedEventArgs e)
        {
            UWPMobileCenter.CheckLaunchedFromNotification(e)
        }
	}
}
#endif
