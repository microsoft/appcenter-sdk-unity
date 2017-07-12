// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "DistributeDelegate.h"
#import <MobileCenterDistribute/MobileCenterDistribute.h>
#import <Foundation/Foundation.h>

static ReleaseAvailableWithDetailsFunction *releaseAvailable;
static UnityDistributeDelegate *del;

void mobile_center_unity_distribute_delegate_provide_release_available_impl(ReleaseAvailableWithDetailsFunction* functionPtr)
{
  releaseAvailable = functionPtr;
}

void mobile_center_unity_distribute_set_delegate()
{
  del = [[UnityDistributeDelegate alloc] init];
  [MSDistribute setDelegate:del];
}

@implementation UnityDistributeDelegate

- (BOOL)distribute:(MSDistribute *)distribute releaseAvailableWithDetails:(MSReleaseDetails *)details
{
  return (*releaseAvailable)(details);
}

@end
