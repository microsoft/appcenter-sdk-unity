// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using UnityEditor;

public static class ApplicationIdHelper
{
    public static string GetApplicationId()
    {
        return PlayerSettings.applicationIdentifier;
    }
}