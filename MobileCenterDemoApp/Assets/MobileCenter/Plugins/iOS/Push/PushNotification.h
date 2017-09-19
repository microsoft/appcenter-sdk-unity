// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#ifndef PUSH_NOTIFICATION_H
#define PUSH_NOTIFICATION_H

#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

extern "C" const char* mobile_center_unity_push_notification_get_title(MSPushNotification* push);
extern "C" const char* mobile_center_unity_push_notification_get_message(MSPushNotification* push);
extern "C" NSDictionary* mobile_center_unity_push_notification_get_custom_data(MSPushNotification* push);

#endif
