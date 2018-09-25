// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "WrapperPropertyConfigurator.h"
#import "AppCenterAnalytics/MSPropertyConfigurator.h"
#import "../Core/Utility/NSStringDictionaryHelper.h"

extern "C" void appcenter_unity_property_configurator_set_app_name(MSPropertyConfigurator *configurator, char* appName) {
  return [configurator setAppName: [NSString stringWithUTF8String:appName]];
}

extern "C" void appcenter_unity_property_configurator_set_app_version(MSPropertyConfigurator *configurator, char* appVersion) {
  return [configurator setAppVersion: [NSString stringWithUTF8String:appVersion]];
}

extern "C" void appcenter_unity_property_configurator_set_app_locale(MSPropertyConfigurator *configurator, char* appLocale) {
  return [configurator setAppLocale: [NSString stringWithUTF8String:appLocale]];
}

extern "C" void appcenter_unity_property_configurator_set_event_property(MSPropertyConfigurator *configurator, char* key, char* value) {
  return [configurator setEventPropertyString: [NSString stringWithUTF8String:value] forKey: [NSString stringWithUTF8String:key]];
}

extern "C" void appcenter_unity_property_configurator_remove_event_property(MSPropertyConfigurator *configurator, char* key) {
  return [configurator removeEventPropertyForKey: [NSString stringWithUTF8String:key]];
}
