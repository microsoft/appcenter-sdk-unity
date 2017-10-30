// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR

using UnityEngine;
using System.Threading;
using System;

namespace Microsoft.AppCenter.Unity
{
    public class UnityMobileCenterConsumer<T> : AndroidJavaProxy
    {
        internal Action<T> CompletionCallback { get; set; }

        internal UnityMobileCenterConsumer() : base("com.microsoft.azure.mobile.utils.async.MobileCenterConsumer")
        {
        }

        void accept(T t)
        {
            if (CompletionCallback != null)
            {
                CompletionCallback(t);
            }
        }
    }
}
#endif
