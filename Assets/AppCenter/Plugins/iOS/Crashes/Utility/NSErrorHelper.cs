// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using Exception = Microsoft.AppCenter.Unity.Crashes.Models.Exception;

namespace Microsoft.AppCenter.Unity.Crashes.Utility
{
    public static class NSErrorHelper
    {
        public static Exception Convert(IntPtr nsErrorPtr)
        {
            if (nsErrorPtr == IntPtr.Zero)
            {
                return null;
            }
            var domain = app_center_unity_nserror_domain(nsErrorPtr);
            var errorCode = app_center_unity_nserror_code(nsErrorPtr);
            return new Exception(string.Format("Domain: {0}, Error code: {1}", domain, errorCode), string.Empty);
        }

        [DllImport("__Internal")]
        private static extern string app_center_unity_nserror_domain(IntPtr error);

        [DllImport("__Internal")]
        private static extern long app_center_unity_nserror_code(IntPtr error);
    }
}
#endif
