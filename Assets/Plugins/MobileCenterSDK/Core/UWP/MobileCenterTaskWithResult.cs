// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {
        private Task<TResult> _task;

        // This will block if it is called before the task is complete
        public TResult Result => _task.Result;

        /// <summary>
        /// Manually set the result of the task and mark it complete.
        /// </summary>
        /// <param name="result">Task result to be set.</param>
        internal void SetResult(TResult result)
        {
            _task = Task<TResult>.FromResult(result);
            CompletionAction();
        }

        public MobileCenterTask()
        {
        }
        
        public MobileCenterTask(Task<TResult> task) : base(task)
        {
            // Need to save the task to access result later
            _task = task;
        }
    }
}
#endif
