// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask
    {
        private readonly List<Action<MobileCenterTask>> _continuationActions = new List<Action<MobileCenterTask>>();

        public bool IsComplete { get; private set; }

        private object _lockObject = new object();

        public void ContinueWith(Action<MobileCenterTask> continuationAction)
        {
            lock (_lockObject)
            {
                _continuationActions.Add(continuationAction);
                InvokeContinuationActions();
            }
        }
        
        protected void CompletionAction()
        {
            lock (_lockObject)
            {
                IsComplete = true;
                InvokeContinuationActions();
            }
        }

        private void InvokeContinuationActions()
        {
            lock (_lockObject)
            {
                if (!IsComplete)
                {
                    return;
                }

                foreach (var action in _continuationActions)
                {
                    if (action != null)
                    {
                        action(this);
                    }
                }
                _continuationActions.Clear();
            }
        }
    }
}
