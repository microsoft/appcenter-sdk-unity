// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using AOT;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    public class PropertyConfiguratorInternal
    {
        public static void SetAppName(IntPtr propertyConfigurator, string appName)
        {
             
        }

        public static void SetAppVersion(IntPtr propertyConfigurator, string appVersion)
        {
            
        }

        public static void SetAppLocale(IntPtr propertyConfigurator, string appLocale)
        {
            
        }

        public static void SetEventProperty(IntPtr propertyConfigurator, string key, string value)
        {

        }

        public static void RemoveEventProperty(IntPtr propertyConfigurator, string key)
        {

        }

#region External

#endregion
    }
}
#endif