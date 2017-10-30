// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.Mobile.Internal.Utility
{
    public class NSDateHelper
    {
        public static IntPtr DateTimeConvert(DateTime date)
        {
            var format = "yyyy-MM-dd'T'HH:mm:ss.fffK";
            var nsdateformat = "yyyy-MM-dd'T'HH:mm:ss.SSS";
            var dateString = date.ToString(format);
            return appcenter_unity_ns_date_convert(nsdateformat, dateString);
        }

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_ns_date_convert(string format, string dateString);
    }
}
#endif
