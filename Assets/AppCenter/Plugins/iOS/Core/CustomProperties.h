// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <AppCenter/AppCenter.h>
#import <Foundation/Foundation.h>

// Don't need to return value because reference is kept by wrapper
extern "C" MSACCustomProperties* appcenter_unity_custom_properties_create();
extern "C" void appcenter_unity_custom_properties_set_string(MSACCustomProperties* properties, char* key, char* val);
extern "C" void appcenter_unity_custom_properties_set_number(MSACCustomProperties* properties, char* key, NSNumber* val);
extern "C" void appcenter_unity_custom_properties_set_bool(MSACCustomProperties* properties, char* key, bool val);
extern "C" void appcenter_unity_custom_properties_set_date(MSACCustomProperties* properties, char* key, NSDate* val);
extern "C" void appcenter_unity_custom_properties_clear(MSACCustomProperties* properties, char* key);
