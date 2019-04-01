// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class EventPropertiesInternal
    {
        public static Dictionary<string, string> Create()
        {
            return new Dictionary<string, string>();
        }

        public static void SetString(Dictionary<string, string> properties, string key, string val)
        {
            properties[key] = val;
        }

        public static void SetNumber(Dictionary<string, string> properties, string key, long val)
        {
            properties[key] = val.ToString(CultureInfo.InvariantCulture);
        }

        public static void SetNumber(Dictionary<string, string> properties, string key, double val)
        {
            properties[key] = val.ToString(CultureInfo.InvariantCulture);
        }

        public static void SetBool(Dictionary<string, string> properties, string key, bool val)
        {
            properties[key] = val.ToString();
        }

        public static void SetDate(Dictionary<string, string> properties, string key, DateTime val)
        {
            var format = "yyyy-MM-dd'T'HH:mm:ss.fffK";
            var dateString = val.ToString(format, CultureInfo.InvariantCulture);
            properties[key] = dateString;
        }
    }
}
#endif
