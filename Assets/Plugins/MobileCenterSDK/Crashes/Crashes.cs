// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity.Crashes.Internal;
using System;
using Microsoft.Azure.Mobile.Unity.Crashes.Models;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity.Crashes
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Crashes
	{
        public static void Initialize()
        {
			CrashesDelegate.mobile_center_unity_crashes_set_delegate();
		}

        public static RawType GetNativeType()
		{
            return CrashesInternal.mobile_center_unity_crashes_get_type();
		}

        public static bool Enabled
        {
            get
            {
                return CrashesInternal.mobile_center_unity_crashes_is_enabled();
            }
            set
            {
                CrashesInternal.mobile_center_unity_crashes_set_enabled(value);
            }
        }
	}
}
