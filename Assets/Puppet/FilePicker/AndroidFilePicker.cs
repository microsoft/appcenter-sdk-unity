// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Assets.Puppet.Plugins.Android.FilePickerManager;

public class AndroidFilePicker : IFilePicker
{
    FilePickerManager filePickerManager;

    public AndroidFilePicker()
    {
        filePickerManager = new FilePickerManager();
    }

    public void InitBytes(string path)
    {
        filePickerManager.ReadTextFromUri(path);
    }

    public void Show()
    {
        filePickerManager.OpenFilePicker();
    }
}
