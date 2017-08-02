// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {
        private TResult _result;

        internal void SetResult(TResult result)
        {
            _result = result;
            CompletionAction();
        }

        public MobileCenterTask(TResult result)
        {
            _result = result;
        }

        public TResult Result
        {
            get
            {
                return _result;
            }
        }
    }
}
#endif
