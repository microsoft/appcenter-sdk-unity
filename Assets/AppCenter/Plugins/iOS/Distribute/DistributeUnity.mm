// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "DistributeUnity.h"
#import <AppCenter/MSACAppCenter.h>
#import "DistributeDelegate.h"
#import <Foundation/Foundation.h>

void* appcenter_unity_distribute_get_type()
{
  return (void *)CFBridgingRetain([MSACDistribute class]);
}

void appcenter_unity_distribute_set_enabled(bool isEnabled)
{
  [MSACDistribute setEnabled:isEnabled];
}

bool appcenter_unity_distribute_is_enabled()
{
  return [MSACDistribute isEnabled];
}

void appcenter_unity_distribute_check_for_update()
{
  [MSACDistribute checkForUpdate];
}

void appcenter_unity_distribute_set_install_url(char* installUrl)
{
  [MSACDistribute setInstallUrl:[NSString stringWithUTF8String:installUrl]];
}

void appcenter_unity_distribute_set_api_url(char* apiUrl)
{
  [MSACDistribute setApiUrl:[NSString stringWithUTF8String:apiUrl]];
}

void appcenter_unity_distribute_notify_update_action(int updateAction)
{
  [MSACDistribute notifyUpdateAction:(MSACUpdateAction)updateAction];
}

void appcenter_unity_distribute_set_release_available_impl(ReleaseAvailableFunction function)
{
  [[UnityDistributeDelegate sharedInstance] setReleaseAvailableImplementation:function];
}

void appcenter_unity_distribute_set_will_exit_app_impl(WillExitAppFunction function)
{
  [[UnityDistributeDelegate sharedInstance] setWillExitAppImplementation:function];
}

void appcenter_unity_distribute_set_no_release_available_impl(NoReleaseAvailableFunction function)
{
  [[UnityDistributeDelegate sharedInstance] setNoReleaseAvailableImplementation:function];
}

void appcenter_unity_start_distribute()
{
  [MSACAppCenter startService:MSACDistribute.class];
}

void appcenter_unity_distribute_replay_release_available()
{
  [[UnityDistributeDelegate sharedInstance] replayReleaseAvailable];
}

void appcenter_unity_distribute_set_delegate()
{
  [MSACDistribute setDelegate:[UnityDistributeDelegate sharedInstance]];
}
