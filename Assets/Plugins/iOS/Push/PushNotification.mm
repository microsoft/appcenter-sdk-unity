//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "PushNotification.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

char* mobile_center_unity_push_notification_get_title(MSPushNotification* push)
{
  return (char*)[[push title] UTF8String];
}

char* mobile_center_unity_push_notification_get_message(MSPushNotification* push)
{
  return (char*)[[push message] UTF8String];
}

NSDictionary* mobile_center_unity_push_notification_get_custom_data(MSPushNotification* push)
{
  return [push customData];
}
