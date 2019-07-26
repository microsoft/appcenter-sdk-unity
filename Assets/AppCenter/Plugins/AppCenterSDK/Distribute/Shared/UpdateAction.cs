// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.AppCenter.Unity.Distribute
{
    /// <summary>
    /// User update action.
    /// </summary>
    public enum UpdateAction
    {

#if UNITY_ANDROID
        /// <summary>
        /// Action to trigger the download of the release.
        /// </summary>
        Update = -1,

        /// <summary>
        /// Action to postpone optional updates for 1 day.
        /// </summary>
        Postpone = -2
#else

        /// <summary>
        /// Action to trigger the download of the release.
        /// </summary>
        Update = 0,

        /// <summary>
        /// Action to postpone optional updates for 1 day.
        /// </summary>
        Postpone = 1
#endif


    }
}
