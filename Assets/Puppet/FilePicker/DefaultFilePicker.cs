using UnityEngine;

internal class DefaultFilePicker : IFilePicker
{
    public void Show()
    {
        Debug.LogError("Something went wrong with file picker. Platfom unsupported.");
    }
}
