// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "NSNumberHelper.h"
#import <Foundation/Foundation.h>

void* mobile_center_unity_nsnumber_convert_int(int val)
{
  return (__bridge void*)[NSNumber numberWithInt:val];
}

NSNumber* mobile_center_unity_nsnumber_convert_long(long val)
{
  return [NSNumber numberWithLong:val];
}

NSNumber* mobile_center_unity_nsnumber_convert_float(float val)
{
  return [NSNumber numberWithFloat:val];
}

NSNumber* mobile_center_unity_nsnumber_convert_double(double val)
{
  return [NSNumber numberWithDouble:val];
}
