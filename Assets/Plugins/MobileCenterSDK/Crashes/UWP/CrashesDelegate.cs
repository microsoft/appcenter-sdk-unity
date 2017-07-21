#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
    public class CrashesDelegate
    {
        public static void mobile_center_unity_crashes_set_delegate()
        {
        }
    }
}
#endif
