// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "MSACWrapperExceptionModel.h"
#import <Foundation/Foundation.h>

// Don't need to return value because reference is kept by wrapper
extern "C" MSACWrapperExceptionModel* appcenter_unity_exception_create();
extern "C" void appcenter_unity_exception_set_type(MSACWrapperExceptionModel* exception, char* type);
extern "C" void appcenter_unity_exception_set_message(MSACWrapperExceptionModel* exception, char* message);
extern "C" void appcenter_unity_exception_set_stacktrace(MSACWrapperExceptionModel* exception, char* stacktrace);
extern "C" void appcenter_unity_exception_set_inner_exception(MSACWrapperExceptionModel* exception, MSACWrapperExceptionModel* innerException);
extern "C" void appcenter_unity_exception_set_wrapper_sdk_name(MSACWrapperExceptionModel* exception, char* sdkName);
