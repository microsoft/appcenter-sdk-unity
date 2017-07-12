// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "DistributeUnity.h"
#import <MobileCenterDistribute/MobileCenterDistribute.h>
#import <Foundation/Foundation.h>

void* mobile_center_unity_distribute_get_type()
{
  Class* ptr = (Class*)malloc(sizeof(Class));
  *ptr = [MSDistribute class];
  return ptr;
}

void mobile_center_unity_distribute_set_enabled(bool isEnabled)
{
  [MSDistribute setEnabled:isEnabled];
}

bool mobile_center_unity_distribute_is_enabled()
{
  return [MSDistribute isEnabled];
}

void mobile_center_unity_distribute_set_install_url(char* installUrl)
{
  [MSDistribute setInstallUrl:[NSString stringWithUTF8String:installUrl]];
}

void mobile_center_unity_distribute_set_api_url(char* apiUrl)
{
  [MSDistribute setApiUrl:[NSString stringWithUTF8String:apiUrl]];
}

void mobile_center_unity_distribute_notify_update_action(int updateAction)
{
  [MSDistribute notifyUpdateAction:(MSUpdateAction)updateAction];
}
