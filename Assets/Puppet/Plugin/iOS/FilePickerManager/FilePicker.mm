// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "FilePicker.h"

#pragma mark Config

const char* kMSCallbackObjectName = "Attachment";
const char* kMSCallbackMethodSuccess = "onSelectFileSuccessful";
const char* kMSCallbackMethodFailure = "onSelectFileFailure";
const char* kMSFailedPickFile = "Failed to pick the file";
const char* kMSFailedFindFile = "Failed to find the file";

#pragma mark FilePicker

@implementation FilePicker

+ (instancetype)sharedInstance {
    static FilePicker *instance;
    static dispatch_once_t token;
    dispatch_once(&token, ^{
        instance = [[FilePicker alloc] init];
    });
    return instance;
}

- (void)show {
    if (self.pickerController != nil) {
        UnitySendMessage(kMSCallbackObjectName, kMSCallbackMethodFailure, kMSFailedPickFile);
        return;
    }
    UIDocumentPickerViewController *documentPicker = [[UIDocumentPickerViewController alloc] initWithDocumentTypes:@[@"public.data"] inMode:UIDocumentPickerModeImport];
    documentPicker.delegate = self;
    documentPicker.modalPresentationStyle = UIModalPresentationFormSheet;
    UIViewController *unityController = UnityGetGLViewController();
    [unityController presentViewController:documentPicker animated:YES completion:^{}];
}

#pragma mark UIDocumentPickerDelegate

- (void)documentPicker:(UIDocumentPickerViewController *)controller didPickDocumentAtURL:(NSURL *)url {
    if (url == nil) {
        UnitySendMessage(kMSCallbackObjectName, kMSCallbackMethodFailure, kMSFailedFindFile);
        [self removePicker];
        return;
    }
    UnitySendMessage(kMSCallbackObjectName, kMSCallbackMethodSuccess, [url.path UTF8String]);
    [self removePicker];
}


- (void)documentPickerWasCancelled:(UIDocumentPickerViewController *)picker {
    UnitySendMessage(kMSCallbackObjectName, kMSCallbackMethodFailure, kMSFailedPickFile);
    [self removePicker];
}

- (void)removePicker {
    if (self.pickerController != nil) {
        [self.pickerController dismissViewControllerAnimated:YES completion:^{
            self.pickerController = nil;
        }];
    }
}

@end

#pragma mark Unity Plugin

extern "C" {
    void CustomFilePicker_show() {
        FilePicker *picker = [FilePicker sharedInstance];
        [picker show];
    }
}
