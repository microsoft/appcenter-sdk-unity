#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using Microsoft.Azure.Mobile.Unity.Internal.Utility;

namespace Microsoft.Azure.Mobile.Push.Internal
{
    class PushNotificationHelper
    {
        [DllImport("__Internal")]
		private static extern string mobile_center_unity_push_notification_get_title(IntPtr push);

		[DllImport("__Internal")]
		private static extern string mobile_center_unity_push_notification_get_message(IntPtr push);

		[DllImport("__Internal")]
		private static extern IntPtr mobile_center_unity_push_notification_get_custom_data(IntPtr push);

		public static PushNotificationReceivedEventArgs PushConvert(IntPtr nativePush)
        {
            var title = mobile_center_unity_push_notification_get_title(nativePush);
            var message = mobile_center_unity_push_notification_get_message(nativePush);
            var customDataNSDict = mobile_center_unity_push_notification_get_custom_data(nativePush);
            var customData = NSStringDictionaryHelper.NSDictionaryConvert(customDataNSDict);
            return new PushNotificationReceivedEventArgs
            {
                CustomData = customData,
                Message = message,
                Title = title
            }; 
        }
	}
}
#endif
