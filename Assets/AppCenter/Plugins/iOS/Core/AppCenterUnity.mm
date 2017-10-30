// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "AppCenterUnity.h"
#import "Utility/NSStringHelper.h"
#import <MobileCenter/MobileCenter.h>
#import <Foundation/Foundation.h>

void appcenter_unity_set_log_level(int logLevel)
{
  [MSMobileCenter setLogLevel:(MSLogLevel)logLevel];
}

int appcenter_unity_get_log_level()
{
  return (int)MSMobileCenter.logLevel;
}

bool appcenter_unity_is_configured()
{
  return [MSMobileCenter isConfigured];
}

void appcenter_unity_set_log_url(const char* logUrl)
{
  [MSMobileCenter setLogUrl:appcenter_unity_cstr_to_ns_string(logUrl)];
}

void appcenter_unity_set_enabled(bool isEnabled)
{
  [MSMobileCenter setEnabled:isEnabled];
}

bool appcenter_unity_is_enabled()
{
  return [MSMobileCenter isEnabled];
}

const char* appcenter_unity_get_install_id()
{
  NSString *uuidString =  [[MSMobileCenter installId] UUIDString];
  return appcenter_unity_ns_string_to_cstr(uuidString);
}

void appcenter_unity_set_custom_properties(MSCustomProperties* properties)
{
  [MSMobileCenter setCustomProperties:properties];
}

void appcenter_unity_set_wrapper_sdk(const char* wrapperSdkVersion,
                                         const char* wrapperSdkName,
                                         const char* wrapperRuntimeVersion,
                                         const char* liveUpdateReleaseLabel,
                                         const char* liveUpdateDeploymentKey,
                                         const char* liveUpdatePackageHash)
{
  MSWrapperSdk *wrapperSdk = [[MSWrapperSdk alloc]
                              initWithWrapperSdkVersion:appcenter_unity_cstr_to_ns_string(wrapperSdkVersion)
                                         wrapperSdkName:appcenter_unity_cstr_to_ns_string(wrapperSdkName)
                                  wrapperRuntimeVersion:appcenter_unity_cstr_to_ns_string(wrapperRuntimeVersion)
                                 liveUpdateReleaseLabel:appcenter_unity_cstr_to_ns_string(liveUpdateReleaseLabel)
                                liveUpdateDeploymentKey:appcenter_unity_cstr_to_ns_string(liveUpdateDeploymentKey)
                                  liveUpdatePackageHash:appcenter_unity_cstr_to_ns_string(liveUpdatePackageHash)];
  [MSMobileCenter setWrapperSdk:wrapperSdk];
}


