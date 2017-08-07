// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_EDITOR

namespace Microsoft.Azure.Mobile.Unity.Internal
{
#if UNITY_IOS
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = System.Object;
#endif

    class CustomPropertiesInternal
    {
        public static RawType Сreate()
        {
            return default(RawType);
        }

        public static void SetString(RawType properties, string key, string val)
        {
        }

        public static void SetNumber(RawType properties, string key, object val)
        {
        }

        public static void SetBool(RawType properties, string key, bool val)
        {
        }

        public static void SetDate(RawType properties, string key, DateTime val)
        {
        }

        public static void Clear(RawType properties, string key)
        {
        }
    }
}
#endif
