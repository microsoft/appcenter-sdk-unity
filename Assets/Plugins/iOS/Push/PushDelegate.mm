//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "PushDelegate.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

static ReceivedPushNotificationFunction *receivedPushNotification;
static UnityPushDelegate *del;

void mobile_center_unity_push_delegate_provide_received_push_impl(ReceivedPushNotificationFunction* functionPtr)
{
  receivedPushNotification = functionPtr;
}

void mobile_center_unity_push_set_delegate()
{
  del = [[UnityPushDelegate alloc] init];
  [MSPush setDelegate:del];
}

@implementation UnityPushDelegate

-(void)push:(MSPush *)push didReceivePushNotification:(MSPushNotification *)pushNotification;
{
  (*receivedPushNotification)(pushNotification);
}

@end
