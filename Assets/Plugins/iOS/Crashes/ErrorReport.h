// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

extern "C" char* mobile_center_unity_crashes_error_report_incident_identifier(void* errorReport);

extern "C" char* mobile_center_unity_crashes_error_report_reporter_key(void* errorReport);

extern "C" char* mobile_center_unity_crashes_error_report_signal(void* errorReport);

extern "C" char* mobile_center_unity_crashes_error_report_exception_name(void* errorReport);

extern "C" char* mobile_center_unity_crashes_error_report_exception_reason(void* errorReport);

extern "C" char* mobile_center_unity_crashes_error_report_app_start_time(void* errorReport);

extern "C" char* mobile_center_unity_crashes_error_report_app_error_time(void* errorReport);

extern "C" void* mobile_center_unity_crashes_error_report_device(void* errorReport);

extern "C" unsigned int mobile_center_unity_crashes_error_report_app_process_identifier(void* errorReport);

extern "C" bool mobile_center_unity_crashes_error_report_is_app_kill(void* errorReport);
