// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR
using System;

namespace Microsoft.Azure.Mobile.Push.Internal
{
	class PushInternal
    {

		public static void Initialize()
		{
		}

        public static IntPtr mobile_center_unity_push_get_type()
        {
            return IntPtr.Zero;
        }

        public static void mobile_center_unity_push_set_enabled(bool isEnabled)
        {
        }

        public static bool mobile_center_unity_push_is_enabled()
        {
            return false;
        }

        public static void mobile_center_unity_push_enable_firebase_analytics()
        {
        }
	}
}
#endif
