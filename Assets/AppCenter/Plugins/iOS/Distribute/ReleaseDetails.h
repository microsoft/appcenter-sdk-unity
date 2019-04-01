// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <AppCenterDistribute/AppCenterDistribute.h>

extern "C" int appcenter_unity_release_details_get_id(MSReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_version(MSReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_short_version(MSReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_release_notes(MSReleaseDetails* details);
extern "C" bool appcenter_unity_release_details_get_mandatory_update(MSReleaseDetails* details);
extern "C" const char* appcenter_unity_release_details_get_url(MSReleaseDetails* details);

