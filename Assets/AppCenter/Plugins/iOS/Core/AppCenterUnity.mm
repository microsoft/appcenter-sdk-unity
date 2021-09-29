// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "AppCenterUnity.h"
#import "Utility/NSStringHelper.h"
#import <AppCenter/AppCenter.h>
#import <Foundation/Foundation.h>

NSMutableArray<Class>* get_services_array(void** services, int count) {
  NSMutableArray<Class>* servicesArray = [NSMutableArray new];
  for (int i = 0; i < count; i++) {
    [servicesArray addObject:(Class)CFBridgingRelease(services[i])];
  }
  return servicesArray;
}

void appcenter_unity_set_log_level(int logLevel)
{
  [MSACAppCenter setLogLevel:(MSACLogLevel)logLevel];
}

int appcenter_unity_get_log_level()
{
  return (int)MSACAppCenter.logLevel;
}

bool appcenter_unity_is_configured()
{
  return [MSACAppCenter isConfigured];
}

void appcenter_unity_set_log_url(const char* logUrl)
{
  [MSACAppCenter setLogUrl:appcenter_unity_cstr_to_ns_string(logUrl)];
}

void appcenter_unity_set_user_id(char* userId)
{
  [MSACAppCenter setUserId: appcenter_unity_cstr_to_ns_string(userId)];
}

void appcenter_unity_set_enabled(bool isEnabled)
{
  [MSACAppCenter setEnabled:isEnabled];
}

void appcenter_unity_set_network_requests_allowed(bool isAllowed)
{
  [MSACAppCenter setNetworkRequestsAllowed:isAllowed];
}

bool appcenter_unity_is_network_requests_allowed()
{
  return [MSACAppCenter isNetworkRequestsAllowed];
}

void appcenter_unity_start(const char* appSecret, void** services, int count) {
  NSMutableArray<Class>* servicesArray = get_services_array(services, count);
  [MSACAppCenter start:appcenter_unity_cstr_to_ns_string(appSecret) withServices:servicesArray];
}

void appcenter_unity_start_no_secret(void** services, int count) {
  NSMutableArray<Class>* servicesArray = get_services_array(services, count);
  [MSACAppCenter startWithServices:servicesArray];
}

void appcenter_unity_start_from_library(void** services, int count) {
  NSMutableArray<Class>* servicesArray = get_services_array(services, count);
  [MSACAppCenter startFromLibraryWithServices:servicesArray];
}

bool appcenter_unity_is_enabled()
{
  return [MSACAppCenter isEnabled];
}

const char* appcenter_unity_get_install_id()
{
  NSString *uuidString = [[MSACAppCenter installId] UUIDString];
  return appcenter_unity_ns_string_to_cstr(uuidString);
}

const char* appcenter_unity_get_sdk_version()
{
  return appcenter_unity_ns_string_to_cstr([MSACAppCenter sdkVersion]);
}

void appcenter_unity_set_wrapper_sdk(const char* wrapperSdkVersion,
                                     const char* wrapperSdkName,
                                     const char* wrapperRuntimeVersion,
                                     const char* liveUpdateReleaseLabel,
                                     const char* liveUpdateDeploymentKey,
                                     const char* liveUpdatePackageHash)
{
  MSACWrapperSdk *wrapperSdk = [[MSACWrapperSdk alloc]
                              initWithWrapperSdkVersion:appcenter_unity_cstr_to_ns_string(wrapperSdkVersion)
                                         wrapperSdkName:appcenter_unity_cstr_to_ns_string(wrapperSdkName)
                                  wrapperRuntimeVersion:appcenter_unity_cstr_to_ns_string(wrapperRuntimeVersion)
                                 liveUpdateReleaseLabel:appcenter_unity_cstr_to_ns_string(liveUpdateReleaseLabel)
                                liveUpdateDeploymentKey:appcenter_unity_cstr_to_ns_string(liveUpdateDeploymentKey)
                                  liveUpdatePackageHash:appcenter_unity_cstr_to_ns_string(liveUpdatePackageHash)];
  [MSACAppCenter setWrapperSdk:wrapperSdk];
}

void appcenter_unity_set_storage_size(long size, void(* completionHandler)(bool))
{
    [MSACAppCenter setMaxStorageSize:size completionHandler:^void(BOOL result){
        completionHandler(result);
    }];
}
