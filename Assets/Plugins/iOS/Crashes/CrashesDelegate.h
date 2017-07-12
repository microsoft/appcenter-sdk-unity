//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import <MobileCenterCrashes/MobileCenterCrashes.h>
#import <Foundation/Foundation.h>

typedef bool (__cdecl *ShouldProcessErrorReportFunction)(MSErrorReport*);

extern "C" void mobile_center_unity_crashes_crashes_delegate_set_should_process_error_report_delegate(ShouldProcessErrorReportFunction* functionPtr);
extern "C" void mobile_center_unity_crashes_set_delegate();
@interface UnityCrashesDelegate : NSObject<MSCrashesDelegate>
-(BOOL)crashes:(MSCrashes *)crashes shouldProcessErrorReport:(MSErrorReport *)errorReport;
@end
