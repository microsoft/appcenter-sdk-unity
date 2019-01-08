# Release 1.3.0

**Breaking changes**

* Method `Crashes.HasCrashedInLastSession` renamed to `Crashes.HasCrashedInLastSessionAsync`
* Method `Crashes.LastSessionCrashReport` renamed to `Crashes.GetLastSessionCrashReportAsync` and now returns an instance of class `AppCenterTask<ErrorReport>`
* Handler for an event `Crashes.FailedToSendErrorReport` must now contain additional parameter with type `Microsoft.AppCenter.Unity.Crashes.Models.Exception`

**Android**

* **[Bug fix]** All of the callbacks and events of the class `Crashes` are now correctly work on Android

**iOS**

* **[Bug fix]** Crash error report is now contains correct information about the device
* **[Bug fix]** Fix incorrect values of properties `ErrorReport.AppStartTime` and `ErrorReport.AppErrorTime` on iOS

**UWP**

* **[Bug fix]** Hidden some of the warnings in Unity Editor console when using App Center Unity SDK
* **[Bug fix]** Fix warnings in Unity Editor console when building app with `IL2CPP` scripting backend and `XAML` build type