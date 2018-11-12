# Release 1.0.0

**Analytics**

* [Feature] Preparation work for a future change in transmission protocol and endpoint for Analytics data. There is no impact on your current workflow when using App Center.

**UWP**

* **[Bug fix]** Fix build errors when building the `UWP` app with `.Net` scripting backend
* **[Bug fix]** Fix issue with reporting analytics events from `Start` method
* **[Bug fix]** Automatically add the `InternetAccess` capability when building `UWP` apps in order for analytics to work properly

**Android**

* **[Bug fix]** Fix error occuring when trying to send crash report
* **[Bug fix]** Fix performance issue caused by exceptions reporting logic