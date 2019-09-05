// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Microsoft.AppCenter.Unity
{
    public partial class AppCenterTask<TResult>
    {
        private Task<TResult> _task;
        private ManualResetEvent _completionEvent = new ManualResetEvent(false);
        private TResult _result;
        private Exception _exception;

        // This will block if it is called before the task is complete
        public TResult Result
        {
            get
            {
                _completionEvent.WaitOne();
                if (_exception == null)
                {
                    return _result;
                }
                else
                {
                    throw _exception;
                }
            }
        }

        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }

        public AppCenterTask(Task<TResult> task) : base(task)
        {
            // Need to save the task to access result later
            _task = task;
        }

        protected override void CompletionAction()
        {
            lock (_lockObject)
            {
                if (IsComplete)
                {
                    return;
                }
                _result = _task.Result;
                _completionEvent.Set();
                base.CompletionAction();
            }
        }

        internal void SetResult(TResult result)
        {
            lock (_lockObject)
            {
                ThrowIfCompleted();
                _result = result;
                _completionEvent.Set();
                base.CompletionAction();
            }
        }

        internal void SetException(Exception exception)
        {
            lock (_lockObject)
            {
                ThrowIfCompleted();
                _exception = exception;
                _completionEvent.Set();
                base.CompletionAction();
            }
        }
    }
}
#endif
