// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <AppCenterCrashes/AppCenterCrashes.h>
#import <Foundation/Foundation.h>

#if __cplusplus
extern "C" {
#endif

void app_center_unity_crashes_set_delegate();
void app_center_unity_crashes_delegate_set_should_process_error_report_delegate(bool(*handler)(MSACErrorReport *));
void app_center_unity_crashes_delegate_set_get_error_attachments_delegate(NSArray<MSACErrorAttachmentLog *> *(*handler)(MSACErrorReport *));
void app_center_unity_crashes_delegate_set_sending_error_report_delegate(void(*handler)(MSACErrorReport *));
void app_center_unity_crashes_delegate_set_sent_error_report_delegate(void(*handler)(MSACErrorReport *));
void app_center_unity_crashes_delegate_set_failed_to_send_error_report_delegate(void(*handler)(MSACErrorReport *, NSError *));

#if __cplusplus
}
#endif

@interface UnityCrashesDelegate : NSObject<MSACCrashesDelegate>
-(BOOL)crashes:(MSACCrashes *)crashes shouldProcessErrorReport:(MSACErrorReport *)errorReport;
- (NSArray<MSACErrorAttachmentLog *> *)attachmentsWithCrashes:(MSACCrashes *)crashes forErrorReport:(MSACErrorReport *)errorReport;
- (void)crashes:(MSACCrashes *)crashes willSendErrorReport:(MSACErrorReport *)errorReport;
- (void)crashes:(MSACCrashes *)crashes didSucceedSendingErrorReport:(MSACErrorReport *)errorReport;
- (void)crashes:(MSACCrashes *)crashes didFailSendingErrorReport:(MSACErrorReport *)errorReport withError:(NSError *)error;
@end
