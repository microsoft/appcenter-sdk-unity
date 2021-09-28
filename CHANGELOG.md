# App Center SDK for Unity Change Log

## Version 4.3.1 (Under active development)

### App Center

* **[Breaking change]** Remove `AppCenter.SetCustomProperties` API.

___

## Version 4.3.0

### AppCenter

#### UWP

* **[Fix]** Fix sending pending logs after the first application start.

#### iOS/Android

* **[Feature]** Improved `AES` token encryption algorithm using `Encrypt-then-MAC` data authentication approach.

### App Center Crashes

#### iOS

* **[Improvement]** Update PLCrashReporter to 1.10.

### App Center Distribute

#### Android

* **[Fix]** Fix a rare deadlock case when a new version starts downloading and at the same moment the download status is checked.
* **[Fix]** Fix passing pending intent flag for a completed download notification on Android lower then 23 API.

___

## Version 4.2.0

### AppCenter

* **[Feature]** Add a `AppCenter.IsNetworkRequestsAllowed` API to block any network requests without disabling the SDK.
* **[Fix]** Remove `android:allowBackup` and `android:supportsRtl` from `AndroidManifest.xml` in `appcenter-loader`, to prevent these attributes from merging into the final `AndroidManifest.xml` in a client app.

#### UWP

* **[Fix]** Fix infinite loop when old logs cannot be purged by a new one with a different channel name in a case when the storage is full.

### App Center Crashes

#### iOS

* **[Fix]** Merge the device information from the crash report with the SDK's device information in order to fix some time sensitive cases where the reported application information was incorrect.
* **[Improvement]** Update PLCrashReporter to 1.9.0.

### App Center Distribute

#### Android

* **[Fix]** Fix crash during downloading a new release when `minifyEnabled` settings is true.
* **[Fix]** Add a missing tag `android:exported` to the manifest required for Android 12.
* **[Fix]** Fix ARR modules unlinking which worked correctly only if the project is built twice due to link/unlink logic stored in PostBuild. Logic has been moved to the PreBuild.

___

## Version 4.1.1

### AppCenter

#### iOS

* **[Improvement]** Use ASWebAuthenticationSession for authentication on iOS 12 or later.

### App Center Crashes

#### Android

* **[Fix]** Fix formatting of stack trace in the `ErrorReport`.

### App Center Distribute

#### Android

* **[Fix]** Fix `NullPointerException` occurring when settings dialog was intended to be shown, but there is no foreground activity at that moment.
* **[Fix]** Fix a crash when download manager application was disabled.
* **[Fix]** Fix showing the title in the push notification while downloading a new release.

#### iOS

* **[Fix]** Fix `kMSACUpdateTokenRequestIdKey` never gets removed.

___

## Release 4.1.0

### AppCenter

#### UWP

* **[Feature]** Add a `SetMaxStorageSizeAsync` API which allows setting a maximum size limit on the local SQLite storage. The default value is 10MiB.

#### iOS

* **[Fix]** Fix a crash when SQLite returns zero for `page_size`.

### App Center Distribute

#### iOS/Android

* **[Feature]** Add `NoReleaseAvailable` callback to distribute listener.
* **[Fix]** Fix show the custom dialog update after the application start.

#### iOS

* **[Feature]** Add `WillExitApp` callback to distribute listener.

#### Android

* **[Fix]** Fix a crash when the app is trying to open the system settings screen from the background.
* **[Fix]** Fix browser opening when using a private distribution group on Android 11.

### App Center Crashes

#### Android

* **[Fix]** Fix removing throwable files after rewriting error logs due to small database size.

___

## Release 4.0.0

### App Center

#### Android

* **[Breaking change]** Bumping the minimum Android SDK version to 21 API level (Android 5.0), because old Android versions do not support root certificate authority used by App Center and would not get CA certificates updates anymore.

#### iOS

* **[Fix]** Fix `NSInvalidArgumentException` when using non-string object as a key in `NSUserDefaults`.
* **[Fix]** Fix `NSDateFormatter` initialization in a concurrent environment.

### App Center Analytics

* **[Fix]** Fix naming conflict with iOS 14 private Apple framework.

### App Center Crashes

#### iOS

* **[Improvement]** Update PLCrashReporter to 1.8.0.

### App Center Push

App Center Push has been removed from the SDK and will be [retired on December 31st, 2020](https://devblogs.microsoft.com/appcenter/migrating-off-app-center-push/). 
As an alternative to App Center Push, we recommend you migrate to [Azure Notification Hubs](https://docs.microsoft.com/en-us/azure/notification-hubs/notification-hubs-push-notification-overview) by following the [Push Migration Guide](https://docs.microsoft.com/en-us/appcenter/migration/push/).

___

## Release 3.3.1

### App Center

#### iOS

* **[Fix]** Fix overriding custom `URLScheme` in the post-build script.

### App Center Analytics

#### iOS

* **[Fix]** Fix processing logs (e.g., events) emitted from the `applicationWillTerminate` application delegate method.

### App Center Crashes

#### Android/iOS

* **[Fix]** Fix sending attachments with a `null` text value.

#### iOS

* **[Improvement]** Update PLCrashReporter to 1.7.2.

### App Center Distribute

#### iOS

* **[Fix]** Fix manually checking for updates before `applicationDidBecomeActive` event.

___

## Release 3.3.0

This version has a breaking change on iOS - it drops Xcode 10 support, Xcode 11 is a minimal supported version now.

### App Center

#### Android

* **[Fix]** Fix possible delays in UI thread when queueing a large number of events.
* **[Fix]** Fix an `IncorrectContextUseViolation` warning when calculating screen size on Android 11.
* **[Fix]** All SQL commands used in SDK are presented as raw strings to avoid any possible static analyzer's SQL injection false alarms.

#### iOS

* **[Fix]** Fix crash when local binary data (where unsent logs or unprocessed crashed are stored) is corrupted.
* **[Fix]** When carrier name is retrieved incorrectly by iOS, show `nil` as expected instead of "carrier" string.

### App Center Analytics

#### iOS

* **[Fix]** Fix `Analytics.TrackEvent` crash if event properties contain `null` value.

### App Center Crashes

#### iOS

* **[Improvement]** Update PLCrashReporter to 1.7.1.
* **[Fix]** Fix reporting stacktraces on iOS simulator.
* **[Fix]** Add attachments verification in `TrackError` function to handle explicitly passed `null` array to variable arguments parameter.

### App Center Distribute

Now when Distribute is turned off in **AppCenterBehavior**, it is unlinked from the application, in order to avoid Google Play flagging the application for malicious behavior. It must be turned off for build variants which are going to be published on Google Play. See the [public documentation](https://docs.microsoft.com/en-us/appcenter/sdk/distribute/unity#remove-in-app-updates-for-google-play-builds) for more details about this change.

#### Android

* **[Fix]** Fix Distribute can't get updates for Realme devices which use Realme UI.

#### iOS

* **[Fix]** Obfuscate app secret value that appears as URI part in verbose logs when getting release updates info.

___

## Release 3.2.1

### App Center

#### iOS

* **[Fix]** Fix reporting crashes caused by a thread exception.
* **[Improvement]** Use namespaced `NSUserDefaults` keys with the **MSAppCenter** prefix for all the keys set by the SDK. Fixed a few keys missing namespace.

### App Center Crashes

#### iOS

* **[Improvement]** Update PLCrashReporter to 1.6.0.

___

## Release 3.2.0

### App Center

* **[Fix]** Fix SDK doesn't work without `Distribute` package.

#### iOS

* **[Fix]** Add missing system dependencies that aren't implicitly included when `APPCENTER_DONT_USE_NATIVE_STARTER` flag is used.

#### UWP

* **[Fix]** Fix retry sending logs after timeout exception.

### App Center Crashes

* **[Fix]** Remove the multiple attachments warning as that is now supported by the portal.

#### Android

* **[Fix]** Change minidump filter to use file extension instead of name.
* **[Fix]** Fix removing minidump files when the sending crash report was discarded.

#### iOS

* **[Improvement]** Update PLCrashReporter to 1.5.1.

### App Center Distribute

#### Android

* **[Feature]** Automatically check for update when application switches from background to foreground (unless automatic checks are disabled).
* **[Fix]** Fix checking for updates after disabling the Distribute module while downloading the release.

___

## Release 3.1.0

### App Center

* **[Breaking change]** Bump the minimal supported Unity version from **5.6** to **2018.1**.

#### Android

* **[Fix]** Fix a Unity warning that appears when building for Android.
* **[Fix]** Fix a `java.lang.IllegalArgumentException: Unknown pattern character 'X'` in `JavaHelper.JavaDateFormatter` on Android 6.
* **[Fix]** Fix infinite recursion when handling encryption errors.

#### iOS

* **[Fix]** Fix converting string with the Unicode symbols (for example in push notification).
* **[Fix]** Optimization of release objects from memory during the execution of a large number of operations.
* **[Fix]** Disable module debugging for release mode in the SDK to fix dSYM warnings.
* **[Fix]** Fix SDK crash at application launch on iOS 12.0 (`CTTelephonyNetworkInfo.serviceSubscriberCellularProviders` issue).
* **[Fix]** The SDK was considering 201-299 status code as HTTP errors and is now fixed to accept all 2XX codes as successful.
* **[Improvement]** Replaced sqlite query concatenation with more secure bindings.

#### UWP

* **[Feature]** Support **ARM64** architecture on Unity 2019.x or above.
* **[Breaking change]** Native UWP libraries require .NETStandard now, therefore .NET 3.5 scripting runtime version and .NET scripting backend are no longer supported.
* **[Fix]** Fix the bug that caused **Install-Id** to be generated every time the application was launched.

### App Center Auth

App Center Auth is [retired](https://aka.ms/MBaaS-retirement-blog-post) and has been removed from the SDK.

### App Center Crashes

#### iOS

* **[Fix]** Fix an issue where Crashes could not be started because `StartCrashes` method was stripped for a high managed stripping level.

### App Center Distribute

* **[Feature]** Add `UpdateTrack` property to be able to explicitly set either `Private` or `Public` update track. By default, a public distribution group is used. **Breaking change**: To allow users to access releases of private groups you now need to migrate your application to call `Distribute.UpdateTrack = UpdateTrack.Private` before the SDK start. Please read the documentation for more details.
* **[Behavior change]** The public distribution is simplified to provide only one public group. If you have existing public groups defined for your application your users will receive the latest version of all public groups.
* **[Feature]** Add a **Automatic Check For Update** checkbox that can be unchecked to turn off automatic check for update.
* **[Feature]** Add a `CheckForUpdate` API to manually check for update.

#### iOS

* **[Fix]** Fix a crash when `SFAuthenticationSession` accesses the controller which is in the process of being released.
* **[Fix]** Fix sign-in when switching to third-party apps while activating updates.

#### Android

* **[Fix]** Avoid opening browser to check for sign-in information after receiving an SSL error while checking for app updates (which often happens when using a public WIFI).
* **[Fix]** When in-app update permissions become invalid and need to open browser again, updates are no longer postponed after sign-in (if user previously selected the action to postpone for a day).
* **[Fix]** Fix a possible deadlock when activity resumes during background operation for some `Distribute` public APIs like `Distribute.isEnabled()`.

### App Center Push

* **[Bug fix]** Fix triggering the notification event when restarting the application from a notification.

#### iOS

* **[Fix]** Fix an issue where Push could not be started because `StartPush` method was stripped for a high managed stripping level.

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

* **[Fix]** Fixed `async`/`await` operators support by SDK's methods for .NETStandard 2.0 profile.
* **[Fix]** Fixed iOS application crash when trying to pass exception without stack trace to `Crashes.TrackError`.

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
