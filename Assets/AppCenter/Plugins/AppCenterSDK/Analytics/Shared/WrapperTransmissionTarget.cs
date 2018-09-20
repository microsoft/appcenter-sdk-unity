// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Analytics.Internal;
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics
{
#if UNITY_IOS || UNITY_EDITOR
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = System.Object;
#endif

    public class WrapperTransmissionTarget
    {
        private readonly RawType _rawObject;

        internal RawType GetRawObject()
        {
            return _rawObject;
        }

        public WrapperTransmissionTarget(RawType rawObject)
        {
            _rawObject = rawObject;
        }

        public void TrackEvent(string eventName)
        {
            WrapperTransmissionTargetInternal.TrackEvent(_rawObject, eventName);
        }

        public void TrackEventWithProperties(string eventName, IDictionary<string, string> properties)
        {
            WrapperTransmissionTargetInternal.TrackEventWithProperties(_rawObject, eventName, properties);
        }

        public AppCenterTask<bool> IsEnabledAsync()
        {
            return WrapperTransmissionTargetInternal.IsEnabledAsync(_rawObject);
        }

        public AppCenterTask SetEnabledAsync(bool enabled)
        {
            return WrapperTransmissionTargetInternal.SetEnabledAsync(_rawObject, enabled);
        }

        public WrapperTransmissionTarget GetTransmissionTarget()
        {
            return new WrapperTransmissionTarget(WrapperTransmissionTargetInternal.GetTransmissionTarget(_rawObject, AppCenterBehavior.SettingsInstance.ChildTransmissionTargetToken));
        }
    }
}