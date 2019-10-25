// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import <UIKit/UIKit.h>

@interface FilePicker : NSObject<UIImagePickerControllerDelegate, UINavigationControllerDelegate>
+ (instancetype)sharedInstance;
- (void)show;
@end
