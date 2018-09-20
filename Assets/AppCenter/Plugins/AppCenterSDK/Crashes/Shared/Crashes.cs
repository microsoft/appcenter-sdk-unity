// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using Microsoft.AppCenter.Unity.Crashes.Internal;
using Microsoft.AppCenter.Unity.Internal.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private static bool _reportUnhandledExceptions = false;

#if UNITY_ANDROID
        private static Queue<Exception> _unhandledExceptions = new Queue<Exception>();
        private static bool _unhandledExceptionsExists = false;
#endif

        public static void PrepareEventHandlers()
        {
            AppCenterBehavior.InitializedAppCenterAndServices += HandleAppCenterInitialized;
        }

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

            var exception = args.ExceptionObject as Exception;
            if (exception != null)
            {
                Debug.Log("Unhandled exception: " + exception.ToString());

#if UNITY_ANDROID
                lock (_unhandledExceptions)
                {
                    _unhandledExceptions.Enqueue(exception);
                    _unhandledExceptionsExists = true;
                }
#else
                var exceptionWrapper = CreateWrapperException(exception);
                CrashesInternal.TrackException(exceptionWrapper.GetRawObject());
#endif
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

        public static Models.ErrorReport LastSessionCrashReport()
        {
            return CrashesInternal.LastSessionCrashReport();
        }

        /// <summary>
        /// Report unhandled exceptions, automatically captured by Unity, as handled errors
        /// </summary>
        /// <param name="enabled">Specify true to enable reporting of unhandled exceptions, automatically captured by Unity, as handled errors; otherwise, false.</param>
        public static void ReportUnhandledExceptions(bool enabled)
        {
            if (_reportUnhandledExceptions == enabled)
            {
                return;
            }

            _reportUnhandledExceptions = enabled;

            if (enabled)
            {
                SubscribeToUnhandledExceptions();
            }
            else
            {
                UnsubscribeFromUnhandledExceptions();
            }
        }

        public static bool IsReportingUnhandledExceptions()
        {
            return _reportUnhandledExceptions;
        }

        private static void SubscribeToUnhandledExceptions()
        {
#if !UNITY_EDITOR
            Application.logMessageReceived += OnHandleLog;
            System.AppDomain.CurrentDomain.UnhandledException += OnHandleUnresolvedException;
#endif

#if !UNITY_EDITOR && UNITY_ANDROID
            UnityCoroutineHelper.StartCoroutine(SendUnhandledExceptionReports);
#endif
        }

        private static void UnsubscribeFromUnhandledExceptions()
        {
#if !UNITY_EDITOR
            Application.logMessageReceived -= OnHandleLog;
            System.AppDomain.CurrentDomain.UnhandledException -= OnHandleUnresolvedException;
#endif
        }

        private static void HandleAppCenterInitialized()
        {
            if (_reportUnhandledExceptions)
            {
                SubscribeToUnhandledExceptions();
            }
        }

#if UNITY_ANDROID
        private static IEnumerator SendUnhandledExceptionReports()
        {
            while (true)
            {
                if (!_reportUnhandledExceptions)
                {
                    yield break;
                }

                if (_unhandledExceptionsExists)
                {
                    Exception exception = null;
                    lock (_unhandledExceptions)
                    {
                        if (_unhandledExceptions.Count > 0)
                        {
                            exception = _unhandledExceptions.Dequeue();
                        }

                        if (_unhandledExceptions.Count == 0)
                        {
                            _unhandledExceptionsExists = false;
                        }
                    }

                    if (exception != null)
                    {
                        var exceptionWrapper = CreateWrapperException(exception);
                        CrashesInternal.TrackException(exceptionWrapper.GetRawObject());
                    }
                }

                yield return null;
            }
        }
#endif

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