// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

var PrivateNugetFeedUrl = "https://msmobilecenter.pkgs.visualstudio.com/_packaging/";


// This list includes all the dependencies required for the SDK
// plus any dependencies of the dependecies. In case of any change or version bump
// this list MUST be changed manually.
var UwpIL2CPPDependencies = new [] {
   new NugetDependency("SQLitePCLRaw.bundle_green", "2.0.2"),
   new NugetDependency("SQLitePCLRaw.provider.e_sqlite3", "2.0.2"),
   new NugetDependency("SQLitePCLRaw.core", "2.0.2"),
   new NugetDependency("SQLitePCLRaw.lib.e_sqlite3", "2.0.2", "UAP10.0")
};

var UwpIL2CPPJsonUrl = SdkStorageUrl + "Newtonsoft.Json.dll";
var ExternalsFolder = "externals/uwp/";

class NugetDependency
{
    public string Name { get; set; }
    public NuGetVersion Version { get; set; }
    public NuGetFramework Framework { get; set; }

    public NugetDependency(string name, string version) : this(name, version, ".NETStandard,Version=v2.0")
    { 
    }

    public NugetDependency(string name, string version, string framework)
    {
        Name = name;
        Version = NuGetVersion.Parse(version);
        Framework = NuGetFramework.Parse(framework);
    }

    public NugetDependency(string name, NuGetVersion version, NuGetFramework framework) {
        Name = name;
        Version = version;
        Framework = framework;
    }
}

async Task<SourcePackageDependencyInfo> GetDependency(DependencyInfoResource dependencyResource, NugetDependency dependency)
{
    Information($"GetDependency {dependency.Name} version {dependency.Version.ToString()}");
    return await dependencyResource.ResolvePackage(new PackageIdentity(dependency.Name, dependency.Version), dependency.Framework, new SourceCacheContext(), NullLogger.Instance, CancellationToken.None); 
}

DependencyInfoResource GetDefaultDependencyResource() 
{
    var sourceRepository = new SourceRepository(new PackageSource(NuGetConstants.V3FeedUrl), Repository.Provider.GetCoreV3());
    return sourceRepository.GetResource<DependencyInfoResource>(CancellationToken.None);
}

async Task ProcessDependency(NugetDependency dependency, string destination) 
{
    var package = await GetDependency(GetDefaultDependencyResource(), dependency);
    var uri = package.DownloadUri.ToString();
    Information($"Downloading {package.Id} from {uri}");
    DownloadFile(uri, ExternalsFolder + package.Id + ".nupkg");
    Information($"Extract NuGet package: {dependency.Name}");
    var path = ExternalsFolder + dependency.Name + ".nupkg";
    var tempPackageFolder = ExternalsFolder + dependency.Name;
    PackageExtractor.Extract(path, tempPackageFolder);
    ExtractNuGetPackage(dependency, tempPackageFolder, destination);
}

string GetNuGetPackage(string packageId, string packageVersion)
{
    var nugetUser = EnvironmentVariable("NUGET_USER");
    var nugetPassword = Argument("NuGetPassword", EnvironmentVariable("NUGET_PASSWORD"));
    var nugetFeedId = Argument("NuGetFeedId", EnvironmentVariable("NUGET_FEED_ID"));
    packageId = packageId.ToLower();
    var url = PrivateNugetFeedUrl + nugetFeedId + "/nuget/v3/flat2/" + packageId + "/" + packageVersion + "/" + packageId + "." + packageVersion + ".nupkg";

    // Get the NuGet package
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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

FilePathCollection ResolveDllFiles(string tempContentPath, NuGetFramework frameworkName) 
{
    var destinationPath = $"{tempContentPath}/lib/{frameworkName.GetShortFolderName()}/*.dll";
    var files = GetFiles(destinationPath);
    if (files.Any()) 
    {
        return files;
    }
    Warning($"Haven't found anything under {tempContentPath} - make sure it's expected.");
    return null;
}

void MoveNativeBinaries(string tempContentPath, string destination)
{
    var runtimesPath = tempContentPath + "/runtimes";
    if (DirectoryExists(runtimesPath)) 
    {
        Information($"MoveNativeBinaries from {tempContentPath} to {destination}");
        foreach (var runtime in GetDirectories(runtimesPath + "/win10-*")) {
            var arch = runtime.GetDirectoryName().ToString().Replace("win10-", "").ToUpper();
            var nativeFiles = GetFiles(runtime + "/**/*.dll");
            var targetArchPath = destination + "/" + arch;
            EnsureDirectoryExists(targetArchPath);
            foreach (var nativeFile in nativeFiles) 
            {
                if (!FileExists(targetArchPath + "/" + nativeFile.GetFilename())) 
                {
                    MoveFileToDirectory(nativeFile, targetArchPath);
                    Information($"Moved native binary file {nativeFile} to {targetArchPath}");
                } else {
                    Information("Native binary file already exists");
                }
            }
        }
    } 
}

void MoveAssemblies(NugetDependency package, string tempContentPath, string destination)
{
    Information($"MoveAssemblies");
    var dllFiles = ResolveDllFiles(tempContentPath, package.Framework);
    if (dllFiles == null)
    {
        return;
    }
    foreach (var matchingFile in dllFiles) 
    {
        var targetPath = $"{destination}/{matchingFile.GetFilename()}";
        if (FileExists(targetPath)) 
        {
            DeleteFile(targetPath);
        }
        MoveFile(matchingFile.FullPath, targetPath);
        Information($"Moving {matchingFile.FullPath} to {targetPath}");
    } 
}

void ExtractNuGetPackage(NugetDependency package, string tempContentPath, string destination)
{
    var packageName = package != null ? package.Name : "null";
    Information($"ExtractNuGetPackage {packageName}; tempcontentpath {tempContentPath}; destination {destination}");
    MoveNativeBinaries(tempContentPath, destination);
    if (package != null) 
    {
        MoveAssemblies(package, tempContentPath, destination);
    }
    if (package == null) 
    {
        var contentPathSuffix = "lib/uap10.0/";
        Information($"package is null, move from {tempContentPath + contentPathSuffix + '*'} to {destination}");
        var files = GetFiles(tempContentPath + contentPathSuffix + "*");
        CleanDirectories(destination);
        MoveFiles(files, destination);
    }
}
