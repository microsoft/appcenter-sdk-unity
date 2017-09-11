// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && ENABLE_IL2CPP && !UNITY_EDITOR
using Microsoft.Azure.Mobile.Utils;

namespace Microsoft.Azure.Mobile.Unity.Internal.Utils
{
    public class UnityApplicationSettingsFactory : IApplicationSettingsFactory
    {
        public IApplicationSettings CreateApplicationSettings()
        {
            return new UnityApplicationSettings();
        }
    }
}
#endif
