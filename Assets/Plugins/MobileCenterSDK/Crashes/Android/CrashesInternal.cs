// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine; 

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
	class CrashesInternal
    {
        private const string CrashesClassName = "com.microsoft.azure.mobile.crashes.Crashes";
        private static AndroidJavaClass _crashes = new AndroidJavaClass(CrashesClassName);

        public static IntPtr mobile_center_unity_crashes_get_type()
        {
            return AndroidJNI.FindClass("com/microsoft/azure/mobile/crashes/Crashes");
        }

        public static void mobile_center_unity_crashes_set_enabled(bool isEnabled)
        {
            _crashes.CallStatic("setEnabled", isEnabled);
        }

        public static bool mobile_center_unity_crashes_is_enabled()
        {
            return _crashes.CallStatic<bool>("isEnabled");
        }

        public static void TrackException(Models.Exception e)
        {
            var instance = _crashes.CallStatic<AndroidJavaObject>("getInstance");
            instance.Call("trackException", Models.ExceptionHelper.ExceptionConvert(e));
        }
	}
}
#endif
