// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

@class MSACEventProperties;

extern "C" void* appcenter_unity_analytics_create_event_properties();
extern "C" void appcenter_unity_analytics_event_properties_set_string(MSACEventProperties *properties, char* key, char* value);
extern "C" void appcenter_unity_analytics_event_properties_set_long(MSACEventProperties *properties, char* key, long value);
extern "C" void appcenter_unity_analytics_event_properties_set_double(MSACEventProperties *properties, char* key, double value);
extern "C" void appcenter_unity_analytics_event_properties_set_bool(MSACEventProperties *properties, char* key, BOOL value);
extern "C" void appcenter_unity_analytics_event_properties_set_date(MSACEventProperties *properties, char* key, NSDate* value);
