// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

namespace Microsoft.AppCenter.Unity
{
    /// <summary>
    /// Log level threshold for logs emitted by the SDK.
    /// </summary>
    public enum StartupType
    {
        /// <summary>
        /// SDK is turned on automatically.
        /// </summary>
        AppCenter = 2,

        /// <summary>
        /// Analytics service is started from library.
        /// </summary>
        OneCollector = 3,

        /// <summary>
        /// Nothing is configured on startup.
        /// </summary>
        None = 4
    }
}
