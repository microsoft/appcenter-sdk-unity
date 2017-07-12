//
//  CustomProperties.h
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import <MobileCenter/MobileCenter.h>
#import <Foundation/Foundation.h>

// Don't need to return value because reference is kept by wrapper
extern "C" MSCustomProperties* mobile_center_unity_custom_properties_create();
extern "C" void mobile_center_unity_custom_properties_set_string(MSCustomProperties* properties, char* key, char* val);
extern "C" void mobile_center_unity_custom_properties_set_number(MSCustomProperties* properties, char* key, NSNumber* val);
extern "C" void mobile_center_unity_custom_properties_set_bool(MSCustomProperties* properties, char* key, bool val);
extern "C" void mobile_center_unity_custom_properties_set_date(MSCustomProperties* properties, char* key, NSDate* val);
extern "C" void mobile_center_unity_custom_properties_clear(MSCustomProperties* properties, char* key);
