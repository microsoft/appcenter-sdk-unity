// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class CrashesDelegate : AndroidJavaProxy
    {
        public static event Crashes.SendingErrorReportHandler SendingErrorReport;
        public static event Crashes.SentErrorReportHandler SentErrorReport;
        public static event Crashes.FailedToSendErrorReportHandler FailedToSendErrorReport;
        internal static Crashes.GetErrorAttachmentsHandler GetErrorAttachmentsHandler;
        private static Crashes.UserConfirmationHandler shouldAwaitUserConfirmationHandler = null;
        private static Crashes.ShouldProcessErrorReportHandler shouldProcessErrorReportHandler = null;
        private static readonly CrashesDelegate instance = new CrashesDelegate();

        private CrashesDelegate() : base("com.microsoft.appcenter.crashes.CrashesListener")
        {
        }

        public static void SetDelegate()
        {
            var crashes = new AndroidJavaClass("com.microsoft.appcenter.crashes.Crashes");
            crashes.CallStatic("setListener", instance);
        }

        public void onBeforeSending(AndroidJavaObject report)
        {
            var handlers = SendingErrorReport;
            if (handlers != null)
            {
                var errorReport = JavaObjectsConverter.ConvertErrorReport(report);
                handlers.Invoke(errorReport);
            }
        }

        public void onSendingFailed(AndroidJavaObject report, AndroidJavaObject exception)
        {
            var handlers = FailedToSendErrorReport;
            if (handlers != null)
            {
                var errorReport = JavaObjectsConverter.ConvertErrorReport(report);
                var exceptionStackTrace = exception.Call<string>("getStackTrace");
                var failCause = JavaObjectsConverter.ConvertException(exceptionStackTrace);
                handlers.Invoke(errorReport, failCause);
            }
        }

        public void onSendingSucceeded(AndroidJavaObject report)
        {
            var handlers = SentErrorReport;
            if (handlers != null)
            {
                var errorReport = JavaObjectsConverter.ConvertErrorReport(report);
                handlers.Invoke(errorReport);
            }
        }

        public bool shouldProcess(AndroidJavaObject report)
        {
            var handler = shouldProcessErrorReportHandler;
            if (handler != null)
            {
                return handler.Invoke(JavaObjectsConverter.ConvertErrorReport(report));
            }
            return true;
        }

        public bool shouldAwaitUserConfirmation()
        {
            var handler = shouldAwaitUserConfirmationHandler;
            if (handler != null)
            {
                return handler.Invoke();
            }
            return false;
        }

        public AndroidJavaObject getErrorAttachments(AndroidJavaObject report)
        {
            if (GetErrorAttachmentsHandler != null)
            {
                var logs = GetErrorAttachmentsHandler(JavaObjectsConverter.ConvertErrorReport(report));
                return JavaObjectsConverter.ToJavaAttachments(logs);
            }
            else
            {
                return null;
            }
        }

        public static void SetShouldAwaitUserConfirmationHandler(Crashes.UserConfirmationHandler handler)
        {
            shouldAwaitUserConfirmationHandler = handler;
        }

        public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
        {
            shouldProcessErrorReportHandler = handler;
        }

        public static void SetGetErrorAttachmentsHandler(Crashes.GetErrorAttachmentsHandler handler)
        {
            GetErrorAttachmentsHandler = handler;
        }
    }
}
#endif
