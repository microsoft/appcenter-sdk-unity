// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

package com.microsoft.appcenter.filepicker;

public interface FilePickerManagerListener {
    void onSelectFileSuccessful(String path);
    void onSelectFileFailure(String message);
}