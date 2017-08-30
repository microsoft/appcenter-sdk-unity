// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

extern "C" void* mobile_center_unity_analytics_get_type();
extern "C" void mobile_center_unity_analytics_track_event(char* eventName);
extern "C" void mobile_center_unity_analytics_track_event_with_properties(char* eventName, char** keys, char** values, int count);
extern "C" void mobile_center_unity_analytics_set_enabled(bool isEnabled);
extern "C" bool mobile_center_unity_analytics_is_enabled();
