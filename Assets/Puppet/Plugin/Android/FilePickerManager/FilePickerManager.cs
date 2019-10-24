// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using UnityEngine;

namespace Assets.Puppet.Plugins.Android.FilePickerManager
{
    public class FilePickerManager
    {
        private static AndroidJavaClass _push = new AndroidJavaClass("com.microsoft.appcenter.filepicker.FilePickerManager");

        public FilePickerManager()
        {
            _push.CallStatic("setListener", new FilePickerManagerDelegate());
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

        void onSelectFileSuccessful(string path)
        {
            FilePickerBehaviour.SetComplete(path);
        }

        void onSelectFileFailure(string message)
        {
            FilePickerBehaviour.SetFailed(message);
        }

        void onGetBytes(AndroidJavaObject bytesObject)
        {
            if (bytesObject == null)
            {
                Debug.LogFormat("Byte count: 0");
            }
            else
            {
                byte[] bytes = bytesObject.Call<byte[]>("toByteArray");
                Debug.LogFormat("Byte count: {0}", bytes.Length);
            }
        }
    }
}