package com.microsoft.appcenter;

import android.app.Activity;
import android.app.Fragment;
import android.app.FragmentTransaction;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

public class FilePickerManager extends Fragment {
    private static final String LOG_TAG = "FilePickerManager";
    private static final int READ_REQUEST_CODE = 42;

    private static FilePickerManagerListener mListener;

    public static void setListener(FilePickerManagerListener listener) {
        mListener = listener;
    }

    public static void openFilePicker(Activity unityActivity) {
        if (unityActivity == null && mListener != null) {
            mListener.onSelectFileFailure("Failed to open the picker");
            return;
        }
        FilePickerManager picker = new FilePickerManager();
        FragmentTransaction transaction = unityActivity.getFragmentManager().beginTransaction();
        transaction.add(picker, LOG_TAG);
        transaction.commit();
    }

    private String readTextFromUri(Activity unityActivity, String path) throws IOException {
        if (path == null) {
            return null;
        }
        Uri uri = Uri.parse(path);
        StringBuilder stringBuilder = new StringBuilder();
        InputStream inputStream = null;
        BufferedReader reader = null;
        String line;

        // noinspection TryFinallyCanBeTryWithResources
        try {
            inputStream = unityActivity.getContentResolver().openInputStream(uri);
            if (inputStream != null) {
                reader = new BufferedReader(new InputStreamReader(inputStream));
                while ((line = reader.readLine()) != null) {
                    stringBuilder.append(line);
                }
            }
        } finally {
            if (inputStream != null) {
                inputStream.close();
            }
            if (reader != null) {
                reader.close();
            }
        }
        return stringBuilder.toString();
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Intent intent = null;
        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.KITKAT) {
            intent = new Intent(Intent.ACTION_OPEN_DOCUMENT);
        } else {
            intent = new Intent(Intent.ACTION_GET_CONTENT);
        }
        intent.addCategory(Intent.CATEGORY_OPENABLE);
        startActivityForResult(intent, READ_REQUEST_CODE);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent resultData) {
        if (requestCode == READ_REQUEST_CODE && resultCode == Activity.RESULT_OK) {
            Uri uri = null;
            if (resultData != null && resultData.getData() != null) {
                uri = resultData.getData();
                Log.i(LOG_TAG, "Uri: " + uri.toString());
                if (mListener != null) {
                    mListener.onSelectFileSuccessful(uri.getPath());
                }
            } else {
                Log.i(LOG_TAG, "Failed to get data from result data. ");
                if (mListener != null) {
                    mListener.onSelectFileFailure("Failed to pick the file");
                }
            }
        } else {
            if (mListener != null) {
                mListener.onSelectFileFailure("Failed to pick the file");
            }
        }
    }

    public interface FilePickerManagerListener {
        void onSelectFileSuccessful(String path);

        void onSelectFileFailure(String message);
    }
}