// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PushDelegate.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

static ReceivedPushNotificationFunction receivedPushNotification;
static UnityPushDelegate *del;
static MSPushNotification *mLastPushReceived;

void mobile_center_unity_push_delegate_provide_received_push_impl(ReceivedPushNotificationFunction functionPtr)
{
  receivedPushNotification = functionPtr;
}

void mobile_center_unity_push_set_delegate()
{
  del = [[UnityPushDelegate alloc] init];
  [MSPush setDelegate:del];
}

@implementation UnityPushDelegate

-(void)push:(MSPush *)push didReceivePushNotification:(MSPushNotification *)pushNotification;
{
  mLastPushReceived = pushNotification;
  (receivedPushNotification)(mLastPushReceived);
}

@end
