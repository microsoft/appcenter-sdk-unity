// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR

using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask
    {
        // For iOS, just assume that the task completes immediately
        public MobileCenterTask()
        {
            CompletionAction();
        }
    }
}
#endif
