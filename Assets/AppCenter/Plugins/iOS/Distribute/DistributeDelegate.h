// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#ifndef DISTRIBUTE_DELEGATE_H
#define DISTRIBUTE_DELEGATE_H

#import <AppCenterDistribute/AppCenterDistribute.h>

typedef bool (__cdecl *ReleaseAvailableFunction)(MSACReleaseDetails*);

@interface UnityDistributeDelegate : NSObject<MSACDistributeDelegate>

- (BOOL)distribute:(MSACDistribute *)distribute releaseAvailableWithDetails:(MSACReleaseDetails *)details;
- (void)noReleaseAvailable;
- (void)setReleaseAvailableImplementation:(ReleaseAvailableFunction)implementation;
- (void)replayReleaseAvailable;
+ (instancetype) sharedInstance;

@end

#endif
