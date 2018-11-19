# App Center SDK for Unity Change Log

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