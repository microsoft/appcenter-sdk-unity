// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;
using Microsoft.Azure.Mobile.Unity.Internal;

namespace Microsoft.Azure.Mobile.Unity
{
#if UNITY_IOS
    using RawType = System.IntPtr;
    using RawDateHelper = Microsoft.Azure.Mobile.Internal.Utility.NSDateHelper;
    using RawNumberHelper = Microsoft.Azure.Mobile.Internal.Utility.NSNumberHelper;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
    using RawDateHelper = Microsoft.Azure.Mobile.Internal.Utility.JavaDateHelper;
    using RawNumberHelper = Microsoft.Azure.Mobile.Internal.Utility.JavaNumberHelper;
#else
    using RawType = System.Object;
#endif

    public class CustomProperties
    {
        private readonly RawType _rawObject;

        internal RawType GetRawObject()
        {
            return _rawObject;
        }

        public CustomProperties()
        {
            _rawObject = CustomPropertiesInternal.mobile_center_unity_custom_properties_create();
        }

        /// <summary>
        /// Set the specified property value with the specified key.
        /// If the properties previously contained a property for the key, the old value is replaced.
        /// </summary>
        /// <param name="key">Key with which the specified value is to be set.</param>
        /// <param name="val">Value to be set with the specified key.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Set(string key, string val)
        {
            CustomPropertiesInternal.mobile_center_unity_custom_properties_set_string(_rawObject, key, val);
            return this;
        }

        /// <summary>
        /// Set the specified property value with the specified key.
        /// If the properties previously contained a property for the key, the old value is replaced.
        /// </summary>
        /// <param name="key">Key with which the specified value is to be set.</param>
        /// <param name="val">Value to be set with the specified key.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Set(string key, DateTime val)
        {
#if UNITY_IOS || UNITY_ANDROID
            var rawDate = RawDateHelper.DateTimeConvert(val);
#else
            var rawDate = val;
#endif
            CustomPropertiesInternal.mobile_center_unity_custom_properties_set_date(_rawObject, key, rawDate);
            return this;
        }

        /// <summary>
        /// Set the specified property value with the specified key.
        /// If the properties previously contained a property for the key, the old value is replaced.
        /// </summary>
        /// <param name="key">Key with which the specified value is to be set.</param>
        /// <param name="val">Value to be set with the specified key.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Set(string key, int val)
        {
#if UNITY_IOS || UNITY_ANDROID
            var rawNumber = RawNumberHelper.Convert(val);
#else
            var rawNumber = val;
#endif
            CustomPropertiesInternal.mobile_center_unity_custom_properties_set_number(_rawObject, key, rawNumber);
            return this;
        }

        /// <summary>
        /// Set the specified property value with the specified key.
        /// If the properties previously contained a property for the key, the old value is replaced.
        /// </summary>
        /// <param name="key">Key with which the specified value is to be set.</param>
        /// <param name="val">Value to be set with the specified key.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Set(string key, long val)
        {
#if UNITY_IOS || UNITY_ANDROID
            var rawNumber = RawNumberHelper.Convert(val);
#else
            var rawNumber = val;
#endif
            CustomPropertiesInternal.mobile_center_unity_custom_properties_set_number(_rawObject, key, rawNumber);
            return this;
        }

        /// <summary>
        /// Set the specified property value with the specified key.
        /// If the properties previously contained a property for the key, the old value is replaced.
        /// </summary>
        /// <param name="key">Key with which the specified value is to be set.</param>
        /// <param name="val">Value to be set with the specified key.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Set(string key, float val)
        {
#if UNITY_IOS || UNITY_ANDROID
            var rawNumber = RawNumberHelper.Convert(val);
#else
            var rawNumber = val;
#endif
            CustomPropertiesInternal.mobile_center_unity_custom_properties_set_number(_rawObject, key, rawNumber);
            return this;
        }

        /// <summary>
        /// Set the specified property value with the specified key.
        /// If the properties previously contained a property for the key, the old value is replaced.
        /// </summary>
        /// <param name="key">Key with which the specified value is to be set.</param>
        /// <param name="val">Value to be set with the specified key.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Set(string key, double val)
        {
#if UNITY_IOS || UNITY_ANDROID
            var rawNumber = RawNumberHelper.Convert(val);
#else
            var rawNumber = val;
#endif
            CustomPropertiesInternal.mobile_center_unity_custom_properties_set_number(_rawObject, key, rawNumber);
            return this;
        }

        /// <summary>
        /// Set the specified property value with the specified key.
        /// If the properties previously contained a property for the key, the old value is replaced.
        /// </summary>
        /// <param name="key">Key with which the specified value is to be set.</param>
        /// <param name="val">Value to be set with the specified key.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Set(string key, bool val)
        {
            CustomPropertiesInternal.mobile_center_unity_custom_properties_set_bool(_rawObject, key, val);
            return this;
        }

        /// <summary>
        /// Clear the property for the specified key.
        /// </summary>
        /// <param name="key">Key whose mapping is to be cleared.</param>
        /// <returns>This instance.</returns>
        public CustomProperties Clear(string key)
        {
            CustomPropertiesInternal.mobile_center_unity_custom_properties_clear(_rawObject, key);
            return this;
        }
    }
}
