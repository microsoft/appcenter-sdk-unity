#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
    class CrashesInternal
    {
        public static Type mobile_center_unity_crashes_get_type()
        {
            return typeof(Microsoft.Azure.Mobile.Crashes.Crashes);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            return new MobileCenterTask(Task.FromResult(0));
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return new MobileCenterTask<bool>(Task.FromResult(false));
        }
    }
}
#endif
