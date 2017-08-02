// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "CrashesDelegate.h"
#import <MobileCenterCrashes/MobileCenterCrashes.h>
#import <Foundation/Foundation.h>

static ShouldProcessErrorReportFunction *shouldProcessErrorReport;
static UnityCrashesDelegate *del;

void mobile_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(ShouldProcessErrorReportFunction* functionPtr)
{
  shouldProcessErrorReport = functionPtr;
}

void mobile_center_unity_crashes_set_delegate()
{
  del = [[UnityCrashesDelegate alloc] init];
  [MSCrashes setDelegate:del];
}

@implementation UnityCrashesDelegate

-(BOOL)crashes:(MSCrashes *)crashes shouldProcessErrorReport:(MSErrorReport *)errorReport
{
  return (*shouldProcessErrorReport)(errorReport);
}

@end
