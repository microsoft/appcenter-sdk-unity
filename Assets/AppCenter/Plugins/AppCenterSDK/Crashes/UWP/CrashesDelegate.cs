#if UNITY_WSA_10_0 && !UNITY_EDITOR

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class CrashesDelegate
    {
        public static void SetDelegate()
        {
        }

        public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
        {
        }
    }
}
#endif
