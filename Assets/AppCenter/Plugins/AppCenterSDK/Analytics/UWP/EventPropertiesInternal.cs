// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class EventPropertiesInternal
    {
       public static object Create()
        {
            return null;
        }

        public static void SetString(object properties, string key, string val)
        {
        }

        public static void SetNumber(object properties, string key, long val)
        {
        }

        public static void SetNumber(object properties, string key, double val)
        {
        }

        public static void SetBool(object properties, string key, bool val)
        {
        }

        public static void SetDate(object properties, string key, DateTime val)
        {
        }
    }
}
#endif
