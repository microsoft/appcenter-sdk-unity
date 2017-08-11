// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PushUnity.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

static UnityPushDelegate *pushDelegate;

void mobile_center_unity_push_set_received_push_impl(ReceivedPushNotificationFunction functionPtr)
{
  [pushDelegate setPushHandlerImplementation:functionPtr];
}

void mobile_center_unity_push_replay_unprocessed_notifications()
{
  [pushDelegate replayUnprocessedNotifications];
}

void mobile_center_unity_push_set_delegate()
{
  pushDelegate = [UnityPushDelegate new];
  [MSPush setDelegate:pushDelegate];
}

void* mobile_center_unity_push_get_type()
{
  return (void *)CFBridgingRetain([MSPush class]);
}

void mobile_center_unity_push_set_enabled(bool isEnabled)
{
  [MSPush setEnabled:isEnabled];
}

bool mobile_center_unity_push_is_enabled()
{
  return [MSPush isEnabled];
}
