// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEngine;

namespace Assets.AppCenter.Plugins.Android.Utility
{
    public class FilePickerManager
    {
        private static AndroidJavaClass _push = new AndroidJavaClass("com.microsoft.appcenter.FilePickerManager");

        public FilePickerManager()
        {
            _push.CallStatic("setListener", new FilePickerManagerDelegate());
            OpenFilePicker();
        }

        public void OpenFilePicker()
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            _push.CallStatic("openFilePicker", activity);
        }

        public void ReadTextFromUri(string path)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            _push.CallStatic("readTextFromUri", activity, path);
        }
    }

    class FilePickerManagerDelegate : AndroidJavaProxy
    {
        public FilePickerManagerDelegate() : base("com.microsoft.appcenter.FilePickerManager.FilePickerManagerListener")
        {
        }

        void onSelectFileSuccessful(AndroidJavaObject objectPath)
        {
            var path = objectPath.Call<string>("toString");
        }

        void onSelectFileFailure(AndroidJavaObject objectMessage)
        {
            var message = objectMessage.Call<string>("toString");
        }
    }
}