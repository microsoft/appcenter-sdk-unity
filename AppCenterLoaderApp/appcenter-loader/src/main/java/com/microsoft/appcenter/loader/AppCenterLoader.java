// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

package com.microsoft.appcenter.loader;

import android.app.Application;
import android.content.ContentProvider;
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.util.Log;

import com.microsoft.appcenter.AppCenter;
import com.microsoft.appcenter.AppCenterService;
import com.microsoft.appcenter.analytics.Analytics;
import com.microsoft.appcenter.crashes.Crashes;
import com.microsoft.appcenter.distribute.Distribute;
import com.microsoft.appcenter.push.Push;
import com.microsoft.appcenter.utils.AppCenterLog;

import java.util.ArrayList;
import java.util.List;

import static com.microsoft.appcenter.loader.StartupType.SKIP_START;

public class AppCenterLoader extends ContentProvider {

    private static final String CUSTOM_LOG_URL_KEY = "appcenter_custom_log_url";
    private static final String USE_CUSTOM_LOG_URL_KEY = "appcenter_use_custom_log_url";
    private static final String MAX_STORAGE_SIZE = "appcenter_max_storage_size";
    private static final String INITIAL_LOG_LEVEL_KEY = "appcenter_initial_log_level";
    private static final String USE_PUSH_KEY = "appcenter_use_push";
    private static final String SENDER_ID_KEY = "appcenter_sender_id";
    private static final String ENABLE_FIREBASE_ANALYTICS_KEY = "appcenter_enable_firebase_analytics";
    private static final String USE_ANALYTICS_KEY = "appcenter_use_analytics";
    private static final String USE_DISTRIBUTE_KEY = "appcenter_use_distribute";
    private static final String USE_CUSTOM_API_URL_KEY = "appcenter_use_custom_api_url";
    private static final String USE_CUSTOM_INSTALL_URL_KEY = "appcenter_use_custom_install_url";
    private static final String CUSTOM_API_URL_KEY = "appcenter_custom_api_url";
    private static final String CUSTOM_INSTALL_URL_KEY = "appcenter_custom_install_url";
    private static final String USE_CRASHES_KEY = "appcenter_use_crashes";
    private static final String APP_SECRET_KEY = "appcenter_app_secret";
    private static final String TRANSMISSION_TARGET_TOKEN_KEY = "appcenter_transmission_target_token";
    private static final String STARTUP_TYPE_KEY = "appcenter_startup_type";
    private static final String TRUE_VALUE = "True";
    private static final String TAG = "AppCenterLoader";

    private Context mContext;

    @Override
    public boolean onCreate() {
        mContext = getApplicationContext();
        String appSecret = getStringResource(APP_SECRET_KEY);
        String transmissionTargetToken = getStringResource(TRANSMISSION_TARGET_TOKEN_KEY);
        int startupTypeInt = Integer.parseInt(getStringResource(STARTUP_TYPE_KEY));
        StartupType startupType = StartupType.values()[startupTypeInt];

        /*
         * If app secret isn't found in resources, return immediately. It's possible that resources
         * weren't added properly.
         */
        if (appSecret == null) {
            AppCenterLog.error(AppCenterLog.LOG_TAG, "Failed to retrieve app secret from " +
                    "resources. App Center cannot be started.");
            return false;
        }
        List<Class<? extends AppCenterService>> classes = new ArrayList<>();
        if (isTrueValue(getStringResource(USE_ANALYTICS_KEY)) &&
            isModuleAvailable("com.microsoft.appcenter.analytics.Analytics", "Analytics")) {
            classes.add(Analytics.class);
        }
        if (isTrueValue(getStringResource(USE_CRASHES_KEY)) &&
            isModuleAvailable("com.microsoft.appcenter.crashes.Crashes", "Crashes")) {
            classes.add(Crashes.class);
        }
        if (isTrueValue(getStringResource(USE_DISTRIBUTE_KEY)) &&
            isModuleAvailable("com.microsoft.appcenter.distribute.Distribute", "Distribute")) {
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
            classes.add(Distribute.class);
        }
        if (isTrueValue(getStringResource(USE_PUSH_KEY)) &&
            isModuleAvailable("com.microsoft.appcenter.push.Push", "Push")) {
            Push.setListener(new UnityAppCenterPushDelegate());
            classes.add(Push.class);
            SetSenderId();
            if (isTrueValue(getStringResource(ENABLE_FIREBASE_ANALYTICS_KEY))) {
                Push.enableFirebaseAnalytics(mContext);
            }
        }
        int logLevel = Integer.parseInt(getStringResource(INITIAL_LOG_LEVEL_KEY));
        AppCenterLog.setLogLevel(logLevel);
        if (isTrueValue(getStringResource(USE_CUSTOM_LOG_URL_KEY))) {
            String customLogUrl = getStringResource(CUSTOM_LOG_URL_KEY);
            if (customLogUrl != null) {
                AppCenter.setLogUrl(customLogUrl);
            }
        }
        if (startupType == SKIP_START) {
            return true;
        }
        SetMaxStorageSize();
        String appIdArg = "";
        switch (startupType) {
            case APP_SECRET:
                appIdArg = appSecret;
                break;
            case TARGET:
                appIdArg = String.format("target=%s", transmissionTargetToken);
                break;
            case BOTH:
                appIdArg = String.format("appsecret=%s;target=%s", appSecret, transmissionTargetToken);
                break;
            case NO_SECRET:
                if (classes.size() > 0) {
                    Class<? extends AppCenterService>[] classesArray = GetClassesArray(classes);
                    AppCenter.start((Application) mContext, classesArray);
                }
                return true;
        }
        if (classes.size() > 0) {
            Class<? extends AppCenterService>[] classesArray = GetClassesArray(classes);
            AppCenter.start((Application) mContext, appIdArg, classesArray);
        } else {
            AppCenter.configure((Application) mContext, appIdArg);
        }
        return true;
    }

    private void SetMaxStorageSize() {
        String maxStorageSizeString = getStringResource(MAX_STORAGE_SIZE);
        if (maxStorageSizeString != null) {
            long maxStorageSize = Long.parseLong(maxStorageSizeString);
            if (maxStorageSize > 0) {
                AppCenter.setMaxStorageSize(maxStorageSize);
            }
        }
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

    private boolean isModuleAvailable(String className, String moduleName) {
        try {
            Class.forName(className);
            return true;
        } catch (ClassNotFoundException e) {
            Log.i(TAG, moduleName + " is not available: " + e.getMessage());
            return false;
        }
    }

    private Class<? extends AppCenterService>[] GetClassesArray(List<Class<? extends AppCenterService>> classes) {
        @SuppressWarnings("unchecked")
        Class<? extends AppCenterService>[] classesArray = classes.toArray(new Class[classes.size()]);
        return classesArray;
    }

    @SuppressWarnings("deprecation")
    private void SetSenderId() {
        Push.setSenderId(getStringResource(SENDER_ID_KEY));
    }
}
