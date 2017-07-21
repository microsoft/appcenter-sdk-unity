﻿﻿﻿﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;
using Microsoft.Azure.Mobile.Unity;
using Microsoft.Azure.Mobile.Unity.Crashes;

public class MobileCenterCrashesBehavior : MonoBehaviour
{
    [Header("Basic Setup")]
    public bool UseCrashes = true;

    public void Awake()
    {
        if (UseCrashes)
        {
            MobileCenter.Start(typeof(Crashes));
        }
    }
}
