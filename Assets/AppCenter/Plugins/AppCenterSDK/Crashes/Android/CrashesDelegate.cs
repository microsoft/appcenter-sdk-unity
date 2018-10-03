// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class CrashesDelegate : AndroidJavaProxy
    {
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
