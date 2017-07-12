//
//  unityMCtest.h
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

extern "C" char* mobile_center_unity_push_notification_get_title(MSPushNotification* push);
extern "C" char* mobile_center_unity_push_notification_get_message(MSPushNotification* push);
extern "C" NSDictionary* mobile_center_unity_push_notification_get_custom_data(MSPushNotification* push);
