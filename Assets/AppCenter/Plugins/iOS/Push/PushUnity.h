// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#ifndef PUSH_UNITY_H
#define PUSH_UNITY_H

#import "PushDelegate.h"

extern "C" void* appcenter_unity_push_get_type();
extern "C" void appcenter_unity_push_set_enabled(bool isEnabled);
extern "C" bool appcenter_unity_push_is_enabled();
extern "C" void appcenter_unity_start_push();
extern "C" void appcenter_unity_push_set_received_push_impl(ReceivedPushNotificationFunction functionPtr);
extern "C" void appcenter_unity_push_replay_unprocessed_notifications();

// TODO this should be included if we support explicit initialization, but
// removed otherwise
//extern "C" void appcenter_unity_push_set_delegate();

//TODO
// +(void)didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken;
// +(void)didFailToRegisterForRemoteNotificationsWithError:(NSError*)error;
//+ (BOOL)didReceiveRemoteNotification:(NSDictionary *)userInfo;

#endif
