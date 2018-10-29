// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.
#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class EventPropertiesInternal
    {
        private static Dictionary<string, string> _properties;

        public static Dictionary<string, string> Create()
        {
            if (_properties == null) 
            {
                _properties = new Dictionary<string, string>();
            }
            return _properties;
        }

        public static void SetString(Dictionary<string, string> properties, string key, string val)
        {
            properties[key] = val;
        }

        public static void SetNumber(Dictionary<string, string> properties, string key, long val)
        {
            properties[key] = val.ToString();
        }

        public static void SetNumber(Dictionary<string, string> properties, string key, double val)
        {
            properties[key] = val.ToString();
        }

        public static void SetBool(Dictionary<string, string> properties, string key, bool val)
        {
            properties[key] = val.ToString();
        }

        public static void SetDate(Dictionary<string, string> properties, string key, DateTime val)
        {
            properties[key] = val.ToLongDateString();
        }
    }
}
#endif
