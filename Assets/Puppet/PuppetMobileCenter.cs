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
		var task = MobileCenter.IsEnabledAsync();
        StartCoroutine(UpdateToggleOnIsEnabled(task));
		LogLevel.value = MobileCenter.LogLevel - Microsoft.Azure.Mobile.Unity.LogLevel.Verbose;
    }

    public void SetEnabled(bool enabled)
    {
        var task = MobileCenter.SetEnabledAsync(enabled);
        StartCoroutine(UpdateToggleOnSetEnabled(task, enabled));
    }

    private IEnumerator UpdateToggleOnSetEnabled(MobileCenterTask task, bool wasEnabled)
    {
        yield return task.Wait();
		Enabled.isOn = wasEnabled;
	}

	private IEnumerator UpdateToggleOnIsEnabled(MobileCenterTask<bool> task)
	{
		yield return task.Wait();
		Enabled.isOn = task.Result;
	}

    public void SetLogLevel(int logLevel)
    {
        MobileCenter.LogLevel = Microsoft.Azure.Mobile.Unity.LogLevel.Verbose + logLevel;
    }
}
