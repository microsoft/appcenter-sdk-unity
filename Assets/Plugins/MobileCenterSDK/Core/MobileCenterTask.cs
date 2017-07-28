// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask
    {
        private Action<MobileCenterTask> _continuationAction;

        public void ContinueWith(Action<MobileCenterTask> continuationAction)
        {
            _continuationAction = continuationAction;
        }

        protected virtual void InvokeContinuationAction()
        {
            if (_continuationAction != null)
            {
                _continuationAction(this);
            }
        }
    }
}
