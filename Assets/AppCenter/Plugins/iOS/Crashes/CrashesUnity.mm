// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <Foundation/Foundation.h>
#import <AppCenter/MSACAppCenter.h>
#import <AppCenterCrashes/MSACErrorAttachmentLog.h>
#import <AppCenterCrashes/MSACWrapperCrashesHelper.h>
#import "../Core/Utility/NSStringDictionaryHelper.h"
#import "../Core/Utility/NSStringHelper.h"
#import "CrashesUnity.h"
#import "CrashesDelegate.h"
#import "NSStringHelper.h"

void* appcenter_unity_crashes_get_type()
{
  return (void *)CFBridgingRetain([MSACCrashes class]);
}

void* appcenter_unity_crashes_track_exception_with_properties_with_attachments(MSACExceptionModel* exception, char** propertyKeys, char** propertyValues, int propertyCount, NSArray<MSACErrorAttachmentLog*>* attachments)
{
  NSDictionary<NSString*, NSString*> *properties = appcenter_unity_create_ns_string_dictionary(propertyKeys, propertyValues, propertyCount);
  NSString *errorId = [MSACCrashes trackException:exception withProperties:properties attachments:attachments];
  return (void *)appcenter_unity_ns_string_to_cstr(errorId);
}

void appcenter_unity_crashes_set_enabled(bool isEnabled)
{
  [MSACCrashes setEnabled:isEnabled];
}

bool appcenter_unity_crashes_is_enabled()
{
  return [MSACCrashes isEnabled];
}

bool appcenter_unity_crashes_has_received_memory_warning_in_last_session() 
{
  return [MSACCrashes hasReceivedMemoryWarningInLastSession];
}

void appcenter_unity_crashes_generate_test_crash()
{
  [MSACCrashes generateTestCrash];
}

bool appcenter_unity_crashes_has_crashed_in_last_session()
{
  return [MSACCrashes hasCrashedInLastSession];
}

void appcenter_unity_crashes_disable_mach_exception_handler()
{
  [MSACCrashes disableMachExceptionHandler];
}

void appcenter_unity_crashes_set_user_confirmation_handler(bool(* userConfirmationHandler)())
{
    [MSACCrashes setUserConfirmationHandler:^BOOL(NSArray<MSACErrorReport *> *_Nonnull errorReports){
        return userConfirmationHandler();
    }];
}

void appcenter_unity_crashes_notify_with_user_confirmation(int userConfirmation)
{
    [MSACCrashes notifyWithUserConfirmation:(MSACUserConfirmation)userConfirmation];
}

void* appcenter_unity_crashes_last_session_crash_report()
{
    return (void *)CFBridgingRetain([MSACCrashes lastSessionCrashReport]);
}

void appcenter_unity_start_crashes()
{
    [MSACAppCenter startService:MSACCrashes.class];
}

void* app_center_unity_crashes_create_error_attachments_array(int capacity)
{
    NSMutableArray<MSACErrorAttachmentLog *>* errorAttachments = [[NSMutableArray alloc] initWithCapacity:capacity];
    return (void *)CFBridgingRetain(errorAttachments);
}

void* app_center_unity_crashes_create_error_attachment_log_text(char* text, char* fileName)
{
    return (void *)CFBridgingRetain(
                                    [[MSACErrorAttachmentLog alloc] initWithFilename:appcenter_unity_cstr_to_ns_string(fileName) attachmentText:appcenter_unity_cstr_to_ns_string(text)]);
}

void* app_center_unity_crashes_create_error_attachment_log_binary(const void* data, int size, char* fileName, char* contentType)
{
    return (void *)CFBridgingRetain(
                                    [[MSACErrorAttachmentLog alloc] initWithFilename:appcenter_unity_cstr_to_ns_string(fileName) attachmentBinary:[[NSData alloc] initWithBytes:data length:size] contentType:appcenter_unity_cstr_to_ns_string(contentType)]);
}

void appcenter_unity_crashes_add_error_attachment(NSMutableArray<MSACErrorAttachmentLog*>* attachments, MSACErrorAttachmentLog* attachment)
{
    [attachments addObject:attachment];
}

void appcenter_unity_crashes_send_error_attachments(char* errorReportId, NSMutableArray<MSACErrorAttachmentLog*>* attachments)
{
  NSString *errorReportIdConverted = appcenter_unity_cstr_to_ns_string(errorReportId);
  [MSACWrapperCrashesHelper sendErrorAttachments:attachments withIncidentIdentifier:errorReportIdConverted];
}

void* appcenter_unity_crashes_build_handled_error_report(char* errorReportId)
{
  NSString *errorReportIdConverted = appcenter_unity_cstr_to_ns_string(errorReportId);
  MSACErrorReport *report = [MSACWrapperCrashesHelper buildHandledErrorReportWithErrorID:errorReportIdConverted];
  return (void *)CFBridgingRetain(report);
}
