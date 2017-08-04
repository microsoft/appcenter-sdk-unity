// ----------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// ----------------------------------------------------------------

#if UNITY_WSA_10_0 && !UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity.Analytics.Tests
{
    using Microsoft.Azure.Mobile.Unity.Analytics;
    using System;
    using XUnit;

    public class UWPAnalyticsTests : IClassFixture<AnalyticsTestFixture> {
        AnalyticsTestFixture fixture;

        public UWPAnalyticsTests(AnalyticsTestFixture fixture) {
            this.fixture = fixture;
        }

        [Fact]
        public void TrackEventWithoutProperties()
    }
}

#endif