// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using Microsoft.Azure.Mobile.Unity.Distribute.Internal;

namespace Microsoft.Azure.Mobile.Unity.Distribute
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Distribute
    {
        public static void Initialize()
        {
            DistributeInternal.Initialize();
        }

        public static void PostInitialize()
        {
            DistributeInternal.PostInitialize();
        }
        
        public static RawType GetNativeType()
        {
            return DistributeInternal.mobile_center_unity_distribute_get_type();
        }

        /// <summary>
        /// Enable or disable Distribute module.
        /// </summary>
        public static bool Enabled
        {
            get
            {
                return DistributeInternal.mobile_center_unity_distribute_is_enabled();
            }
            set
            {
                DistributeInternal.mobile_center_unity_distribute_set_enabled(value);
            }
        }

        /// <summary>
        /// Change the base URL opened in the browser to get update token from user login information.
        /// </summary>
        /// <param name="installUrl">Install base URL.</param>
        public static void SetInstallUrl(string installUrl)
        {
            DistributeInternal.mobile_center_unity_distribute_set_install_url(installUrl);
        }

        /// <summary>
        /// Change the base URL used to make API calls.
        /// </summary>
        /// <param name="apiUrl">API base URL.</param>
        public static void SetApiUrl(string apiUrl)
        {
            DistributeInternal.mobile_center_unity_distribute_set_api_url(apiUrl);
        }

        /// <summary>
        /// Sets the release available callback.
        /// </summary>
        /// <value>The release available callback.</value>
        public static ReleaseAvailableCallback ReleaseAvailable
        {
            get; set;
        }

        /// <summary>
        /// If update dialog is customized by returning <c>true</c> in <see cref="ReleaseAvailableCallback"/>,
        /// You need to tell the distribute SDK using this function what is the user action.
        /// </summary>
        /// <param name="updateAction">Update action. On mandatory update, you can only pass <see cref="UpdateAction.Update"/></param>
        public static void NotifyUpdateAction(UpdateAction updateAction)
        {
            DistributeInternal.mobile_center_unity_distribute_notify_update_action((int)updateAction);
        }
    }
}
