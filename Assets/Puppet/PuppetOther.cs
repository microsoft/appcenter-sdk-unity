using Microsoft.Azure.Mobile.Unity.Distribute;
using Microsoft.Azure.Mobile.Unity.Push;
using UnityEngine;
using UnityEngine.UI;

public class PuppetOther : MonoBehaviour
{
    public Toggle DistributeEnabled;
    public Toggle PushEnabled;

    void OnEnable()
    {
        DistributeEnabled.isOn = Distribute.Enabled;
        PushEnabled.isOn = Push.Enabled;
    }

    public void SetDistributeEnabled(bool enabled)
    {
        Distribute.Enabled = enabled;
        DistributeEnabled.isOn = Distribute.Enabled;
    }

    public void SetPushEnabled(bool enabled)
    {
        Push.Enabled = enabled;
        PushEnabled.isOn = Push.Enabled;
    }
}
