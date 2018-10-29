// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using Microsoft.AppCenter.Unity.Internal.Utility;

namespace Microsoft.AppCenter.Unity.Analytics.Internal
{
    class EventPropertiesInternal
    {
        public static IntPtr Create()
        {
        }

        public static void SetString(IntPtr properties, string key, string val)
        {
        }

        public static void SetNumber(IntPtr properties, string key, long val)
        {
        }

        public static void SetNumber(IntPtr properties, string key, double val)
        {
        }

        public static void SetBool(IntPtr properties, string key, bool val)
        {
        }

        public static void SetDate(IntPtr properties, string key, DateTime val)
        {
        }

#region External
#endregion
    }
}
#endif
