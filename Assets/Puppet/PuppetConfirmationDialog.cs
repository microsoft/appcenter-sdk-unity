using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AppCenter.Unity.Crashes;

public class PuppetConfirmationDialog : MonoBehaviour
{
    public void Send()
    {
        Crashes.NotifyWithUserConfirmation(Crashes.ConfirmationResult.Send);
        Hide();
    }

    public void AlwaysSend()
    {
        Crashes.NotifyWithUserConfirmation(Crashes.ConfirmationResult.AlwaysSend);
        Hide();
    }

    public void DontSend()
    {
        Crashes.NotifyWithUserConfirmation(Crashes.ConfirmationResult.DontSend);
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

