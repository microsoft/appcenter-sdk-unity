// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using UnityEngine;

internal class DefaultFilePicker : IFilePicker
{
    public void InitBytes(string path)
    {
        Debug.LogError("There is no applicable file picker. Platfom unsupported.");
    }

    public void Show()
    {
        Debug.LogError("There is no applicable file picker. Platfom unsupported.");
    }
}
