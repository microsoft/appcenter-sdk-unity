// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult> : MobileCenterTask
    {        
        public void ContinueWith(Action<MobileCenterTask<TResult>> continuationAction)
        {
            base.ContinueWith(task =>
            {
                continuationAction(this);
            });

        }
    }
}
