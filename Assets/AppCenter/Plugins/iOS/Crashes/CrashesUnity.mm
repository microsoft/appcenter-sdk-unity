// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "CrashesUnity.h"
#import "CrashesDelegate.h"
#import <AppCenterCrashes/AppCenterCrashes.h>
#import <Foundation/Foundation.h>

void* app_center_unity_crashes_get_type()
{
  return (void *)CFBridgingRetain([MSCrashes class]);
}

void app_center_unity_crashes_set_enabled(bool isEnabled)
{
  [MSCrashes setEnabled:isEnabled];
}

bool app_center_unity_crashes_is_enabled()
{
  return [MSCrashes isEnabled];
}

void app_center_unity_crashes_generate_test_crash()
{
  [MSCrashes generateTestCrash];
}

bool app_center_unity_crashes_has_crashes_in_last_session()
{
  return [MSCrashes hasCrashedInLastSession];
}

MSErrorReport* app_center_unity_crashes_last_session_crash_report()
{
  return [MSCrashes lastSessionCrashReport];
}

//void app_center_unity_crashes_set_user_confirmation_handler(void* userConfirmationHandler);
//
//void app_center_unity_crashes_notify_with_user_confirmation(int userConfirmation);
//
