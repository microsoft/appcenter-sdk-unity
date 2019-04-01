// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "TransmissionTarget.h"
#import "AppCenterAnalytics/MSAnalyticsTransmissionTarget.h"
#import "AppCenterAnalytics/MSPropertyConfigurator.h"
#import "../Core/Utility/NSStringDictionaryHelper.h"

void appcenter_unity_transmission_target_track_event(MSAnalyticsTransmissionTarget *transmission, char* eventName, int flags) {
  [transmission trackEvent:[NSString stringWithUTF8String:eventName] withProperties:NULL flags:flags];
}

void appcenter_unity_transmission_target_track_event_with_props(MSAnalyticsTransmissionTarget *transmission, char* eventName, char** keys, char** values, int count, int flags) {
  NSDictionary<NSString*, NSString*> *properties = appcenter_unity_create_ns_string_dictionary(keys, values, count);
  [transmission trackEvent:[NSString stringWithUTF8String:eventName] withProperties: properties flags:flags];
}

void appcenter_unity_transmission_target_track_event_with_typed_props(MSAnalyticsTransmissionTarget *transmission, char* eventName, MSEventProperties* properties, int flags) {
  [transmission trackEvent: [NSString stringWithUTF8String:eventName] withTypedProperties:properties flags:flags];
}

void appcenter_unity_transmission_target_set_enabled(MSAnalyticsTransmissionTarget *transmission, BOOL enabled) {
  [transmission setEnabled: enabled];
}

BOOL appcenter_unity_transmission_target_is_enabled(MSAnalyticsTransmissionTarget *transmission) {
  return [transmission isEnabled];
}

MSAnalyticsTransmissionTarget *appcenter_unity_transmission_transmission_target_for_token(MSAnalyticsTransmissionTarget *transmissionParent, char* transmissionTargetToken) {
  return [transmissionParent transmissionTargetForToken: [NSString stringWithUTF8String:transmissionTargetToken]];
}

MSPropertyConfigurator *appcenter_unity_transmission_get_property_configurator(MSAnalyticsTransmissionTarget *transmission) {
  return [transmission propertyConfigurator];
}

void appcenter_unity_transmission_pause(MSAnalyticsTransmissionTarget *transmission)
{
  [transmission pause];
}

void appcenter_unity_transmission_resume(MSAnalyticsTransmissionTarget *transmission)
{
  [transmission resume];
}
