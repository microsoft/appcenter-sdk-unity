// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;

namespace Microsoft.Azure.Mobile.Unity
{
    /// <summary>
    /// MobileCenterTask&lt;TResult&gt; extends the functionality of MobileCenterTask
    /// to support return values.
    /// </summary>
    /// <typeparam name="TResult">The return type of the task</typeparam>
    /// <seealso cref="MobileCenterTask"/>
    public partial class MobileCenterTask<TResult> : MobileCenterTask
    {
		/// <summary>
		/// Adds a callback that will be invoked once the task is complete. If
		/// the task is already complete, it is invoked immediately after being set.
		/// </summary>
		/// <param name="continuationAction">Callback to be invoked after task completion.</param>
		public void ContinueWith(Action<MobileCenterTask<TResult>> continuationAction)
        {
            base.ContinueWith(task => continuationAction(this));
        }
    }
}
