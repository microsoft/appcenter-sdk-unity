// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <AppCenterDistribute/AppCenterDistribute.h>

extern "C" int appcenter_unity_release_details_get_id(MSACReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_version(MSACReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_short_version(MSACReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_release_notes(MSACReleaseDetails* details);
extern "C" bool appcenter_unity_release_details_get_mandatory_update(MSACReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_url(MSACReleaseDetails* details);

