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

    public void SetEnabled(bool enabled)
    {
        StartCoroutine(SetEnabledCoroutine(enabled));
    }

    public void SignIn()
    {
        var signInTaskTwo = Auth.SignInAsync();
        signInTaskTwo.ContinueWith(t => 
        {
            if (t.Exception != null)
            {
                AuthStatus.text = "Sign In failed. User is not authenticated";
                AccountIdName.text = "Exception";
                AccountIdLabel.text = t.Exception.ToString();
            }
            else{
                var userInformation = t.Result;
                if (userInformation == null)
                {
                    AuthStatus.text = "Sign In failed. User is not authenticated";
                }
                else
                {
                    var accountId = userInformation.AccountId;
                    var accessToken = userInformation.AccessToken;
                    var idToken = userInformation.IdToken;
                    AuthStatus.text = "Sign in succeeded. User is authenticated";
                    AccountIdName.text = "Account Id";
                    AccessTokenName.text = "Access Token";
                    IdTokenName.text = "Id Token";
                    AccountIdLabel.text = accountId;
                    AccessTokenLabel.text = convertToken(accessToken);
                    IdTokenLabel.text = convertToken(idToken);
                }
            }
        });
    }

    public void SignOut()
    {
        Auth.SignOut();
        userInformation = null;
        AuthStatus.text = "Sign out succeeded. User is not authenticated";
        AccountIdName.enabled = false;
        AccessTokenName.enabled = false;
        IdTokenName.enabled = false;
        AccountIdLabel.enabled = false;
        AccessTokenLabel.enabled = false;
        IdTokenLabel.enabled = false;
    }

    private IEnumerator SetEnabledCoroutine(bool enabled)
    {
        yield return Auth.SetEnabledAsync(enabled);
        var isEnabled = Auth.IsEnabledAsync();
        yield return isEnabled;
        AuthEnabled.isOn = isEnabled.Result;
    }

    public string convertToken(string encodedText)
    {
        char[] separator = { '.' };
        string[] splitedText = encodedText.Split(separator);
        if (splitedText.Length < 2) 
        {
            return null;
        }
        string textToDecode = splitedText[1];
        if (textToDecode.Length % 4 != 0)
        {
            int extraChars = 4 - textToDecode.Length % 4;
            textToDecode = String.Concat(textToDecode, new string('=', extraChars));
        }
        byte[] decodedBytes = Convert.FromBase64String (textToDecode);
        string decodedText = Encoding.UTF8.GetString (decodedBytes);
        return decodedText;
    }
}