// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Runtime.InteropServices;

internal class IOSFilePicker : IFilePicker
{
    [DllImport("__Internal")]
    private static extern void CustomFilePicker_show();

    [DllImport("__Internal")]
    private static extern int GetFileBytes(string fileUrl, out IntPtr data);

    public static byte[] GetFileBytes(string fileUrl)
    {
        IntPtr unmanagedPtr;
        int size = GetFileBytes(fileUrl, out unmanagedPtr);
        byte[] mangedData = new byte[size];
        Marshal.Copy(unmanagedPtr, mangedData, 0, size);
        Marshal.FreeHGlobal(unmanagedPtr);
        return mangedData;
    }

    public void Show()
    {
        CustomFilePicker_show();
    }
}
