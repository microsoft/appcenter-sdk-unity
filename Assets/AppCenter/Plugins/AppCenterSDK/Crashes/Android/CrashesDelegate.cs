﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class CrashesDelegate : AndroidJavaProxy
    {
        public static event Crashes.SendingErrorReportHandler SendingErrorReport;

        public static event Crashes.SentErrorReportHandler SentErrorReport;

        public static event Crashes.FailedToSendErrorReportHandler FailedToSendErrorReport;

        private CrashesDelegate() : base("com.microsoft.appcenter.crashes.CrashesListener")
        {
        }

        public static void SetDelegate()
        {
            var crashes = new AndroidJavaClass("com.microsoft.appcenter.crashes.Crashes");
            crashes.CallStatic("setListener", new CrashesDelegate());
        }

        //TODO bind error report; implement these
        void onBeforeSending(AndroidJavaObject report)
        {
        }

        void onSendingFailed(AndroidJavaObject report, AndroidJavaObject exception)
        {
        }

        void onSendingSucceeded(AndroidJavaObject report)
        {
        }

        public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
        {
        }

        public static void SetGetErrorAttachmentsHandler(Crashes.GetErrorAttachmentsHandler handler)
        {
        }
    }
}
#endif
