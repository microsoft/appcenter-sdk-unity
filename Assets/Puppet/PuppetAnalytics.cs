using Microsoft.Azure.Mobile.Unity.Analytics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuppetAnalytics : MonoBehaviour
{
    public Toggle Enabled;
    public InputField EventName;
    public GameObject EventProperty;
    public RectTransform EventPropertiesList;

    void OnEnable()
    {
        Enabled.isOn = Analytics.Enabled;
    }

    public void SetEnabled(bool enabled)
    {
        Analytics.Enabled = enabled;
        Enabled.isOn = Analytics.Enabled;
    }

    public void AddProperty()
    {
        var property = Instantiate(EventProperty);
        property.transform.SetParent(EventPropertiesList, false);
    }

    public void TrackEvent()
    {
        Analytics.TrackEvent(EventName.text, GetProperties());
    }

    private Dictionary<string, string> GetProperties()
    {
        var properties = EventPropertiesList.GetComponentsInChildren<PuppetEventProperty>();
        if (properties == null || properties.Length == 0)
        {
            return null;
        }
        return properties.ToDictionary(i => i.Key.text, i => i.Value.text);
    }
}
