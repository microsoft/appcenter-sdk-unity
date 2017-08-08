// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using Microsoft.Azure.Mobile.Unity;
using Microsoft.Azure.Mobile.Unity.Push;

public class MobileCenterPushBehavior : MonoBehaviour
{
    [Header("Basic Setup")]
    public bool UsePush = true;

    public void Awake()
    {
        if (UsePush)
        {
            Push.PushNotificationReceived += HandlePushNotification;
            MobileCenter.Start(typeof(Push));
        }
    }

    public void HandlePushNotification(object sender, PushNotificationReceivedEventArgs push)
    {
        //TODO Custom implementation here to handle Push notifications.
        Debug.Log("Handle push notification");
    }
}
