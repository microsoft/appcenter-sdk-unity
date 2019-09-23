// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Text;
using System.Collections;
using Microsoft.AppCenter.Unity.Auth;
using UnityEngine;
using UnityEngine.UI;

public class PuppetAuth : MonoBehaviour
{
    public Toggle AuthEnabled;
    public Text AuthStatus;
    public UserInformation userInformation;
    public Text AccountIdName;
    public Text AccessTokenName;
    public Text IdTokenName;
    public Text AccountIdLabel;
    public Text AccessTokenLabel;
    public Text IdTokenLabel;
    public const string UserNotAuthenticated = "Sign In failed. User is not authenticated.";
    public const string TokenSet = "Set";
    public const string TokenUnset = "Unset";

    void OnEnable()
    {
        StartCoroutine(OnEnableCoroutine());
    }

    private IEnumerator OnEnableCoroutine()
    {
        var task = Auth.IsEnabledAsync();
        yield return task;
        AuthEnabled.isOn = task.Result;
    }

    private IEnumerator SignInCoroutine()
    {
        var signInTask = Auth.SignInAsync();
            if (signInTask.Exception != null)
            {
            yield return signInTask.Exception;
                AuthStatus.text = UserNotAuthenticated;
                AccountIdName.enabled = enabled;
                AccountIdName.text = "Exception";
                AccountIdLabel.enabled = enabled;
                AccountIdLabel.text = signInTask.Exception.ToString();
            }
            else
        {
            var userInformation = signInTask.Result;
            yield return userInformation;
            if (userInformation == null)
                {
                    AuthStatus.text = UserNotAuthenticated;
                }
                else
                {
                    AuthStatus.text = "Sign in succeeded. User is authenticated.";
                    ShowLabels(true);
                    var accountId = userInformation.AccountId;
                    var accessToken = userInformation.AccessToken;
                    var idToken = userInformation.IdToken;
                    AccountIdName.text = "Account Id";
                    AccessTokenName.text = "Access Token";
                    IdTokenName.text = "Id Token";
                    AccountIdLabel.text = accountId;
                    AccessTokenLabel.text = accessToken == null ? TokenUnset : TokenSet;
                    IdTokenLabel.text = idToken == null ? TokenUnset : TokenSet;
                }
            }
        
    }

    public void SetEnabled(bool enabled)
    {
        StartCoroutine(SetEnabledCoroutine(enabled));
    }

    public void SignIn()
    {
        StartCoroutine(SignInCoroutine());
    }

    public void SignOut()
    {
        Auth.SignOut();
        userInformation = null;
        AuthStatus.text = "Sign out succeeded. User is not authenticated.";
        ShowLabels(false);
    }

    private IEnumerator SetEnabledCoroutine(bool enabled)
    {
        yield return Auth.SetEnabledAsync(enabled);
        var isEnabled = Auth.IsEnabledAsync();
        yield return isEnabled;
        AuthEnabled.isOn = isEnabled.Result;
    }

    private void ShowLabels(bool enabled)
    {
        AccountIdName.enabled = enabled;
        AccessTokenName.enabled = enabled;
        IdTokenName.enabled = enabled;
        AccountIdLabel.enabled = enabled;
        AccessTokenLabel.enabled = enabled;
        IdTokenLabel.enabled = enabled;
    }
}
