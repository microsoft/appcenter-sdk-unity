// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR

using Microsoft.Azure.Mobile.Utils;

namespace Microsoft.Azure.Mobile.Unity.Internal.Utils
{
    public class UnityScreenSizeProviderFactory : IScreenSizeProviderFactory
    {
        public IScreenSizeProvider CreateScreenSizeProvider()
        {
            return new UnityScreenSizeProvider();
        }
    }
}

#endif
