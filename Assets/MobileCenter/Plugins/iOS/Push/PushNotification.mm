// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PushNotification.h"
#import "../Core/Utility/NSStringHelper.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

char* mobile_center_unity_push_notification_get_title(MSPushNotification* push)
{
  return mobile_center_unity_ns_string_to_cstr([push title]);
}

char* mobile_center_unity_push_notification_get_message(MSPushNotification* push)
{
  return mobile_center_unity_ns_string_to_cstr([push message]);
}

NSDictionary* mobile_center_unity_push_notification_get_custom_data(MSPushNotification* push)
{
  return [push customData];
}
