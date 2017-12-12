// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using System;
using System.Threading.Tasks;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    class CrashesInternal
    {

        public static Type GetNativeType()
        {
            return typeof(Microsoft.AppCenter.Crashes.Crashes);
        }

        public static void TrackException(object exception)
        {
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
        {
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }

        public static void GenerateTestCrash()
        {
        }

        public static AppCenterTask<bool> HasCrashedInLastSession()
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }

        public static void DisableMachExceptionHandler()
        {
        }
    }
}
#endif