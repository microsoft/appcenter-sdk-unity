// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

typedef void (__cdecl *ReceivedPushNotificationFunction)(MSPushNotification*);

extern "C" void mobile_center_unity_push_delegate_provide_received_push_impl(ReceivedPushNotificationFunction* functionPtr);

extern "C" void mobile_center_unity_push_set_delegate();

@interface UnityPushDelegate : NSObject<MSPushDelegate>
-(void)push:(MSPush *)push didReceivePushNotification:(MSPush *)pushNotification;
@end
