// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Distribute;
using UnityEngine;
using UnityEngine.UI;

public class PuppetUpdateDialog : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private Text _messageText;
#pragma warning restore 0649

    public void PerformUpdate ()
    {
        Distribute.NotifyUpdateAction(UpdateAction.Update);
        Hide();
    }

    public void PostponeUpdate ()
    {
        Distribute.NotifyUpdateAction(UpdateAction.Postpone);
        Hide();
    }

    public void Show(ReleaseDetails releaseDetails)
    {
        _messageText.text = "Perform update to version " + releaseDetails.Version + " ?";
        gameObject.SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}
