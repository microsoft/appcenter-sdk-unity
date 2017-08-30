// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#ifndef DISTRIBUTE_DELEGATE_H
#define DISTRIBUTE_DELEGATE_H

#import <MobileCenterDistribute/MobileCenterDistribute.h>

typedef bool (__cdecl *ReleaseAvailableFunction)(MSReleaseDetails*);

@interface UnityDistributeDelegate : NSObject<MSDistributeDelegate>

- (BOOL)distribute:(MSDistribute *)distribute releaseAvailableWithDetails:(MSReleaseDetails *)details;
- (void)useCustomDialog;
- (void)setReleaseAvailableImplementation:(ReleaseAvailableFunction)implementation;
- (void)replayReleaseAvailable;

@end

#endif
