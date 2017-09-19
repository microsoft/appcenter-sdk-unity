// ----------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// ----------------------------------------------------------------

using Microsoft.Azure.Mobile.Unity;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

namespace Microsoft.Azure.Mobile.Unity
{

    [TestFixture]
    public class CoreTests {

        [Test]
        [UnityPlatform(include = new[] {RuntimePlatform.WSAPlayerARM, RuntimePlatform.WSAPlayerX64, RuntimePlatform.WSAPlayerX86, RuntimePlatform.OSXPlayer, RuntimePlatform.Android})]
        public void SetCustomPropertyTest() {
            MobileCenter.IsEnabledAsync().ContinueWith(task => {
                Assert.False(task.Result);
            });
        }
    }
}
