// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if (!UNITY_IOS && !UNITY_ANDROID && !UNITY_WSA_10_0) || UNITY_EDITOR

using System;

namespace Microsoft.AppCenter.Unity
{
    public partial class AppCenterTask<TResult>
    {
        public TResult Result { get; private set; }

        public Exception Exception { get; private set; }

        internal void SetResult(TResult result)
        {
            lock (_lockObject)
            {
                ThrowIfCompleted();
                Result = result;
                CompletionAction();
            }
        }

        internal void SetException(Exception exception)
        {
            lock (_lockObject)
            {
                ThrowIfCompleted();
                Exception = exception;
                CompletionAction();
            }
        }
    }
}
#endif
