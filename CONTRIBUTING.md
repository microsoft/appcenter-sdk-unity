# Contributing to Visual Studio App Center SDK for Unity

## Making changes

1. Fork the repo and download it.
2. Make the necessary changes:
The main SDK code is located in the `Assets/AppCenter` folder. Inside it:
- `Plugins/Android` contains the native android SDK libraries;
- `Plugins/iOS` contains the native iOS SDK frameworks + the code to access them;
- `Plugins/WSA` contains the native .Net SDK dlls;
- `Plugins/AppCenterSDK` contains the general C# code. The classes available for use from the outside are located in the `Shared` folder. The code in this folder references the classes in platform-specific folders. For example, if you are referencing `AppCenterInternal.someMethod()` from `AppCenter.cs`, you will need to implement that code in *each* of the platform-specific files `AppCenterInternal.cs`.
- `AppCenterBehavior.cs` contains the code initializing and settings up the services.
- `AppCenterSettings.cs` contains the settings that are going to be available on App Center game object.

## Building the packages

You can build SDK and create Unity Packages using these two commands, one after another:
 - On Windows:

    `.\build.ps1 -Target Externals`    
    `.\build.ps1 -Target Package`
 - On Mac

    `./build.sh -target=Externals`    
    `./build.sh -target=Package`
    
The packages will be located in the `output` folder.

## Contributing

[Refer to the contributing section](./README.md)
