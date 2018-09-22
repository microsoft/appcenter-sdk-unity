// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "CrashesDelegate.h"
#import <AppCenterCrashes/AppCenterCrashes.h>
#import <Foundation/Foundation.h>

static bool (*shouldProcessErrorReport)(MSErrorReport *);

static UnityCrashesDelegate *unityCrashesDelegate = NULL;

void app_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(bool(*handler)(MSErrorReport *))
{
    shouldProcessErrorReport = handler;
}

void app_center_unity_crashes_set_delegate()
{
  unityCrashesDelegate = [[UnityCrashesDelegate alloc] init];
  [MSCrashes setDelegate:unityCrashesDelegate];
}

@implementation UnityCrashesDelegate

-(BOOL)crashes:(MSCrashes *)crashes shouldProcessErrorReport:(MSErrorReport *)errorReport
{
    if (shouldProcessErrorReport)
        return (*shouldProcessErrorReport)(errorReport);
    else
        return true;
}

@end
