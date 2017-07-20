#tool nuget:?package=XamarinComponent
#addin nuget:?package=Cake.Xamarin
#addin nuget:?package=Cake.FileHelpers
#addin "Cake.AzureStorage"
#addin nuget:?package=Cake.Git

using System.Net;

// Prefix for temporary intermediates that are created by this script
var TEMPORARY_PREFIX = "CAKE_SCRIPT_TEMP";

// Native SDK versions
var ANDROID_SDK_VERSION = "0.11.1";
var IOS_SDK_VERSION = "0.11.0";
var UWP_SDK_VERSION = "0.14.0";

// URLs for downloading binaries.
/*
 * Read this: http://www.mono-project.com/docs/faq/security/.
 * On Windows,
 *     you have to do additional steps for SSL connection to download files.
 *     http://stackoverflow.com/questions/4926676/mono-webrequest-fails-with-https
 *     By running mozroots and install part of Mozilla's root certificates can make it work. 
 */

var SDK_STORAGE_URL = "https://mobilecentersdkdev.blob.core.windows.net/sdk/";
var ANDROID_URL = SDK_STORAGE_URL + "MobileCenter-SDK-Android-" + ANDROID_SDK_VERSION + ".zip";
var IOS_URL = SDK_STORAGE_URL + "MobileCenter-SDK-iOS-" + IOS_SDK_VERSION + ".zip";

// Task TARGET for build
var TARGET = Argument("target", Argument("t", "Default"));

// Available MobileCenter modules.
// MobileCenter module class definition.
class MobileCenterModule {
    public string AndroidModule { get; set; }
    public string IosModule { get; set; }
    public string DotNetModule { get; set; }
    public string Moniker { get; set; }
    public bool UWPHasNativeCode { get; set; }
    public string[] NativeArchitectures { get; set; }

    public MobileCenterModule(string android, string ios, string dotnet, string moniker, bool hasNative = false) {
        AndroidModule = android;
        IosModule = ios;
        DotNetModule = dotnet;
        Moniker = moniker;
        UWPHasNativeCode = hasNative;
        if (hasNative)
        {
            NativeArchitectures = new string[] {"x86", "x64", "arm"};
        }
    }
}

class UnityPackage
{
    public static ICakeContext Context;
    public UnityPackage(string specFilePath)
    {
        _packageName = Context.XmlPeek(specFilePath, "package/@name");
        if (_packageName == null)
        {
            Context.Error("Invalid format for UnityPackageSpec file '" + specFilePath + "': missing package name");
            return;
        }

        var xpathPrefix = "/package/include/file[";
        var xpathSuffix= "]/@path";

        string lastPath = Context.XmlPeek(specFilePath, xpathPrefix + "last()" + xpathSuffix);
        var currentIdx = 1;
        var currentPath =  Context.XmlPeek(specFilePath, xpathPrefix + currentIdx++ + xpathSuffix);

        if (currentPath != null)
        {
            _includePaths.Add(currentPath);
        }
        while (currentPath != lastPath)
        {
            currentPath =  Context.XmlPeek(specFilePath, xpathPrefix + currentIdx++ + xpathSuffix); 
            _includePaths.Add(currentPath);
        }
    }

    public void CreatePackage(string targetDirectory)
    {
        var exec = Context.EnvironmentVariable("UNITY_PATH");
        var projectDir = Context.MakeAbsolute(Context.Directory("."));
        var args = "-batchmode -quit -projectPath " + projectDir + " -exportPackage ";
        foreach (var path in _includePaths)
        {
            args += " " + path;
        }
        args += " " + targetDirectory + "/" + _packageName;
        var result = Context.StartProcess(exec, args);
        if (result != 0)
        {
            Context.Error("Something went wrong while creating cake package '" + _packageName + "'");
        }
    }

    private string _packageName;
    private List<string> _includePaths = new List<string>();
}


var MOBILECENTER_MODULES = new [] {
    new MobileCenterModule("mobile-center-release.aar", "MobileCenter.framework", "Microsoft.Azure.Mobile", "Core"),
    new MobileCenterModule("mobile-center-analytics-release.aar", "MobileCenterAnalytics.framework", "Microsoft.Azure.Mobile.Analytics", "Analytics"),
    new MobileCenterModule("mobile-center-crashes-release.aar", "MobileCenterCrashes.framework", "Microsoft.Azure.Mobile.Crashes", "Crashes", true),
    new MobileCenterModule("mobile-center-distribute-release.aar", "MobileCenterDistribute.framework", "Microsoft.Azure.Mobile.Distribute", "Distribute"),
    new MobileCenterModule("mobile-center-push-release.aar", "MobileCenterPush.framework", "Microsoft.Azure.Mobile.Push", "Push")	
};

// Downloading Android binaries.
Task("Externals-Android")
    .Does(() => 
{
    CleanDirectory("./externals/android");

    // Download zip file.
    DownloadFile(ANDROID_URL, "./externals/android/android.zip");
    Unzip("./externals/android/android.zip", "./externals/android/");

    // Copy files
    foreach (var module in MOBILECENTER_MODULES)
    {
        var files = GetFiles("./externals/android/*/" + module.AndroidModule);
        CopyFiles(files, "Assets/Plugins/Android/");
    }
}).OnError(HandleError);

// Downloading iOS binaries.
Task("Externals-Ios")
    .Does(() =>
{
    CleanDirectory("./externals/ios");

    // Download zip file containing MobileCenter frameworks
    DownloadFile(IOS_URL, "./externals/ios/ios.zip");
    Unzip("./externals/ios/ios.zip", "./externals/ios/");

    // Copy files
    foreach (var module in MOBILECENTER_MODULES)
    {
        var destinationFolder = "Assets/Plugins/iOS/" + module.Moniker + "/" + module.IosModule;
        DeleteDirectoryIfExists(destinationFolder);
        MoveDirectory("./externals/ios/MobileCenter-SDK-iOS/" + module.IosModule, "Assets/Plugins/iOS/" + module.Moniker + "/" + module.IosModule);
    }
}).OnError(HandleError);

// Downloading UWP binaries.
Task("Externals-Uwp")
    .Does(() =>
{	
    CleanDirectory("externals/uwp");
    // Download the nugets. We will use these to extract the dlls
    foreach (var module in MOBILECENTER_MODULES)
    {
        if (module.Moniker == "Distribute")
        {
            Warning("Skipping 'Distribute' for UWP.");
            continue;
        }
        Information("Downloading " + module.DotNetModule + "...");
        // Download nuget package
        var nupkgPath = GetNuGetPackage(module.DotNetModule, UWP_SDK_VERSION);

        var tempContentPath = "externals/uwp/" + module.Moniker + "/";
        DeleteDirectoryIfExists(tempContentPath);
        // Unzip into externals/uwp/
        Unzip(nupkgPath, tempContentPath);
        // Delete the package
        DeleteFiles(nupkgPath);

        var contentPathSuffix = "lib/uap10.0/";

        // Prepare destination
        var destination = "Assets/Plugins/UWP/" + module.Moniker + "/";
        DeleteFiles(destination + "*.dll");
        DeleteFiles(destination + "*.winmd");
        
        // Deal with any native components
        if (module.UWPHasNativeCode)
        {
            foreach (var arch in module.NativeArchitectures)
            {
                var dest = "Assets/Plugins/UWP/" + module.Moniker + "/" + arch + "/";
                var nativeFiles = GetFiles(tempContentPath + "runtimes/" + "win10-" + arch + "/native/*");
                DeleteFiles(dest + "*.dll");
                MoveFiles(nativeFiles, dest);
            }
            // Use managed runtimes from one of the architecture for all architectures.
            // Even though they are architecture dependent, Unity converts
            // them to AnyCPU automatically
            contentPathSuffix = "runtimes/win10-" + module.NativeArchitectures[0] + "/" + contentPathSuffix;
        }

        // Move the files to the proper location
        var files = GetFiles(tempContentPath + contentPathSuffix + "*");
        MoveFiles(files, destination);
    }

}).OnError(HandleError);

// Create a common externals task depending on platform specific ones
Task("Externals").IsDependentOn("Externals-Ios").IsDependentOn("Externals-Android").IsDependentOn("Externals-Uwp").Does(()=>
{
    DeleteDirectoryIfExists("externals");
});

// Creates Unity packages corresponding to all ".unitypackagespec" files
// in "UnityPackageSpecs" folder
Task("Package").Does(()=>
{
    // Need to provide cake context so class methods can use cake apis
    UnityPackage.Context = Context;

    // Store packages in a clean folder
    const string outputDirectory = "output";
    CleanDirectory(outputDirectory);
    var specFiles = GetFiles("UnityPackageSpecs/*.unitypackagespec");
    foreach (var spec in specFiles)
    {
        var package = new UnityPackage(spec.FullPath);
        package.CreatePackage(outputDirectory);
    }
});

// Creates Unity packages corresponding to all ".unitypackagespec" files
// in "UnityPackageSpecs" folder (and downloads binaries)
Task("CreatePackages").IsDependentOn("Externals").IsDependentOn("Package");

// Default Task.
Task("Default").IsDependentOn("Externals");

// Remove all temporary files and folders
Task("RemoveTemporaries").Does(()=>
{
    DeleteFiles(TEMPORARY_PREFIX + "*");
    var dirs = GetDirectories(TEMPORARY_PREFIX + "*");
    foreach (var directory in dirs)
    {
        DeleteDirectory(directory, true);
    }
    DeleteFiles("./nuget/*.temp.nuspec");
});

// Clean up files/directories.
Task("clean")
    .IsDependentOn("RemoveTemporaries")
    .Does(() =>
{
    DeleteDirectoryIfExists("externals");
    DeleteDirectoryIfExists("output");
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
});

// Copy files to a clean directory using string names instead of FilePath[] and DirectoryPath
void CopyFiles(IEnumerable<string> files, string targetDirectory, bool clean = true)
{
    if (clean)
    {
        CleanDirectory(targetDirectory);
    }
    foreach (var file in files)
    {
        CopyFile(file, targetDirectory + "/" + System.IO.Path.GetFileName(file));
    }
}

void DeleteDirectoryIfExists(string directoryName)
{
    if (DirectoryExists(directoryName))
    {
        DeleteDirectory(directoryName, true);	
    }
}

void CleanDirectory(string directoryName)
{
    DeleteDirectoryIfExists(directoryName);
    CreateDirectory(directoryName);
}

void HandleError(Exception exception)
{
    RunTarget("clean");
    throw exception;
}

string GetNuGetPackage(string packageId, string packageVersion)
{	
    var nugetUser = EnvironmentVariable("NUGET_USER");
    var nugetPassword = Argument("NuGetPassword", EnvironmentVariable("NUGET_PASSWORD"));
    var nugetFeedId = Argument("NuGetFeedId", EnvironmentVariable("NUGET_FEED_ID"));
    packageId = packageId.ToLower();
    
    var url = "https://msmobilecenter.pkgs.visualstudio.com/_packaging/";
    url += nugetFeedId + "/nuget/v3/flat2/" + packageId + "/" + packageVersion + "/" + packageId + "." + packageVersion + ".nupkg";
    
    // Get the NuGet package
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
    request.Headers["X-NuGet-ApiKey"] = nugetPassword;
    request.Credentials = new NetworkCredential(nugetUser, nugetPassword);
    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    var responseString = String.Empty;
    var filename = packageId + "." + packageVersion +  ".nupkg";
    using (var fstream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
    {
        response.GetResponseStream().CopyTo(fstream);
    }
    return filename;
}

RunTarget(TARGET);
