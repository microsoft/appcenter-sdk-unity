// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.Azure.Mobile.Unity.Distribute.Internal
{
    class DistributeDelegate
    {
#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        delegate bool ReleaseAvailableDelegate(IntPtr details);

        static ReleaseAvailableDelegate del;
        static IntPtr ptr;

        static DistributeDelegate()
        {
            del = ReleaseAvailableFunc;
            mobile_center_unity_distribute_delegate_provide_release_available_impl(del);
        }

        public static void SetDelegate()
        {
            mobile_center_unity_distribute_set_delegate();
        }

        [MonoPInvokeCallback(typeof(ReleaseAvailableDelegate))]
        static bool ReleaseAvailableFunc(IntPtr details)
        {
            if (Distribute.ReleaseAvailable == null)
            {
                return false;
            }
            var releaseDetails = ReleaseDetailsHelper.ReleaseDetailsConvert(details);
            return Distribute.ReleaseAvailable.Invoke(releaseDetails);
        }

#region External

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_distribute_set_delegate();

        [DllImport("__Internal")]
        private static extern void mobile_center_unity_distribute_delegate_provide_release_available_impl(ReleaseAvailableDelegate functionPtr);

#endregion
    }
}
#endif
