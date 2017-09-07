// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "ReleaseDetails.h"
#import "../Core/Utility/NSStringHelper.h"
#import <MobileCenterDistribute/MobileCenterDistribute.h>
#import <Foundation/Foundation.h>

int mobile_center_unity_release_details_get_id(MSReleaseDetails* details)
{
  return [[details id] intValue];
}

char* mobile_center_unity_release_details_get_version(MSReleaseDetails* details)
{
  return mobile_center_unity_ns_string_to_cstr([details version]);
}

char* mobile_center_unity_release_details_get_short_version(MSReleaseDetails* details)
{
  return mobile_center_unity_ns_string_to_cstr([details shortVersion]);
}

char* mobile_center_unity_release_details_get_release_notes(MSReleaseDetails* details)
{
  return mobile_center_unity_ns_string_to_cstr([details releaseNotes]);
}

bool mobile_center_unity_release_details_get_mandatory_update(MSReleaseDetails* details)
{
  return [details mandatoryUpdate];
}

char* mobile_center_unity_release_details_get_url(MSReleaseDetails* details)
{
  return mobile_center_unity_ns_string_to_cstr([[details releaseNotesUrl] absoluteString]);
}

