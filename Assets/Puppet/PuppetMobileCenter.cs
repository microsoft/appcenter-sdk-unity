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
            Debug.Log("Is enabled == " + task.Result);
            Enabled.isOn = task.Result;
        });
        
        LogLevel.value = MobileCenter.LogLevel - Microsoft.Azure.Mobile.Unity.LogLevel.Verbose;
    }

    public void SetEnabled(bool enabled)
    {
        MobileCenter.SetEnabledAsync(enabled).ContinueWith(task =>
        {
            Debug.Log("Set enabled complete");
            Enabled.isOn = enabled;
        });
    }

    public void SetLogLevel(int logLevel)
    {
        MobileCenter.LogLevel = Microsoft.Azure.Mobile.Unity.LogLevel.Verbose + logLevel;
    }
}
