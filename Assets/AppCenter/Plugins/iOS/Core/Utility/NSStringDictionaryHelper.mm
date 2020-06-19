// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "NSStringDictionaryHelper.h"
#import "NSStringHelper.h"
#import <Foundation/Foundation.h>

const char *appcenter_unity_ns_string_dictionary_get_key_at_idx(NSDictionary *dictionary, int idx) {
  id sortKeys = ^(NSString *key1, NSString *key2) {
    return [key1 compare:key2];
  };
  NSArray *keys = [[dictionary allKeys] sortedArrayUsingComparator:sortKeys];
  return appcenter_unity_ns_string_to_cstr(keys[idx]);
}

const char *appcenter_unity_ns_string_dictionary_get_value_for_key(NSDictionary *dictionary, char *key) {
  NSString *keyString = [NSString stringWithUTF8String:key];
  return appcenter_unity_ns_string_to_cstr([dictionary objectForKey:keyString]);
}

NSDictionary *appcenter_unity_create_ns_string_dictionary(char **keys, char **values, int count) {
  if (count == 0) {
    return nil;
  }

  // Convert the two arrays to a single dictionary
  NSMutableDictionary<NSString *, NSString *> *nsdictionary = [[NSMutableDictionary alloc] initWithCapacity:count];
  for (int i = 0; i < count; ++i) {
    NSString *key = appcenter_unity_cstr_to_ns_string(keys[i]);
    NSString *value = appcenter_unity_cstr_to_ns_string(values[i]);
    [nsdictionary setValue:value forKey:key];
  }
  return nsdictionary;
}

size_t appcenter_unity_ns_dictionary_get_length(NSDictionary *dictionary) { return [dictionary count]; }
