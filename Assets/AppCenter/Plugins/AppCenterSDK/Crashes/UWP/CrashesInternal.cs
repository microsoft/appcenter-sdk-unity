#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Threading.Tasks;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    class CrashesInternal
    {
        public static Type GetNativeType()
        {
            return typeof(Microsoft.AppCenter.Crashes.Crashes);
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }
    }
}
#endif
