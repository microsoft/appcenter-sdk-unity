// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "NSStringHelper.h"
#import "WrapperException.h"
#import <Foundation/Foundation.h>

MSACWrapperExceptionModel* appcenter_unity_exception_create()
{
    return [[MSACWrapperExceptionModel alloc] init];
}

void appcenter_unity_exception_set_type(MSACWrapperExceptionModel* exception, char* type)
{
    [exception setType:[NSString stringWithUTF8String:type]];
}

void appcenter_unity_exception_set_message(MSACWrapperExceptionModel* exception, char* message)
{
    [exception setMessage:[NSString stringWithUTF8String:message]];
}

void appcenter_unity_exception_set_stacktrace(MSACWrapperExceptionModel* exception, char* stacktrace)
{
    [exception setStackTrace:appcenter_unity_cstr_to_ns_string(stacktrace)];
}

void appcenter_unity_exception_set_inner_exception(MSACWrapperExceptionModel* exception, MSACWrapperExceptionModel* innerException)
{
    NSArray* innerExceptions = @[innerException];
    [exception setInnerExceptions:innerExceptions];
}

void appcenter_unity_exception_set_wrapper_sdk_name(MSACWrapperExceptionModel* exception, char* sdkName)
{
    [exception setWrapperSdkName:[NSString stringWithUTF8String:sdkName]];
}

