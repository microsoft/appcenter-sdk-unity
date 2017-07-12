#if UNITY_EDITOR
using System;
namespace Microsoft.Azure.Mobile.Crashes.Internal
{
	class CrashesInternal
    {
        public static IntPtr mobile_center_unity_crashes_get_type()
        {
            return IntPtr.Zero;
        }

		public static void mobile_center_unity_crashes_set_enabled(bool isEnabled)
		{
		}

		public static bool mobile_center_unity_crashes_is_enabled()
		{
            return false;
		}
	}
}
#endif
