// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "DistributeDelegate.h"
#import <Foundation/Foundation.h>

@interface UnityDistributeDelegate ()

@property MSACReleaseDetails *unprocessedDetails;
@property NSObject *lockObject;
@property ReleaseAvailableFunction releaseAvailableHandler;
@property WillExitAppFunction willExitAppHandler;
@property NoReleaseAvailableFunction noReleaseAvailableHandler;

@end

@implementation UnityDistributeDelegate

+ (instancetype)sharedInstance {
  static UnityDistributeDelegate *sharedInstance = nil;
  static dispatch_once_t onceToken;
  dispatch_once(&onceToken, ^{
    if (sharedInstance == nil) {
      sharedInstance = [[self alloc] init];
    }
  });
  return sharedInstance;
}

- (instancetype)init {
  if ((self = [super init])) {
    _lockObject = [NSObject new];
    _unprocessedDetails = nil;
  }
  return self;
}

- (void)setReleaseAvailableImplementation:(ReleaseAvailableFunction)implementation {
  @synchronized (_lockObject) {
    _releaseAvailableHandler = implementation;
  }
}

- (void)setWillExitAppImplementation:(WillExitAppFunction)implementation {
  @synchronized (_lockObject) {
    _willExitAppHandler = implementation;
  }
}

- (void)setNoReleaseAvailableImplementation:(NoReleaseAvailableFunction)implementation {
  @synchronized (_lockObject) {
    _noReleaseAvailableHandler = implementation;
  }
}

- (BOOL)distribute:(MSACDistribute *)distribute releaseAvailableWithDetails:(MSACReleaseDetails *)details {
  ReleaseAvailableFunction handlerCopy = nil;
  @synchronized (_lockObject) {
    handlerCopy = _releaseAvailableHandler;
    if (handlerCopy) {
      return handlerCopy(details);
    }
    return YES;
  }
}

- (void) distributeWillExitApp:(MSACDistribute *)distribute {
  WillExitAppFunction handlerCopy = nil;
  @synchronized (_lockObject) {
    handlerCopy = _willExitAppHandler;
  }
  if (handlerCopy) {
    handlerCopy();
  }
}

- (void)replayReleaseAvailable {
  ReleaseAvailableFunction handlerCopy = nil;
  MSACReleaseDetails *unprocessedCopy = _unprocessedDetails;
  @synchronized (_lockObject) {
    unprocessedCopy = nil;
    handlerCopy = _releaseAvailableHandler;
  }
  if (unprocessedCopy && handlerCopy) {
    handlerCopy(unprocessedCopy);
  }
}

- (void)distributeNoReleaseAvailable:(MSACDistribute *)distribute {
  NoReleaseAvailableFunction handlerCopy = nil;
  @synchronized (_lockObject) {
    handlerCopy = _noReleaseAvailableHandler;
    if (handlerCopy) {
      handlerCopy();
    }
    return;
  }
}

@end
