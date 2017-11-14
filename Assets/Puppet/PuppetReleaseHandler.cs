// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Distribute;
using UnityEngine;

// Very simple release handler.
public class PuppetReleaseHandler : MonoBehaviour
{
    private static ReleaseDetails _releaseDetails = null;
    private static readonly object _releaseLock = new object();

    public PuppetUpdateDialog Dialog;

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

    void Update()
    {
        if (_releaseDetails == null)
        {
            return;
        }
        lock (_releaseLock)
        {
            if (_releaseDetails != null)
            {
                if (Dialog != null)
                {
                    Dialog.Show(_releaseDetails);
                }
                print("There's a release available! Version = " + _releaseDetails.Version);
                _releaseDetails = null;
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Test update available")]
    void TestNewUpdate()
    {
        _releaseDetails = new ReleaseDetails
        {
            Version = "1.0.2"
        };
    }
#endif
}
