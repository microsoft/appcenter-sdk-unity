// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "WrapperTransmissionTarget.h"
#import "AppCenterAnalytics/MSAnalyticsTransmissionTarget.h"
#import "AppCenterAnalytics/MSPropertyConfigurator.h"
#import "../Core/Utility/NSStringDictionaryHelper.h"

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

extern "C" MSAnalyticsTransmissionTarget *appcenter_unity_transmission_transmission_target_for_token(MSAnalyticsTransmissionTarget *transmissionParent, char* transmissionTargetToken) {
  return [transmissionParent transmissionTargetForToken: [NSString stringWithUTF8String:transmissionTargetToken]];
}

extern "C" MSPropertyConfigurator *appcenter_unity_transmission_get_property_configurator(MSAnalyticsTransmissionTarget *transmission) {
  return [transmission propertyConfigurator];
}
