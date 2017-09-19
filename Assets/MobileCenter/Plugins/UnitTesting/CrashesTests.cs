// // ----------------------------------------------------------------
// //  Copyright (c) Microsoft Corporation.  All rights reserved.
// // ----------------------------------------------------------------

// using Microsoft.Azure.Mobile.Unity.Crashes;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;

// namespace Microsoft.Azure.Mobile.Unity.Crashes
// {

//     [TestFixture]
//     public class CrashesTests {

//         [Test]
//         [UnityPlatform(include = new[] {RuntimePlatform.WSAPlayerARM, RuntimePlatform.WSAPlayerX64, RuntimePlatform.WSAPlayerX86, RuntimePlatform.OSXPlayer, RuntimePlatform.Android})]
//         public void CrashesEnabledTests() {
//             Crashes.IsEnabledAsync().ContinueWith(task => {
//                 Assert.False(task.Result);
//             });
//         }
//     }
// }
