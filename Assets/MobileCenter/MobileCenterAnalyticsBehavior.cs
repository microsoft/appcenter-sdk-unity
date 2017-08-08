// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using Microsoft.Azure.Mobile.Unity;
using Microsoft.Azure.Mobile.Unity.Analytics;

public class MobileCenterAnalyticsBehavior : MonoBehaviour
{
    [Header("Basic Setup")]
    public bool UseAnalytics = true;


    public void Awake()
    {
       MobileCenter.Start(typeof(Analytics));
    }
}
