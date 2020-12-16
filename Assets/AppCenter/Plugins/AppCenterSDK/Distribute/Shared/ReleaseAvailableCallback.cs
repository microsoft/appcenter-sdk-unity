// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.AppCenter.Unity.Distribute
{
    /// <summary>
    /// Release available callback.
    /// </summary>
    public delegate bool ReleaseAvailableCallback(ReleaseDetails releaseDetails);

    /// <summary>
    /// App will exit callback.
    /// </summary>
    public delegate void WillExitAppCallback();

    /// <summary>
    /// No Release available callback.
    /// </summary>
    public delegate void NoReleaseAvailableCallback();
}
