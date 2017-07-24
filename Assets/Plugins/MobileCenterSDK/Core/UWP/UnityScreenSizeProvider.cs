// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_WSA_10_0 && !UNITY_EDITOR
using System;
using UnityEngine;
using Microsoft.Azure.Mobile.Utils;

namespace Microsoft.Azure.Mobile.Unity.Internal.Utils
{
    public class UnityScreenSizeProvider : ScreenSizeProviderBase
    {
        private static int _height;
        private static int _width;
        private static SemaphoreSlim _screenSizeSemaphore = new SemaphoreSlim(0);
        public override int Height => _height;
        public override int Width => _width;

        public static void Initialize()
        {
            // This must occur on the main thread
            _height = Screen.currentResolution.height;
            _width = Screen.currentResolution.width;
            _screenSizeSemaphore.Release();
        }

        public override event EventHandler ScreenSizeChanged;

        public override async Task WaitUntilReadyAsync()
        {
            return Task.Run(() =>
            {
                _screenSizeSemaphore.Wait();
            });
        }
    }
}
#endif
