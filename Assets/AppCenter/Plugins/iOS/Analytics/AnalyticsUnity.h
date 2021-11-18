// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
@class MSACAnalyticsTransmissionTarget;
@class MSACEventProperties;

extern "C" void* appcenter_unity_analytics_get_type();
extern "C" void appcenter_unity_analytics_track_event(char* eventName, int flags);
extern "C" void appcenter_unity_analytics_track_event_with_properties(char* eventName, char** keys, char** values, int count, int flags);
extern "C" void appcenter_unity_analytics_track_event_with_typed_properties(char* eventName, MSACEventProperties* properties, int flags);
extern "C" void appcenter_unity_analytics_set_enabled(bool isEnabled);
extern "C" bool appcenter_unity_analytics_is_enabled();
extern "C" MSACAnalyticsTransmissionTarget *appcenter_unity_analytics_transmission_target_for_token(char* transmissionTargetToken);
extern "C" void appcenter_unity_analytics_pause();
extern "C" void appcenter_unity_analytics_resume();
extern "C" void appcenter_unity_analytics_enable_manual_session_tracker();
extern "C" void appcenter_unity_analytics_start_session();
