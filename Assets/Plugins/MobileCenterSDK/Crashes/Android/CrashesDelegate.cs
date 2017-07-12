// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.Azure.Mobile.Crashes.Internal
{
	public class CrashesDelegate
	{
		public static void mobile_center_unity_crashes_set_delegate()
        {
        }
		static CrashesDelegate()
		{
		}
	}
}
#endif
