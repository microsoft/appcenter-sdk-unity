// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR

using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    class CrashesDelegate
    {
#pragma warning disable 0067, 0414
        public static event Crashes.SendingErrorReportHandler SendingErrorReport;

        public static event Crashes.SentErrorReportHandler SentErrorReport;

        public static event Crashes.FailedToSendErrorReportHandler FailedToSendErrorReport;
#pragma warning restore 0067, 0414

#pragma warning disable 0649
        internal static Crashes.GetErrorAttachmentsHandler GetErrorAttachmentsHandler;
#pragma warning restore 0649

#if UNITY_2019_3_OR_NEWER && UNITY_EDITOR
        /// <summary>
        /// Clean up static references that may be around due to Domain Reload being disabled
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Cleanup()
        {
            SendingErrorReport = null;
            SentErrorReport = null;
            FailedToSendErrorReport = null;
        }
#endif

        public static void SetDelegate()
        {
        }

        public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
        {
        }

        public static void SetGetErrorAttachmentsHandler(Crashes.GetErrorAttachmentsHandler handler)
        {
        }

        public static void SetSentErrorReportHandler(Crashes.SentErrorReportHandler handler)
        {
        }

        public static void SetFailedToSendErrorReportHandler(Crashes.FailedToSendErrorReportHandler handler)
        {
        }

        public static void SetShouldAwaitUserConfirmationHandler(Crashes.UserConfirmationHandler handler)
        {
        }
    }
}
#endif