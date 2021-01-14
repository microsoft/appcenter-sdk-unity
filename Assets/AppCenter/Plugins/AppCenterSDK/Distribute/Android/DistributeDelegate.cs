// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Distribute.Internal
{
    class DistributeDelegate : AndroidJavaProxy
    {
        private DistributeDelegate() : base("com.microsoft.appcenter.distribute.DistributeListener")
        {
        }

        public static void SetDelegate()
        {
            var distribute = new AndroidJavaClass("com.microsoft.appcenter.distribute.Distribute");
            distribute.CallStatic("setListener", new DistributeDelegate());
        }

        bool onReleaseAvailable(AndroidJavaObject activity, AndroidJavaObject details)
        {
            if (Distribute.ReleaseAvailable == null)
            {
                return false;
            }
            var releaseDetails = ReleaseDetailsHelper.ReleaseDetailsConvert(details);
            return Distribute.ReleaseAvailable.Invoke(releaseDetails);
        }

        void onNoReleaseAvailable(AndroidJavaObject activity)
        {
            if (Distribute.NoReleaseAvailable == null)
            {
                return;
            }
            Distribute.NoReleaseAvailable.Invoke();
        }
    }
}
#endif
