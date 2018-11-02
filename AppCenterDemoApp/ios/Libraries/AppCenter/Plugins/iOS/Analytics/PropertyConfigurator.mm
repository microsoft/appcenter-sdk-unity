// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "PropertyConfigurator.h"
#import "AppCenterAnalytics/MSPropertyConfigurator.h"
#import "../Core/Utility/NSStringDictionaryHelper.h"

void appcenter_unity_property_configurator_set_app_name(MSPropertyConfigurator *configurator, char* appName) {
  [configurator setAppName: [NSString stringWithUTF8String:appName]];
}

void appcenter_unity_property_configurator_set_app_version(MSPropertyConfigurator *configurator, char* appVersion) {
  [configurator setAppVersion: [NSString stringWithUTF8String:appVersion]];
}

void appcenter_unity_property_configurator_set_app_locale(MSPropertyConfigurator *configurator, char* appLocale) {
  [configurator setAppLocale: [NSString stringWithUTF8String:appLocale]];
}

void appcenter_unity_property_configurator_clear_app_name(MSPropertyConfigurator *configurator) {
  [configurator setAppName: nil];
}

void appcenter_unity_property_configurator_clear_app_version(MSPropertyConfigurator *configurator) {
  [configurator setAppVersion: nil];
}

void appcenter_unity_property_configurator_clear_app_locale(MSPropertyConfigurator *configurator) {
  [configurator setAppLocale: nil];
}

void appcenter_unity_property_configurator_collect_device_id(MSPropertyConfigurator *configurator) {
  [configurator collectDeviceId];
}

void appcenter_unity_property_configurator_set_event_property(MSPropertyConfigurator *configurator, char* key, char* value) {
  [configurator setEventPropertyString: [NSString stringWithUTF8String:value] forKey: [NSString stringWithUTF8String:key]];
}

void appcenter_unity_property_configurator_set_event_long_property(MSPropertyConfigurator *configurator, char* key, long value) {
  //[configurator setEventPropertyInt64:value forKey: [NSString stringWithUTF8String:key]];
}

void appcenter_unity_property_configurator_set_event_double_property(MSPropertyConfigurator *configurator, char* key, double value) {
  //[configurator setEventPropertyDouble:value forKey: [NSString stringWithUTF8String:key]];
}

void appcenter_unity_property_configurator_set_event_bool_property(MSPropertyConfigurator *configurator, char* key, bool value) {
  //[configurator setEventPropertyBool:value forKey: [NSString stringWithUTF8String:key]];
}

void appcenter_unity_property_configurator_set_event_datetime_property(MSPropertyConfigurator *configurator, char* key, NSDate* value) {
  //[configurator setEventPropertyDate:value forKey: [NSString stringWithUTF8String:key]];
}

void appcenter_unity_property_configurator_remove_event_property(MSPropertyConfigurator *configurator, char* key) {
  [configurator removeEventPropertyForKey: [NSString stringWithUTF8String:key]];
}
