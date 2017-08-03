// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {
        public TResult Result { get; private set; }

        internal void SetResult(TResult result)
        {
            lock (_lockObject)
            {
                ThrowIfCompleted();
                Result = result;
                CompletionAction();
            }
        }
    }
}
#endif
