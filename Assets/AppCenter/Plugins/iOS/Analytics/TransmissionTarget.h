// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <Foundation/Foundation.h>

@class MSACAnalyticsTransmissionTarget;
@class MSACPropertyConfigurator;
@class MSACEventProperties;

extern "C" void appcenter_unity_transmission_target_track_event(MSACAnalyticsTransmissionTarget *transmission, char* eventName, int flags);
extern "C" void appcenter_unity_transmission_target_set_enabled(MSACAnalyticsTransmissionTarget *transmission, BOOL enabled);
extern "C" BOOL appcenter_unity_transmission_target_is_enabled(MSACAnalyticsTransmissionTarget *transmission);
extern "C" void appcenter_unity_transmission_target_track_event_with_props(MSACAnalyticsTransmissionTarget *transmission, char* eventName, char** keys, char** values, int count, int flags);
extern "C" void appcenter_unity_transmission_target_track_event_with_typed_props(MSACAnalyticsTransmissionTarget *transmission, char* eventName, MSACEventProperties* properties, int flags);
extern "C" MSACAnalyticsTransmissionTarget *appcenter_unity_transmission_transmission_target_for_token(MSACAnalyticsTransmissionTarget *transmissionParent, char* transmissionTargetToken);
extern "C" MSACPropertyConfigurator *appcenter_unity_transmission_get_property_configurator(MSACAnalyticsTransmissionTarget *transmission);
extern "C" void appcenter_unity_transmission_pause(MSACAnalyticsTransmissionTarget *transmission);
extern "C" void appcenter_unity_transmission_resume(MSACAnalyticsTransmissionTarget *transmission);


