using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AppCenter.Unity.Crashes;

public class PuppetConfirmationDialog : MonoBehaviour
{
    public void Send()
    {
        Crashes.NotifyWithUserConfirmation(1);
        Hide();
    }

    public void AlwaysSend()
    {
        Crashes.NotifyWithUserConfirmation(2);
        Hide();
    }

    public void DontSend()
    {
        Crashes.NotifyWithUserConfirmation(0);
        Hide();
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

