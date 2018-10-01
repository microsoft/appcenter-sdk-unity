// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import <AppCenterCrashes/AppCenterCrashes.h>
#import <Foundation/Foundation.h>

extern "C" void app_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(bool(*handler)(MSErrorReport *));
extern "C" void app_center_unity_crashes_set_delegate();
@interface UnityCrashesDelegate : NSObject<MSCrashesDelegate>
-(BOOL)crashes:(MSCrashes *)crashes shouldProcessErrorReport:(MSErrorReport *)errorReport;
@end
