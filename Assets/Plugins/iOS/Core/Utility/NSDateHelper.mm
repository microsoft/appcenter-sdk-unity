//
//  unityMCtest.m
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "NSDateHelper.h"
#import <Foundation/Foundation.h>

NSDate* mobile_center_unity_ns_date_convert(char* format, char* dateString)
{
  NSString *formatNSStr = [NSString stringWithUTF8String:format];
  NSString *dateNSStr = [NSString stringWithUTF8String:dateString];
  NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
  [formatter setDateFormat:formatNSStr];
  return [formatter dateFromString:dateNSStr];
}
