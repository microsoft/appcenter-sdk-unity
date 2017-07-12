//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "PushUnity.h"
#import <MobileCenterPush/MobileCenterPush.h>
#import <Foundation/Foundation.h>

void* mobile_center_unity_push_get_type()
{
  Class* ptr = (Class*)malloc(sizeof(Class));
  *ptr = [MSPush class];
  return ptr;
}

void mobile_center_unity_push_set_enabled(bool isEnabled)
{
  [MSPush setEnabled:isEnabled];
}

bool mobile_center_unity_push_is_enabled()
{
  return [MSPush isEnabled];
}
