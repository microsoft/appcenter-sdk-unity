// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "NSStringHelper.h"
#import "WrapperException.h"
#import <Foundation/Foundation.h>

MSACException* appcenter_unity_exception_create()
{
    return [[MSACException alloc] init];
}

void appcenter_unity_exception_set_type(MSACException* exception, char* type)
{
    [exception setType:[NSString stringWithUTF8String:type]];
}

void appcenter_unity_exception_set_message(MSACException* exception, char* message)
{
    [exception setMessage:[NSString stringWithUTF8String:message]];
}

void appcenter_unity_exception_set_stacktrace(MSACException* exception, char* stacktrace)
{
    [exception setStackTrace:appcenter_unity_cstr_to_ns_string(stacktrace)];
}

void appcenter_unity_exception_set_inner_exception(MSACException* exception, MSACException* innerException)
{
    NSArray* innerExceptions = @[innerException];
    [exception setInnerExceptions:innerExceptions];
}

void appcenter_unity_exception_set_wrapper_sdk_name(MSACException* exception, char* sdkName)
{
    [exception setWrapperSdkName:[NSString stringWithUTF8String:sdkName]];
}

