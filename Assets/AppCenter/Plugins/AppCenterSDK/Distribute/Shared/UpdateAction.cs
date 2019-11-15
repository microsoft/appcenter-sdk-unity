// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.AppCenter.Unity.Distribute
{
    /// <summary>
    /// User update action.
    /// </summary>
    public enum UpdateAction
    {
        /// <summary>
        /// Action to trigger the download of the release.
        /// </summary>
        Update,

        /// <summary>
        /// Action to postpone optional updates for 1 day.
        /// </summary>
        Postpone
    }
}
