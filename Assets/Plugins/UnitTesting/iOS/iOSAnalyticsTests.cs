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
    public class iOSAnalyticsTests {

        [Test]
        [UnityPlatform(RuntimePlatform.OSXPlayer)]
        public void iOSAnalyticsEnabledTests() {
            Analytics.IsEnabledAsync().ContinueWith(task => {
                Assert.False(task.Result);
            });
        }
    }
}
