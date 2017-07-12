// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

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
