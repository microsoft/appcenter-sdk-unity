// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.
using Microsoft.AppCenter.Unity;
using Microsoft.AppCenter.Unity.Crashes.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Microsoft.AppCenter.Unity.Crashes
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Crashes
    {
        public static RawType GetNativeType()
        {
            return CrashesInternal.GetNativeType();
        }

        public static void OnHandleLog(string logString, string stackTrace, LogType type)
        {
            if (LogType.Assert == type || LogType.Exception == type || LogType.Error == type)
            {
                var exception = CreateWrapperException(logString, stackTrace);
                CrashesInternal.TrackException(exception.GetRawObject());
            }
        }

        public static void OnHandleUnresolvedException(object sender, UnhandledExceptionEventArgs args)
        {
            if (args == null || args.ExceptionObject == null)
            {
                return;
            }

            if (args.ExceptionObject.GetType() == typeof(Exception))
            {
                Exception e = (Exception)args.ExceptionObject;
                var exception = CreateWrapperException(e.Source, e.StackTrace);
                CrashesInternal.TrackException(exception.GetRawObject());
            }
        }

        public static AppCenterTask<bool> IsEnabledAsync()
        {
            return CrashesInternal.IsEnabledAsync();
        }

        public static AppCenterTask SetEnabledAsync(bool enabled)
        {
            return CrashesInternal.SetEnabledAsync(enabled);
        }

        public static void GenerateTestCrash()
        {
            CrashesInternal.GenerateTestCrash();
        }

        public static AppCenterTask<bool> HasCrashedInLastSession()
        {
            return CrashesInternal.HasCrashedInLastSession();
        }

        public static void DisableMachExceptionHandler()
        {
            CrashesInternal.DisableMachExceptionHandler();
        }

        private static WrapperException CreateWrapperException(string logString, string stackTrace)
        {
            var exception = new WrapperException();
            exception.SetWrapperSdkName(WrapperSdk.Name);

            string sanitizedLogString = logString.Replace("\n", " ");
            var logStringComponents = sanitizedLogString.Split(new [] { ':' }, 2);
            if(logStringComponents.Length > 1)
            {
                var type = logStringComponents[0].Trim();
                exception.SetType(type);
                var message = logStringComponents[1].Trim();
                exception.SetMessage(message);
            }

            string[] stacktraceLines = stackTrace.Split('\n');
            string stackTraceString = "";
            foreach (string line in stacktraceLines)
            {
                if (line.Length > 0)
                {
                    stackTraceString += "at " + line + "\n";
                }
            }
            exception.SetStacktrace(stackTraceString);

            return exception;
        }
    }
}