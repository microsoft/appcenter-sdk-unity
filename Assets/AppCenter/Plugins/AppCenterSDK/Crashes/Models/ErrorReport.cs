// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

using System;

namespace Microsoft.AppCenter.Unity.Crashes.Models
{
    public class ErrorReport
    {

        public ErrorReport(string id, DateTimeOffset appStartTime, DateTimeOffset appErrorTime, Models.Exception exception)
        {
            Id = id;
            AppStartTime = appStartTime;
            AppErrorTime = appErrorTime;
            Exception = exception;
        }

        /// <summary>
        /// Gets the report identifier.
        /// </summary>
        /// <value>UUID for the report.</value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the app start time.
        /// </summary>
        /// <value>Date and time the app started</value>
        public DateTimeOffset AppStartTime { get; private set; }

        /// <summary>
        /// Gets the app error time.
        /// </summary>
        /// <value>Date and time the error occured</value>
        public DateTimeOffset AppErrorTime { get; private set; }

        /// <summary>
        /// Gets the device that the crashed app was being run on.
        /// </summary>
        /// <value>Device information at the crash time.</value>
        //public Device Device { get; private set; }

        /// <summary>
        /// Gets the model exception associated with the error.
        /// </summary>
        /// <value>The exception.</value>
        public Models.Exception Exception { get; private set; }

        //TODO don't have android or ios details
        //TODO bind device
    }
}
