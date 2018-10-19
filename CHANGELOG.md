# Release 0.1.3

App Center SDK for Unity now uses the latest native SDKs:
* App Center SDK for Android version 1.9.0
* App Center SDK for iOS version 1.10.0
* App Center SDK for .NET version 1.10.0

**iOS**
* **[Fix]** Add missing network request error logging.

# Release 0.1.2

* **[Feature]** Added support for events `Crashes.SendingErrorReport`, `Crashes.SentErrorReport` and `Crashes.FailedToSendErrorReport` on iOS

# Release 0.1.1

* **[Feature]** Allow to store the SDK in directories other than "Assets/AppCenter" using the property `AppCenterContext.AppCenterPath`

# Release 0.1.0

* **[Fix]** Fixed Unity logs parsing that can sometimes break unhandled exceptions reporting
* **[Fix]** Fixed missing SDK logs on UWP platform
* **[Feature]** Preparation work for a future change in transmission protocol and endpoint for Analytics data. There is no impact on your current workflow when using App Center.

# Release 0.0.2

Removed some internal classes.
There is no impact on your current workflow when using App Center Unity SDK.

# Release 0.0.1

Initial release