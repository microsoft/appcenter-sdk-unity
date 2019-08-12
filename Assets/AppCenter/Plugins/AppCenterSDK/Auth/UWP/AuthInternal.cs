// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Auth.Internal
{
    using UWPAuth = Microsoft.AppCenter.Auth.Auth;

    class AuthInternal
    {
        private static bool _warningLogged = false;

        public static void PrepareEventHandlers()
        {
        }

        public static void AddNativeType(List<Type> nativeTypes)
        {
            nativeTypes.Add(typeof(UWPAuth));
        }

        public static AppCenterTask<UserInformation> SignInAsync()
        {
            return AppCenterTask<UserInformation>.FromCompleted(UWPAuth.SignInAsync());
        }

        public static void SignOut()
        {
            UWPAuth.SignOut();
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            return new AppCenterTask(UWPAuth.SetEnabledAsync(isEnabled));
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return new AppCenterTask<bool>(UWPAuth.IsEnabledAsync());
        }
    }
}
#endif