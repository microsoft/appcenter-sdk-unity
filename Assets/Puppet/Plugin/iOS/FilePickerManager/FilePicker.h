// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <UIKit/UIKit.h>

@interface FilePicker : NSObject<UIDocumentPickerDelegate>

@property(nonatomic) UIDocumentPickerViewController* pickerController;

+ (instancetype)sharedInstance;

- (void)show;

@end
