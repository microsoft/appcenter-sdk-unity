// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Auth.Internal
{
    class AuthInternal
    {
        public static void PrepareEventHandlers()
        {
        }

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(appcenter_unity_auth_get_type());
        }

        public static AppCenterTask<UserInformation> SignInAsync()
        {
            var userInformation = appcenter_unity_auth_sign_in();
            return AppCenterTask<UserInformation>.FromCompleted(userInformation);
        }

        public static void SignOut()
        {
            appcenter_unity_auth_sign_out();
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            appcenter_unity_auth_set_enabled(isEnabled);
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = appcenter_unity_auth_is_enabled();
            return AppCenterTask<bool>.FromCompleted(isEnabled);
        }


#region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_auth_get_type();

        [DllImport("__Internal")]
        private static extern UserInformation appcenter_unity_auth_sign_in();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_auth_sign_out();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_auth_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_auth_is_enabled();

#endregion
    }
}
#endif
