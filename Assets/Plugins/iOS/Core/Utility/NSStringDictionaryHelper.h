//
//  unityMCtest.h
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#ifndef NS_STRING_DICTIONARY_HELPER_H
#define NS_STRING_DICTIONARY_HELPER_H

#import <Foundation/Foundation.h>

extern "C" char* mobile_center_unity_ns_string_dictionary_get_key_at_idx(NSDictionary *dictionary, int idx);

extern "C" char* mobile_center_unity_ns_string_dictionary_get_value_for_key(NSDictionary *dictionary, char* key);

extern "C" size_t mobile_center_unity_ns_dictionary_get_length(NSDictionary *dictionary);

extern "C" NSDictionary* mobile_center_unity_create_ns_string_dictionary(char** keys, char** values, int count);

#endif
