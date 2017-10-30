// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

package com.microsoft.appcenter.appcenterloader;

import android.app.Application;
import android.content.ContentProvider;
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;

import com.microsoft.azure.mobile.MobileCenter;
import com.microsoft.azure.mobile.MobileCenterService;
import com.microsoft.azure.mobile.distribute.Distribute;
import com.microsoft.azure.mobile.push.Push;
import com.microsoft.azure.mobile.utils.MobileCenterLog;

import java.util.ArrayList;
import java.util.List;

public class AppCenterLoader extends ContentProvider {

    private static final String CUSTOM_LOG_URL_KEY = "mobile_center_custom_log_url";
    private static final String USE_CUSTOM_LOG_URL_KEY = "mobile_center_use_custom_log_url";
    private static final String INITIAL_LOG_LEVEL_KEY = "mobile_center_initial_log_level";
    private static final String USE_PUSH_KEY = "mobile_center_use_push";
    private static final String ENABLE_FIREBASE_ANALYTICS_KEY = "mobile_center_enable_firebase_analytics";
    private static final String USE_ANALYTICS_KEY = "mobile_center_use_analytics";
    private static final String USE_DISTRIBUTE_KEY = "mobile_center_use_distribute";
    private static final String USE_CUSTOM_API_URL_KEY = "mobile_center_use_custom_api_url";
    private static final String USE_CUSTOM_INSTALL_URL_KEY = "mobile_center_use_custom_install_url";
    private static final String CUSTOM_API_URL_KEY = "mobile_center_custom_api_url";
    private static final String CUSTOM_INSTALL_URL_KEY = "mobile_center_custom_install_url";
    private static final String USE_CRASHES_KEY = "mobile_center_use_crashes";
    private static final String APP_SECRET_KEY = "mobile_center_app_secret";
    private static final String TRUE_VALUE = "True";

    private Context mContext;

    @Override
    public boolean onCreate() {
        mContext = getApplicationContext();
        List<Class<? extends MobileCenterService>> classes = new ArrayList<>();
        if (isTrueValue(getStringResource(USE_ANALYTICS_KEY))) {
            classes.add(com.microsoft.azure.mobile.analytics.Analytics.class);
        }
        if (isTrueValue(getStringResource(USE_CRASHES_KEY))) {
            classes.add(com.microsoft.azure.mobile.crashes.Crashes.class);
        }
        if (isTrueValue(getStringResource(USE_DISTRIBUTE_KEY))) {
            if (isTrueValue(getStringResource(USE_CUSTOM_API_URL_KEY))) {
                String customApiUrl = getStringResource(CUSTOM_API_URL_KEY);
                if (customApiUrl != null) {
                    Distribute.setApiUrl(customApiUrl);
                }
            }
            if (isTrueValue(getStringResource(USE_CUSTOM_INSTALL_URL_KEY))) {
                String customInstallUrl = getStringResource(CUSTOM_INSTALL_URL_KEY);
                if (customInstallUrl != null) {
                    Distribute.setInstallUrl(customInstallUrl);
                }
            }
            classes.add(com.microsoft.azure.mobile.distribute.Distribute.class);
        }
        if (isTrueValue(getStringResource(USE_PUSH_KEY))) {
            Push.setListener(new UnityAppCenterPushDelegate());
            classes.add(com.microsoft.azure.mobile.push.Push.class);

            if (isTrueValue(getStringResource(ENABLE_FIREBASE_ANALYTICS_KEY))) {
                Push.enableFirebaseAnalytics(mContext);
            }
        }
        int logLevel = Integer.parseInt(getStringResource(INITIAL_LOG_LEVEL_KEY));
        MobileCenterLog.setLogLevel(logLevel);
        if (isTrueValue(getStringResource(USE_CUSTOM_LOG_URL_KEY))) {
            String customLogUrl = getStringResource(CUSTOM_LOG_URL_KEY);
            if (customLogUrl != null) {
                MobileCenter.setLogUrl(customLogUrl);
            }
        }
        String appSecret = getStringResource(APP_SECRET_KEY);
        if (classes.size() > 0) {
            @SuppressWarnings("unchecked")
            Class<? extends MobileCenterService>[] classesArray = classes.toArray(new Class[classes.size()]);
            MobileCenter.start((Application) mContext, appSecret, classesArray);
        } else {
            MobileCenter.configure((Application) mContext, appSecret);
        }
        return true;
    }

    @Nullable
    @Override
    public Cursor query(@NonNull Uri uri, @Nullable String[] strings, @Nullable String s, @Nullable String[] strings1, @Nullable String s1) {
        return null;
    }

    @Nullable
    @Override
    public String getType(@NonNull Uri uri) {
        return null;
    }

    @Nullable
    @Override
    public Uri insert(@NonNull Uri uri, @Nullable ContentValues contentValues) {
        return null;
    }

    @Override
    public int delete(@NonNull Uri uri, @Nullable String s, @Nullable String[] strings) {
        return 0;
    }

    @Override
    public int update(@NonNull Uri uri, @Nullable ContentValues contentValues, @Nullable String s, @Nullable String[] strings) {
        return 0;
    }

    private Context getApplicationContext() {

        //TODO: if Unity supports instant apps, need to modify this method to account for them
        return getContext();
    }

    private boolean isTrueValue(String value) {
        return value != null && value.equals(TRUE_VALUE);
    }

    private String getStringResource(String key) {
        int identifier = mContext.getResources().getIdentifier(key, "string", mContext.getPackageName());
        if (identifier == 0) {
            return null;
        }
        return mContext.getResources().getString(identifier);
    }
}
