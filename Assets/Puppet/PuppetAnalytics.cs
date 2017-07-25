using Microsoft.Azure.Mobile.Unity.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class PuppetAnalytics : MonoBehaviour
{
    public Toggle Enabled;
    public InputField EventName;

    void OnEnable()
    {
        Enabled.isOn = Analytics.Enabled;
    }

    public void SetEnabled(bool enabled)
    {
        Analytics.Enabled = enabled;
        Enabled.isOn = Analytics.Enabled;
    }

    public void TrackEvent()
    {
        Analytics.TrackEvent(EventName.text);
    }
}
