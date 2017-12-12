// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "MSException.h"
#import "CrashesUnity.h"
#import <Foundation/Foundation.h>

void* appcenter_unity_crashes_get_type() {
  return (void *)CFBridgingRetain([MSCrashes class]);
}

void appcenter_unity_crashes_track_model_exception(MSException* exception) {
  [MSCrashes trackModelException: exception];
}

void appcenter_unity_crashes_set_enabled(bool isEnabled)
{
  [MSCrashes setEnabled:isEnabled];
}

bool appcenter_unity_crashes_is_enabled()
{
  return [MSCrashes isEnabled];
}

void appcenter_unity_crashes_generate_test_crash()
{
  [MSCrashes generateTestCrash];
}

bool appcenter_unity_crashes_has_crashed_in_last_session()
{
  return [MSCrashes hasCrashedInLastSession];
}

void appcenter_unity_crashes_disable_mach_exception_handler()
{
  [MSCrashes disableMachExceptionHandler];
}


