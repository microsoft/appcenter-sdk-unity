#tool nuget:?package=XamarinComponent
#addin nuget:?package=Cake.Xamarin
#addin nuget:?package=Cake.FileHelpers
#addin "Cake.AzureStorage"
#addin nuget:?package=Cake.Git
#addin nuget:?package=NuGet.Core

using System.Net;
using System.Collections.Generic;
using System.Runtime.Versioning;
using NuGet;

// Prefix for temporary intermediates that are created by this script
var TemporaryPrefix = "CAKE_SCRIPT_TEMP";

// Native SDK versions
var AndroidSdkVersion = "0.11.1";
var IosSdkVersion = "0.11.0";
var UwpSdkVersion = "0.14.0";

// URLs for downloading binaries.
/*
 * Read this: http://www.mono-project.com/docs/faq/security/.
 * On Windows,
 *     you have to do additional steps for SSL connection to download files.
 *     http://stackoverflow.com/questions/4926676/mono-webrequest-fails-with-https
 *     By running mozroots and install part of Mozilla's root certificates can make it work. 
 */

var SdkStorageUrl = "https://mobilecentersdkdev.blob.core.windows.net/sdk/";
var AndroidUrl = SdkStorageUrl + "MobileCenter-SDK-Android-" + AndroidSdkVersion + ".zip";
var IosUrl = SdkStorageUrl + "MobileCenter-SDK-iOS-" + IosSdkVersion + ".zip";

var MobileCenterModules = new [] {
    new MobileCenterModule("mobile-center-release.aar", "MobileCenter.framework", "Microsoft.Azure.Mobile", "Core"),
    new MobileCenterModule("mobile-center-analytics-release.aar", "MobileCenterAnalytics.framework", "Microsoft.Azure.Mobile.Analytics", "Analytics"),
    new MobileCenterModule("mobile-center-crashes-release.aar", "MobileCenterCrashes.framework", "Microsoft.Azure.Mobile.Crashes", "Crashes", true),
    new MobileCenterModule("mobile-center-distribute-release.aar", "MobileCenterDistribute.framework", "Microsoft.Azure.Mobile.Distribute", "Distribute"),
    new MobileCenterModule("mobile-center-push-release.aar", "MobileCenterPush.framework", "Microsoft.Azure.Mobile.Push", "Push")
};

// External Unity Packages
var JarResolverPackageName =  "play-services-resolver-" + ExternalUnityPackage.VersionPlaceholder + ".unitypackage";
var JarResolverVersion = "1.2.35.0";
var JarResolverUrl = SdkStorageUrl + ExternalUnityPackage.NamePlaceholder;

var NewtonsoftJsonPackageName = "JsonNet." + ExternalUnityPackage.VersionPlaceholder + ".unitypackage";
var NewtonsoftJsonVersion = "9.0.1";
var NewtonsoftJsonUrl = "https://github.com/SaladLab/Json.Net.Unity3D/releases/download/v" + ExternalUnityPackage.VersionPlaceholder + "/" + ExternalUnityPackage.NamePlaceholder;

var ExternalUnityPackages = new [] {
    new ExternalUnityPackage(JarResolverPackageName, JarResolverVersion, JarResolverUrl),
    new ExternalUnityPackage(NewtonsoftJsonPackageName, NewtonsoftJsonVersion, NewtonsoftJsonUrl)
};

// UWP dependencies
var SqlitePackageName = "sqlite-net-pcl";
var SqliteVersion = "1.3.1";

var UwpDependencies = new Dictionary<string, string> {
    { SqlitePackageName, SqliteVersion }
};

// Task TARGET for build
var Target = Argument("target", Argument("t", "Default"));

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

class ExternalUnityPackage
{
    public static string VersionPlaceholder = "<version>";
    public static string NamePlaceholder = "<name>";

    public string Name { get; private set; }
    public string Version { get; private set; }
    public string Url { get; private set; }

    public ExternalUnityPackage(string name, string version, string url)
    {
        Version = version;
        Name = name.Replace(VersionPlaceholder, Version);
        Url = url.Replace(NamePlaceholder, Name).Replace(VersionPlaceholder, Version);
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
        var args = "-exportPackage ";
        foreach (var path in _includePaths)
        {
            args += " " + path;
        }
        args += " " + targetDirectory + "/" + _packageName;
        var result = ExecuteUnityCommand(args, Context);
        if (result != 0)
        {
            Context.Error("Something went wrong while creating cake package '" + _packageName + "'");
        }
    }

    private string _packageName;
    private List<string> _includePaths = new List<string>();
}

// Downloading Android binaries.
Task("Externals-Android")
    .Does(() => 
{
    CleanDirectory("./externals/android");

    // Download zip file.
    DownloadFile(AndroidUrl, "./externals/android/android.zip");
    Unzip("./externals/android/android.zip", "./externals/android/");

    // Copy files
    foreach (var module in MobileCenterModules)
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
    DownloadFile(IosUrl, "./externals/ios/ios.zip");
    Unzip("./externals/ios/ios.zip", "./externals/ios/");

    // Copy files
    foreach (var module in MobileCenterModules)
    {
        var destinationFolder = "Assets/Plugins/iOS/" + module.Moniker + "/" + module.IosModule;
        DeleteDirectoryIfExists(destinationFolder);
        MoveDirectory("./externals/ios/MobileCenter-SDK-iOS/" + module.IosModule, "Assets/Plugins/iOS/" + module.Moniker + "/" + module.IosModule);
    }
}).OnError(HandleError);

// Downloading UWP binaries.
Task("Externals-Uwp")
    .IsDependentOn("Externals-Uwp-Dependencies")
    .Does(() =>
{
    CleanDirectory("externals/uwp");

    // Download the nugets. We will use these to extract the dlls
    foreach (var module in MobileCenterModules)
    {
        if (module.Moniker == "Distribute")
        {
            Warning("Skipping 'Distribute' for UWP.");
            continue;
        }
        Information("Downloading " + module.DotNetModule + "...");
        // Download nuget package
        var nupkgPath = GetNuGetPackage(module.DotNetModule, UwpSdkVersion);

        var tempContentPath = "externals/uwp/" + module.Moniker + "/";
        DeleteDirectoryIfExists(tempContentPath);
        // Unzip into externals/uwp/
        Unzip(nupkgPath, tempContentPath);
        // Delete the package
        DeleteFiles(nupkgPath);

        var contentPathSuffix = "lib/uap10.0/";

        // Prepare destination
        var destination = "Assets/Plugins/WSA/" + module.Moniker + "/";
        DeleteFiles(destination + "*.dll");
        DeleteFiles(destination + "*.winmd");
        
        // Deal with any native components
        if (module.UWPHasNativeCode)
        {
            foreach (var arch in module.NativeArchitectures)
            {
                var dest = "Assets/Plugins/WSA/" + module.Moniker + "/" + arch.ToString().ToUpper() + "/";
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

// Downloading UWP dependencies.
Task("Externals-Uwp-Dependencies")
    .Does(() =>
{
    var targetPath = "Assets/Plugins/WSA";
    var frameworkName = new FrameworkName("UAP, Version=v10.0");
    var packageSource = "https://www.nuget.org/api/v2/";
    var dependencies = new List<IPackage>();
    foreach (var i in UwpDependencies)
    {
        dependencies.AddRange(GetNuGetDependencies(i.Key, i.Value, packageSource, frameworkName));
    }
    ExtractNuGetPackages(dependencies, targetPath, frameworkName);
}).OnError(HandleError);

// Download and install all external Unity packages required
Task("Externals-Unity-Packages").Does(()=>
{
    var directoryName = "externals/unity-packages";
    CleanDirectory(directoryName);
    foreach (var package in ExternalUnityPackages)
    {
        var destination = directoryName + "/" + package.Name;
        DownloadFile(package.Url, destination);
        var command = "-importPackage " + destination;
        Information("Importing package " + package.Name + ". This could take a minute.");
        ExecuteUnityCommand(command, Context);
    }
}).OnError(HandleError);

// Create a common externals task depending on platform specific ones
// NOTE: It is important to execute Externals-Unity-Packages *last* or the step in Externals-Unity-Packages that runs
// the Unity commands might cause the *.meta files to be deleted! (Unity deletes meta data files 
// when it is opened if the corresponding files are not on disk.)
Task("Externals")
    .IsDependentOn("Externals-Uwp")
    .IsDependentOn("Externals-Ios")
    .IsDependentOn("Externals-Android")
    .IsDependentOn("Externals-Unity-Packages")
    .Does(()=>
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
    DeleteFiles(TemporaryPrefix + "*");
    var dirs = GetDirectories(TemporaryPrefix + "*");
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

void ExtractNuGetPackages(IEnumerable<IPackage> packages, string dest, FrameworkName frameworkName)
{
    EnsureDirectoryExists(dest);
    var fileSystem = new PhysicalFileSystem(Environment.CurrentDirectory);
    foreach (var package in packages)
    {
        Console.WriteLine(package);
        
        // Extract.
        var path = "externals/uwp/" + package.Id;
        package.ExtractContents(fileSystem, path);

        // Get assemblies list.
        IEnumerable<IPackageAssemblyReference> assemblies;
        VersionUtility.TryGetCompatibleItems(frameworkName, package.AssemblyReferences, out assemblies);

        // Move assemblies.
        foreach (var i in assemblies)
        {
            if (!FileExists(dest + "/" + i.Name))
            {
                MoveFile(path + "/" + i.Path, dest + "/" + i.Name);
            }
        }

        // Move native binaries.
        var runtimesPath = path + "/runtimes";
        if (DirectoryExists(runtimesPath))
        {
            foreach (var runtime in GetDirectories(runtimesPath + "/win10-*"))
            {
                var arch = runtime.GetDirectoryName().ToString().Replace("win10-", "").ToUpper();
                var nativeFiles = GetFiles(runtime + "/native/*");
                var targetArchPath = dest + "/" + arch;
                CreateDirectory(targetArchPath);
                MoveFiles(nativeFiles, targetArchPath);
            }
        }
    }
}

static readonly ISet<string> IgnoreNuGetDependencies = new HashSet<string>
{
    "Microsoft.NETCore.UniversalWindowsPlatform",
    "NETStandard.Library"
};

IList<IPackage> GetNuGetDependencies(string packageName, string packageVersion, string packageSource, FrameworkName frameworkName)
{
    var repository = PackageRepositoryFactory.Default.CreateRepository(packageSource);
    var package = repository.FindPackage(packageName, SemanticVersion.Parse(packageVersion));
    var dependencies = new List<IPackage>();
    GetNuGetDependencies(dependencies, repository, frameworkName, package);
    return dependencies;
}

void GetNuGetDependencies(IList<IPackage> dependencies, IPackageRepository repository, FrameworkName frameworkName, IPackage package)
{
    dependencies.Add(package);
    foreach (var dependency in package.GetCompatiblePackageDependencies(frameworkName))
    {
        if (IgnoreNuGetDependencies.Contains(dependency.Id))
            continue;
        var subPackage = repository.ResolveDependency(dependency, false, true);
        GetNuGetDependencies(dependencies, repository, frameworkName, subPackage);
    }
}

static int ExecuteUnityCommand(string extraArgs, ICakeContext context)
{
    var projectDir = context.MakeAbsolute(context.Directory("."));
    var exec = context.EnvironmentVariable("UNITY_PATH");
    var args = "-batchmode -quit -projectPath " + projectDir + " " + extraArgs;
    return context.StartProcess(exec, args);
}

RunTarget(Target);
