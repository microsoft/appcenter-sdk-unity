//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "CrashesUnity.h"
#import "CrashesDelegate.h"
#import <MobileCenterCrashes/MobileCenterCrashes.h>
#import <Foundation/Foundation.h>

void* mobile_center_unity_crashes_get_type()
{
  Class* ptr = (Class*)malloc(sizeof(Class));
  *ptr = [MSCrashes class];
  return ptr;
}

void mobile_center_unity_crashes_set_enabled(bool isEnabled)
{
  [MSCrashes setEnabled:isEnabled];
}

bool mobile_center_unity_crashes_is_enabled()
{
  return [MSCrashes isEnabled];
}

void mobile_center_unity_crashes_generate_test_crash()
{
  [MSCrashes generateTestCrash];
}

bool mobile_center_unity_crashes_has_crashes_in_last_session()
{
  return [MSCrashes hasCrashedInLastSession];
}

extern "C" void* mobile_center_unity_crashes_last_session_crash_report()
{
  return (__bridge void*)[MSCrashes lastSessionCrashReport];
}

extern "C" void mobile_center_unity_crashes_set_user_confirmation_handler(void* userConfirmationHandler);

extern "C" void mobile_center_unity_crashes_notify_with_user_confirmation(int userConfirmation);

