// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Internal.Utility
{
    public class JavaStringMapHelper
    {
        public static Dictionary<string, string> JavaMapConvert(AndroidJavaObject map)
        {
            var keySet = map.Call<AndroidJavaObject>("keySet");
            var keyArray = keySet.Call<AndroidJavaObject>("toArray");
            string[] keys = AndroidJNIHelper.ConvertFromJNIArray<string[]>(keyArray.GetRawObject());
            var dictionary = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                var val = map.Call<string>("get", key);
                dictionary[key] = val;
            }
            return dictionary;
        }
    }
}
#endif
