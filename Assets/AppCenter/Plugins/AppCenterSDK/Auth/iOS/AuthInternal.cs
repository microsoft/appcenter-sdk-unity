// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.AppCenter.Unity.Internal.Utility;
using AOT;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Auth.Internal
{
    class AuthInternal
    {

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate void NativeSignInCompletionHandler(IntPtr userInformation, IntPtr error);
        private static AppCenterTask<UserInformation> _signInTask;

        [MonoPInvokeCallback(typeof(NativeSignInCompletionHandler))]
        public static void SignInCompletionHandlerNativeFunc(IntPtr userInformationPtr, IntPtr nsErrorPtr)
        {
            if (_signInTask != null)
            {
                var userInformation = GetUserInformationFromIntPtr(userInformationPtr);
                if (nsErrorPtr == IntPtr.Zero)
                {        
                    _signInTask.SetResult(userInformation);
                }
                else
                {
                    _signInTask.SetException(NSErrorHelper.ToSystemException(nsErrorPtr));
                }
                _signInTask = null;
            }
        }

        public static void PrepareEventHandlers()
        {
        }

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(appcenter_unity_auth_get_type());
        }
        
        public static AppCenterTask<UserInformation> SignInAsync()
        {          
            if (_signInTask != null)
            {
                var localsignInTask = new AppCenterTask<UserInformation>();
                var exception = new Exception("Sign-in already in progress");
                localsignInTask.SetException(exception);
                return localsignInTask;
            }
            else
            {
                // Create a local variable to avoid race condition
                var localsignInTask = new AppCenterTask<UserInformation>();
                _signInTask = localsignInTask;
                appcenter_unity_auth_sign_in_with_completion_handler(SignInCompletionHandlerNativeFunc);
                return localsignInTask;
            }
        }

        public static void SignOut()
        {
            if (_signInTask == null)
            {
                appcenter_unity_auth_sign_out();
            }
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

        public static UserInformation GetUserInformationFromIntPtr(IntPtr userInformationPtr)
        {
            if (userInformationPtr == IntPtr.Zero)
            {
                return null;
            }
            var accountId = app_center_unity_auth_user_information_account_id(userInformationPtr);
            var accessToken = app_center_unity_auth_user_information_access_token(userInformationPtr);
            var idToken = app_center_unity_auth_user_information_id_token(userInformationPtr);
            return new UserInformation(accountId, accessToken, idToken);
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_auth_get_type();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_auth_sign_in_with_completion_handler(NativeSignInCompletionHandler functionPtr);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_auth_sign_out();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_auth_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_auth_is_enabled();

        [DllImport("__Internal")]
        private static extern string app_center_unity_auth_user_information_account_id(IntPtr userInformationPtr);

        [DllImport("__Internal")]
        private static extern string app_center_unity_auth_user_information_access_token(IntPtr userInformationPtr);

        [DllImport("__Internal")]
        private static extern string app_center_unity_auth_user_information_id_token(IntPtr userInformationPtr);

#endregion
    }
}
#endif
