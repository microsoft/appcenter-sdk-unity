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
    public class WrapperPropertyConfiguratorInternal
    {
        public static void SetAppName(UnityEngine.AndroidJavaObject propertyConfigurator, string appName)
        {
            propertyConfigurator.Call("setAppName", appName);
        }

        public static void SetAppVersion(UnityEngine.AndroidJavaObject propertyConfigurator, string appVersion)
        {
            propertyConfigurator.Call("setAppVersion", appVersion);
        }

        public static void SetAppLocale(UnityEngine.AndroidJavaObject propertyConfigurator, string appLocale)
        {
            propertyConfigurator.Call("setAppLocale", appLocale);
        }

        public static void SetEventProperty(UnityEngine.AndroidJavaObject propertyConfigurator, string key, string value)
        {
            IntPtr rawClass = propertyConfigurator.GetRawClass();
            IntPtr rawObject = propertyConfigurator.GetRawObject();
            IntPtr method = AndroidJNI.GetMethodID(rawClass, "setEventProperty", "(Ljava/lang/String;Ljava/lang/String;)V");
            AndroidJNI.CallVoidMethod(rawObject, method, new jvalue[]
            {
                new jvalue { l = new AndroidJavaObject( "java.lang.String", key ).GetRawObject() }, 
                new jvalue { l = new AndroidJavaObject( "java.lang.String", value ).GetRawObject() } 
            });
        }

        public static void RemoveEventProperty(UnityEngine.AndroidJavaObject propertyConfigurator, string key)
        {
            IntPtr rawClass = propertyConfigurator.GetRawClass();
            IntPtr rawObject = propertyConfigurator.GetRawObject();
            IntPtr method = AndroidJNI.GetMethodID(rawClass, "removeEventProperty", "(Ljava/lang/String;)V");
            AndroidJNI.CallVoidMethod(rawObject, method, new jvalue[]
            {
                new jvalue { l = new AndroidJavaObject( "java.lang.String", key ).GetRawObject() }
            });
        }
    }
}
#endif