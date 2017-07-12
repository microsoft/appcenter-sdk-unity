// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import <MobileCenter/MobileCenter.h>

extern "C" void mobile_center_unity_configure(char* appSecret);
extern "C" void mobile_center_unity_start(char* appSecret, void** services, int numServices);
extern "C" void mobile_center_unity_start_services(void** services, int numServices);
extern "C" void mobile_center_unity_set_log_level(int logLevel);
extern "C" int mobile_center_unity_get_log_level();
extern "C" bool mobile_center_unity_is_configured();
extern "C" void mobile_center_unity_set_log_url(char* logUrl);
extern "C" void mobile_center_unity_set_enabled(bool isEnabled);
extern "C" bool mobile_center_unity_is_enabled();
extern "C" char* mobile_center_unity_get_install_id();
extern "C" void mobile_center_unity_set_custom_properties(MSCustomProperties* properties);
extern "C" void mobile_center_unity_set_wrapper_sdk(char* wrapperSdkVersion, char* wrapperSdkName, char* wrapperRuntimeVersion, char* liveUpdateReleaseLabel, char* liveUpdateDeploymentKey, char* liveUpdatePackageHash);
