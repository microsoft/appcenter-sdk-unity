#addin nuget:?package=Cake.FileHelpers
#load "utility.cake"

using System.Text.RegularExpressions;

// Task TARGET for build
var Target = Argument("target", Argument("t", ""));

Task("StartNewVersion").Does(()=>
{
    var newVersion = Argument<string>("NewVersion");

    // Replace sdk version.
    UpdateSDKVersion("Assets/AppCenter/Plugins/AppCenterSDK/Core/Shared/WrapperSdk.cs", "WrapperSdkVersion", newVersion);
    UpdateSDKVersion("Assets/AppCenter/Plugins/AppCenterSDK/Analytics/Shared/Analytics.cs", "AnalyticsSDKVersion", newVersion);
    UpdateSDKVersion("Assets/AppCenter/Plugins/AppCenterSDK/Crashes/Shared/Crashes.cs", "CrashesSDKVersion", newVersion);
    UpdateSDKVersion("Assets/AppCenter/Plugins/AppCenterSDK/Distribute/Shared/Distribute.cs", "DistributeSDKVersion", newVersion);
    UpdateSDKVersion("Assets/AppCenter/Plugins/AppCenterSDK/Push/Shared/Push.cs", "PushSDKVersion", newVersion);
    
    // Replace versions in unitypackagespecs.
    var specFiles = GetFiles("UnityPackageSpecs/*.unitypackagespec");
    foreach (var spec in specFiles)
    {
        XmlPoke(spec.ToString(), "package/@version", newVersion);
    }

    // Increment app versions (they will use wrappersdkversion). The platform doesn't matter.
    ExecuteUnityMethod("BuildPuppet.SetVersionNumber", "ios");
});

// Changes the Version field in file to the given version
void UpdateSDKVersion(string path, string patternPrefix, string newVersion)
{
    var patternString = patternPrefix + " = \"[^\"]+\";";
    var newString = patternPrefix + " = \"" + newVersion + "\";";
    ReplaceRegexInFiles(path, patternString, newString);
}

RunTarget(Target);
