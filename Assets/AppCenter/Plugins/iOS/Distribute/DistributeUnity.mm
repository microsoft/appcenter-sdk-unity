// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "DistributeUnity.h"
#import "DistributeDelegate.h"
#import <Foundation/Foundation.h>

void* appcenter_unity_distribute_get_type()
{
  return (void *)CFBridgingRetain([MSDistribute class]);
}

void appcenter_unity_distribute_set_enabled(bool isEnabled)
{
  [MSDistribute setEnabled:isEnabled];
}

bool appcenter_unity_distribute_is_enabled()
{
  return [MSDistribute isEnabled];
}

void appcenter_unity_distribute_check_for_update()
{
  [MSDistribute checkForUpdate];
}

void appcenter_unity_distribute_set_install_url(char* installUrl)
{
  [MSDistribute setInstallUrl:[NSString stringWithUTF8String:installUrl]];
}

void appcenter_unity_distribute_set_api_url(char* apiUrl)
{
  [MSDistribute setApiUrl:[NSString stringWithUTF8String:apiUrl]];
}

void appcenter_unity_distribute_notify_update_action(int updateAction)
{
  [MSDistribute notifyUpdateAction:(MSUpdateAction)updateAction];
}

void appcenter_unity_distribute_set_release_available_impl(ReleaseAvailableFunction function)
{
  [[UnityDistributeDelegate sharedInstance] setReleaseAvailableImplementation:function];
}

void appcenter_unity_distribute_replay_release_available()
{
  [[UnityDistributeDelegate sharedInstance] replayReleaseAvailable];
}

void appcenter_unity_distribute_set_delegate()
{
  [MSDistribute setDelegate:[UnityDistributeDelegate sharedInstance]];
}
