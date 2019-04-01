// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Distribute.Internal
{
    class ReleaseDetailsHelper
    {
        public static ReleaseDetails ReleaseDetailsConvert(AndroidJavaObject details)
        {
            var id = details.Call<int>("getId");
            var version = details.Call<int>("getVersion").ToString();
            var shortVersion = details.Call<string>("getShortVersion");
            var releaseNotes = details.Call<string>("getReleaseNotes");
            var mandatoryUpdate = details.Call<bool>("isMandatoryUpdate");
            var javaUri = details.Call<AndroidJavaObject>("getReleaseNotesUrl");
            var uriString = javaUri.Call<string>("toString");
            var uri = new Uri(uriString);

            return new ReleaseDetails
            {
                Id = id,
                Version = version,
                ShortVersion = shortVersion,
                ReleaseNotes = releaseNotes,
                MandatoryUpdate = mandatoryUpdate,
                ReleaseNotesUrl = uri
            };
        }
    }
}
#endif
