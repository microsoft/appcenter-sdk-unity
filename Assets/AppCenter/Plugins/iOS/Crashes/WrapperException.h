// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "MSACExceptionInternal.h"
#import <Foundation/Foundation.h>

// Don't need to return value because reference is kept by wrapper
extern "C" MSACExceptionInternal* appcenter_unity_exception_create();
extern "C" void appcenter_unity_exception_set_type(MSACExceptionInternal* exception, char* type);
extern "C" void appcenter_unity_exception_set_message(MSACExceptionInternal* exception, char* message);
extern "C" void appcenter_unity_exception_set_stacktrace(MSACExceptionInternal* exception, char* stacktrace);
extern "C" void appcenter_unity_exception_set_inner_exception(MSACExceptionInternal* exception, MSACExceptionInternal* innerException);
extern "C" void appcenter_unity_exception_set_wrapper_sdk_name(MSACExceptionInternal* exception, char* sdkName);
