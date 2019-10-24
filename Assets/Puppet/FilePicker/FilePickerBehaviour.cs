// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using UnityEngine;
using UnityEngine.UI;

public class FilePickerBehaviour : MonoBehaviour
{
    [SerializeField]
    private Text BinaryAttachment;
    public delegate void FileDelegate(string path);
    public delegate void ErrorDelegate(string message);
    public delegate void GetBytesDelegate(byte[] bytes);
    public static event FileDelegate Completed;
    public static event ErrorDelegate Failed;
    public static event GetBytesDelegate GetBytes;
    private IFilePicker filePicker;

    public void Awake()
    {
        Completed += OnFilePicked;
        GetBytes += OnGetBytes;
        filePicker =
#if UNITY_IOS && !UNITY_EDITOR
        new IOSFilePicker();
#elif UNITY_ANDROID && !UNITY_EDITOR
        new AndroidFilePicker();
#else
        new DefaultFilePicker();
#endif
    }

    private void OnFilePicked(string filePath)
    {
        BinaryAttachment.text = filePath;
        PlayerPrefs.SetString(PuppetAppCenter.BinaryAttachmentKey, filePath);
    }

    private void OnGetBytes(byte[] bytes)
    {
        // todo send attachments
    }

    public void SelectFileErrorAttachment()
    {
        filePicker.Show();
    }

    public void InitBytesFileErrorAttachment(string path)
    {
        filePicker.InitBytes(path);
    }

    #region for iOS

    private void onSelectFileSuccessful(string path)
    {
        SetComplete(path);
    }

    private void onSelectFileFailure(string message)
    {
        SetFailed(message);
    }

    #endregion

    public static void SetComplete(string path)
    {
        var handler = Completed;
        if (handler != null)
        {
            handler(path);
        }
    }

    public static void SetFailed(string message)
    {
        var handler = Failed;
        if (handler != null)
        {
            handler(message);
        }
    }

    public static void SetBytes(byte[] bytes)
    {
        var handler = GetBytes;
        if (handler != null)
        {
            handler(bytes);
        }
    }
}
