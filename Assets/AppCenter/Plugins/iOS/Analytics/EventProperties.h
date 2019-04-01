// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

@class MSEventProperties;

extern "C" void* appcenter_unity_analytics_create_event_properties();
extern "C" void appcenter_unity_analytics_event_properties_set_string(MSEventProperties *properties, char* key, char* value);
extern "C" void appcenter_unity_analytics_event_properties_set_long(MSEventProperties *properties, char* key, long value);
extern "C" void appcenter_unity_analytics_event_properties_set_double(MSEventProperties *properties, char* key, double value);
extern "C" void appcenter_unity_analytics_event_properties_set_bool(MSEventProperties *properties, char* key, BOOL value);
extern "C" void appcenter_unity_analytics_event_properties_set_date(MSEventProperties *properties, char* key, NSDate* value);
