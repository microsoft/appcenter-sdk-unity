#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
    class CrashesInternal
    {
        public static Type mobile_center_unity_crashes_get_type()
        {
            return typeof(Microsoft.Azure.Mobile.Crashes.Crashes);
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
