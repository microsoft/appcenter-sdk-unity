// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "PushUnity.h"
#import "PushDelegate.h"
#import <AppCenterPush/AppCenterPush.h>
#import <AppCenter/MSAppCenter.h>
#import <Foundation/Foundation.h>

void appcenter_unity_push_set_received_push_impl(ReceivedPushNotificationFunction functionPtr)
{
  [[UnityPushDelegate sharedInstance] setPushHandlerImplementation:functionPtr];
}

void appcenter_unity_push_replay_unprocessed_notifications()
{
  [[UnityPushDelegate sharedInstance] replayUnprocessedNotifications];
}

void appcenter_unity_start_push()
{
    [MSAppCenter startService:MSPush.class];
}

void* appcenter_unity_push_get_type()
{
  return (void *)CFBridgingRetain([MSPush class]);
}

void appcenter_unity_push_set_enabled(bool isEnabled)
{
  [MSPush setEnabled:isEnabled];
}

bool appcenter_unity_push_is_enabled()
{
  return [MSPush isEnabled];
}
