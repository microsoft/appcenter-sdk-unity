// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;

namespace Microsoft.Azure.Mobile.Unity
{
    /// <summary>
    /// MobileCenterTask provides a way of performing long-running
    /// tasks on any thread, and invoke a callback upon completion.
    /// upon completion.
    /// </summary>
    public partial class MobileCenterTask
    {
        private readonly List<Action<MobileCenterTask>> _continuationActions = new List<Action<MobileCenterTask>>();

        private readonly object _lockObject = new object();

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Microsoft.Azure.Mobile.Unity.MobileCenterTask"/> is complete.
        /// </summary>
        /// <value><c>true</c> if it is complete; otherwise, <c>false</c>.</value>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Adds a callback that will be invoked once the task is complete. If 
        /// the task is already complete, it is invoked immediately after being set.
        /// </summary>
        /// <param name="continuationAction">Callback to be invoked after task completion.</param>
        public void ContinueWith(Action<MobileCenterTask> continuationAction)
        {
            lock (_lockObject)
            {
                _continuationActions.Add(continuationAction);
                InvokeContinuationActions();
            }
        }

        /// <summary>
        /// Takes care of invoking callbacks and setting completion flag upon
        /// the task completing.
        /// </summary>
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
            // Save the actions and then invoke them; could have a deadlock
            // if one of the actions calls ContinueWith on another thread for
            // the same task object
            var continuationActionsSnapshot = new List<Action<MobileCenterTask>>();
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
                        continuationActionsSnapshot.Add(action);
                    }
                }
                _continuationActions.Clear();
            }
            foreach (var action in continuationActionsSnapshot)
            {
                action(this);
            }
        }
    }
}
