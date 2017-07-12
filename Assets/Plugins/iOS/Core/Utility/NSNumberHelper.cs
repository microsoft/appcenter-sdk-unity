// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Internal.Utility
{
	public class NSNumberHelper
	{
        [DllImport("__Internal")]
        private static extern IntPtr mobile_center_unity_nsnumber_convert_int(int val);

		[DllImport("__Internal")]
		private static extern IntPtr mobile_center_unity_nsnumber_convert_long(long val);

		[DllImport("__Internal")]
		private static extern IntPtr mobile_center_unity_nsnumber_convert_float(float val);

		[DllImport("__Internal")]
		private static extern IntPtr mobile_center_unity_nsnumber_convert_double(double val);

		public static IntPtr Convert(int val)
		{
            return mobile_center_unity_nsnumber_convert_int(val);
		}

		public static IntPtr Convert(long val)
		{
			return mobile_center_unity_nsnumber_convert_long(val);
		}

		public static IntPtr Convert(float val)
		{
			return mobile_center_unity_nsnumber_convert_float(val);
		}

		public static IntPtr Convert(double val)
		{
			return mobile_center_unity_nsnumber_convert_double(val);
		}

        //TODO how to support 'decimal'?
	}
}
#endif
