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
		    Application.logMessageReceived += HandleLog;
			CrashesDelegate.mobile_center_unity_crashes_set_delegate();
		}

        private static void HandleLog(string condition, string stackTrace, UnityEngine.LogType type)
        {
			//TODO other log types? HA seems to use Error, Assert, and Exception
			if (type == LogType.Exception)
            {
				var modelException = new Models.Exception(condition, stackTrace);
				CrashesInternal.TrackException(modelException);
            }
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
