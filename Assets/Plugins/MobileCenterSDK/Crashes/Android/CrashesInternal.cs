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

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            var future = _crashes.CallStatic<AndroidJavaObject>("setEnabled", isEnabled);
            return new MobileCenterTask(future);
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            var future = _crashes.CallStatic<AndroidJavaObject>("isEnabled");
            return new MobileCenterTask<bool>(future);
        }
    }
}
#endif
