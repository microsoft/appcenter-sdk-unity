// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

@class MSPropertyConfigurator;

extern "C" void appcenter_unity_property_configurator_set_app_name(MSPropertyConfigurator *configurator, char* appName);
extern "C" void appcenter_unity_property_configurator_set_app_version(MSPropertyConfigurator *configurator, char* appVersion);
extern "C" void appcenter_unity_property_configurator_set_app_locale(MSPropertyConfigurator *configurator, char* appLocale);
extern "C" void appcenter_unity_property_configurator_collect_device_id(MSPropertyConfigurator *configurator);
extern "C" void appcenter_unity_property_configurator_set_event_property(MSPropertyConfigurator *configurator, char* key, char* value);
extern "C" void appcenter_unity_property_configurator_remove_event_property(MSPropertyConfigurator *configurator, char* key);