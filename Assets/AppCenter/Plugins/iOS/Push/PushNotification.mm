// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "PushNotification.h"
#import "../Core/Utility/NSStringHelper.h"
#import <AppCenterPush/AppCenterPush.h>
#import <Foundation/Foundation.h>

const char* appcenter_unity_push_notification_get_title(MSPushNotification* push)
{
  return appcenter_unity_ns_string_to_cstr([push title]);
}

const char* appcenter_unity_push_notification_get_message(MSPushNotification* push)
{
  return appcenter_unity_ns_string_to_cstr([push message]);
}

NSDictionary* appcenter_unity_push_notification_get_custom_data(MSPushNotification* push)
{
  return [push customData];
}
