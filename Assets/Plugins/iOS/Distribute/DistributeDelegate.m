// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "DistributeDelegate.h"
#import <Foundation/Foundation.h>

@interface UnityDistributeDelegate ()

@property MSReleaseDetails *unprocessedDetails;
@property NSObject *lockObject;
@property BOOL shouldUseCustomDialog;
@property ReleaseAvailableFunction releaseAvailableHandler;

@end

@implementation UnityDistributeDelegate

- (instancetype)init {
  if ((self = [super init])) {
    _lockObject = [NSObject new];
    _unprocessedDetails = nil;
    _shouldUseCustomDialog = NO;
  }
  return self;
}

- (void)useCustomDialog {
  @synchronized (_lockObject) {
    _shouldUseCustomDialog = YES;
  }
}

- (void)setReleaseAvailableImplementation:(ReleaseAvailableFunction)implementation {
  @synchronized (_lockObject) {
    _releaseAvailableHandler = implementation;
  }
}

- (BOOL)distribute:(MSDistribute *)distribute releaseAvailableWithDetails:(MSReleaseDetails *)details {
  @synchronized (_lockObject) {
    if (_shouldUseCustomDialog) {
      _unprocessedDetails = details;
    }
    return _shouldUseCustomDialog;
  }
}

- (void) replayReleaseAvailable {
  ReleaseAvailableFunction handlerCopy = nil;
  MSReleaseDetails *unprocessedCopy = _unprocessedDetails;
  @synchronized (_lockObject) {
    unprocessedCopy = nil;
    handlerCopy = _releaseAvailableHandler;
  }
  if (unprocessedCopy && handlerCopy) {
    handlerCopy(unprocessedCopy);
  }
}

@end
