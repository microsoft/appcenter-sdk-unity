using Microsoft.Azure.Mobile.Unity.Crashes;
using UnityEngine;
using UnityEngine.UI;

public class PuppetCrashes : MonoBehaviour
{
    public Toggle Enabled;

    void OnEnable()
    {
        Enabled.isOn = Crashes.Enabled;
    }

    public void SetEnabled(bool enabled)
    {
        Crashes.Enabled = enabled;
        Enabled.isOn = Crashes.Enabled;
    }
    public void TestCrash()
    {
        //Crashes.GenerateTestCrash();
    }

    public void DivideByZero()
    {
        Debug.Log(42 / int.Parse("0"));
    }

    public void NullReferenceException()
    {
        string str = null;
        Debug.Log(str.Length);
    }
}
