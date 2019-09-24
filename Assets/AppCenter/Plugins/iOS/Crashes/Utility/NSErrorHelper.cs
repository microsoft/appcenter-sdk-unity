// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using Exception = Microsoft.AppCenter.Unity.Crashes.Models.Exception;

namespace Microsoft.AppCenter.Unity.Internal.Utility
{
    public static partial class NSErrorHelper
    {
        public static Exception Convert(IntPtr nsErrorPtr)
        {
            if (nsErrorPtr == IntPtr.Zero)
            {
                return null;
            }
            var domain = app_center_unity_nserror_domain(nsErrorPtr);
            var errorCode = app_center_unity_nserror_code(nsErrorPtr);
            var description = app_center_unity_nserror_description(nsErrorPtr);
            return new Exception(string.Format("Domain: {0}, error code: {1}, description: {2}", domain, errorCode, description), string.Empty);
        }
    }
}
#endif
