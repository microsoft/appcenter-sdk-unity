// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "AnalyticsUnity.h"
#import "../Core/Utility/NSStringDictionaryHelper.h"
#import <MobileCenterAnalytics/MobileCenterAnalytics.h>
#import <Foundation/Foundation.h>

void* appcenter_unity_analytics_get_type()
{
  return (void *)CFBridgingRetain([MSAnalytics class]);
}

void appcenter_unity_analytics_track_event(char* eventName)
{
  [MSAnalytics trackEvent:[NSString stringWithUTF8String:eventName]];
}

void appcenter_unity_analytics_track_event_with_properties(char* eventName, char** keys, char** values, int count)
{
  NSDictionary<NSString*, NSString*> *properties = appcenter_unity_create_ns_string_dictionary(keys, values, count);
  NSString * eventNameString = [NSString stringWithUTF8String:eventName];
  [MSAnalytics trackEvent:eventNameString withProperties:properties];
}

void appcenter_unity_analytics_set_enabled(bool isEnabled)
{
  [MSAnalytics setEnabled:isEnabled];
}

bool appcenter_unity_analytics_is_enabled()
{
  return [MSAnalytics isEnabled];
}
