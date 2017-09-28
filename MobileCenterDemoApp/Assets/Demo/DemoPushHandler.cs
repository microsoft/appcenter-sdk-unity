// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity.Push;
using UnityEngine;

public class DemoPushHandler : MonoBehaviour
{
    private static PushNotificationReceivedEventArgs _pushEventArgs = null;
    private static object _pushLock = new object();

    public DemoPushDialog Dialog;

    void Awake()
    {
        Push.PushNotificationReceived += (sender, e) =>
        {
            lock (_pushLock)
            {
                _pushEventArgs = e;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (_pushEventArgs == null)
        {
            return;
        }
        lock (_pushLock)
        {
            if (_pushEventArgs == null)
            {
                return;
            }

            // Show dialog.
            if (Dialog != null)
            {
                Dialog.Title = _pushEventArgs.Title;
                Dialog.Message = _pushEventArgs.Message;
                Dialog.CustomData = _pushEventArgs.CustomData;
                Dialog.Show();
            }

            var pushSummary = "Push notification received:" +
                "\n\tNotification title: " + _pushEventArgs.Title +
                "\n\tMessage: " + _pushEventArgs.Message;
            if (_pushEventArgs.CustomData != null)
            {
                pushSummary += "\n\tCustom data:\n";
                foreach (var key in _pushEventArgs.CustomData.Keys)
                {
                    pushSummary += "\t\t" + key + " : " + _pushEventArgs.CustomData[key] + "\n";
                }
            }
            print(pushSummary);
            _pushEventArgs = null;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test Notification")]
    void TestNotification()
    {
        _pushEventArgs = new PushNotificationReceivedEventArgs
        {
            Title = "Some notification",
            Message = 
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. " +
            "Etiam non est sit amet dui porta varius quis sed massa. " +
            "Nullam libero libero, porta at augue eget, malesuada venenatis risus.",
            CustomData = null
        };
    }
#endif
}
