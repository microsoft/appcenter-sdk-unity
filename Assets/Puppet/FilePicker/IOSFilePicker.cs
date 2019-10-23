using System.Runtime.InteropServices;

internal class IOSFilePicker : IFilePicker
{
    [DllImport("__Internal")]
    private static extern void CustomFilePicker_show();

    public void Show()
    {
        CustomFilePicker_show();
    }
}
