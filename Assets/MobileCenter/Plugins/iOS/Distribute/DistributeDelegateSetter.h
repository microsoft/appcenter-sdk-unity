// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#ifndef DISTRIBUTE_DELEGATE_SETTER_H
#define DISTRIBUTE_DELEGATE_SETTER_H

#include "DistributeDelegate.h"

// We need a bridge like this because headers using "extern" cannot be
// included from an ".m" file (at least in this case)
void mobile_center_distribute_set_delegate(bool useCustomDialog);

UnityDistributeDelegate* mobile_center_unity_distribute_get_delegate();

#endif
