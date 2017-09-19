// ----------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// ----------------------------------------------------------------

using Microsoft.Azure.Mobile.Unity.Push;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Microsoft.Azure.Mobile.Unity.Push
{

    [TestFixture]
    public class PushTests {

        [Test]
        [UnityPlatform(include = new[] {RuntimePlatform.WSAPlayerARM, RuntimePlatform.WSAPlayerX64, RuntimePlatform.WSAPlayerX86, RuntimePlatform.OSXPlayer, RuntimePlatform.Android})]
        public void PushEnabledTests() {
            Push.IsEnabledAsync().ContinueWith(task => {
                Assert.False(task.Result);
            });
        }
    }
}
