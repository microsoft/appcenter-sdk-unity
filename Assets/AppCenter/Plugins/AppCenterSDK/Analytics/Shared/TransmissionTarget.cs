// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Analytics.Internal;
using System;
using System.Collections.Generic;

namespace Microsoft.AppCenter.Unity.Analytics
{
#if UNITY_IOS
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = System.Object;
#endif

    public class TransmissionTarget
    {
        private readonly RawType _rawObject;

        internal RawType GetRawObject()
        {
            return _rawObject;
        }

        public TransmissionTarget(RawType rawObject)
        {
            _rawObject = rawObject;
        }

        public void TrackEvent(string eventName)
        {
            TransmissionTargetInternal.TrackEvent(_rawObject, eventName);
        }

        public void TrackEvent(string eventName, IDictionary<string, string> properties)
        {
            TransmissionTargetInternal.TrackEventWithProperties(_rawObject, eventName, properties);
        }

        public void TrackEvent(string eventName, EventProperties properties)
        {
            TransmissionTargetInternal.TrackEventWithProperties(_rawObject, eventName, properties);
        }

        public AppCenterTask<bool> IsEnabledAsync()
        {
            return TransmissionTargetInternal.IsEnabledAsync(_rawObject);
        }

        public AppCenterTask SetEnabledAsync(bool enabled)
        {
            return TransmissionTargetInternal.SetEnabledAsync(_rawObject, enabled);
        }

        public TransmissionTarget GetTransmissionTarget(string childTransmissionTargetToken)
        {
            if (string.IsNullOrEmpty(childTransmissionTargetToken))
            {
                return null;
            }
            var internalObject = TransmissionTargetInternal.GetTransmissionTarget(_rawObject, childTransmissionTargetToken);
            if (internalObject == null)
            {
                return null;
            }
            return new TransmissionTarget(internalObject);
        }

        public PropertyConfigurator GetPropertyConfigurator()
        {
            return new PropertyConfigurator(TransmissionTargetInternal.GetPropertyConfigurator(_rawObject));
        }

        public void Pause()
        {
            TransmissionTargetInternal.Pause(_rawObject);
        }

        public void Resume()
        {
            TransmissionTargetInternal.Resume(_rawObject);
        }
    }
}