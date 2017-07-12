#if !UNITY_WSA_10_0 || UNITY_EDITOR
using Microsoft.Azure.Mobile.Crashes.Internal;
using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Crashes
{
    public class ErrorReport
    {
        
    }

	public class Crashes
	{
        public static void Initialize()
        {
			CrashesDelegate.mobile_center_unity_crashes_set_delegate();
		}

		public static IntPtr GetNativeType()
		{
            return CrashesInternal.mobile_center_unity_crashes_get_type();
		}

        public static bool Enabled
        {
            get
            {
                return CrashesInternal.mobile_center_unity_crashes_is_enabled();
            }
            set
            {
                CrashesInternal.mobile_center_unity_crashes_set_enabled(value);
            }
        }


	}
}
#endif
