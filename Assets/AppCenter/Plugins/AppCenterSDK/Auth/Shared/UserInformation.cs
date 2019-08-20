// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Auth.Internal;

namespace Microsoft.AppCenter.Unity.Auth
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class UserInformation
    {
        public string AccountId;

        public string AccessToken;

        public string IdToken;

        public UserInformation(string accountId, string accessToken, string idToken)
        {
            AccountId = accountId;
            AccessToken = accessToken;
            IdToken = idToken;
        }

        public string getAccountId()
        {
            return AccountId;
        }
        
        public string getAccessToken()
        {
            return AccessToken;
        }

        public string getIdToken()
        {
            return IdToken;
        }

        private void setAccountId(string accountId)
        {
            AccountId = accountId;
        }

        private void setAccessToken(string accessToken)
        {
            AccessToken = accessToken;
        }

        private void setIdToken(string idToken)
        {
            IdToken = idToken;
        }
    }
}
