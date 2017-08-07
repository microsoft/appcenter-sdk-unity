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
            return DistributeInternal.GetType();
        }

        public static MobileCenterTask<bool> IsEnabledAsync()
        {
            return DistributeInternal.IsEnabledAsync();
        }

        public static MobileCenterTask SetEnabledAsync(bool enabled)
        {
            return DistributeInternal.SetEnabledAsync(enabled);
        }

        /// <summary>
        /// Change the base URL opened in the browser to get update token from user login information.
        /// </summary>
        /// <param name="installUrl">Install base URL.</param>
        public static void SetInstallUrl(string installUrl)
        {
            DistributeInternal.SetInstallUrl(installUrl);
        }

        /// <summary>
        /// Change the base URL used to make API calls.
        /// </summary>
        /// <param name="apiUrl">API base URL.</param>
        public static void SetApiUrl(string apiUrl)
        {
            DistributeInternal.SetApiUrl(apiUrl);
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
            DistributeInternal.NotifyUpdateAction((int)updateAction);
        }
    }
}
