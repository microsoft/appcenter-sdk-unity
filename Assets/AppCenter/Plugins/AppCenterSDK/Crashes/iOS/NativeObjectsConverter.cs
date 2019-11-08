// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS
using System;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public static class NativeObjectsConverter
    {
        public static IntPtr ToNativeAttachments(ErrorAttachmentLog[] logs)
        {
            if (logs == null)
            {
                return IntPtr.Zero;
            }
            var nativeArray = app_center_unity_crashes_create_error_attachments_array(logs.Length);
            foreach (var errorAttachmentLog in logs)
            {
                if (errorAttachmentLog != null)
                {
                    IntPtr nativeLog;
                    if (errorAttachmentLog.Type == ErrorAttachmentLog.AttachmentType.Text)
                    {
                        nativeLog = app_center_unity_crashes_create_error_attachment_log_text(errorAttachmentLog.Text, errorAttachmentLog.FileName);
                    }
                    else
                    {
                        nativeLog = app_center_unity_crashes_create_error_attachment_log_binary(errorAttachmentLog.Data, errorAttachmentLog.Data.Length, errorAttachmentLog.FileName, errorAttachmentLog.ContentType);
                    }
                    appcenter_unity_crashes_add_error_attachment(nativeArray, nativeLog);
                }
            }
            return nativeArray;
        }

        #region External

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_crashes_create_error_attachments_array(int capacity);

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_crashes_create_error_attachment_log_text(string text, string fileName);

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_crashes_create_error_attachment_log_binary(byte[] data, int size, string fileName, string contentType);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_crashes_add_error_attachment(IntPtr attachments, IntPtr attachment);

        #endregion
    }
}
#endif
