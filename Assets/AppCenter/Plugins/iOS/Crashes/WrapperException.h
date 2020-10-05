// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "MSACException.h"
#import <Foundation/Foundation.h>

// Don't need to return value because reference is kept by wrapper
extern "C" MSACException* appcenter_unity_exception_create();
extern "C" void appcenter_unity_exception_set_type(MSACException* exception, char* type);
extern "C" void appcenter_unity_exception_set_message(MSACException* exception, char* message);
extern "C" void appcenter_unity_exception_set_stacktrace(MSACException* exception, char* stacktrace);
extern "C" void appcenter_unity_exception_set_inner_exception(MSACException* exception, MSACException* innerException);
extern "C" void appcenter_unity_exception_set_wrapper_sdk_name(MSACException* exception, char* sdkName);
