// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "CrashesDelegate.h"
#import <AppCenterCrashes/AppCenterCrashes.h>
#import <Foundation/Foundation.h>

static bool (*shouldProcessErrorReport)(MSACErrorReport *);
static NSArray<MSACErrorAttachmentLog *>* (*getErrorAttachments)(MSACErrorReport *);
static void (*sendingErrorReport)(MSACErrorReport *);
static void (*sentErrorReport)(MSACErrorReport *);
static void (*failedToSendErrorReport)(MSACErrorReport *, NSError *);

// we need static instance var because we have weak reaf in native part
static UnityCrashesDelegate *unityCrashesDelegate = NULL;

void app_center_unity_crashes_set_delegate()
{
    unityCrashesDelegate = [[UnityCrashesDelegate alloc] init];
    [MSACCrashes setDelegate:unityCrashesDelegate];
}

void app_center_unity_crashes_delegate_set_should_process_error_report_delegate(bool(*handler)(MSACErrorReport *))
{
    shouldProcessErrorReport = handler;
}

void app_center_unity_crashes_delegate_set_get_error_attachments_delegate(NSArray<MSACErrorAttachmentLog *> *(*handler)(MSACErrorReport *))
{
    getErrorAttachments = handler;
}

void app_center_unity_crashes_delegate_set_sending_error_report_delegate(void(*handler)(MSACErrorReport *))
{
    sendingErrorReport = handler;
}

void app_center_unity_crashes_delegate_set_sent_error_report_delegate(void(*handler)(MSACErrorReport *))
{
    sentErrorReport = handler;
}

void app_center_unity_crashes_delegate_set_failed_to_send_error_report_delegate(void(*handler)(MSACErrorReport *, NSError *))
{
    failedToSendErrorReport = handler;
}

@implementation UnityCrashesDelegate

-(BOOL)crashes:(MSACCrashes *)crashes shouldProcessErrorReport:(MSACErrorReport *)errorReport
{
    if (shouldProcessErrorReport)
    {
        return (*shouldProcessErrorReport)(errorReport);
    }
    else
    {
        return true;
    }
}

- (NSArray<MSACErrorAttachmentLog *> *)attachmentsWithCrashes:(MSACCrashes *)crashes forErrorReport:(MSACErrorReport *)errorReport
{
    if (getErrorAttachments)
    {
        return (*getErrorAttachments)(errorReport);
    }
    else
    {
        return nil;
    }
}

- (void)crashes:(MSACCrashes *)crashes willSendErrorReport:(MSACErrorReport *)errorReport
{
    if (sendingErrorReport)
    {
        (*sendingErrorReport)(errorReport);
    }
}

- (void)crashes:(MSACCrashes *)crashes didSucceedSendingErrorReport:(MSACErrorReport *)errorReport
{
    if (sentErrorReport)
    {
        (*sentErrorReport)(errorReport);
    }
}

- (void)crashes:(MSACCrashes *)crashes didFailSendingErrorReport:(MSACErrorReport *)errorReport withError:(NSError *)error
{
    if (failedToSendErrorReport)
    {
        (*failedToSendErrorReport)(errorReport, error);
    }
}

@end
