// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class NativeObjectsConverter
    {
        public static IntPtr ToNativeAttachments(ErrorAttachmentLog[] logs)
        {
            var nativeLogs = new List<IntPtr>();
            foreach (var errorAttachmentLog in logs)
            {
                if (errorAttachmentLog != null)
                {
                    IntPtr nativeLog = IntPtr.Zero;
                    if (errorAttachmentLog.Type == ErrorAttachmentLog.AttachmentType.Text)
                    {
                        nativeLog = app_center_unity_crashes_get_error_attachment_log_text(errorAttachmentLog.Text, errorAttachmentLog.FileName);
                    }
                    else
                    {
                        nativeLog = app_center_unity_crashes_get_error_attachment_log_binary(errorAttachmentLog.Data, errorAttachmentLog.Data.Length, errorAttachmentLog.FileName, errorAttachmentLog.ContentType);
                    }
                    nativeLogs.Add(nativeLog);
                }
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

        #region External

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_crashes_get_error_attachment_log_text(string text, string fileName);

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_crashes_get_error_attachment_log_binary(byte[] data, int size, string fileName, string contentType);

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_create_error_attachments_array(IntPtr errorAttachment0, IntPtr errorAttachment1);

        #endregion
    }
}