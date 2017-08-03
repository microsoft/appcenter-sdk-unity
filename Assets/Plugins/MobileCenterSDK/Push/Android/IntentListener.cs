// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if PSYOUIKJ && UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

public class IntentListener : AndroidJavaProxy
{
    private IntentListener() : base("com.microsoft.azure.mobile.mobilecenterunityplayeractivity.IntentListener")
    {
    }

    public static void SetListener()
    {
        MonoBehaviour.print("UNITY: setting intent listener");
        AndroidJavaClass mobileCenterActivity = new AndroidJavaClass("com.microsoft.azure.mobile.mobilecenterunityplayeractivity.MobileCenterUnityPlayerActivity");
        mobileCenterActivity.CallStatic("setListener", new IntentListener());
    }

    public void onNewIntent(AndroidJavaObject activity, AndroidJavaObject intent)
    {

    }
}
#endif
