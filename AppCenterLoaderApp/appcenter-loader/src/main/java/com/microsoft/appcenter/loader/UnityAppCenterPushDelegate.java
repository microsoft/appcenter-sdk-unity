package com.microsoft.appcenter.loader;

import android.app.Activity;

import com.microsoft.appcenter.push.PushListener;
import com.microsoft.appcenter.push.PushNotification;

import java.util.LinkedList;
import java.util.List;

public class UnityAppCenterPushDelegate implements PushListener
{
    static List<PushNotification> mUnprocessedNotifications = new LinkedList<>();
    static PushListener mPushListener;

    synchronized
    public static void setListener(PushListener listener)
    {
        mPushListener = listener;
    }

    @Override
    synchronized
    public void onPushNotificationReceived(Activity activity, PushNotification pushNotification)
    {
        if (mUnprocessedNotifications != null) {
            mUnprocessedNotifications.add(pushNotification);
        }
        if (mPushListener != null) {
            mPushListener.onPushNotificationReceived(activity, pushNotification);
        }
    }

    synchronized
    public static void replayPushNotifications()
    {
        if (mUnprocessedNotifications != null) {
            for (PushNotification notification : mUnprocessedNotifications) {
                mPushListener.onPushNotificationReceived(null, notification);
            }
            mUnprocessedNotifications = null;
        }
    }
}
