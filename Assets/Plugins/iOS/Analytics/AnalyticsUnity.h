//
//  unityMCtest.h
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

extern "C" void* mobile_center_unity_analytics_get_type();

extern "C" void mobile_center_unity_analytics_track_event(char* eventName);

extern "C" void mobile_center_unity_analytics_track_event_with_properties(char* eventName, char** keys, char** values, int count);

extern "C" void mobile_center_unity_analytics_set_enabled(bool isEnabled);

extern "C" bool mobile_center_unity_analytics_is_enabled();
