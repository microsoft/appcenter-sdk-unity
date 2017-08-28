// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using System.Reflection;

namespace Microsoft.Azure.Mobile.Unity.Utility
{
    public class MobileCenterServiceHelper
    {
        public static void InitializeServices(params Type[] services)
        {
            foreach (var serviceType in services)
            {
                var method = serviceType.GetMethod("Initialize");
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }

        public static void PostInitializeServices(params Type[] services)
        {
            foreach (var serviceType in services)
            {
                var method = serviceType.GetMethod("PostInitialize");
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }
    }
}