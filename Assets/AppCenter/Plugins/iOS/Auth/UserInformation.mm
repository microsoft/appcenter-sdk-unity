// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "../Core/Utility/NSStringHelper.h"
#import "UserInformation.h"
#import <AppCenterAuth/AppCenterAuth.h>

const char* app_center_unity_auth_user_information_account_id(void* userInformation) {
  MSUserInformation *information = (__bridge MSUserInformation*)userInformation;
  return appcenter_unity_ns_string_to_cstr([information accountId]);
}

const char* app_center_unity_auth_user_information_access_token(void* userInformation) {
  MSUserInformation *information = (__bridge MSUserInformation*)userInformation;
  return appcenter_unity_ns_string_to_cstr([information accessToken]);
}

const char* app_center_unity_auth_user_information_id_token(void* userInformation) {
  MSUserInformation *information = (__bridge MSUserInformation*)userInformation;
  return appcenter_unity_ns_string_to_cstr([information idToken]);
}
