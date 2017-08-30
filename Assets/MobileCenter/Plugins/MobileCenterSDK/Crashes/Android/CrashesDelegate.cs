// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity.Crashes.Internal
{
    public class CrashesDelegate : AndroidJavaProxy
    {
        private CrashesDelegate() : base("com.microsoft.azure.mobile.crashes.CrashesListener")
        {
        }

        public static void SetDelegate()
        {
            var crashes = new AndroidJavaClass("com.microsoft.azure.mobile.crashes.Crashes");
            crashes.CallStatic("setListener", new CrashesDelegate());
        }

        //TODO bind error report; implement these
        void onBeforeSending(AndroidJavaObject report)
        {
            
        }
 
        void onSendingFailed(AndroidJavaObject report, AndroidJavaObject exception)
        {
            
        }
    
        void onSendingSucceeded(AndroidJavaObject report)
        {
            
        }
    }
}
#endif
