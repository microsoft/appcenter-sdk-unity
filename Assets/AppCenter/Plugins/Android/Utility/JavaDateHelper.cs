// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID

using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Internal.Utility
{
    public class JavaDateHelper
    {
        public static AndroidJavaObject DateTimeConvert(DateTime date)
        {
            var format = "yyyy-MM-dd'T'HH:mm:ss.fffK";
            var javaFormat = "yyyy-MM-dd'T'HH:mm:ss.SSS";
            var dateFormatter = new AndroidJavaObject("java.text.SimpleDateFormat", javaFormat);
            var dateString = date.ToString(format);
            return dateFormatter.Call<AndroidJavaObject>("parse", dateString);
        }
    }
}
#endif
