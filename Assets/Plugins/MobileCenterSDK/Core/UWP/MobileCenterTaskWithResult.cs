// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {
        private Task<TResult> _task;

        // This will block if it is called before the task is complete
        public TResult Result => _task.Result;

        public MobileCenterTask(Task<TResult> task) : base(task)
        {
            // Need to save the task to access result later
            _task = task;;
        }
    }
}
#endif
