//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "MobileCenterUnity.h"
#import "Utility/NSStringHelper.h"
#import <MobileCenter/MobileCenter.h>
#import <Foundation/Foundation.h>

void mobile_center_unity_configure(char* appSecret)
{
  [MSMobileCenter configureWithAppSecret:[NSString stringWithUTF8String:appSecret]];
}

void mobile_center_unity_start(char* appSecret, void** services, int numServices)
{
  NSMutableArray<Class> *servicearray = [[NSMutableArray<Class> alloc] init];

  for (int i = 0; i < numServices; ++i)
  {
    void* service = services[i];
    Class clazz = *(Class*)service;
    free(service);
    [servicearray addObject:clazz];
  }

  [MSMobileCenter start:[NSString stringWithUTF8String:appSecret] withServices:servicearray];
}

void mobile_center_unity_start_services(void** services, int numServices)
{
  for (int i = 0; i < numServices; ++i)
  {
    void* service = services[i];
    Class clazz = *(Class*)service;
    free(service);
    [MSMobileCenter startService:clazz];
  }
}

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

void mobile_center_unity_set_log_url(char* logUrl)
{
  [MSMobileCenter setLogUrl:[NSString stringWithUTF8String:logUrl]];
}

void mobile_center_unity_set_enabled(bool isEnabled)
{
  [MSMobileCenter setEnabled:isEnabled];
}

bool mobile_center_unity_is_enabled()
{
  return [MSMobileCenter isEnabled];
}

char* mobile_center_unity_get_install_id()
{
  NSString *uuidString =  [[MSMobileCenter installId] UUIDString];
  return mobile_center_unity_ns_string_to_cstr(uuidString);
}

void mobile_center_unity_set_custom_properties(MSCustomProperties* properties)
{
  [MSMobileCenter setCustomProperties:properties];
}

static NSString* NSStringOrNil(char* str)
{
  return str ? [NSString stringWithUTF8String:str] : nil;
}

void mobile_center_unity_set_wrapper_sdk(char* wrapperSdkVersion, char* wrapperSdkName, char* wrapperRuntimeVersion, char* liveUpdateReleaseLabel, char* liveUpdateDeploymentKey, char* liveUpdatePackageHash)
{


  MSWrapperSdk *wrapperSdk = [[MSWrapperSdk alloc]
                              initWithWrapperSdkVersion:NSStringOrNil(wrapperSdkVersion)
                              wrapperSdkName:NSStringOrNil(wrapperSdkName)
                              wrapperRuntimeVersion:NSStringOrNil(wrapperRuntimeVersion)
                              liveUpdateReleaseLabel:NSStringOrNil(liveUpdateReleaseLabel)
                              liveUpdateDeploymentKey:NSStringOrNil(liveUpdateDeploymentKey)
                              liveUpdatePackageHash:NSStringOrNil(liveUpdatePackageHash)];
  [MSMobileCenter setWrapperSdk:wrapperSdk];
}


