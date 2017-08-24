#tool nuget:?package=XamarinComponent
#addin nuget:?package=Cake.Xamarin
#addin nuget:?package=Cake.FileHelpers
#addin "Cake.AzureStorage"
#addin nuget:?package=Cake.Git
#addin nuget:?package=NuGet.Core
#addin "Cake.Xcode"

using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Runtime.Versioning;
using NuGet;

// Native SDK versions
var AndroidSdkVersion = "0.11.2";
var IosSdkVersion = "0.11.2";
var UwpSdkVersion = "0.14.2";

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
    new MobileCenterModule("mobile-center-distribute-release.aar", "MobileCenterDistribute.framework", "Microsoft.Azure.Mobile.Distribute", "Distribute"),
    new MobileCenterModule("mobile-center-push-release.aar", "MobileCenterPush.framework", "Microsoft.Azure.Mobile.Push", "Push")
};

// Prefix for temporary intermediates that are created by this script
var TemporaryPrefix = "CAKE_SCRIPT_TEMP";

// Location of puppet application builds
 var PuppetBuildsFolder = "PuppetBuilds";

// External Unity Packages
var JarResolverPackageName =  "play-services-resolver-" + ExternalUnityPackage.VersionPlaceholder + ".unitypackage";
var JarResolverVersion = "1.2.35.0";
var JarResolverUrl = SdkStorageUrl + ExternalUnityPackage.NamePlaceholder;

var ExternalUnityPackages = new [] {
    new ExternalUnityPackage(JarResolverPackageName, JarResolverVersion, JarResolverUrl)
};

// UWP IL2CPP dependencies.
var UwpIL2CPPDependencies = new [] {

    // Force use assembly for .NET 2.0 to avoid IL2CPP convert problems.
    new NugetDependency("Newtonsoft.Json", "10.0.3", ".NETFramework, Version=v2.0"),
    new NugetDependency("sqlite-net-pcl", "1.3.1", "UAP, Version=v10.0"),

    // Force use this version to avoid types conflicts.
    new NugetDependency("System.Threading.Tasks", "4.0.10", ".NETCore, Version=v5.0", false)
};

// Task TARGET for build
var Target = Argument("target", Argument("t", "Default"));

// Available MobileCenter modules.
// MobileCenter module class definition.
class MobileCenterModule
{
    public string AndroidModule { get; set; }
    public string IosModule { get; set; }
    public string DotNetModule { get; set; }
    public string Moniker { get; set; }
    public bool UWPHasNativeCode { get; set; }
    public string[] NativeArchitectures { get; set; }

    public MobileCenterModule(string android, string ios, string dotnet, string moniker, bool hasNative = false)
    {
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

class NugetDependency
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string Framework { get; set; }
    public bool IncludeDependencies { get; set; }

    public NugetDependency(string name, string version, string framework, bool includeDependencies = true)
    {
        Name = name;
        Version = version;
        Framework = framework;
        IncludeDependencies = includeDependencies;
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

    private string _packageName;
    private List<string> _includePaths = new List<string>();

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
            Context.Error("Something went wrong while creating Unity package '" + _packageName + "'");
        }
    }
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
    .Does(() =>
{
    CleanDirectory("externals/uwp");
    EnsureDirectoryExists("Assets/Plugins/WSA/");
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
        EnsureDirectoryExists(destination);
        DeleteFiles(destination + "*.dll");
        DeleteFiles(destination + "*.winmd");

        // Deal with any native components
        if (module.UWPHasNativeCode)
        {
            foreach (var arch in module.NativeArchitectures)
            {
                var dest = "Assets/Plugins/WSA/" + module.Moniker + "/" + arch.ToString().ToUpper() + "/";
                EnsureDirectoryExists(dest);
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
    ExecuteUnityCommand("-executeMethod MobileCenterPostBuild.ProcessUwpMobileCenterBinaries", Context);
}).OnError(HandleError);

// Downloading UWP IL2CPP dependencies.
Task("Externals-Uwp-IL2CPP-Dependencies")
    .Does(() =>
{
    var targetPath = "Assets/Plugins/WSA/IL2CPP";
    EnsureDirectoryExists(targetPath);
    EnsureDirectoryExists(targetPath + "/ARM");
    EnsureDirectoryExists(targetPath + "/X86");
    EnsureDirectoryExists(targetPath + "/X64");

    // NuGet.Core support only v2.
    var packageSource = "https://www.nuget.org/api/v2/";
    var repository = PackageRepositoryFactory.Default.CreateRepository(packageSource);
    foreach (var i in UwpIL2CPPDependencies)
    {
        var frameworkName = new FrameworkName(i.Framework);
        var package = repository.FindPackage(i.Name, SemanticVersion.Parse(i.Version));
        IEnumerable<IPackage> dependencies;
        if (i.IncludeDependencies)
        {
            dependencies = GetNuGetDependencies(repository, frameworkName, package);
        }
        else
        {
            dependencies = new [] { package };
        }
        ExtractNuGetPackages(dependencies, targetPath, frameworkName);
    }

    // Process UWP IL2CPP dependencies.
    Information("Processing UWP IL2CPP dependencies. This could take a minute.");
    ExecuteUnityCommand("-executeMethod MobileCenterPostBuild.ProcessUwpIl2CppDependencies", Context);
}).OnError(HandleError);

// Download and install all external Unity packages required.
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
// NOTE: It is important to execute Externals-Unity-Packages and Externals-Uwp-IL2CPP-Dependencies *last*
// or the steps that runs the Unity commands might cause the *.meta files to be deleted!
// (Unity deletes meta data files when it is opened if the corresponding files are not on disk.)
Task("Externals")
    .IsDependentOn("Externals-Uwp")
    .IsDependentOn("Externals-Ios")
    .IsDependentOn("Externals-Android")
    .IsDependentOn("Externals-Unity-Packages")
    .IsDependentOn("Externals-Uwp-IL2CPP-Dependencies")
    .Does(()=>
{
    DeleteDirectoryIfExists("externals");
});

// Creates Unity packages corresponding to all ".unitypackagespec" files
// in "UnityPackageSpecs" folder.
Task("Package").Does(()=>
{
    // Need to provide cake context so class methods can use cake apis.
    UnityPackage.Context = Context;

    // Store packages in a clean folder.
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
// in "UnityPackageSpecs" folder (and downloads binaries).
Task("CreatePackages").IsDependentOn("Externals").IsDependentOn("Package");

// Builds the puppet applications and throws an exception on failure.
Task("TestBuildPuppetApps")
    .IsDependentOn("Externals")
    .Does(()=>
{
    if (IsRunningOnUnix())
    {
        // Android
        string[] androidBuildMethods = {
            "BuildPuppet.BuildPuppetSceneAndroidMono",
            "BuildPuppet.BuildPuppetSceneAndroidIl2CPP"
        };
        foreach (var androidMethod in androidBuildMethods)
        {
            // Remove all current builds and create new build.
            DeleteFiles(PuppetBuildsFolder + "/*");
            TestBuildPuppet(androidMethod);

            // Verify that an APK was generated. (".Single()" should throw an exception if the 
            // collection is empty).
            GetFiles(PuppetBuildsFolder + "/*.apk").Single();
        }
        
        // iOS
        string[] iOSBuildMethods = {
            "BuildPuppet.BuildPuppetSceneIosMono",
            "BuildPuppet.BuildPuppetSceneIosIl2CPP"
        };
        foreach (var iOSBuildMethod in iOSBuildMethods)
        {
            // Remove all current builds and create new build.
            DeleteFiles(PuppetBuildsFolder + "/*");
            TestBuildPuppet(iOSBuildMethod);
            
            // Verify that an Xcode project was created and that it builds properly.
            var xcodeProjectPath = GetDirectories(PuppetBuildsFolder + "/*/*.xcodeproj").Single();
            
            // Only one Xcode project should exist, so assume the first in the array is the correct one.
            Information("Attempting to build '" + xcodeProjectPath.FullPath + "'...");
            BuildXcodeProject(xcodeProjectPath.FullPath);
            Information("Successfully built '" + xcodeProjectPath.FullPath + "'");
        }
    }
    else
    {
        // UWP
        string[] uwpBuildMethods = {
            "BuildPuppet.BuildPuppetSceneWsaNetXaml",
            "BuildPuppet.BuildPuppetSceneWsaIl2CPPXaml",
            "BuildPuppet.BuildPuppetSceneWsaNetD3D",
            "BuildPuppet.BuildPuppetSceneWsaIl2CPPD3D"
        };
        foreach (var uwpBuildMethod in uwpBuildMethods)
        {
            // Remove all existing builds and create new build.
            DeleteFiles(PuppetBuildsFolder + "/*");
            TestBuildPuppet(uwpBuildMethod);
            
            // Verify that a solution file was created and that it builds properly.
            var solutionFilePath = GetFiles("PuppetBuilds/*/*.sln").Single();            
            // For now, only build for x86.
            // TODO also build for ARM and x64 (.NET-D3D build is currently broken for ARM, though).
            Information("Attempting to build '" + solutionFilePath.ToString() + "'...");
            MSBuild(solutionFilePath.ToString(), c => c.SetConfiguration("Release").WithProperty("Platform", "x86"));
            Information("Successfully built '" + solutionFilePath.ToString() + "'");
        }
    }
    
    // Remove all remaining builds.
    DeleteFiles(PuppetBuildsFolder + "/*");
}).OnError(HandleError);

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
    DeleteFiles(PuppetBuildsFolder + "/*");
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
        Information("Extract NuGet package: " + package);

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
                EnsureDirectoryExists(targetArchPath);
                foreach (var nativeFile in nativeFiles)
                {
                    if (!FileExists(targetArchPath + "/" + nativeFile.GetFilename()))
                    {
                        MoveFileToDirectory(nativeFile, targetArchPath);
                    }
                }
            }
        }
    }
}

IList<IPackage> GetNuGetDependencies(IPackageRepository repository, FrameworkName frameworkName, IPackage package)
{
    var dependencies = new List<IPackage>();
    GetNuGetDependencies(dependencies, repository, frameworkName, package);
    return dependencies;
}

void GetNuGetDependencies(IList<IPackage> dependencies, IPackageRepository repository, FrameworkName frameworkName, IPackage package)
{
    // Declaring this outside the method causes a parse error on Cake for Mac.
    string[] IgnoreNuGetDependencies = {
        "Microsoft.NETCore.UniversalWindowsPlatform",
        "NETStandard.Library"
    };

    dependencies.Add(package);
    foreach (var dependency in package.GetCompatiblePackageDependencies(frameworkName))
    {
        if (IgnoreNuGetDependencies.Contains(dependency.Id))
        {
            continue;
        }
        var subPackage = repository.ResolveDependency(dependency, false, true);
        if (!dependencies.Contains(subPackage))
        {
            GetNuGetDependencies(dependencies, repository, frameworkName, subPackage);
        }
    }
}

void BuildXcodeProject(string projectPath)
{
    var projectFolder = System.IO.Path.GetDirectoryName(projectPath);
    var buildOutputFolder =  System.IO.Path.Combine(projectFolder, "build");
    XCodeBuild(new XCodeBuildSettings {
        Project = projectPath,
        Scheme = "Unity-iPhone",
        Configuration = "Release",
        DerivedDataPath = buildOutputFolder 
    });
}

static int ExecuteUnityCommand(string extraArgs, ICakeContext context)
{
    var projectDir = context.MakeAbsolute(context.Directory("."));
    var exec = context.EnvironmentVariable("UNITY_PATH");
    var args = "-batchmode -quit -logFile -projectPath " + projectDir + " " + extraArgs;
    return context.StartProcess(exec, args);
}

void TestBuildPuppet(string buildMethodName)
{
    Information("Executing method " + buildMethodName + ", this could take a while...");
    var command = "-executeMethod " + buildMethodName;
    var result = ExecuteUnityCommand(command, Context);
    if (result != 0)
    {
        throw new Exception("Failed to execute method " + buildMethodName + ".");
    }
}

RunTarget(Target);
