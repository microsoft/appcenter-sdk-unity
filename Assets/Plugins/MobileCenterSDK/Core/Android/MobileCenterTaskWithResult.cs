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
        private AutoResetEvent _completionEvent = new AutoResetEvent(false);
        private TResult _result;

        // This will block if it is called before the task is complete
        public TResult Result
        {
            get
            {
                _completionEvent.WaitOne();
                _completionEvent.Set();
                return _result;
            }
        }

        public MobileCenterTask(AndroidJavaObject javaFuture)
        {
            var consumer = new UnityMobileCenterConsumer<TResult>();
            consumer.CompletionCallback = (t => 
            {
                _result = t;
                _completionEvent.Set();
                CompletionAction();
            });
            javaFuture.Call("thenAccept", consumer);
        }
    }
}
#endif
