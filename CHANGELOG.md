# App Center SDK for Unity Change Log

## Version 2.6.1 (Under development)

### App Center Crashes

#### iOS

* **[Fix]** Fix that Crashes could not be started because `StartCrashes` method was stripped for a high managed stripping level.

### App Center Push

#### iOS

* **[Fix]** Fix that Push could not be started because `StartPush` method was stripped for a high managed stripping level.

___

## Release 2.6.0

Updated native SDK versions:
* Android from `2.5.0` to [2.5.1](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.5.1)
* iOS from `2.5.1` to [2.5.3](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.5.3)
* UWP from `2.6.1` to [2.6.4](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/2.6.4)

### App Center

* **[Fix]** Fix Unity 2019.3 iOS build ('Use of undeclared identifier' error).
* **[Fix]** Fix Unity 2019.3 Android build ('Requested value X86 was not found' error).
* **[Improvement]** Detect App Center SDK location automatically.
* **[Fix]** Mark changes as dirty when setting up Settings in AppCenterBehavior.

### App Center Crashes

#### Android/iOS

* **[Fix]** Validate error attachment size to avoid server error or out of memory issues (using the documented limit which is 7MB).

#### iOS

* **[Fix]** Fix to send crashes when an application was launched in background and enters foreground.
* **[Fix]** Fix an issue where crash might contain incorrect data if two consecutive crashes occurred in a previous version of the application.

### App Center Push

* **[Bug fix]** Fix missing `NotifyPushNotificationReceived` callback if push notification is received from the background.
* **[Improvement]** Add ability to delay native start using a scripting define symbol `APPCENTER_DONT_USE_NATIVE_STARTER`.

### App Center Distribute

#### iOS

* **[Fix]** Fix missing alert dialogs in apps that use iOS 13's new UIScene API (multiple scenes are not yet supported).
* **[Fix]** Fix an issue where users would sometimes be prompted multiple times to sign in with App Center.

### App Center Auth

#### iOS

* **[Fix]** Fix build warnings when adding App Center Auth framework in project.

___

## Release 2.5.1

Updated native SDK versions:
* Android from `2.3.0` to [2.5.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.5.0)
* iOS from `2.4.0` to [2.5.1](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.5.1)
* UWP from `2.1.0` to [2.6.1](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/2.6.1)

### App Center

* **[Fix]** Fix `AppCenterStarter.m` was not included in the first build.
* **[Fix]** Fix Unity 2019.3 iOS build.

### App Center Auth

* **[Fix]** Fix Auth was incorrectly caching the old app secret in the manifest if it was changed.

### App Center Crashes

#### Android/iOS

* **[Feature]** Support sending attachments in handled errors.

#### Android

* **[Behavior change]** Fix a security issue with the `Exception` field on `ErrorReport` objects. As a result, the `Exception.StackTrace` now holds the raw stack trace, and the `Exception.Message` field is `null`.

### App Center Push

#### UWP

* **[Feature]** Allow developers to push notifications to a specific userId.

### App Center Distribute

* **[Fix]** Fix line endings Unity warning in `UpdateAction` class.

#### Android

* **[Feature]** Downloading in-app update APK file has been failing on Android 4.x since TLS 1.2 has been enforced early September. The file is now downloaded using HTTPS direct connection when running on Android 4 instead of relying on system's download manager.
* **[Feature]** Fix a crash and improve logging when downloading an update fails on Android 5+.
* **[Behavior change]** If your minSdkVersion is lower than 19, Android requires the WRITE_EXTERNAL_STORAGE permission to store new downloaded updates. Please refer to the updated documentation site for detailed instructions. This is related to the download fix.

___

## Release 2.4.0

Updated native SDK versions:

* iOS from 2.3.0 to [2.4.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.4.0)

### App Center Auth

This version of App Center Unity SDK includes a new module: Auth. 

App Center Auth is a cloud-based identity management service that enables developers to authenticate application users and manage user identities. The service integrates with other parts of App Center, enabling developers to leverage the user identity to view user data in other services and even send push notifications to users instead of individual devices.

### App Center Analytics

#### iOS

* **[Fix]** Fix crash involving SDK's `ms_viewWillAppear` method.

___

## Release 2.3.0

Updated native SDK versions:
* Android from 2.2.0 to [2.3.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.3.0)
* iOS from 2.2.0 to [2.3.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.3.0)

* **[Feature]** Catch "low memory warning" and provide the API to check if it has happened in last session:  `Crashes.HasReceivedMemoryWarningInLastSessionAsync()`.
* **[Bug fix]** Fixed receiving push notifications from background.

**Android**

* **[Bug fix]** Fixed handling the update actions in custom update dialog in Distributed module.

___

## Release 2.2.0

Updated native SDK versions:
* Android from `2.1.0` to [2.2.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.2.0)
* iOS from `2.1.0` to [2.2.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.2.0)

* **[Bug fix]** Separeted Push logic in order to avoid `ClassNotFoundException`.
* **[Bug fix]** Fixed wrong type for max data storage which causes an issue with the archiving.

___

## Release 2.1.0

Updated native SDK versions:
* Android from `2.0.0` to [2.1.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.1.0)
* iOS from `2.0.1` to [2.1.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.1.0)
* UWP from `2.0.0` to [2.1.0](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/2.1.0)

* **[Feature]** Add `Distribute.setEnabledForDebuggableBuild(boolean)` method to allow in-app updates in debuggable builds.
* **[Bug fix]** Fixed UWP build in Unity 2019.

___

## Release 2.0.0

Updated native SDK versions:
* Android from `1.11.4` to [2.0.0](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/2.0.0)
* iOS from `1.14.0` to [2.0.1](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/2.0.1)
* UWP from `1.14.0` to [2.0.0](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/2.0.0)

**Breaking changes**

* This version has a breaking change, it only supports Xcode 10.0.0+.

___

## Release 1.4.1

Updated native SDK versions:
* Android from `1.11.3` to [1.11.4](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/1.11.4)
* iOS from `1.13.2` to [1.14.0](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/1.14.0)
* UWP from `1.13.2` to [1.14.0](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/1.14.0)

* **[Fix]** Fixed `async`/`await` operators support by SDK’s methods for .NETStandard 2.0 profile
* **[Fix]** Fixed iOS application crash when trying to pass exception without stack trace to `Crashes.TrackError`

___

## Release 1.4.0

Updated native SDK versions:
* Android from `1.11.0` to [1.11.3](https://github.com/Microsoft/AppCenter-SDK-Android/releases/tag/1.11.3)
* iOS from `1.12.0` to [1.13.2](https://github.com/Microsoft/AppCenter-SDK-Apple/releases/tag/1.13.2)
* UWP from `1.12.0` to [1.13.2](https://github.com/Microsoft/AppCenter-SDK-DotNet/releases/tag/1.13.2)

___

## Release 1.3.0

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

___

## Release 1.2.1

**UWP**

* App Center SDK for Unity now uses the latest native App Center SDK for .NET version 1.12.0
* **[Bug fix]** Fix UWP app build when using the Push package

**Android**

* **[Bug fix]** Fixed `Crashes.GenerateTestCrash` method, which now correctly crashes the application when it is built with "Development Build" checkbox

___

## Release 1.2.0

App Center SDK for Unity now uses the latest native SDKs:
* App Center SDK for Android version 1.11.0
* App Center SDK for iOS version 1.12.0
* App Center SDK for .NET version 1.11.0

* **[Feature]** Add support for `Push`.
* **[Feature]** Implement `AppCenter.SetUserId` that allows users to set userId that applies to crashes, error and push logs.
* **[Feature]** Work for a future change in transmission protocol and endpoint for Analytics data. There is no impact on your current workflow when using App Center.
* **[Bug Fix]** Fix AppCenter Analytics working incorrectly in case there's no advanced behavior but this file exists.

___

## Release 1.1.0

**Analytics**

* **[Feature]** Add new trackEvent APIs that take priority (normal or critical) of event logs. Events tracked with critical flag will take precedence over all other logs except crash logs (when AppCenterCrashes is enabled), and only be dropped if storage is full and must make room for newer critical events or crashes logs.
* **[Feature]** Add support for typed properties. Note that these APIs still convert properties back to strings on the App Center backend. More work is needed to store and display typed properties in the App Center portal. Using the new APIs now will enable future scenarios, but for now the behavior will be the same as it is for current event properties.

**Android**

* **[Fix]** Preventing stack overflow crash while reading a huge throwable file.

___

## Release 1.0.0

**Analytics**

* [Feature] Preparation work for a future change in transmission protocol and endpoint for Analytics data. There is no impact on your current workflow when using App Center.

**UWP**

* **[Bug fix]** Fix build errors when building the `UWP` app with `.Net` scripting backend
* **[Bug fix]** Fix issue with reporting analytics events from `Start` method
* **[Bug fix]** Automatically add the `InternetAccess` capability when building `UWP` apps in order for analytics to work properly

**Android**

* **[Bug fix]** Fix error occuring when trying to send crash report
* **[Bug fix]** Fix performance issue caused by exceptions reporting logic

___

## Release 0.1.4

* **[Feature]** Add pause/resume APIs which pause/resume sending Analytics logs to App Center.
* **[Feature]** Add ability to specify maximum size limit on the local SQLite storage. Previously, up to 300 logs were stored of any size. The default value is 10MB.

**UWP**
* **[Bug fix]** Fixed missing namespace import when building with `.Net` scripting backend.

___

## Release 0.1.3

App Center SDK for Unity now uses the latest native SDKs:
* App Center SDK for Android version 1.9.0
* App Center SDK for iOS version 1.10.0
* App Center SDK for .NET version 1.10.0

**iOS**
* **[Fix]** Add missing network request error logging.

___

## Release 0.1.2

* **[Feature]** Added support for events `Crashes.SendingErrorReport`, `Crashes.SentErrorReport` and `Crashes.FailedToSendErrorReport` on iOS

___

## Release 0.1.1

* **[Feature]** Allow to store the SDK in directories other than "Assets/AppCenter" using the property `AppCenterContext.AppCenterPath`

___

## Release 0.1.0

* **[Fix]** Fixed Unity logs parsing that can sometimes break unhandled exceptions reporting
* **[Fix]** Fixed missing SDK logs on UWP platform
* **[Feature]** Preparation work for a future change in transmission protocol and endpoint for Analytics data. There is no impact on your current workflow when using App Center.

___

## Release 0.0.2

Removed some internal classes.
There is no impact on your current workflow when using App Center Unity SDK.

___

## Release 0.0.1

Initial release
