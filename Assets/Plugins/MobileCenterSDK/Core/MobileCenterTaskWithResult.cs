// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {        
        private Action<MobileCenterTask<TResult>> _continuationAction;

        public void ContinueWith(Action<MobileCenterTask<TResult>> continuationAction)
        {
            _continuationAction = continuationAction;
        }
    }
}
