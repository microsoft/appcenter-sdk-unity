// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    class CrashesInternal
    {
        public static RawType mobile_center_unity_crashes_get_type()
        {
            return default(RawType);
        }

		public static void mobile_center_unity_crashes_set_enabled(bool isEnabled)
		{
		}

		public static bool mobile_center_unity_crashes_is_enabled()
		{
            return false;
		}

        public static void TrackException(Models.Exception e)
        {
			System.Diagnostics.Debug.WriteLine("ZANDER track exception in editor");
		}
    }
}
#endif
