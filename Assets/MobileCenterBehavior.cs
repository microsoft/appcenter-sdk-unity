﻿﻿﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Microsoft.Azure.Mobile.Unity;
using Microsoft.Azure.Mobile.Unity.Analytics;
using Microsoft.Azure.Mobile.Unity.Crashes;
using Microsoft.Azure.Mobile.Unity.Push;
using Microsoft.Azure.Mobile.Unity.Distribute;

public class MobileCenterBehavior : MonoBehaviour
{
    private static PushNotificationReceivedEventArgs _pushEventArgs = null;
    private static object _pushLock = new object();
	void Start()
    {
		MobileCenter.LogLevel = LogLevel.Verbose;
#if UNITY_IOS
        const string appSecret = "dcf0de16-000e-477a-a55f-232380938aa8";
#elif UNITY_ANDROID
		const string appSecret = "56638f03-9802-4b53-95d4-e08ba9c9ac24";
#elif UNITY_WSA_10_0
        const string appSecret = "630db31d-f6b3-480e-925f-cbc1c5b12cdc";
#else
		const string appSecret = "secret";
#endif

		MobileCenter.SetLogUrl("https://in-integration.dev.avalanch.es");
		MobileCenter.Start(appSecret, typeof(Crashes), typeof(Push), typeof(Analytics), typeof(Distribute));
		SimpleLog("Found Install ID: " + MobileCenter.InstallId);

		Analytics.Enabled = false;
		Analytics.TrackEvent("should not be sent");
		Analytics.Enabled = true;

		Analytics.TrackEvent("event without properties");
		Analytics.TrackEvent("event with properties", new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } });

		Crashes.Enabled = true;

		var properties = new CustomProperties();
		properties.Set("stringkey", "stringval").Set("intkey", 4)
		            .Set("floatkey", 9.25f).Set("longkey", (long)43)
		            .Set("doublekey", (double)4.1349).Set("boolkey", false)
		            .Set("datekey", new DateTime(2017, 1, 4));

		//MobileCenter.SetCustomProperties(properties);
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
        const string printTag = "[MobileCenterBehavior]: ";
        print(printTag + message + '\n');
    }
}