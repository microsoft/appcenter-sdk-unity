#include <stddef.h>
#include <android/log.h>
#include "google-breakpad/src/client/linux/handler/exception_handler.h"
#include "google-breakpad/src/client/linux/handler/minidump_descriptor.h"

    static bool dumpCallback(const google_breakpad::MinidumpDescriptor &descriptor,
                             void *context,
                             bool succeeded) {

        /* Allow system to log the native stack trace. */
        __android_log_print(ANDROID_LOG_INFO, "AppCenterPuppet",
                            "Wrote breakpad minidump at %s succeeded=%d\n", descriptor.path(),
                            succeeded);
        return false;
    }

    static google_breakpad::ExceptionHandler eh(google_breakpad::MinidumpDescriptor(), NULL, dumpCallback, NULL, true, -1);

    extern "C" void nativeCrashNullPointer() {
        __android_log_print(ANDROID_LOG_INFO, "AppCenterPuppet",
                            "Force null pointer crash...");
        volatile int *a = reinterpret_cast<volatile int *>(NULL);
        *a = 1;
    }

    extern "C" void nativeDivideByZero() {
        __android_log_print(ANDROID_LOG_INFO, "AppCenterPuppet",
                            "Force divide by zero...");
        int* test = 42;
        *test = test / 0; //crash
    }

    /**
     * Registers breakpad as the exception handler for NDK code.
     * @param path minidump directory path returned from Crashes.getMinidumpDirectory()
     */
    extern "C" void setupNativeCrashesListener(const char *path) {
        __android_log_print(ANDROID_LOG_INFO, "AppCenterSasquatch",
                            "Setup path %s", path);
        google_breakpad::MinidumpDescriptor descriptor(path);
        eh.set_minidump_descriptor(descriptor);
    }