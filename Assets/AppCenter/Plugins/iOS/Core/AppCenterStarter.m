#define APPCENTER_UNITY_USE_DISTRIBUTE
#define APPCENTER_UNITY_USE_CUSTOM_INSTALL_URL
#define APPCENTER_UNITY_USE_CUSTOM_API_URL
#define APPCENTER_UNITY_USE_CRASHES
#define APPCENTER_UNITY_USE_ANALYTICS
#define APPCENTER_UNITY_USE_PUSH
#define APPCENTER_UNITY_USE_CUSTOM_LOG_URL
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "AppCenterStarter.h"
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@import AppCenter;

#ifdef APPCENTER_UNITY_USE_CRASHES
@import AppCenterCrashes;
#endif

#ifdef APPCENTER_UNITY_USE_PUSH
@import AppCenterPush;
#import "../Push/PushDelegate.h"
#endif

#ifdef APPCENTER_UNITY_USE_ANALYTICS
@import AppCenterAnalytics;
#endif

#ifdef APPCENTER_UNITY_USE_DISTRIBUTE
@import AppCenterDistribute;
#import "../Distribute/DistributeDelegate.h"
#endif

enum StartupMode {
  APPCENTER,
  ONECOLLECTOR,
  BOTH,
  NONE,
  SKIP
};

@implementation AppCenterStarter

static NSString *const kMSAppSecret = @"c7d464ba-8c6b-4e7b-a866-3d53112132eb";
static NSString *const kMSTargetToken = @"a5f11422a6f74e1a8c69afa1e0f9d5bd-c10bc995-8731-4b41-8cef-dd5e4333d594-6833";
static NSString *const kMSCustomLogUrl = @"https://in-integration.dev.avalanch.es";
static NSString *const kMSCustomApiUrl = @"https://api-gateway-core-integration.dev.avalanch.es/v0.1";
static NSString *const kMSCustomInstallUrl = @"https://install.portal-server-core-integration.dev.avalanch.es";
static NSString *const kMSStartTargetKey = @"MSAppCenterStartTargetUnityKey";
static NSString *const kMSStorageSizeKey = @"MSAppCenterMaxStorageSizeUnityKey";
static NSString *const kMSLogUrlKey = @"MSAppCenterLogUrlUnityKey";
static NSString *const kMSAppSecretKey = @"MSAppCenterAppSecretUnityKey";
static NSString *const kMSUpdateTrackKey = @"MSAppCenterUpdateTrackUnityKey";

static const int kMSLogLevel = 0 /*LOG_LEVEL*/;
static const int kMSStartupType = 0 /*STARTUP_TYPE*/;
static const int kMSUpdateTrack = 1;

+ (void)load {
  [[NSNotificationCenter defaultCenter] addObserver:self
                                           selector:@selector(startAppCenter)
                                               name:UIApplicationDidFinishLaunchingNotification
                                             object:nil];
}

+ (void)startAppCenter {
  NSNumber *startTarget = [[NSUserDefaults standardUserDefaults] objectForKey:kMSStartTargetKey];
  int startTargetValue = startTarget == nil ? kMSStartupType : startTarget.intValue;
  [MSACAppCenter setLogLevel:(MSACLogLevel)kMSLogLevel];
  if (startTargetValue == SKIP) {
    return;
  }

  NSMutableArray<Class> *classes = [[NSMutableArray alloc] init];

  NSNumber *maxStorageSize = [[NSUserDefaults standardUserDefaults] objectForKey:kMSStorageSizeKey];
  if (maxStorageSize != nil) {
    [MSACAppCenter setMaxStorageSize:maxStorageSize
                 completionHandler:^void(BOOL result) {
                   if (!result) {
                     MSACLogWarning(@"MSACAppCenter", @"setMaxStorageSize failed");
                   }
                 }];
  } else {
#ifdef APPCENTER_USE_CUSTOM_MAX_STORAGE_SIZE
    [MSACAppCenter setMaxStorageSize:APPCENTER_MAX_STORAGE_SIZE
                 completionHandler:^void(BOOL result) {
                   if (!result) {
                     MSACLogWarning(@"MSACAppCenter", @"setMaxStorageSize failed");
                   }
                 }];
#endif
  }

#ifdef APPCENTER_UNITY_USE_ANALYTICS
  [classes addObject:MSACAnalytics.class];
#endif

#ifdef APPCENTER_UNITY_USE_PUSH
  [classes addObject:MSPush.class];
  [MSPush setDelegate:[UnityPushDelegate sharedInstance]];
#endif

#ifdef APPCENTER_UNITY_USE_DISTRIBUTE

[MSACDistribute setUpdateTrack:(MSACUpdateTrack)kMSUpdateTrack];
  
#ifdef APPCENTER_UNITY_USE_CUSTOM_API_URL
  [MSACDistribute setApiUrl:kMSCustomApiUrl];
#endif // APPCENTER_UNITY_USE_CUSTOM_API_URL

#ifdef APPCENTER_UNITY_USE_CUSTOM_INSTALL_URL
  [MSACDistribute setInstallUrl:kMSCustomInstallUrl];
#endif // APPCENTER_UNITY_USE_CUSTOM_INSTALL_URL
  [classes addObject:MSACDistribute.class];

#ifdef APPCENTER_DISTRIBUTE_DISABLE_AUTOMATIC_CHECK_FOR_UPDATE
  [MSACDistribute disableAutomaticCheckForUpdate];
#endif // APPCENTER_DISTRIBUTE_DISABLE_AUTOMATIC_CHECK_FOR_UPDATE

#endif // APPCENTER_UNITY_USE_DISTRIBUTE

  NSString *customLogUrl = [[NSUserDefaults standardUserDefaults] objectForKey:kMSLogUrlKey];
  if (customLogUrl != nil) {
    [MSACAppCenter setLogUrl:customLogUrl];
  } else {
#ifdef APPCENTER_UNITY_USE_CUSTOM_LOG_URL
    [MSACAppCenter setLogUrl:kMSCustomLogUrl];
#endif
  }
  NSString *customAppSecret = [[NSUserDefaults standardUserDefaults] objectForKey:kMSAppSecretKey];
  NSString *customAppSecretValue = customAppSecret == nil ? kMSAppSecret : customAppSecret;
  switch (startTargetValue) {
    case APPCENTER:
      [MSACAppCenter start:customAppSecretValue withServices:classes];
      break;
    case ONECOLLECTOR:
      [MSACAppCenter start:[NSString stringWithFormat:@"target=%@", kMSTargetToken] withServices:classes];
      break;
    case BOTH:
      [MSACAppCenter start:[NSString stringWithFormat:@"appsecret=%@;target=%@", customAppSecretValue, kMSTargetToken] withServices:classes];
      break;
    case NONE:
      [MSACAppCenter startWithServices:classes];
      break;
  }
}

@end
