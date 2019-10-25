// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "FilePicker.h"
#import <Photos/Photos.h>

#pragma mark Config

const char* kMSCallbackObjectName = "Attachment";
const char* kMSCallbackMethodSuccess = "onSelectFileSuccessful";
const char* kMSCallbackMethodFailure = "onSelectFileFailure";
const char* kMSImageNotPicked = "Image not picked";
const char* kMSImageNotFound = "Image not found";

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
    UIImagePickerController *imagePicker = [[UIImagePickerController alloc] init];
    imagePicker.delegate = self;
    imagePicker.allowsEditing = NO;
    imagePicker.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    UIViewController *unityController = UnityGetGLViewController();
    [unityController presentViewController:imagePicker animated:YES completion:nil];
}

#pragma mark UIImagePickerControllerDelegate

-(void)imagePickerController:(UIImagePickerController *)imagePicker didFinishPickingMediaWithInfo:(NSDictionary *)info {
    NSURL *imageUrl = [info objectForKey:UIImagePickerControllerReferenceURL];
    if (imageUrl == nil) {
        UnitySendMessage(kMSCallbackObjectName, kMSCallbackMethodFailure, kMSImageNotFound);
        [imagePicker dismissViewControllerAnimated:YES completion:nil];
    }
    UnitySendMessage(kMSCallbackObjectName, kMSCallbackMethodSuccess, [imageUrl.absoluteString UTF8String]);
    [imagePicker dismissViewControllerAnimated:YES completion:nil];
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)imagePicker {
    UnitySendMessage(kMSCallbackObjectName, kMSCallbackMethodFailure, kMSImageNotPicked);
    [imagePicker dismissViewControllerAnimated:YES completion:nil];
}
@end

#pragma mark Unity Plugin

extern "C" {
    void CustomFilePicker_show() {
        FilePicker *picker = [FilePicker sharedInstance];
        [picker show];
    }

    int GetFileBytes(const char* url, unsigned char* dataPtr) {
        NSString* urlString = [NSString stringWithUTF8String:url];
        NSURL* imageUrl = [NSURL URLWithString:urlString];
        PHAsset *asset = [[PHAsset fetchAssetsWithALAssetURLs:@[imageUrl] options:nil] lastObject];
        __block int result = 0;
        __block unsigned char* data;
        if (asset) {
            PHImageRequestOptions *options = [[PHImageRequestOptions alloc] init];
            options.synchronous = YES;
            options.networkAccessAllowed = YES;
            [[PHImageManager defaultManager]
                requestImageDataForAsset:asset
                                options:options
                            resultHandler:^(NSData *_Nullable imageData, NSString *_Nullable dataUTI, __unused UIImageOrientation orientation,
                                            __unused NSDictionary *_Nullable info) {
                            NSString *myString = [[NSString alloc] initWithData:imageData encoding:NSUTF8StringEncoding];
                            /*unsigned char* imageBytes = (unsigned char*)[imageData bytes];
                            data = (unsigned char*)malloc(imageData.length);
                            for (int i = 0; i < imageData.length; i++)
                                data[i] = imageBytes[i];
                            result = (int)imageData.length;*/
                            }];
            //*dataPtr = *data;
            }
        return result;
    }
}
