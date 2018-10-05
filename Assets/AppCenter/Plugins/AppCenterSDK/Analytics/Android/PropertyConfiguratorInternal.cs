// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AppCenter.Unity.Analytics.Internal;
using Microsoft.AppCenter.Unity.Internal.Utility;

namespace Microsoft.AppCenter.Unity.Analytics
{
    public class PropertyConfiguratorInternal
    {
        public static void SetAppName(AndroidJavaObject propertyConfigurator, string appName)
        {
            propertyConfigurator.Call("setAppName", appName);
        }

        public static void SetAppVersion(AndroidJavaObject propertyConfigurator, string appVersion)
        {
            propertyConfigurator.Call("setAppVersion", appVersion);
        }

        public static void SetAppLocale(AndroidJavaObject propertyConfigurator, string appLocale)
        {
            propertyConfigurator.Call("setAppLocale", appLocale);
        }

        public static void CollectDeviceId(AndroidJavaObject propertyConfigurator)
        {
            propertyConfigurator.Call("collectDeviceId");
        }

        public static void SetEventProperty(AndroidJavaObject propertyConfigurator, string key, string value)
        {
            var rawClass = propertyConfigurator.GetRawClass();
            var rawObject = propertyConfigurator.GetRawObject();
            var method = AndroidJNI.GetMethodID(rawClass, "setEventProperty", "(Ljava/lang/String;Ljava/lang/String;)V");
            AndroidJNI.CallVoidMethod(rawObject, method, new jvalue[]
            {
                new jvalue { l = AndroidJNI.NewStringUTF(key) }, 
                new jvalue { l = AndroidJNI.NewStringUTF(value) } 
            });
        }

        public static void RemoveEventProperty(AndroidJavaObject propertyConfigurator, string key)
        {
            var rawClass = propertyConfigurator.GetRawClass();
            var rawObject = propertyConfigurator.GetRawObject();
            var method = AndroidJNI.GetMethodID(rawClass, "removeEventProperty", "(Ljava/lang/String;)V");
            AndroidJNI.CallVoidMethod(rawObject, method, new jvalue[]
            {
                new jvalue { l = AndroidJNI.NewStringUTF(key) }
            });
        }
    }
}
#endif