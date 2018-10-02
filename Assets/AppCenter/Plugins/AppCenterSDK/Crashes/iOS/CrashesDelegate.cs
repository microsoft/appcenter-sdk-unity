// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Microsoft.AppCenter.Unity.Crashes;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class CrashesDelegate
    {
#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        delegate bool NativeShouldProcessErrorReportDelegate(IntPtr report);
        static NativeShouldProcessErrorReportDelegate delegateShouldProcess;
        static Crashes.ShouldProcessErrorReportHandler externalHandler = null;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate IntPtr NativeGetErrorAttachmentsDelegate(IntPtr report);
        static NativeGetErrorAttachmentsDelegate delegatelGetAttachments;
        static Crashes.GetErrorAttachmentsHandler getErrorAttachmentsHandler = null;

        static CrashesDelegate()
        {
            delegateShouldProcess = ShouldProcessErrorReportNativeFunc;
            app_center_unity_crashes_delegate_set_should_process_error_report_delegate(delegateShouldProcess);
            delegatelGetAttachments = GetErrorAttachmentsNativeFunc;
            app_center_unity_crashes_delegate_set_get_error_attachments_delegate(delegatelGetAttachments);
        }

        public static void SetDelegate()
        {
            app_center_unity_crashes_set_delegate();
        }

        [MonoPInvokeCallback(typeof(NativeShouldProcessErrorReportDelegate))]
        public static bool ShouldProcessErrorReportNativeFunc(IntPtr report)
        {
            if (externalHandler != null)
            {
                ErrorReport errorReport = CrashesInternal.GetErrorReportFromIntPtr(report); 
                return externalHandler(errorReport);   
            }
            else
            {
                return true;
            }
        }

        public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
        {
            externalHandler = handler;
        }

        [MonoPInvokeCallback(typeof(NativeGetErrorAttachmentsDelegate))]
        public static IntPtr GetErrorAttachmentsNativeFunc(IntPtr report)
        {
            if (externalHandler != null)
            {
                ErrorReport errorReport = CrashesInternal.GetErrorReportFromIntPtr(report); 
                ErrorAttachmentLog[] logs = getErrorAttachmentsHandler(errorReport);
                List<IntPtr> nativeLogs = new List<IntPtr>();
                foreach (ErrorAttachmentLog errorAttachmetLog in logs)
                {
                    IntPtr nativeLog = IntPtr.Zero;
                    if (errorAttachmetLog.Type == ErrorAttachmentLog.AttachmentType.Text)
                    {
                        nativeLog = app_center_unity_crashes_get_error_attachment_log_text(errorAttachmetLog.Text, errorAttachmetLog.FileName);
                    }
                    else
                    {
                        nativeLog = app_center_unity_crashes_get_error_attachment_log_binary(errorAttachmetLog.Data, errorAttachmetLog.Data.Length, errorAttachmetLog.FileName, errorAttachmetLog.ContentType);
                    }
                    nativeLogs.Add(nativeLog);
                }

                IntPtr log0 = IntPtr.Zero;
                if (nativeLogs.Count > 0)
                {   
                    log0 = nativeLogs[0];
                }
                IntPtr log1 = IntPtr.Zero;
                if (nativeLogs.Count > 1) 
                {
                    log1 = nativeLogs[1];
                }
                return app_center_unity_create_error_attachments_array(log0, log1);
            }
            else
            {
                return IntPtr.Zero;
            }   
        }

        public static void SetGetErrorAttachmentsHandler(Crashes.GetErrorAttachmentsHandler handler)
        {
            getErrorAttachmentsHandler = handler;
        }

#region External

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_set_delegate();

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_delegate_set_should_process_error_report_delegate(NativeShouldProcessErrorReportDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_delegate_set_get_error_attachments_delegate(NativeGetErrorAttachmentsDelegate functionPtr);

        [DllImport("__Internal")]   
        private static extern IntPtr app_center_unity_crashes_get_error_attachment_log_text(string text, string fileName);

        [DllImport("__Internal")]   
        private static extern IntPtr app_center_unity_crashes_get_error_attachment_log_binary(byte[] data, int size, string fileName, string contentType);

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_create_error_attachments_array(IntPtr errorAttachment0, IntPtr errorAttachment1);
#endregion
    }
}
#endif
