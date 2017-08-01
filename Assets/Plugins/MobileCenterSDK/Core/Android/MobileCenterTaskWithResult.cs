// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR

using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {
        private ManualResetEvent _completionEvent = new ManualResetEvent(false);
        private TResult _result;

        // This will block if it is called before the task is complete
        public TResult Result
        {
            get
            {
                _completionEvent.WaitOne();
                return _result;
            }
        }

        internal void SetResult(TResult result)
        {
            _result = result;
            _completionEvent.Set();
            CompletionAction();
        }

        public MobileCenterTask()
        {
        }

        public MobileCenterTask(AndroidJavaObject javaFuture)
        {
            var consumer = new UnityMobileCenterConsumer<TResult>();
            consumer.CompletionCallback = SetResult;
            javaFuture.Call("thenAccept", consumer);
        }
    }
}
#endif
