// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PushDelegate.h"


extern "C" void* mobile_center_unity_push_get_type();
extern "C" void mobile_center_unity_push_set_enabled(bool isEnabled);
extern "C" bool mobile_center_unity_push_is_enabled();
extern "C" void mobile_center_unity_push_set_received_push_impl(ReceivedPushNotificationFunction functionPtr);
extern "C" void mobile_center_unity_push_replay_unprocessed_notifications();

void mobile_center_unity_push_set_delegate();

//TODO
// +(void)didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken;
// +(void)didFailToRegisterForRemoteNotificationsWithError:(NSError*)error;
//+ (BOOL)didReceiveRemoteNotification:(NSDictionary *)userInfo;
