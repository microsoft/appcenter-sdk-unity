// ----------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// ----------------------------------------------------------------

using Microsoft.Azure.Mobile.Unity.Analytics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Microsoft.Azure.Mobile.Unity.Analytics
{

    [TestFixture]
    public class AnalyticsTests {

        [Test]
        [UnityPlatform(include = new[] {RuntimePlatform.WSAPlayerARM, RuntimePlatform.WSAPlayerX64, RuntimePlatform.WSAPlayerX86, RuntimePlatform.OSXPlayer, RuntimePlatform.Android})]
        public void AnalyticsEnabledTests() {
            Analytics.IsEnabledAsync().ContinueWith(task => {
                Assert.False(task.Result);
            });
        }
    }
}
