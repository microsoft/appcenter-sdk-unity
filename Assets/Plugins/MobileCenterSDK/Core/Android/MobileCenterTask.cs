// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_ANDROID && !UNITY_EDITOR

using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask
    {
        protected MobileCenterTask()
        {
        }

        public MobileCenterTask(AndroidJavaObject javaFuture)
        {
            var consumer = new UnityMobileCenterConsumer<AndroidJavaObject>();
            consumer.CompletionCallback = t => 
            {
                CompletionAction();
            };
            javaFuture.Call("thenAccept", consumer);
        }
    }
}
#endif
