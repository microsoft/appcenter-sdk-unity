// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

namespace Microsoft.AppCenter.Crashes
{
    // This is just a placeholder class since we don't support UWP crashes for now. When it's passed
    // to App Center, a message will be emitted that says it isn't yet supported.
    class Crashes
    {
    }
}
#endif