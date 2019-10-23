using UnityEngine;

public class FilePickerBehaviour : MonoBehaviour
{
    public delegate void FileDelegate(string path);

    public delegate void ErrorDelegate(string message);

    public event FileDelegate Completed;

    public event ErrorDelegate Failed;

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
        var handler = Completed;
        if (handler != null)
        {
            handler(path);
        }
    }

    private void onSelectFileFailure(string message)
    {
        var handler = Failed;
        if (handler != null)
        {
            handler(message);
        }
    }
}
