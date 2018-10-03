using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.AppCenter.Unity.Crashes;

public class PuppetConfirmationDialog : MonoBehaviour
{
    public void Send()
    {
        Crashes.NotifyUserConfirmation(Crashes.ConfirmationResult.Send);
        Hide();
    }

    public void AlwaysSend()
    {
        Crashes.NotifyUserConfirmation(Crashes.ConfirmationResult.AlwaysSend);
        Hide();
    }

    public void DontSend()
    {
        Crashes.NotifyUserConfirmation(Crashes.ConfirmationResult.DontSend);
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

