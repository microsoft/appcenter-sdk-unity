// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.AppCenter.Unity.Distribute.Internal
{
    class DistributeInternal
    {
        public static void PrepareEventHandlers()
        {
        }

        public static void AddNativeType(List<Type> nativeTypes)
        {
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return AppCenterTask<bool>.FromCompleted(false);
        }

        public static void SetInstallUrl(string installUrl)
        {
        }

        public static void SetApiUrl(string apiUrl)
        {
        }

        public static void NotifyUpdateAction(int updateAction)
        {
        }

        public static void StartDistribute()
        {
        }

        public static void CheckForUpdate()
        {
        }

        private class Distribute
        {
        }
    }
}
#endif
