MY_ROOT_PATH := $(call my-dir)/src/main/cpp

include $(MY_ROOT_PATH)/google-breakpad/android/google_breakpad/Android.mk

LOCAL_PATH := $(MY_ROOT_PATH)
include $(CLEAR_VARS)

LOCAL_MODULE    := PuppetBreakpad
LOCAL_SRC_FILES := main.cpp

LOCAL_LDFLAGS := -latomic

LOCAL_LALIBS += -llog
LOCAL_STATIC_LIBRARIES += breakpad_client
TARGET_OUT = $(LOCAL_PATH)/../../../../../Assets/Plugins/Android/$(TARGET_ARCH_ABI)
include $(BUILD_SHARED_LIBRARY)