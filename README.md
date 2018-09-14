
# Introduction
The App Center Unity SDK enables you to use App Center Analytics, Crash and Distribution within your Unity games. 

# Getting Started

## 1. Prerequisites

Before you begin, please make sure that your project is set up in Unity 2017.1 or later.

The App Center SDK for Unity supports the following platforms:

* iOS (9.0 or later)
* Android (5.0/API 21 or later)
* UWP (Build 10240 or later)

Please also note that the App Center SDK for Unity is only available in C#.

## 2. Create your app in the App Center Portal to obtain the App Secret

If you have already created your app in the App Center portal, you can skip this step.

1. Head over to [appcenter.ms](https://appcenter.ms).
2. Sign up or log in and click the blue button on the top right corner of the portal that says **Add new** and select **Add new app** from the dropdown menu.
3. Enter a name and an optional description for your app.
4. Select the appropriate OS and platform depending on your project as described above.
5. Click the **Add new app** button in the bottom-right of the page.

Once you have created an app, you can obtain its **App Secret** on the **Getting Started** or **Manage App** sections of the App Center Portal.

## 3. Add the App Center SDK to your project

The App Center SDK is integrated by importing Unity Packages into your project.

### 3.1 Download the package(s)

The App Center Unity packages are downloaded from the [releases tab](https://github.com/Microsoft/AppCenter-SDK-Unity/releases). Download the package(s) that you want to use, from the latest release.

### 3.2 Import the package

Open your Unity project, then double-click each of the Unity packages you downloaded. A pop-up window should appear in your Unity project containing a list of files. Select **Import**, and the SDK will be added to your project. Do this for each package you downloaded and plan to use with your project.

## 4. Enable the SDK

### 4.1 Create an empty Game Object

App Center works as a component that you attach to a game object in the scene that your game launches into. Navigate to this scene and add an empty game object. Give it a descriptive name, such as "App Center".

### 4.2 Attach the App Center script

In the **Project** window, navigate to the "AppCenter" folder that was added to your project. Locate the script whose icon is the App Center logo, named *AppCenterBehavior*, and drag it onto your newly created game object in the **Hierarchy** window.

> **Note:** You do not need to add App Center to every scene in which you wish to use it. Adding it to the first loaded scene is enough.

### 4.3 Configure App Center settings

Click on the new "App Center" object and add your app secrets to the corresponding fields in the **Inspector** window. Make sure to also check the "Use {service}" boxes for each App Center service you intend to use

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