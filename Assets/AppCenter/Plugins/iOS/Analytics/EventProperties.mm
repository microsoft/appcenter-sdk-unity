// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "EventProperties.h"
#import "AppCenterAnalytics/MSEventProperties.h"
#import "../Core/Utility/NSDateHelper.h"

void* appcenter_unity_analytics_create_event_properties() {
    return (void *)CFBridgingRetain([[MSEventProperties alloc] init]);
}

void appcenter_unity_analytics_event_properties_set_string(MSEventProperties *properties, char* key, char* val) {
    [properties setString:[NSString stringWithUTF8String:val] forKey:[NSString stringWithUTF8String:key]];
}

void appcenter_unity_analytics_event_properties_set_long(MSEventProperties *properties, char* key, long val) {
    [properties setInt64:val forKey:[NSString stringWithUTF8String:key]];
}

void appcenter_unity_analytics_event_properties_set_double(MSEventProperties *properties, char* key, double val) {
    [properties setDouble:val forKey:[NSString stringWithUTF8String:key]];
}

void appcenter_unity_analytics_event_properties_set_bool(MSEventProperties *properties, char* key, BOOL val) {
    [properties setBool:val forKey:[NSString stringWithUTF8String:key]];
}

void appcenter_unity_analytics_event_properties_set_date(MSEventProperties *properties, char* key, NSDate* val) {
    [properties setDate:val forKey:[NSString stringWithUTF8String:key]];
}
