#if UNITY_ANDROID && !UNITY_EDITOR

using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes.Models
{
    public class ExceptionHelper
    {
        public static AndroidJavaObject ExceptionConvert(Exception exception)
        {
            var androidException = new AndroidJavaObject("com.microsoft.appcenter.crashes.ingestion.models.Exception");
            androidException.Call("setType", exception.Type);
            androidException.Call("setMessage", exception.Message);
            androidException.Call("setStackTrace", exception.StackTrace);
            androidException.Call("setWrapperSdkName", exception.WrapperSdkName);

            // Set an empty list for the frames
            var list = new AndroidJavaObject("java.util.ArrayList");
            androidException.Call("setFrames", list);
            return androidException;
        }
    }
}
#endif