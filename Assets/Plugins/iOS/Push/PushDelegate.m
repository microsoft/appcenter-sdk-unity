// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PushDelegate.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

@interface UnityPushDelegate ()

@property NSMutableArray<MSPushNotification*> *unprocessedNotifications;
@property ReceivedPushNotificationFunction receivedPushNotification;
@property NSObject *lockObject;

@end

@implementation UnityPushDelegate

- (instancetype)init {
  if ((self = [super init])) {
    _lockObject = [NSObject new];
    _unprocessedNotifications = [NSMutableArray new];
  }
  return self;
}

- (void)push:(MSPush *)push didReceivePushNotification:(MSPushNotification *)pushNotification {
  ReceivedPushNotificationFunction handlerCopy = nil;
  @synchronized (_lockObject) {
    if (_unprocessedNotifications && pushNotification) {
      [_unprocessedNotifications addObject:pushNotification];
    }
    handlerCopy = _receivedPushNotification;
  }
  if (handlerCopy) {
    (handlerCopy)(pushNotification);
  }
}

-(void) setPushHandlerImplementation:(ReceivedPushNotificationFunction)implementation {
  _receivedPushNotification = implementation;
}

- (void) replayUnprocessedNotifications {
  ReceivedPushNotificationFunction handlerCopy = nil;
  NSMutableArray<MSPushNotification*> *unprocessedCopy = nil;
  @synchronized (_lockObject) {
    if (_unprocessedNotifications) {
      unprocessedCopy = [[NSMutableArray alloc] initWithArray:_unprocessedNotifications];
      _unprocessedNotifications = nil;
    }
    handlerCopy = _receivedPushNotification;
  }
  if (unprocessedCopy && handlerCopy) {
    for (MSPushNotification *notification in unprocessedCopy) {
      (_receivedPushNotification)(notification);
    }
  }
}

@end
