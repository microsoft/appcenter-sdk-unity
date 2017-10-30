// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "NSDateHelper.h"
#import <Foundation/Foundation.h>

NSDate* appcenter_unity_ns_date_convert(char* format, char* dateString)
{
  NSString *formatNSStr = [NSString stringWithUTF8String:format];
  NSString *dateNSStr = [NSString stringWithUTF8String:dateString];
  NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
  [formatter setDateFormat:formatNSStr];
  return [formatter dateFromString:dateNSStr];
}
