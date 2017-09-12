// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.Azure.Mobile.Unity.Distribute;
using UnityEngine;

// Very simple release handler.
public class PuppetReleaseHandler : MonoBehaviour
{
    private static ReleaseDetails _releaseDetails = null;
    private static readonly object _releaseLock = new object();

    void Awake()
    {
        Distribute.ReleaseAvailable = details =>
        {
            lock (_releaseLock)
            {
                _releaseDetails = details;
                return false;
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        lock (_releaseLock)
        {
            if (_releaseDetails != null)
            {
                print("There's a release available! Version = " + _releaseDetails.Version);
                _releaseDetails = null;
            }
        }
    }
}
