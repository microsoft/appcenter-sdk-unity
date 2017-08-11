// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PushDelegate.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

@interface UnityPushDelegate ()

@property NSMutableArray<MSPushNotification*> *unprocessedNotifications;
@property ReceivedPushNotificationFunction receivedPushNotification;
@property BOOL shouldCollectUnprocessedNotifications;
@property NSObject *lockObject;

@end

@implementation UnityPushDelegate

- (instancetype)init {
  if ((self = [super init])) {
    _lockObject = [NSObject new];
    _unprocessedNotifications = [NSMutableArray new];
    _shouldCollectUnprocessedNotifications = YES;
  }
  return self;
}

- (void)push:(MSPush *)push didReceivePushNotification:(MSPushNotification *)pushNotification;
{
  @synchronized (_lockObject) {
    if (_shouldCollectUnprocessedNotifications && pushNotification && _unprocessedNotifications) {
      [_unprocessedNotifications addObject:pushNotification];
    }
  }
  (_receivedPushNotification)(pushNotification);
}

-(void) setPushHandlerImplementation:(ReceivedPushNotificationFunction)implementation {
  @synchronized (_lockObject) {
    _shouldCollectUnprocessedNotifications = false;
  }
  _receivedPushNotification = implementation;
}

- (void) replayUnprocessedNotifications {
  NSMutableArray<MSPushNotification*> *unprocessedCopy = nil;
  @synchronized (_lockObject) {
    if (_unprocessedNotifications) {
      unprocessedCopy = [[NSMutableArray alloc] initWithArray:_unprocessedNotifications];
      _unprocessedNotifications = nil;
    }
  }
  if (unprocessedCopy) {
    for (MSPushNotification *notification : unprocessedCopy) {
      (_receivedPushNotification)(notification);
    }
  }
}

@end
