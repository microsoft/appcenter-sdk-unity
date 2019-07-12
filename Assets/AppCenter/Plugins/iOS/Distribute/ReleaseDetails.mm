// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "ReleaseDetails.h"
#import "../Core/Utility/NSStringHelper.h"
#import <AppCenterDistribute/AppCenterDistribute.h>
#import <Foundation/Foundation.h>

int appcenter_unity_release_details_get_id(MSReleaseDetails* details)
{
  return [[details id] intValue];
}

const char* appcenter_unity_release_details_get_version(MSReleaseDetails* details)
{
  return appcenter_unity_ns_string_to_cstr([details version]);
}

const char* appcenter_unity_release_details_get_short_version(MSReleaseDetails* details)
{
  return appcenter_unity_ns_string_to_cstr([details shortVersion]);
}

const char* appcenter_unity_release_details_get_release_notes(MSReleaseDetails* details)
{
  return appcenter_unity_ns_string_to_cstr([details releaseNotes]);
}

bool appcenter_unity_release_details_get_mandatory_update(MSReleaseDetails* details)
{
  return [details isMandatoryUpdate];
}

const char* appcenter_unity_release_details_get_url(MSReleaseDetails* details)
{
  return appcenter_unity_ns_string_to_cstr([[details releaseNotesUrl] absoluteString]);
}

