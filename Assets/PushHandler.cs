﻿﻿﻿﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using Microsoft.Azure.Mobile.Unity.Push;

public class PushHandler : MonoBehaviour
{
    private static PushNotificationReceivedEventArgs _pushEventArgs = null;
    private static object _pushLock = new object();

    void Start()
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
        lock (_pushLock)
        {
            if (_pushEventArgs != null)
            {
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
                SimpleLog(pushSummary);
                _pushEventArgs = null;
            }
        }
    }


    void SimpleLog(object message)
    {
        const string printTag = "[PushHandler]: ";
        print(printTag + message + '\n');
    }
}
