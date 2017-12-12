using System;

namespace Microsoft.AppCenter.Unity.Crashes.Models
{
    public class Exception
    {
        public Exception(string condition, string stackTrace)
        {
            var colonIdx = condition.IndexOf(":", StringComparison.Ordinal);
            Type = condition.Substring(0, colonIdx);
            Message = condition.Substring(colonIdx + 2, condition.Length - colonIdx - 2);
            StackTrace = stackTrace;
            WrapperSdkName = WrapperSdk.Name;
        }

        public string Type { get; private set; }
        public string Message { get; private set; }
        public string StackTrace { get; private set; }
        public string WrapperSdkName { get; private set; }
    }
}
