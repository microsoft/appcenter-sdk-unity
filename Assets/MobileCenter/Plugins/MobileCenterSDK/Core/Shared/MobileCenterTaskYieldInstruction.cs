// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using UnityEngine;

namespace Microsoft.Azure.Mobile.Unity
{
    public partial class MobileCenterTask : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get { return !IsComplete; }
        }
    }
}
