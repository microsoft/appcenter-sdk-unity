//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "CustomProperties.h"
#import <MobileCenter/MobileCenter.h>
#import <Foundation/Foundation.h>

MSCustomProperties* mobile_center_unity_custom_properties_create()
{
  return [[MSCustomProperties alloc] init];
}

void mobile_center_unity_custom_properties_set_string(MSCustomProperties* properties, char* key, char* val)
{
  [properties setString:[NSString stringWithUTF8String:val] forKey:[NSString stringWithUTF8String:key]];
}

void mobile_center_unity_custom_properties_set_number(MSCustomProperties* properties, char* key, NSNumber* val)
{
  [properties setNumber:val forKey:[NSString stringWithUTF8String:key]];
}

void mobile_center_unity_custom_properties_set_bool(MSCustomProperties* properties, char* key, bool val)
{
  [properties setBool:val forKey:[NSString stringWithUTF8String:key]];
}

void mobile_center_unity_custom_properties_set_date(MSCustomProperties* properties, char* key, NSDate* val)
{
  [properties setDate:val forKey:[NSString stringWithUTF8String:key]];
}

void mobile_center_unity_custom_properties_clear(MSCustomProperties* properties, char* key)
{
  [properties clearPropertyForKey:[NSString stringWithUTF8String:key]];
}
