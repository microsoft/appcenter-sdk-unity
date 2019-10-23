using System;
using UnityEngine;

public class FilePickerBehaviour : MonoBehaviour
{
    public delegate void FileDelegate(string path);

    public delegate void ErrorDelegate(string message);

    public static event FileDelegate Completed;

    public static event ErrorDelegate Failed;

    private IFilePicker picker =
#if UNITY_IOS && !UNITY_EDITOR
        new IOSFilePicker();
#elif UNITY_ANDROID && !UNITY_EDITOR
        new AndroidFilePicker();
#else
        new DefaultFilePicker();
#endif

    public void Show()
    {
        picker.Show();
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
