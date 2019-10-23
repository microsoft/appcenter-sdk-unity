// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using UnityEngine;

namespace Assets.AppCenter.Plugins.Android.Utility
{
    public class FilePickerManager
    {
        private static AndroidJavaClass _push = new AndroidJavaClass("com.microsoft.appcenter.filepicker.FilePickerManager");

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
        public FilePickerManagerDelegate() : base("com.microsoft.appcenter.filepicker.FilePickerManagerListener")
        {
        }

        void onSelectFileSuccessful(string objectPath)
        {
            PlayerPrefs.SetString(PuppetAppCenter.BinaryAttachmentKey, objectPath);
        }

        void onSelectFileFailure(string objectMessage)
        {
            System.Diagnostics.Debug.WriteLine("Faild select file with error message: {0}", objectMessage);
        }
    }
}