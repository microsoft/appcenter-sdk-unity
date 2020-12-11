// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#ifndef DISTRIBUTE_DELEGATE_H
#define DISTRIBUTE_DELEGATE_H

#import <AppCenterDistribute/AppCenterDistribute.h>

typedef bool (__cdecl *ReleaseAvailableFunction)(MSACReleaseDetails*);
typedef void (__cdecl *WillExitAppFunction)();

@interface UnityDistributeDelegate : NSObject<MSACDistributeDelegate>

- (BOOL)distribute:(MSACDistribute *)distribute releaseAvailableWithDetails:(MSACReleaseDetails *)details;
- (void)distributeWillExitApp:(MSACDistribute *)distribute;
- (void)setReleaseAvailableImplementation:(ReleaseAvailableFunction)implementation;
- (void)setWillExitAppImplementation:(WillExitAppFunction)implementation;
- (void)replayReleaseAvailable;
+ (instancetype) sharedInstance;

@end

#endif
