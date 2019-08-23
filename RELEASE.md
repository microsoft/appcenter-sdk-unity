## Release 2.3.0

Updated native SDK versions:
* Android from 2.2.0 to [2.3.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.3.0)
* iOS from 2.2.0 to [2.3.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.3.0)
* **[Feature]** Catch "low memory warning" and provide the API to check if it has happened in last session:  `Crashes.HasReceivedMemoryWarningInLastSessionAsync()`.

**Android**

* **[Bug fix]** Fixed handling the update actions in custom update dialog in Distributed module.

## Release 2.2.0

Updated native SDK versions:
* Android from `2.1.0` to [2.2.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.2.0)
* iOS from `2.1.0` to [2.2.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.2.0)

* **[Bug fix]** Separeted Push logic in order to avoid `ClassNotFoundException`.

# Release 2.1.0

Updated native SDK versions:
* Android from `2.0.0` to [2.1.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.1.0)
* iOS from `2.0.1` to [2.1.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.1.0)
* UWP from `2.0.0` to [2.1.0](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/2.1.0)

* **[Feature]**  Add `Distribute.setEnabledForDebuggableBuild(boolean)` method to allow in-app updates in debuggable builds.
* **[Bug fix]**  Fixed UWP build in Unity 2019

# Release 2.0.0

Updated native SDK versions:
* Android from `1.11.4` to [2.0.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.0.0)
* iOS from `1.14.0` to [2.0.1](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.0.1)
* UWP from `1.14.0` to [2.0.0](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/2.0.0)

**Breaking changes**
* “This version has a breaking change, it only supports Xcode 10.0.0+.”

# Release 1.4.1

Updated native SDK versions:
* Android from `1.11.3` to [1.11.4](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/1.11.4)
* iOS from `1.13.2` to [1.14.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/1.14.0)
* UWP from `1.13.2` to [1.14.0](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/1.14.0)

* **[Fix]** Fixed `async`/`await` operators support by SDK’s methods for .NETStandard 2.0 profile
* **[Fix]** Fixed iOS application crash when trying to pass exception without stack trace to `Crashes.TrackError`

# Release 1.4.0

Updated native SDK versions:
* Android from `1.11.0` to [1.11.3](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/1.11.3)
* iOS from `1.12.0` to [1.13.2](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/1.13.2)
* UWP from `1.12.0` to [1.13.2](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/1.13.2)

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
