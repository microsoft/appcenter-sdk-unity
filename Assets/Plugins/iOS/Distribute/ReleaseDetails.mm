// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "ReleaseDetails.h"
#import <MobileCenterDistribute/MobileCenterDistribute.h>
#import <Foundation/Foundation.h>

int mobile_center_unity_release_details_get_id(MSReleaseDetails* details)
{
  return [[details id] intValue];
}

char* mobile_center_unity_release_details_get_version(MSReleaseDetails* details)
{
  return (char*)[[details version] UTF8String];
}

char* mobile_center_unity_release_details_get_short_version(MSReleaseDetails* details)
{
  return (char*)[[details shortVersion] UTF8String];
}

char* mobile_center_unity_release_details_get_release_notes(MSReleaseDetails* details)
{
  return (char*)[[details releaseNotes] UTF8String];
}

bool mobile_center_unity_release_details_get_mandatory_update(MSReleaseDetails* details)
{
  return [details mandatoryUpdate];
}

char* mobile_center_unity_release_details_get_url(MSReleaseDetails* details)
{
  return (char*)[[[details releaseNotesUrl] absoluteString] UTF8String];
}

