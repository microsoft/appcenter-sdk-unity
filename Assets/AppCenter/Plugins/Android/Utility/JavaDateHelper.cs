// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID
using System;
using System.Globalization;
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

        public static DateTimeOffset DateTimeConvert(AndroidJavaObject date)
        {
            // Unable to use DateTimeOffset.ParseExact(dateString, format, CultureInfo.InvariantCulture) here
            // because it throws "Invalid format string" exception
            var format = "yyyy-MM-dd'T'HH:mm:ss.fffK";
            var javaFormat = "yyyy-MM-dd'T'HH:mm:ss.SSSXXX";
            var dateFormatter = new AndroidJavaObject("java.text.SimpleDateFormat", javaFormat);
            var dateString = dateFormatter.Call<string>("format", date);
            var dateTime = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            return new DateTimeOffset(dateTime);
        }
    }
}
#endif
