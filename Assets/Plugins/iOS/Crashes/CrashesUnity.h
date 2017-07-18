// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

@class MSErrorReport;

extern "C" void* mobile_center_unity_crashes_get_type();

extern "C" void mobile_center_unity_crashes_set_enabled(bool isEnabled);

extern "C" bool mobile_center_unity_crashes_is_enabled();

extern "C" void mobile_center_unity_crashes_generate_test_crash();

extern "C" bool mobile_center_unity_crashes_has_crashes_in_last_session();

extern "C" MSErrorReport* mobile_center_unity_crashes_last_session_crash_report();

extern "C" void mobile_center_unity_crashes_set_user_confirmation_handler(void* userConfirmationHandler);

extern "C" void mobile_center_unity_crashes_notify_with_user_confirmation(int userConfirmation);
