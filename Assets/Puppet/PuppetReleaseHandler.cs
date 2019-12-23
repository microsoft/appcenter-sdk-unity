// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Distribute;
using UnityEngine;

// Very simple release handler.
public class PuppetReleaseHandler : MonoBehaviour
{
    private static ReleaseDetails _releaseDetails;
    private static readonly object _releaseLock = new object();
    public static bool IsDialogCustom;
    public PuppetUpdateDialog Dialog;

    void Awake()
    {
        Distribute.ReleaseAvailable = OnReleaseAvailable;
        IsDialogCustom = PlayerPrefs.GetInt(PuppetAppCenter.FlagCustomDialog, 0);
    }

    bool OnReleaseAvailable(ReleaseDetails details)
    {
        lock (_releaseLock)
        {
            if (IsDialogCustom)
            {
                _releaseDetails = details;
                return true;
            }
            return false;
        }
    }

    void Update()
    {
        if (_releaseDetails == null)
        {
            return;
        }
        lock (_releaseLock)
        {
           if (Dialog != null && IsDialogCustom)
           {
               Dialog.Show(_releaseDetails);
           }
           print("There's a release available! Version = " + _releaseDetails.Version);
           _releaseDetails = null;
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
