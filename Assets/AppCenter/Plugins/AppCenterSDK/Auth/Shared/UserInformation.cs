// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Unity.Auth.Internal;

namespace Microsoft.AppCenter.Unity.Auth
{
    public class UserInformation
    {
        public string AccountId { get; private set; }
        public string AccessToken { get; private set; }
        public string IdToken { get; private set; }

        public UserInformation(string accountId, string accessToken, string idToken)
        {
            AccountId = accountId;
            AccessToken = accessToken;
            IdToken = idToken;
        }
    }
}
