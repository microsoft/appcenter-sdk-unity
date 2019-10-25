// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Assets.Puppet.Plugins.Android.FilePickerManager;

public class AndroidFilePicker : IFilePicker
{
    public static byte[] GetFileBytes(string fileUrl)
    {
        throw new System.NotImplementedException();
    }

    public void Show()
    {
        FilePickerManager filePickerManager = new FilePickerManager();
        filePickerManager.OpenFilePicker();
    }
}
