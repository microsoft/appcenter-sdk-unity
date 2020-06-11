#addin nuget:?package=Cake.FileHelpers
#addin "Cake.Json"
#addin "Cake.Http"
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

Task("UpdateCgManifestSHA").IsDependeeOf("StartNewVersion").Does(()=>
{
    try
    {
        var reposToUpdate = new List<string>(){ 
            "https://github.com/microsoft/appcenter-sdk-android.git",
            "https://github.com/microsoft/appcenter-sdk-apple.git",
            "https://github.com/microsoft/appcenter-sdk-dotnet.git"
        };
        var manifestFilePath = "cgmanifest.json";
        var gitHubApiPrefix = "https://api.github.com/repos";
        var gitHubPrefix = "https://github.com";
        var content = ParseJsonFromFile(manifestFilePath);
        var registrations = (JArray)content["Registrations"];
        foreach (var registration in registrations.Children())
        {
            var component = registration["component"];
            if (component != null) 
            {
                var typeObject = component["type"];
                if (typeObject != null && typeObject.Value<string>() == "git")
                {
                    var gitData = component["git"];
                    var repoUrl = gitData["repositoryUrl"].Value<string>();
                    if (reposToUpdate.IndexOf(repoUrl) >= 0)
                    {
                        var latestReleaseUrl = repoUrl.Replace(".git", "/releases/latest").Replace(gitHubPrefix, gitHubApiPrefix);
                        var releaseJson = HttpGet(latestReleaseUrl);
                        var releaseObject = ParseJson(releaseJson);
                        var releaseTagObject = releaseObject["tag_name"];
                        if (releaseTagObject != null)
                        {
                            var releaseTag = releaseTagObject.Value<string>();
                            var tagsUrl = repoUrl.Replace(".git", "/tags").Replace(gitHubPrefix, gitHubApiPrefix);
                            var tagsListJson = HttpGet(tagsUrl);
                            var tags = JArray.Parse(tagsListJson);
                            foreach (var tag in tags.Children())
                            {
                                if (tag["name"].Value<string>() == releaseTag)
                                {
                                    gitData["commitHash"] = tag["commit"]["sha"].Value<string>();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Warning($"Repository url: {repoUrl}. Latest release has no field 'tag_name'");
                        }
                    }
                    else
                    {
                        Warning($"Repository url: {repoUrl}. Current component should not be updated.");
                    }
                }
                else
                {
                    Warning("Current component has no field 'type' or 'type' is not 'git'.");
                }
            }
            else
            {
                Warning("Current registration has no 'component' property.");
            }
        }

        SerializeJsonToPrettyFile(manifestFilePath, content);
    }
    catch (Exception e)
    {
        Warning($"Can't update 'cgmanifest.json'. Error message: {e.Message}");
    }
});

RunTarget(Target);
