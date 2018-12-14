# Release 1.2.0

App Center SDK for Unity now uses the latest native SDKs:
* App Center SDK for Android version 1.11.0
* App Center SDK for iOS version 1.12.0
* App Center SDK for .NET version 1.11.0

* **[Feature]** Add support for `Push`.
* **[Feature]** Implement `AppCenter.SetUserId` that allows users to set userId that applies to crashes, error and push logs. 
* **[Feature]** Work for a future change in transmission protocol and endpoint for Analytics data. There is no impact on your current workflow when using App Center.
* **[Bug Fix]** Fix AppCenter Analytics working incorrectly in case there's no advanced behavior but this file exists.