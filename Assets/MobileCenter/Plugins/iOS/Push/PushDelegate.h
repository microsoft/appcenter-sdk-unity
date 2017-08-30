// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#ifndef PUSH_DELEGATE_H
#define PUSH_DELEGATE_H

#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

typedef void (__cdecl *ReceivedPushNotificationFunction)(MSPushNotification*);

@interface UnityPushDelegate : NSObject<MSPushDelegate>

-(void)push:(MSPush *)push didReceivePushNotification:(MSPush *)pushNotification;
-(void)setPushHandlerImplementation:(ReceivedPushNotificationFunction)implementation;
-(void)replayUnprocessedNotifications;

@end

#endif
