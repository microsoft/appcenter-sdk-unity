// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "TransmissionTarget.h"
#import "AppCenterAnalytics/MSACAnalyticsTransmissionTarget.h"
#import "AppCenterAnalytics/MSACPropertyConfigurator.h"
#import "../Core/Utility/NSStringDictionaryHelper.h"

void appcenter_unity_transmission_target_track_event(MSACAnalyticsTransmissionTarget *transmission, char* eventName, int flags) {
  [transmission trackEvent:[NSString stringWithUTF8String:eventName] withProperties:NULL flags:flags];
}

void appcenter_unity_transmission_target_track_event_with_props(MSACAnalyticsTransmissionTarget *transmission, char* eventName, char** keys, char** values, int count, int flags) {
  NSDictionary<NSString*, NSString*> *properties = appcenter_unity_create_ns_string_dictionary(keys, values, count);
  [transmission trackEvent:[NSString stringWithUTF8String:eventName] withProperties: properties flags:flags];
}

void appcenter_unity_transmission_target_track_event_with_typed_props(MSACAnalyticsTransmissionTarget *transmission, char* eventName, MSACEventProperties* properties, int flags) {
  [transmission trackEvent: [NSString stringWithUTF8String:eventName] withTypedProperties:properties flags:flags];
}

void appcenter_unity_transmission_target_set_enabled(MSACAnalyticsTransmissionTarget *transmission, BOOL enabled) {
  [transmission setEnabled: enabled];
}

BOOL appcenter_unity_transmission_target_is_enabled(MSACAnalyticsTransmissionTarget *transmission) {
  return [transmission isEnabled];
}

MSACAnalyticsTransmissionTarget *appcenter_unity_transmission_transmission_target_for_token(MSACAnalyticsTransmissionTarget *transmissionParent, char* transmissionTargetToken) {
  return [transmissionParent transmissionTargetForToken: [NSString stringWithUTF8String:transmissionTargetToken]];
}

MSACPropertyConfigurator *appcenter_unity_transmission_get_property_configurator(MSACAnalyticsTransmissionTarget *transmission) {
  return [transmission propertyConfigurator];
}

void appcenter_unity_transmission_pause(MSACAnalyticsTransmissionTarget *transmission)
{
  [transmission pause];
}

void appcenter_unity_transmission_resume(MSACAnalyticsTransmissionTarget *transmission)
{
  [transmission resume];
}
