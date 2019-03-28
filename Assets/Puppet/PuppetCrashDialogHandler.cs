// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using AOT;
using Microsoft.AppCenter.Unity.Crashes;
using System.Threading;
using UnityEngine;

public class PuppetCrashDialogHandler : MonoBehaviour
{
    public PuppetConfirmationDialog ConfirmationDialog;
    private static int shouldShowDialog;

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
        Interlocked.Exchange(ref shouldShowDialog, 1);
        return true;
    }

    void Update()
    {
        if (Interlocked.CompareExchange(ref shouldShowDialog, 0, 1) == 1)
        {
            ConfirmationDialog.Show();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test crash dialog")]
    void TestShowDialog()
    {
        Interlocked.Exchange(ref shouldShowDialog, 1);
    }
#endif
}
