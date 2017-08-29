// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#include "DistributeDelegateSetter.h"
#include "DistributeDelegate.h"

static UnityDistributeDelegate* distributeDelegate;

void mobile_center_unity_distribute_set_delegate(bool useCustomDialog)
{
  distributeDelegate = [UnityDistributeDelegate new];
  if (useCustomDialog) {
      [distributeDelegate useCustomDialog];
  }
  [MSDistribute setDelegate:distributeDelegate];
}

UnityDistributeDelegate* mobile_center_unity_distribute_get_delegate()
{
  return distributeDelegate;
}

