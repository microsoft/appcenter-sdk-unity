package com.microsoft.azure.mobile.mobilecenterunityplayeractivity;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.util.Log;

import com.unity3d.player.UnityPlayer;
import com.unity3d.player.UnityPlayerActivity;

public class MobileCenterUnityPlayerActivity extends UnityPlayerActivity {

    private static IntentListener mIntentListener;

    private static Intent mLastIntent;

    public static void setListener(@NonNull IntentListener listener) {
        Log.i("MC ACTIVITY", "setListener");
        mIntentListener = listener;
        if (mLastIntent != null) {
            Log.i("MC ACTIVITY", "last intent is not null so triggering event");
            mIntentListener.onNewIntent(UnityPlayer.currentActivity, mLastIntent);
            mLastIntent = null;
        }
    }
    @Override
    protected void onCreate(Bundle bundle)
    {
        super.onCreate(bundle);
        Log.i("MC ACTIVITY", "on create");
    }

    @Override
    protected void onNewIntent(Intent intent) {
        super.onNewIntent(intent);
        Log.i("MC ACTIVITY", "onNewIntent");
        if (mIntentListener != null) {
            Log.i("MC ACTIVITY", "listener is not null so triggering event");
            mIntentListener.onNewIntent(this, intent);
        }
        else {
            Log.i("MC ACTIVITY", "listener is null so saving intent");
            mLastIntent = intent;
        }
    }

    @Override
    protected void onDestroy()
    {
        super.onDestroy();
        Log.i("MC ACTIVITY", "on destroy");
    }

}
