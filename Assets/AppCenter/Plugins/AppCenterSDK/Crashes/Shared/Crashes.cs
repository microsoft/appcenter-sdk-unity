// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Crashes.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Microsoft.AppCenter.Unity.Crashes
{
#if UNITY_IOS || UNITY_ANDROID
    using RawType = System.IntPtr;
#else
    using RawType = System.Type;
#endif

    public class Crashes
    {
        private static bool _reportUnhandledExceptions = false;

        public static void Initialize()
        {
            CrashesDelegate.SetDelegate();
        }

        public static RawType GetNativeType()
        {
            return CrashesInternal.GetNativeType();
        }

        public static void TrackError(Exception exception, IDictionary<string, string> properties = null)
        {
            if (exception != null)
            {
                var exceptionWrapper = CreateWrapperException(exception);
                if (properties == null || properties.Count == 0)
                {
                    CrashesInternal.TrackException(exceptionWrapper.GetRawObject());
                }
                else
                {
                    CrashesInternal.TrackException(exceptionWrapper.GetRawObject(), properties);
                }
            }
        }

        public static void OnHandleLog(string logString, string stackTrace, LogType type)
        {
            if (_reportUnhandledExceptions && (LogType.Assert == type || LogType.Exception == type || LogType.Error == type))
            {
                var exception = CreateWrapperException(logString, stackTrace);
                CrashesInternal.TrackException(exception.GetRawObject());
            }
        }

        public static void OnHandleUnresolvedException(object sender, UnhandledExceptionEventArgs args)
        {
            if (!_reportUnhandledExceptions || args == null || args.ExceptionObject == null)
            {
                return;
            }

            var exception = args.ExceptionObject as Exception;
            if (exception != null)
            {
                var exceptionWrapper = CreateWrapperException(exception);
                CrashesInternal.TrackException(exceptionWrapper.GetRawObject());
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

        public static ErrorReport LastSessionCrashReport()
        {
            return CrashesInternal.LastSessionCrashReport();
        }

        /// <summary>
        /// Report unhandled exceptions, automatically captured by Unity, as handled errors
        /// </summary>
        /// <param name="enabled">Specify true to enable reporting of unhandled exceptions, automatically captured by Unity, as handled errors; otherwise, false.</param>
        public static void ReportUnhandledExceptions(bool enabled)
        {
            _reportUnhandledExceptions = enabled;
        }

        public static bool IsReportingUnhandledExceptions()
        {
            return _reportUnhandledExceptions;
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate bool UserConfirmationHandler();

        public static UserConfirmationHandler ShouldAwaitUserConfirmation
        {
            set
            {
                CrashesInternal.SetUserConfirmationHandler(value);
            }
        }

        public enum ConfirmationResult { DontSend, Send, AlwaysSend };
        
        public static void NotifyWithUserConfirmation(ConfirmationResult answer)
        {
            CrashesInternal.NotifyWithUserConfirmation(answer);
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate bool ShouldProcessErrorReportHandler(ErrorReport errorReport);

        public static ShouldProcessErrorReportHandler ShouldProcessErrorReport 
        {
            set
            {
                CrashesDelegate.SetShouldProcessErrorReportHandler(value);
            }
        }

        public static void StartCrashes()
        {
            CrashesInternal.StartCrashes();
        }

        private static WrapperException CreateWrapperException(Exception exception)
        {
            var exceptionWrapper = new WrapperException();
            exceptionWrapper.SetWrapperSdkName(WrapperSdk.Name);
            exceptionWrapper.SetStacktrace(exception.StackTrace);
            exceptionWrapper.SetMessage(exception.Message);
            exceptionWrapper.SetType(exception.GetType().ToString());

            if (exception.InnerException != null)
            {
                var innerExceptionWrapper = CreateWrapperException(exception.InnerException).GetRawObject();
                exceptionWrapper.SetInnerException(innerExceptionWrapper);
            }

            return exceptionWrapper;
        }

        private static WrapperException CreateWrapperException(string logString, string stackTrace)
        {
            var exception = new WrapperException();
            exception.SetWrapperSdkName(WrapperSdk.Name);

            string sanitizedLogString = logString.Replace("\n", " ");
            var logStringComponents = sanitizedLogString.Split(new[] { ':' }, 2);
            if (logStringComponents.Length > 1)
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