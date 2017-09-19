using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuppetPushDialog : MonoBehaviour
{
    [SerializeField]
    private Text _titleText;

    [SerializeField]
    private Text _messageText;

    [SerializeField]
    private Text _dataText;

    public string Title
    {
        set { _titleText.text = value; }
    }

    public string Message
    {
        set { _messageText.text = value; }
    }

    public IDictionary<string, string> CustomData
    {
        set { _dataText.text = value != null ? string.Join("\n", value.Select(i => i.Key + " : " + i.Value).ToArray()) : ""; }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
