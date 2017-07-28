// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR

using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {
        public TResult Result { get; private set; }

        internal void SetResult(TResult result)
        {
            Result = result;
            CompletionAction();
        }

        public MobileCenterTask()
        {
        }

        // For iOS, just assume that the task completes immediately (until the 
        // SDK's API changes)
        public MobileCenterTask(TResult result)
        {
            SetResult(result);
        }
    }
}
#endif
