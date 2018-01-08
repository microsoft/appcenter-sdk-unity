// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

using UnityEditor;

public static class ApplicationIdHelper
{
    public static string GetApplicationId()
    {
        string appId = null;
#if UNITY_5_6_OR_NEWER
        appId = PlayerSettings.applicationIdentifier;
#else
        appId = PlayerSettings.bundleIdentifier;
#endif
        return appId;
    }
}
#endif