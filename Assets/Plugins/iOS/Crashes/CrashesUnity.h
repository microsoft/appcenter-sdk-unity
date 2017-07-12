//
//  unityMCtest.h
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

extern "C" void* mobile_center_unity_crashes_get_type();

extern "C" void mobile_center_unity_crashes_set_enabled(bool isEnabled);

extern "C" bool mobile_center_unity_crashes_is_enabled();

extern "C" void mobile_center_unity_crashes_generate_test_crash();

extern "C" bool mobile_center_unity_crashes_has_crashes_in_last_session();

extern "C" void* mobile_center_unity_crashes_last_session_crash_report();

extern "C" void mobile_center_unity_crashes_set_user_confirmation_handler(void* userConfirmationHandler);

extern "C" void mobile_center_unity_crashes_notify_with_user_confirmation(int userConfirmation);
