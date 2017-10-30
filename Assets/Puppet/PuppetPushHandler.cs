// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System.Linq;
using Microsoft.AppCenter.Unity.Push;
using UnityEngine;

public class PuppetPushHandler : MonoBehaviour
{
    private static PushNotificationReceivedEventArgs _pushEventArgs = null;
    private static object _pushLock = new object();

    public PuppetPushDialog Dialog;

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

            // Print summary to log.
            var pushSummary = "Push notification received:" +
                              "\n\tNotification title: " + _pushEventArgs.Title +
                              "\n\tMessage: " + _pushEventArgs.Message;
            if (_pushEventArgs.CustomData != null)
            {
                pushSummary += string.Join("\n",
                    _pushEventArgs.CustomData.Select(i => "\t\t" + i.Key + " : " + i.Value).ToArray());
            }
            print(pushSummary);

            // Clear event arguments.
            _pushEventArgs = null;
        }
    }
}
