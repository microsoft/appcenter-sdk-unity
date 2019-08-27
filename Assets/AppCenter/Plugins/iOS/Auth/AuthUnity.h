// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#ifndef AUTH_UNITY_H
#define AUTH_UNITY_H

#import <AppCenterAuth/AppCenterAuth.h>

extern "C" void* appcenter_unity_auth_get_type();
extern "C" void appcenter_unity_auth_sign_in_with_completion_handler(void(* signInCompletionHandler)(MSUserInformation *, NSError *));
extern "C" void appcenter_unity_auth_sign_out();
extern "C" void appcenter_unity_auth_set_enabled(bool isEnabled);
extern "C" bool appcenter_unity_auth_is_enabled();

#endif
