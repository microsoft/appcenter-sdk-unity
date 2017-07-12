//
//  unityMCtest.h
//  unityMCtest
//
//  Created by Alexander Chocron on 6/9/17.
//  Copyright © 2017 Alexander Chocron. All rights reserved.
//

#import "ErrorReport.h"
#import <MobileCenterCrashes/MobileCenterCrashes.h>

char* get_cstring(NSString* nsstring);

char* mobile_center_unity_crashes_error_report_incident_identifier(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return get_cstring([report incidentIdentifier]);
}

char* mobile_center_unity_crashes_error_report_reporter_key(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return get_cstring([report reporterKey]);
}

char* mobile_center_unity_crashes_error_report_signal(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return get_cstring([report signal]);
}

char* mobile_center_unity_crashes_error_report_exception_name(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return get_cstring([report exceptionName]);
}

char* mobile_center_unity_crashes_error_report_exception_reason(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return get_cstring([report exceptionReason]);
}

extern "C" char* mobile_center_unity_crashes_error_report_app_start_time(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return "";
}

extern "C" char* mobile_center_unity_crashes_error_report_app_error_time(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return "";
}

extern "C" void* mobile_center_unity_crashes_error_report_device(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return (__bridge void*)[report device];
}

extern "C" unsigned int mobile_center_unity_crashes_error_report_app_process_identifier(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return [report appProcessIdentifier];
}

extern "C" bool mobile_center_unity_crashes_error_report_is_app_kill(void* errorReport)
{
  MSErrorReport *report = (__bridge MSErrorReport*)errorReport;
  return [report isAppKill];
}

char* get_cstring(NSString* nsstring)
{
  // It seems that with (at least) IL2CPP, when returning a char* that is to be
  // converted to a System.String in C#, the char array is freed - which causes
  // a double-deallocation if ARC also tries to free it. To prevent this, we
  // must return a manually allocated copy of the string returned by "UTF8String"
  size_t cstringLength = [nsstring length] + 1; // +1 for '\0'
  const char *cstring = [nsstring UTF8String];
  char *cstringCopy = (char*)malloc(cstringLength);
  strncpy(cstringCopy, cstring, cstringLength);
  return cstringCopy;
}
