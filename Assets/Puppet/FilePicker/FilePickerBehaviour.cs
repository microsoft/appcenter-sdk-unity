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
    public static event FileDelegate Completed;
    public static event ErrorDelegate Failed;
    private IFilePicker filePicker;

    public void Awake()
    {
        Completed += OnFilePicked;
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

    public void SelectFileErrorAttachment()
    {
        filePicker.Show();
    }

    private void onSelectFileSuccessful(string path)
    {
        SetComplete(path);
    }

    private void onSelectFileFailure(string message)
    {
        SetFailed(message);
    }

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

}
