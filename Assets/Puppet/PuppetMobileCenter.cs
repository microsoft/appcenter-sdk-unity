using Microsoft.Azure.Mobile.Unity;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuppetMobileCenter : MonoBehaviour
{
    public Toggle Enabled;
    public Dropdown LogLevel;

    void OnEnable()
    {
        MobileCenter.IsEnabledAsync().ContinueWith(task =>
        {
            Enabled.isOn = task.Result;
        });
        MobileCenter.GetInstallIdAsync().ContinueWith(task =>
        {
            if (task.Result.HasValue)
            {
                print("Install ID = " + task.Result.ToString());
            }
        });
        LogLevel.value = MobileCenter.LogLevel - Microsoft.Azure.Mobile.Unity.LogLevel.Verbose;
    }

    public void SetEnabled(bool enabled)
    {
        MobileCenter.SetEnabledAsync(enabled).ContinueWith(task =>
        {
            Enabled.isOn = enabled;
        });
    }

    public void SetLogLevel(int logLevel)
    {
        MobileCenter.LogLevel = Microsoft.Azure.Mobile.Unity.LogLevel.Verbose + logLevel;
    }
}
