#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
    class CrashesInternal
    {
        public static Type GetType()
        {
            return typeof(Microsoft.Azure.Mobile.Crashes.Crashes);
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return MobileCenterTask<bool>.FromCompleted(false);
        }
    }
}
#endif
