// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR

using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask
    {
        public MobileCenterTask(AndroidJavaObject javaFuture)
        {
            var consumer = new UnityMobileCenterConsumer<AndroidJavaObject>
            {
                CompletionCallback = (t => 
                {
                    InvokeContinuationAction();
                })
            };
            javaFuture.Call("thenAccept", consumer);
        }
    }
}
#endif
