//
//  unityMCtest.h
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#ifndef NS_NUMBER_HELPER_H
#define NS_NUMBER_HELPER_H

#import <Foundation/Foundation.h>

extern "C" void* mobile_center_unity_nsnumber_convert_int(int val);
extern "C" NSNumber* mobile_center_unity_nsnumber_convert_long(long val);
extern "C" NSNumber* mobile_center_unity_nsnumber_convert_float(float val);
extern "C" NSNumber* mobile_center_unity_nsnumber_convert_double(double val);

#endif
