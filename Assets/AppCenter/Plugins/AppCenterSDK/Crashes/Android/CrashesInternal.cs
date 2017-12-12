// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine; 

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    class CrashesInternal
    {
        private const string CrashesClassName = "com.microsoft.appcenter.crashes.Crashes";
        private static AndroidJavaClass _crashes = new AndroidJavaClass(CrashesClassName);

        public static IntPtr GetNativeType()
        {
            return AndroidJNI.FindClass("com/microsoft/appcenter/crashes/Crashes");
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            var future = _crashes.CallStatic<AndroidJavaObject>("setEnabled", isEnabled);
            return new AppCenterTask(future);
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var future = _crashes.CallStatic<AndroidJavaObject>("isEnabled");
            return new AppCenterTask<bool>(future);
        }
    }
}
#endif
