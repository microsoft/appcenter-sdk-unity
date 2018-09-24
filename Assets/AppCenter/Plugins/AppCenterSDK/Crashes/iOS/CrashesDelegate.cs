// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class CrashesDelegate
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool NativeShouldProcessErrorReportDelegate(IntPtr report);
        static NativeShouldProcessErrorReportDelegate del;
        static Crashes.ShouldProcessErrorReportHandler externalHandler = null;

        static CrashesDelegate()
        {
            del = ShouldProcessErrorReportNativeFunc;
            app_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(del);
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
                return true;
        }

        public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
        {
            externalHandler = handler;
        }

#region External

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_set_delegate();

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(NativeShouldProcessErrorReportDelegate functionPtr);

#endregion
    }
}
#endif
