// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Internal.Utility;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Auth.Internal
{
    class AuthInternal
    {
        private static AndroidJavaClass _auth = new AndroidJavaClass("com.microsoft.appcenter.auth.Auth");

        public static void PrepareEventHandlers()
        {
            AppCenterBehavior.InitializedAppCenterAndServices += PostInitialize;
        }

        private static void PostInitialize()
        {
            var instance = _auth.CallStatic<AndroidJavaObject>("getInstance");
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            instance.Call("onActivityResumed", activity);
        }

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(_auth.GetRawClass());
        }

        public static AppCenterTask<UserInformation> SignInAsync()
        {
            var future = _auth.CallStatic<AndroidJavaObject>("signIn");
            var javaTask = new AppCenterTask<AndroidJavaObject>(future);
            var signInTask = new AppCenterTask<UserInformation>();
            javaTask.ContinueWith(t =>
            {
                var exception = t.Result.Call<AndroidJavaObject>("getException");
                if (exception != null)
                {
                    //TODO handle exception
                }
                var userInfo = t.Result.Call<AndroidJavaObject>("getUserInformation");
                var aid = userInfo.Call<string>("getAccountId");
                var atoken = userInfo.Call<string>("getAccessToken");
                var it = userInfo.Call<string>("getIdToken");
                signInTask.SetResult(new UserInformation(aid, atoken, it));
            });
            return signInTask;
        }

        public static void SignOut()
        {
            _auth.CallStatic("signOut");
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            var future = _auth.CallStatic<AndroidJavaObject>("setEnabled", isEnabled);
            return new AppCenterTask(future);
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var future = _auth.CallStatic<AndroidJavaObject>("isEnabled");
            return new AppCenterTask<bool>(future);
        }
    }
}
#endif
