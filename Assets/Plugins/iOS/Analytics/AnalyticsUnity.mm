// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "AnalyticsUnity.h"
#import "../Core/Utility/NSStringDictionaryHelper.h"
#import <MobileCenterAnalytics/MobileCenterAnalytics.h>
#import <Foundation/Foundation.h>

void* mobile_center_unity_analytics_get_type()
{
  Class* ptr = (Class*)malloc(sizeof(Class));
  *ptr = [MSAnalytics class];
  return ptr;
}

void mobile_center_unity_analytics_track_event(char* eventName)
{
  [MSAnalytics trackEvent:[NSString stringWithUTF8String:eventName]];
}

void mobile_center_unity_analytics_track_event_with_properties(char* eventName, char** keys, char** values, int count)
{
  NSDictionary<NSString*, NSString*> *properties = mobile_center_unity_create_ns_string_dictionary(keys, values, count);
  NSString * eventNameString = [NSString stringWithUTF8String:eventName];
  [MSAnalytics trackEvent:eventNameString withProperties:properties];
}

void mobile_center_unity_analytics_set_enabled(bool isEnabled)
{
  [MSAnalytics setEnabled:isEnabled];
}

bool mobile_center_unity_analytics_is_enabled()
{
  return [MSAnalytics isEnabled];
}
