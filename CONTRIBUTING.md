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
You can build SDK and create Unity Packages using these two commands, one after another:
 - On Windows:

    `.\build.ps1 -Target Externals`    
    `.\build.ps1 -Target Package`
 - On Mac

    `./build.sh -target=Externals`    
    `./build.sh -target=Package`
    
The packages will be located in the `output` folder.

# Contributing
 This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.
 When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.
 This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
