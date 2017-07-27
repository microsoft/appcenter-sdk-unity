using Microsoft.Azure.Mobile.Unity;
using UnityEngine;
using UnityEngine.UI;

public class PuppetMobileCenter : MonoBehaviour
{
    public Toggle Enabled;
    public Dropdown LogLevel;
    
    void OnEnable()
    {
        Enabled.isOn = MobileCenter.Enabled;
        LogLevel.value = MobileCenter.LogLevel - Microsoft.Azure.Mobile.Unity.LogLevel.Verbose;
    }

    public void SetEnabled(bool enabled)
    {
        MobileCenter.Enabled = enabled;
        Enabled.isOn = MobileCenter.Enabled;
    }

    public void SetLogLevel(int logLevel)
    {
        MobileCenter.LogLevel = Microsoft.Azure.Mobile.Unity.LogLevel.Verbose + logLevel;
    }
}
