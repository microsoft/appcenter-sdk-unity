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

        public static void SetGetErrorAttachmentsHandler(Crashes.GetErrorAttachmentsHandler handler)
        {
        }

        public static void SetSendingErrorReportHandler(Crashes.SendingErrorReportHandler handler)
        {
        }

        public static void SetSentErrorReportHandler(Crashes.SentErrorReportHandler handler)
        {
        }

        public static void SetFailedToSendErrorReportHandler(Crashes.FailedToSendErrorReportHandler handler)
        {
        }
    }
}
#endif
