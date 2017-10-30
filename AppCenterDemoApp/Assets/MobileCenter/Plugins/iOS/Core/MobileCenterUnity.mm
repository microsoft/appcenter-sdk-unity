// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "MobileCenterUnity.h"
#import "Utility/NSStringHelper.h"
#import <MobileCenter/MobileCenter.h>
#import <Foundation/Foundation.h>

void mobile_center_unity_set_log_level(int logLevel)
{
  [MSMobileCenter setLogLevel:(MSLogLevel)logLevel];
}

int mobile_center_unity_get_log_level()
{
  return (int)MSMobileCenter.logLevel;
}

bool mobile_center_unity_is_configured()
{
  return [MSMobileCenter isConfigured];
}

void mobile_center_unity_set_log_url(const char* logUrl)
{
  [MSMobileCenter setLogUrl:mobile_center_unity_cstr_to_ns_string(logUrl)];
}

void mobile_center_unity_set_enabled(bool isEnabled)
{
  [MSMobileCenter setEnabled:isEnabled];
}

bool mobile_center_unity_is_enabled()
{
  return [MSMobileCenter isEnabled];
}

const char* mobile_center_unity_get_install_id()
{
  NSString *uuidString =  [[MSMobileCenter installId] UUIDString];
  return mobile_center_unity_ns_string_to_cstr(uuidString);
}

void mobile_center_unity_set_custom_properties(MSCustomProperties* properties)
{
  [MSMobileCenter setCustomProperties:properties];
}

void mobile_center_unity_set_wrapper_sdk(const char* wrapperSdkVersion,
                                         const char* wrapperSdkName,
                                         const char* wrapperRuntimeVersion,
                                         const char* liveUpdateReleaseLabel,
                                         const char* liveUpdateDeploymentKey,
                                         const char* liveUpdatePackageHash)
{
  MSWrapperSdk *wrapperSdk = [[MSWrapperSdk alloc]
                              initWithWrapperSdkVersion:mobile_center_unity_cstr_to_ns_string(wrapperSdkVersion)
                                         wrapperSdkName:mobile_center_unity_cstr_to_ns_string(wrapperSdkName)
                                  wrapperRuntimeVersion:mobile_center_unity_cstr_to_ns_string(wrapperRuntimeVersion)
                                 liveUpdateReleaseLabel:mobile_center_unity_cstr_to_ns_string(liveUpdateReleaseLabel)
                                liveUpdateDeploymentKey:mobile_center_unity_cstr_to_ns_string(liveUpdateDeploymentKey)
                                  liveUpdatePackageHash:mobile_center_unity_cstr_to_ns_string(liveUpdatePackageHash)];
  [MSMobileCenter setWrapperSdk:wrapperSdk];
}


