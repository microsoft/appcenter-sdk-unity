# Contributing to Visual Studio App Center SDK for Unity

## Making changes
1. Fork the repo and download it.
2. Using `Android Studio`, open android project located in folder `AppCenterLoaderApp`, download any missing SDK’s and click `Sync Project with Gradle Files` button (you might need to open some gradle file for it to become active).
3. Make the necessary changes:
The main SDK code is located in the `Assets/AppCenter` folder. Inside it:
- `Plugins/Android` contains the native android SDK libraries;
- `Plugins/iOS` contains the native iOS SDK frameworks + the code to access them;
- `Plugins/WSA` contains the native .Net SDK dlls;
- `Plugins/AppCenterSDK` contains the general C# code. The classes available for use from the outside are located in the `Shared` folder. The code in this folder references the classes in platform-specific folders. For example, if you are referencing `AppCenterInternal.someMethod()` from `AppCenter.cs`, you will need to implement that code in *each* of the platform-specific files `AppCenterInternal.cs`.
- `AppCenterBehavior.cs` contains the code initializing and settings up the services.
- `AppCenterSettings.cs` contains the settings that are going to be available on App Center game object.

## Building the packages
1. Use existing or [create](https://msmobilecenter.visualstudio.com/_usersSettings/tokens) new VSTS token with scopes Release (read) and Packaging (read).
2. Before running the build script set the following environment variables in the console (replace _VSTS_PAT_ with your token value):
- On Windows

    `$Env:NUGET_USER="mobilecenter"`    
    `$Env:NUGET_FEED_ID="56ee7f9f-bc95-4d96-bce5-11b0d8ff66d6"`
    `$Env:NUGET_PASSWORD="_VSTS_PAT_"`
 - On Mac

    `export NUGET_USER=mobilecenter`    
    `export NUGET_FEED_ID=56ee7f9f-bc95-4d96-bce5-11b0d8ff66d6`
    `export NUGET_PASSWORD=_VSTS_PAT_`
3. Build SDK and create Unity Packages using these two commands, one after another:
 - On Windows:

    `.\build.ps1`    
    `.\build.ps1 -Target Package`
 - On Mac

    `./build.sh`    
    `./build.sh -target=Package`
    
The packages will be located in the `output` folder.