// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Analytics.Internal;
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics
{
#if UNITY_IOS
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = System.Object;
#endif

    public class WrapperPropertyConfigurator
    {
        private readonly RawType _rawObject;

        internal RawType GetRawObject()
        {
            return _rawObject;
        }

        public WrapperPropertyConfigurator(RawType rawObject)
        {
            _rawObject = rawObject;
        }

        public void SetAppName(string appName)
        {
            WrapperPropertyConfiguratorInternal.SetAppName(_rawObject, appName);
        }

        public void SetAppVersion(string appVersion)
        {
            WrapperPropertyConfiguratorInternal.SetAppVersion(_rawObject, appVersion);
        }

        public void SetAppLocale(string appLocale)
        {
            WrapperPropertyConfiguratorInternal.SetAppLocale(_rawObject, appLocale);
        }

        public void SetEventProperty(string key, string value)
        {
            WrapperPropertyConfiguratorInternal.SetEventProperty(_rawObject, key, value);
        }

        public void RemoveEventProperty(String key)
        {
            WrapperPropertyConfiguratorInternal.RemoveEventProperty(_rawObject, key);
        }
    }
}