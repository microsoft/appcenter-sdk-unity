// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "AuthUnity.h"
#import <Foundation/Foundation.h>

void* appcenter_unity_auth_get_type() {
  return (void *)CFBridgingRetain([MSAuth class]);
}

void appcenter_unity_auth_sign_in_with_completion_handler(void(* handler)(MSUserInformation *, NSError *)) {
  [MSAuth signInWithCompletionHandler:^void(MSUserInformation *_Nullable userInformation, NSError *_Nullable error) {
    return handler(userInformation, error);
  }];
}

void appcenter_unity_auth_sign_out() {
  [MSAuth signOut];
}

void appcenter_unity_auth_set_enabled(bool isEnabled) {
  [MSAuth setEnabled:isEnabled];
}

bool appcenter_unity_auth_is_enabled() {
  return [MSAuth isEnabled];
}
