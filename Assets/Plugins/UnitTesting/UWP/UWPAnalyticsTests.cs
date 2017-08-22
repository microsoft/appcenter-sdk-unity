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
    public class UWPAnalyticsTests {

        [Test]
        [UnityPlatform(include = new[] {RuntimePlatform.WSAPlayerARM, RuntimePlatform.WSAPlayerX64, RuntimePlatform.WSAPlayerX86})]
        public void UWPAnalyticsEnabledTests() {
            Analytics.IsEnabledAsync().ContinueWith(task => {
                Assert.False(task.Result);
            });
        }
    }
}
