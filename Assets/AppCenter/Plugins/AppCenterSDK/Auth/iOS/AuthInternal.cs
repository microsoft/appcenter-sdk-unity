// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

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
                else{
                    _signInTask.SetException(ConvertException(nsErrorPtr));
                }
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
            _signInTask = new AppCenterTask<UserInformation>();
            appcenter_unity_auth_sign_in_with_completion_handler(SignInCompletionHandlerNativeFunc);
            return _signInTask;
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

        public static Exception ConvertException(IntPtr nsErrorPtr)
        {
            if (nsErrorPtr == IntPtr.Zero)
            {
                return null;
            }
            var domain = app_center_unity_nserror_domain(nsErrorPtr);
            var errorCode = app_center_unity_nserror_code(nsErrorPtr);
            var description = app_center_unity_nserror_description(nsErrorPtr);
            return new Exception(string.Format("Domain: {0}, error code: {1}, description: {2}", domain, errorCode, description));
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

        [DllImport("__Internal")]
        private static extern string app_center_unity_nserror_domain(IntPtr error);

        [DllImport("__Internal")]
        private static extern long app_center_unity_nserror_code(IntPtr error);

        [DllImport("__Internal")]
        private static extern string app_center_unity_nserror_description(IntPtr error);

#endregion
    }
}
#endif
