// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask<TResult>
    {
        internal void SetResult(TResult result)
        {
        }

        public MobileCenterTask()
        {
        }
        
        public TResult Result
        { 
            get
            {
                return default(TResult);
            }
        }
    }
}
#endif
