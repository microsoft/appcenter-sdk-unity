// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.Azure.Mobile.Crashes.Internal
{
	public class CrashesDelegate
	{
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate bool NativeShouldProcessErrorReportDelegate(IntPtr report);
        static NativeShouldProcessErrorReportDelegate del;
        static IntPtr ptr;

		[DllImport("__Internal")]
		public static extern void mobile_center_unity_crashes_set_delegate();

		[DllImport("__Internal")]
		private static extern void mobile_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(NativeShouldProcessErrorReportDelegate functionPtr);

		static CrashesDelegate()
		{
            del = ShouldProcessErrorReportNativeFunc;
            mobile_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(del);
		}

		[MonoPInvokeCallback(typeof(NativeShouldProcessErrorReportDelegate))]
		static bool ShouldProcessErrorReportNativeFunc(IntPtr report)
		{
			return false;
		}
	}
}
#endif
