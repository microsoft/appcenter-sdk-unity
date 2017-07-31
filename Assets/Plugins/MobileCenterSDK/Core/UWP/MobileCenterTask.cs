// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using UnityEngine;
using System.Threading.Tasks;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask
    {
        protected MobileCenterTask()
        {
        }

        // Task parameter should be started already!
        public MobileCenterTask(Task task)
        {
            // Use the *actual* TPL's ContinueWith implementation to 
            // perform the user's ContinueWith callbacks
            task.ContinueWith(t => {
                CompletionAction();
            });
        }
    }
}
#endif
