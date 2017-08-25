// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#include "PushDelegateSetter.h"
#include "PushDelegate.h"

static UnityPushDelegate* pushDelegate;

void mobile_center_unity_push_set_delegate()
{
  pushDelegate = [UnityPushDelegate new];
  [MSPush setDelegate:pushDelegate];
}

UnityPushDelegate* mobile_center_unity_push_get_delegate()
{
  return pushDelegate;
}

