// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using System;
using System.Threading.Tasks;

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
    class DistributeInternal
    {
        public static void PrepareEventHandlers()
        {
        }

        public static Type GetNativeType()
        {
            return null;
        }

        public static MobileCenterTask SetEnabledAsync(bool isEnabled)
        {
            return MobileCenterTask.FromCompleted();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return MobileCenterTask<bool>.FromCompleted(false);
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
    }
}
#endif
