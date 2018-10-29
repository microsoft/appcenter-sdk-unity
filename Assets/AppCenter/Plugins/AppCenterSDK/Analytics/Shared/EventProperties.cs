using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AppCenter.Unity.Analytics.Internal;

namespace Microsoft.AppCenter.Unity.Analytics
{
#if UNITY_IOS
    using RawType = System.IntPtr;
#elif UNITY_ANDROID
    using RawType = UnityEngine.AndroidJavaObject;
#else
    using RawType = Dictionary<string, string>;
#endif

    public class EventProperties
    {
        private readonly RawType _rawObject;

        internal RawType GetRawObject()
        {
            return _rawObject;
        }

        public EventProperties()
        {
            _rawObject = EventPropertiesInternal.Create();
        }

        public EventProperties Set(string key, string val)
        {
            EventPropertiesInternal.SetString(_rawObject, key, val);
            return this;
        }

        public EventProperties Set(string key, DateTime val)
        {
            EventPropertiesInternal.SetDate(_rawObject, key, val);
            return this;
        }
        
        public EventProperties Set(string key, long val)
        {
            EventPropertiesInternal.SetNumber(_rawObject, key, val);
            return this;
        }
        
        public EventProperties Set(string key, double val)
        {
            EventPropertiesInternal.SetNumber(_rawObject, key, val);
            return this;
        }
        
        public EventProperties Set(string key, bool val)
        {
            EventPropertiesInternal.SetBool(_rawObject, key, val);
            return this;
        }
    }
}
