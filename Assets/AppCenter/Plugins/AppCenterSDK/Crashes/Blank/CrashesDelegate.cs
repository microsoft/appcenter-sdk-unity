// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    class CrashesDelegate
    {
        public static void SetDelegate()
        {
        }
    }
}
#endif