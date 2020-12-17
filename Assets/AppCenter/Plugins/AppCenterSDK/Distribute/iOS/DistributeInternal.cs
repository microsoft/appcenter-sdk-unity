// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

namespace Microsoft.AppCenter.Unity.Distribute.Internal
{
    class DistributeInternal
    {
#region Distribute delegate
#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        delegate void WillExitAppDelegate();
        delegate void NoReleaseAvailableDelegate();
        delegate bool ReleaseAvailableDelegate(IntPtr details);

        static WillExitAppDelegate willExitAppDel;
        static NoReleaseAvailableDelegate noReleaseDel;
        static ReleaseAvailableDelegate del;
        static IntPtr ptr;
        
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

        [MonoPInvokeCallback(typeof(NoReleaseAvailableDelegate))]
        static void NoReleaseAvailableFunc()
        {
            Distribute.NoReleaseAvailable?.Invoke();
        }

        [MonoPInvokeCallback(typeof(WillExitAppDelegate))]
        static void WillExitAppFunc()
        {
            Distribute.WillExitApp?.Invoke();
        }
#endregion

        public static void PrepareEventHandlers()
        {
            AppCenterBehavior.InitializingServices += Initialize;
            AppCenterBehavior.Started += StartBehavior;
        }

        private static void StartBehavior()
        {
            appcenter_unity_distribute_replay_release_available();
        }

        private static void Initialize()
        {
            appcenter_unity_distribute_set_delegate();
            del = ReleaseAvailableFunc;
            willExitAppDel = WillExitAppFunc;
            noReleaseDel = NoReleaseAvailableFunc;
            appcenter_unity_distribute_set_release_available_impl(del);
            appcenter_unity_distribute_set_no_release_available_impl(noReleaseDel);
            appcenter_unity_distribute_set_will_exit_app_impl(willExitAppDel);
        }

        public static void StartDistribute()
        {
            appcenter_unity_start_distribute();
        }

        public static void AddNativeType(List<IntPtr> nativeTypes)
        {
            nativeTypes.Add(appcenter_unity_distribute_get_type());
        }

        public static AppCenterTask SetEnabledAsync(bool isEnabled)
        {
            appcenter_unity_distribute_set_enabled(isEnabled);
            return AppCenterTask.FromCompleted();
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            var isEnabled = appcenter_unity_distribute_is_enabled();
            return AppCenterTask<bool>.FromCompleted(isEnabled);
        }

        public static void SetInstallUrl(string installUrl)
        {
            appcenter_unity_distribute_set_install_url(installUrl);
        }

        public static void SetApiUrl(string apiUrl)
        {
            appcenter_unity_distribute_set_api_url(apiUrl);
        }

        public static void NotifyUpdateAction(int updateAction)
        {
            appcenter_unity_distribute_notify_update_action(updateAction);
        }

        public static void CheckForUpdate()
        {
            appcenter_unity_distribute_check_for_update();
        }

#region External

        [DllImport("__Internal")]
        private static extern IntPtr appcenter_unity_distribute_get_type();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_set_enabled(bool isEnabled);

        [DllImport("__Internal")]
        private static extern bool appcenter_unity_distribute_is_enabled();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_set_install_url(string installUrl);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_set_api_url(string apiUrl);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_notify_update_action(int updateAction);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_set_release_available_impl(ReleaseAvailableDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_set_will_exit_app_impl(WillExitAppDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_set_no_release_available_impl(NoReleaseAvailableDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_replay_release_available();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_set_delegate();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_distribute_check_for_update();

        [DllImport("__Internal")]
        private static extern void appcenter_unity_start_distribute();

#endregion
    }
}
#endif
