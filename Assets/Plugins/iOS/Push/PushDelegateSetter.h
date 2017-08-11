// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#ifndef PUSH_DELEGATE_SETTER_H
#define PUSH_DELEGATE_SETTER_H

// We need a bridge like this because headers using "extern" cannot be
// included from an ".m" file (at least in this case)
void mobile_center_unity_push_set_delegate();

#endif
