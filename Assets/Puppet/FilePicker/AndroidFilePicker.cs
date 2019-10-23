using Assets.Puppet.Plugins.Android.FilePickerManager;

public class AndroidFilePicker : IFilePicker
{
    public void Show()
    {
        FilePickerManager filePickerManager = new FilePickerManager();
        filePickerManager.OpenFilePicker();
    }
}
