// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.
@class MSAnalyticsTransmissionTarget;
@class MSEventProperties;

extern "C" void* appcenter_unity_analytics_get_type();
extern "C" void appcenter_unity_analytics_track_event(char* eventName);
extern "C" void appcenter_unity_analytics_track_event_with_properties(char* eventName, char** keys, char** values, int count);
extern "C" void appcenter_unity_analytics_track_event_with_typed_properties(char* eventName, MSEventProperties* properties);
extern "C" void appcenter_unity_analytics_set_enabled(bool isEnabled);
extern "C" bool appcenter_unity_analytics_is_enabled();
extern "C" MSAnalyticsTransmissionTarget *appcenter_unity_analytics_transmission_target_for_token(char* transmissionTargetToken);
extern "C" void appcenter_unity_analytics_pause();
extern "C" void appcenter_unity_analytics_resume();
