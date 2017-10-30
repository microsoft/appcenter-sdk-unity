// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR

using UnityEngine;

namespace Microsoft.AppCenter.Unity
{
    public partial class MobileCenterTask
    {
        internal void SetResult()
        {
            CompletionAction();
        }
    }
}
#endif
