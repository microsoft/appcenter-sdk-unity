// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using Microsoft.Azure.Mobile.Unity;
using Microsoft.Azure.Mobile.Unity.Distribute;

public class MobileCenterDistributeBehavior : MonoBehaviour
{
    [Header("Basic Setup")]
    public bool UseDistribute = true;

    public void Awake()
    {
        if (UseDistribute)
        {
            MobileCenter.Start(typeof(Distribute));
        }
    }
}
