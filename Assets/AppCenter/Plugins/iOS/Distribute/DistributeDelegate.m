// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#import "DistributeDelegate.h"
#import <Foundation/Foundation.h>

@interface UnityDistributeDelegate ()

@property MSReleaseDetails *unprocessedDetails;
@property NSObject *lockObject;
@property ReleaseAvailableFunction releaseAvailableHandler;

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

- (BOOL)distribute:(MSDistribute *)distribute releaseAvailableWithDetails:(MSReleaseDetails *)details {
  ReleaseAvailableFunction handlerCopy = nil;
  @synchronized (_lockObject) {
    handlerCopy = _releaseAvailableHandler;
    if (handlerCopy) {
      return handlerCopy(details);
    }
    return YES;
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
