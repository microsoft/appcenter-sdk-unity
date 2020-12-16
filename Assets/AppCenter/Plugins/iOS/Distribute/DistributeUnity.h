// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#ifndef DISTRIBUTE_UNITY_H
#define DISTRIBUTE_UNITY_H

#import <AppCenterDistribute/AppCenterDistribute.h>
#import "DistributeDelegate.h"

extern "C" void* appcenter_unity_distribute_get_type();
extern "C" void appcenter_unity_distribute_set_enabled(bool isEnabled);
extern "C" bool appcenter_unity_distribute_is_enabled();
extern "C" void appcenter_unity_distribute_set_install_url(char* installUrl);
extern "C" void appcenter_unity_distribute_set_api_url(char* apiUrl);
extern "C" void appcenter_unity_distribute_notify_update_action(int updateAction);
extern "C" void appcenter_unity_distribute_replay_release_available();
extern "C" void appcenter_unity_distribute_set_release_available_impl(ReleaseAvailableFunction handler);
extern "C" void appcenter_unity_distribute_set_will_exit_app_impl(WillExitAppFunction handler);
extern "C" void appcenter_unity_distribute_set_no_release_available_impl(NoReleaseAvailableFunction handler);
extern "C" void appcenter_unity_distribute_set_delegate();
extern "C" void appcenter_unity_distribute_check_for_update();
extern "C" void appcenter_unity_start_distribute(); 

#endif
