// NOTE: This cannot be run from a Mac.

#addin nuget:?package=Cake.FileHelpers
#addin nuget:?package=Cake.Git
#tool "nuget:?package=gitreleasemanager"

// Task TARGET for build
var TARGET = Argument("target", Argument("t", "Default"));
Task("Default").IsDependentOn("GitRelease");

// Create a tag and release on GitHub
Task("GitRelease")
    .Does(() =>
{
    var specFilePath = GetFiles("UnityPackageSpecs/*.unitypackagespec").First().ToString();
    var publishVersion = XmlPeek(specFilePath, "package/@version");

    var username = "user";
    var password = Argument<string>("GithubToken");
    var owner = "evgeny-pol";
    var repo = "AppCenter-SDK-Unity";

    // Create temp release file.
    System.IO.File.Create("tempRelease.md").Dispose();
    var releaseFile = File("tempRelease.md");
    FileWriteText(releaseFile, "Please update description. It will be pulled out automatically from release.md next time.");

    // Build a string containing paths to NuGet packages
    var files = GetFiles("output/*.unitypackage");
    var assets = new List<string>();
    Information("Releasing packages:");
    foreach (var file in files)
    {
        if (!file.FullPath.EndsWith("AppCenter-v" + publishVersion + ".unitypackage") &&
            !file.FullPath.EndsWith("AppCenterPush-v" + publishVersion + ".unitypackage"))
        {
            Information(file.FullPath);
            assets.Add(file.FullPath);
        }
    }
    GitReleaseManagerCreate(username, password, owner, repo, new GitReleaseManagerCreateSettings
    {
        Prerelease = false,
        Assets = string.Join(",", assets),
        TargetCommitish = "master",
        InputFilePath = releaseFile.Path.FullPath,
        Name = publishVersion
    });
    GitReleaseManagerPublish(username, password, owner, repo, publishVersion);
    DeleteFile(releaseFile);
});

RunTarget(TARGET);
