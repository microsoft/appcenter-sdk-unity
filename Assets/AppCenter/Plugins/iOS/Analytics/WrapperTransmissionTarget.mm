// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "AppCenterAnalytics/MSAnalyticsTransmissionTarget.h"
#import <Foundation/Foundation.h>
#import "../Core/Utility/NSStringDictionaryHelper.h"

extern "C" MSAnalyticsTransmissionTarget* appcenter_unity_transmission_target_create() {
    return [[MSAnalyticsTransmissionTarget alloc] init];
}
extern "C" void appcenter_unity_transmission_target_track_event(MSAnalyticsTransmissionTarget *transmission, char* eventName) {
  [transmission trackEvent:[NSString stringWithUTF8String:eventName]];
}
extern "C" void appcenter_unity_transmission_target_track_event_with_props(MSAnalyticsTransmissionTarget *transmission, char* eventName, char** keys, char** values, int count) {
NSDictionary<NSString*, NSString*> *properties = appcenter_unity_create_ns_string_dictionary(keys, values, count); 
[transmission trackEvent:[NSString stringWithUTF8String:eventName] withProperties: properties];
}

extern "C" void appcenter_unity_transmission_target_set_enabled(MSAnalyticsTransmissionTarget *transmission, BOOL enabled) {
[transmission setEnabled: enabled];
}

extern "C" BOOL appcenter_unity_transmission_target_is_enabled(MSAnalyticsTransmissionTarget *transmission) {
return [transmission isEnabled];
}
