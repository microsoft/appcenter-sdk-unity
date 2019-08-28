// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR

using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Microsoft.AppCenter.Unity
{
    public partial class AppCenterTask<TResult>
    {
        private ManualResetEvent _completionEvent = new ManualResetEvent(false);
        private TResult _result;
        private Exception _exception;

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

        internal void SetResult(TResult result)
        {
            lock (_lockObject)
            {
                ThrowIfCompleted();
                _result = result;
                _completionEvent.Set();
                CompletionAction();
            }
        }

        internal void SetException(Exception exception)
        {
            lock (_lockObject)
            {
                ThrowIfCompleted();
                _exception = exception;
                _completionEvent.Set();
                CompletionAction();
            }
        }
    }
}
#endif
