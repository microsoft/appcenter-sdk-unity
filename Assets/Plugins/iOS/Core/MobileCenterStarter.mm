// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "MobileCenterStarter.h"
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <MobileCenter/MSMobileCenter.h>

#ifdef MOBILE_CENTER_UNITY_USE_PUSH
#import <MobileCenterPush/MSPush.h>
#endif

@implementation MobileCenterStarter

static NSString *kMSAppSecret = @"mobile-center-app-secret";
static NSMutableArray<Class>* classes = [[NSMutableArray alloc] init];

static NSString *kMSCustomLogUrl = @"custom-log-url";
static int kMSLogLevel = 0/*LOG_LEVEL*/;

+ (void)load {
  [[NSNotificationCenter defaultCenter] addObserver:self
                                           selector:@selector(startMobileCenter)
                                               name:UIApplicationDidFinishLaunchingNotification
                                             object:nil];
}

+ (void)startMobileCenter {
#ifdef MOBILE_CENTER_UNITY_USE_PUSH
  [classes addObject:MSPush.class];
#endif

  [MSMobileCenter setLogLevel:(MSLogLevel)kMSLogLevel];

#ifdef MOBILE_CENTER_UNITY_USE_CUSTOM_LOG_URL
  [MSMobileCenter setLogUrl:kMSCustomLogUrl];
#endif


  [MSMobileCenter start:kMSAppSecret withServices:classes];
}

@end
