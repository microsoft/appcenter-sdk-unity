// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using System.Threading.Tasks;
using UnityEngine;
using Microsoft.AppCenter.Utils;

namespace Microsoft.AppCenter.Unity.Internal.Utils
{
    public class UnityScreenSizeProvider : ScreenSizeProviderBase
    {
        private static int _height;
        private static int _width;
        private static TaskCompletionSource<bool> _initializationTaskSource = new TaskCompletionSource<bool>();
        public override int Height => _height;
        public override int Width => _width;

        public static void Initialize()
        {
            // This must occur on the main thread
            _height = Screen.currentResolution.height;
            _width = Screen.currentResolution.width;
            _initializationTaskSource.SetResult(true);
        }

        public override event EventHandler ScreenSizeChanged;

        public override Task WaitUntilReadyAsync()
        {
            return _initializationTaskSource.Task;
        }
    }
}
#endif
