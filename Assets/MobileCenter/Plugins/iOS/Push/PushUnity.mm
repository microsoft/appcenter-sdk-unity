// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PushUnity.h"
#import "PushDelegateSetter.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

void mobile_center_unity_push_set_received_push_impl(ReceivedPushNotificationFunction functionPtr)
{
  [mobile_center_unity_push_get_delegate() setPushHandlerImplementation:functionPtr];
}

void mobile_center_unity_push_replay_unprocessed_notifications()
{
  [mobile_center_unity_push_get_delegate() replayUnprocessedNotifications];
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
