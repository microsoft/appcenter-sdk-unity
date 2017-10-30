// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Distribute.Internal
{
    class ReleaseDetailsHelper
    {
        public static ReleaseDetails ReleaseDetailsConvert(IntPtr details)
        {
            var id = mobile_center_unity_release_details_get_id(details);
            var version = mobile_center_unity_release_details_get_version(details);
            var shortVersion = mobile_center_unity_release_details_get_short_version(details);
            var releaseNotes = mobile_center_unity_release_details_get_release_notes(details);
            var mandatoryUpdate = mobile_center_unity_release_details_get_mandatory_update(details);
            var urlString = mobile_center_unity_release_details_get_url(details);
            var uri = new Uri(urlString);

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

#region External

        [DllImport("__Internal")]
        private static extern int mobile_center_unity_release_details_get_id(IntPtr details);

        [DllImport("__Internal")]
        private static extern string mobile_center_unity_release_details_get_version(IntPtr details);

        [DllImport("__Internal")]
        private static extern string mobile_center_unity_release_details_get_short_version(IntPtr details);

        [DllImport("__Internal")]
        private static extern string mobile_center_unity_release_details_get_release_notes(IntPtr details);

        [DllImport("__Internal")]
        private static extern bool mobile_center_unity_release_details_get_mandatory_update(IntPtr details);

        [DllImport("__Internal")]
        private static extern string mobile_center_unity_release_details_get_url(IntPtr details);

#endregion
    }
}
#endif
