#addin nuget:?package=Cake.FileHelpers
#addin nuget:?package=Cake.Git
#load "utility.cake"

using System.Text.RegularExpressions;

// Task TARGET for build
var Target = Argument("target", Argument("t", ""));

Task("StartNewVersion").Does(()=>
{
    var newVersion = Argument<string>("NewVersion");

    // Replace wrapper sdk version.
    UpdateWrapperSdkVersion(newVersion);

    // Replace versions in unitypackagespecs.
    var specFiles = GetFiles("UnityPackageSpecs/*.unitypackagespec");
    foreach (var spec in specFiles)
    {
        XmlPoke(spec.ToString(), "package/@version", newVersion);
    }

    // Increment app versions (they will use wrappersdkversion). The platform doesn't matter.
    ExecuteUnityMethod("BuildPuppet.SetVersionNumber", "ios");
    ExecuteUnityMethod("BuildDemo.SetVersionNumber", "ios", "AppCenterDemoApp");
});

// Changes the Version field in WrapperSdk.cs to the given version
void UpdateWrapperSdkVersion(string newVersion)
{
    var path = "Assets/AppCenter/Plugins/AppCenterSDK/Core/Shared/WrapperSdk.cs";
    var patternString = "WrapperSdkVersion = \"[^\"]+\";";
    var newString = "WrapperSdkVersion = \"" + newVersion + "\";";
    ReplaceRegexInFiles(path, patternString, newString);
}

RunTarget(Target);
