// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

@class MSACPropertyConfigurator;

extern "C" void appcenter_unity_property_configurator_set_app_name(MSACPropertyConfigurator *configurator, char* appName);
extern "C" void appcenter_unity_property_configurator_set_user_id(MSACPropertyConfigurator *configurator, char* userId);
extern "C" void appcenter_unity_property_configurator_set_app_version(MSACPropertyConfigurator *configurator, char* appVersion);
extern "C" void appcenter_unity_property_configurator_set_app_locale(MSACPropertyConfigurator *configurator, char* appLocale);
extern "C" void appcenter_unity_property_configurator_collect_device_id(MSACPropertyConfigurator *configurator);
extern "C" void appcenter_unity_property_configurator_set_event_property(MSACPropertyConfigurator *configurator, char* key, char* value);
extern "C" void appcenter_unity_property_configurator_set_event_datetime_property(MSACPropertyConfigurator *configurator, char* key, NSDate* value);
extern "C" void appcenter_unity_property_configurator_set_event_long_property(MSACPropertyConfigurator *configurator, char* key, long value);
extern "C" void appcenter_unity_property_configurator_set_event_double_property(MSACPropertyConfigurator *configurator, char* key, double value);
extern "C" void appcenter_unity_property_configurator_set_event_bool_property(MSACPropertyConfigurator *configurator, char* key, bool value);
extern "C" void appcenter_unity_property_configurator_remove_event_property(MSACPropertyConfigurator *configurator, char* key);
