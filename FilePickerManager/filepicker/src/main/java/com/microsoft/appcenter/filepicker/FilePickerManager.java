// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

package com.microsoft.appcenter.filepicker;

import android.app.Activity;
import android.app.Fragment;
import android.app.FragmentTransaction;
import android.content.Intent;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStream;

public class FilePickerManager extends Fragment {
    private static final String LOG_TAG = "FilePickerManager";
    private static final int READ_REQUEST_CODE = 42;

    private static FilePickerManagerListener mListener;

    public static void setListener(FilePickerManagerListener listener) {
        mListener = listener;
    }

    public static void openFilePicker(Activity unityActivity) {
        if (unityActivity == null && mListener != null) {
            mListener.onSelectFileFailure("Failed to open the picker.");
            return;
        }
        FilePickerManager picker = new FilePickerManager();
        FragmentTransaction transaction = unityActivity.getFragmentManager().beginTransaction();
        transaction.add(picker, LOG_TAG);
        transaction.commit();
    }

    public static void readTextFromUri(Activity unityActivity, String path) {
        if (path == null) {
            if (mListener != null) {
                mListener.onSelectFileFailure("Path is null.");
            }
            return;
        }
        InputStream inputStream = null;
        ByteArrayOutputStream outputStream = null;
        Uri uri = Uri.parse(path);
        try
        {
            inputStream = unityActivity.getApplicationContext().getContentResolver().openInputStream(uri);
            if (inputStream == null) {
                if (mListener != null) {
                    mListener.onSelectFileFailure("Failed file name.");
                }
            }
            outputStream = new ByteArrayOutputStream();
            byte[] buffer = new byte[4096];
            int read;
            while ((read = inputStream.read(buffer)) != -1) {
                outputStream.write(buffer, 0, read);
            }
        } catch (IOException e) {
            if (mListener != null) {
                mListener.onSelectFileFailure("Couldn't read file.");
            }
        } finally {
            try {
                if (inputStream != null) {
                    inputStream.close();
                }
            } catch (IOException ignore) {
            }
            try {
                if (outputStream != null) {
                    outputStream.close();
                }
            } catch (IOException ignore) {
            }
        }
        if (mListener != null) {
            mListener.onGetBytes(outputStream);
        }
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Intent intent = null;
        if (android.os.Build.VERSION.SDK_INT >= android.os.Build.VERSION_CODES.KITKAT) {
            intent = new Intent(Intent.ACTION_OPEN_DOCUMENT);
            intent.addCategory(Intent.CATEGORY_OPENABLE);
        } else {
            intent = new Intent(Intent.ACTION_GET_CONTENT);
        }
        intent.setFlags(Intent.FLAG_GRANT_READ_URI_PERMISSION);
        intent.setType("*/*");
        startActivityForResult(Intent.createChooser(intent, "Select attachment file"), READ_REQUEST_CODE);
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent resultData) {
        if (requestCode == READ_REQUEST_CODE && resultCode == Activity.RESULT_OK) {
            Uri uri = null;
            if (resultData != null && resultData.getData() != null) {
                uri = resultData.getData();
                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
                    getActivity().getContentResolver().takePersistableUriPermission(uri, resultData.getFlags() & Intent.FLAG_GRANT_READ_URI_PERMISSION);
                }
                if (mListener != null) {
                    mListener.onSelectFileSuccessful(uri.toString());
                }
            } else {
                if (mListener != null) {
                    mListener.onSelectFileFailure("Failed to pick the file.");
                }
            }
        } else {
            if (mListener != null) {
                mListener.onSelectFileFailure("Failed to pick the file.");
            }
        }
    }
}