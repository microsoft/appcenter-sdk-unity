//
//  unityMCtest.h
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

extern "C" void* mobile_center_unity_push_get_type();

extern "C" void mobile_center_unity_push_set_enabled(bool isEnabled);

extern "C" bool mobile_center_unity_push_is_enabled();

//TODO
// +(void)didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken;
// +(void)didFailToRegisterForRemoteNotificationsWithError:(NSError*)error;
//+ (BOOL)didReceiveRemoteNotification:(NSDictionary *)userInfo;
