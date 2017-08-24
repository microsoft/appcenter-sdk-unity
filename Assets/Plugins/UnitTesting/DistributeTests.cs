// ----------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// ----------------------------------------------------------------

using Microsoft.Azure.Mobile.Unity.Distribute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Microsoft.Azure.Mobile.Unity.Distribute
{

    [TestFixture]
    public class DistributeTests {

        [Test]
        [UnityPlatform(include = new[] {RuntimePlatform.WSAPlayerARM, RuntimePlatform.WSAPlayerX64, RuntimePlatform.WSAPlayerX86, RuntimePlatform.OSXPlayer, RuntimePlatform.Android})]
        public void DistributeEnabledTests() {
            Distribute.IsEnabledAsync().ContinueWith(task => {
                Assert.False(task.Result);
            });
        }
    }
}
