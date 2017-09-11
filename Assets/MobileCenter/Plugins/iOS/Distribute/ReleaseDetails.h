// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import <MobileCenterDistribute/MobileCenterDistribute.h>

extern "C" int mobile_center_unity_release_details_get_id(MSReleaseDetails* details);
extern "C" char* mobile_center_unity_release_details_get_version(MSReleaseDetails* details);
extern "C" char* mobile_center_unity_release_details_get_short_version(MSReleaseDetails* details);
extern "C" char* mobile_center_unity_release_details_get_release_notes(MSReleaseDetails* details);
extern "C" bool mobile_center_unity_release_details_get_mandatory_update(MSReleaseDetails* details);
extern "C" char* mobile_center_unity_release_details_get_url(MSReleaseDetails* details);

