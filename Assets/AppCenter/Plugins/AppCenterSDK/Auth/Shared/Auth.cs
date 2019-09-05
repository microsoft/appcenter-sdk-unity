// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.AppCenter.Unity.Auth.Internal;

namespace Microsoft.AppCenter.Unity.Auth
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Auth
    {
        // Used by App Center Unity Editor Extensions: https://github.com/Microsoft/AppCenter-SDK-Unity-Extension
        public const string AuthSDKVersion = "2.2.0";

        public static void AddNativeType(List<RawType> nativeTypes)
        {
            AuthInternal.AddNativeType(nativeTypes);
        }

        public static AppCenterTask<UserInformation> SignInAsync()
        {
            return AuthInternal.SignInAsync();
        }

        public static void SignOut()
        {
            AuthInternal.SignOut();
        }
        
        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return AuthInternal.IsEnabledAsync();
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
        {
            return AuthInternal.SetEnabledAsync(enabled);
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate bool SignInCompletionHandler(UserInformation userInformation, Exception error);
    }
}
