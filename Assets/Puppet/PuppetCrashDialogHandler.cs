using AOT;
using Microsoft.AppCenter.Unity.Crashes;
using UnityEngine;

public class PuppetCrashDialogHandler : MonoBehaviour
{
    public PuppetConfirmationDialog ConfirmationDialog;

    private static object uiLocker = new object();
    private static bool shouldShowDialog;

    private void Awake()
    {
        Crashes.ShouldAwaitUserConfirmation = UserConfirmationHandler;
        Crashes.ShouldProcessErrorReport = PuppetCrashes.ShouldProcessErrorReportHandler;
        Crashes.GetErrorAttachments = PuppetCrashes.GetErrorAttachmentstHandler;
        Crashes.SendingErrorReport += PuppetCrashes.SendingErrorReportHandler;
        Crashes.SentErrorReport += PuppetCrashes.SentErrorReportHandler;
        Crashes.FailedToSendErrorReport += PuppetCrashes.FailedToSendErrorReportHandler;
    }

    [MonoPInvokeCallback(typeof(Crashes.UserConfirmationHandler))]
    public static bool UserConfirmationHandler()
    {
        shouldShowDialog = true;
        return true;
    }

    void Update()
    {
        if (shouldShowDialog)
        {
            lock (uiLocker)
            {
                ConfirmationDialog.Show();
                shouldShowDialog = false;
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test crash dialog")]
    void TestShowDialog()
    {
        shouldShowDialog = true;
    }
#endif
}
