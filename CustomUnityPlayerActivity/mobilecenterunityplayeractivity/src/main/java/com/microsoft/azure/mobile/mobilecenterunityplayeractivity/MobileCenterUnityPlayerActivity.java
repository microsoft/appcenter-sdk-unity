package com.microsoft.azure.mobile.mobilecenterunityplayeractivity;

import android.content.Intent;
import com.microsoft.azure.mobile.push.Push;
import com.unity3d.player.UnityPlayerActivity;

public class MobileCenterUnityPlayerActivity extends UnityPlayerActivity {
    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        Push.checkLaunchedFromNotification(this, intent);
    }
}
