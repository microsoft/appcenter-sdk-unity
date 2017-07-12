// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

extern "C" char* mobile_center_unity_push_notification_get_title(MSPushNotification* push);
extern "C" char* mobile_center_unity_push_notification_get_message(MSPushNotification* push);
extern "C" NSDictionary* mobile_center_unity_push_notification_get_custom_data(MSPushNotification* push);
