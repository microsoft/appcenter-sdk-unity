// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#addin nuget:?package=NuGet.Protocol&loaddependencies=true
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

const string UwpIL2CPPJsonUrl = SdkStorageUrl + "Newtonsoft.Json.dll";
const string ExternalsFolder = "externals/uwp/";
const PackageSaveMode packageSaveMode = PackageSaveMode.Defaultv3;

// This list includes all the dependencies required for the SDK
// plus any dependencies of the dependecies. In case of any change or version bump
// this list MUST be changed manually.
var UwpIL2CPPDependencies = new [] {
   new NugetDependency("SQLitePCLRaw.bundle_green", "2.0.2"),
   new NugetDependency("SQLitePCLRaw.provider.e_sqlite3", "2.0.2"),
   new NugetDependency("SQLitePCLRaw.core", "2.0.2"),
   new NugetDependency("SQLitePCLRaw.lib.e_sqlite3", "2.0.2", "UAP10.0")
};

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

DependencyInfoResource GetDefaultDependencyResource() 
{
    var sourceRepository = new SourceRepository(new PackageSource(NuGetConstants.V3FeedUrl), Repository.Provider.GetCoreV3());
    return sourceRepository.GetResource<DependencyInfoResource>(CancellationToken.None);
}

async Task<SourcePackageDependencyInfo> GetDependency(DependencyInfoResource dependencyResource, NugetDependency dependency)
{
    Information($"Get dependency {dependency.Name} version {dependency.Version.ToString()}");
    return await dependencyResource.ResolvePackage(new PackageIdentity(dependency.Name, dependency.Version), dependency.Framework, new SourceCacheContext(), NullLogger.Instance, CancellationToken.None); 
}

async Task ProcessDependency(NugetDependency dependency, string destination) 
{
    var package = await GetDependency(GetDefaultDependencyResource(), dependency);
    var uri = package.DownloadUri.ToString();
    Information($"Downloading {package.Id} from {uri}");
    var path = ExternalsFolder + dependency.Name + ".nupkg";
    DownloadFile(uri, path);
    Information($"Extract NuGet package: {dependency.Name}");
    var tempPackageFolder = ExternalsFolder + dependency.Name;
    Unzip(path, tempPackageFolder);
    ProcessPackageDlls(dependency, tempPackageFolder, destination);
    DeleteFiles(ExternalsFolder);
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

void MoveDllsByArchitecture(string tempContentPath, string destination)
{
    var runtimesPath = tempContentPath + "/runtimes";
    if (!DirectoryExists(runtimesPath)) 
    {
        return;
    }
    Information($"Move dlls by architecture from {tempContentPath} to {destination}");
    foreach (var runtime in GetDirectories($"{runtimesPath}/win10-*")) {
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
            } 
            else 
            {
                Information("Native binary file already exists");
            }
        }
    } 
}

void MoveDlls(NugetDependency package, string tempContentPath, string destination)
{
    Information($"Move assemblies for {package.Name} from {tempContentPath} to {destination}.");
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

void ProcessPackageDlls(NugetDependency package, string tempContentPath, string destination)
{
    Information($"Process {package.Name} dlls. Temp content path is {tempContentPath}; Destination is {destination}");
    MoveDllsByArchitecture(tempContentPath, destination);
    MoveDlls(package, tempContentPath, destination);
}

void ProcessAppCenterDlls(string tempContentPath, string destination) 
{
    MoveDllsByArchitecture(tempContentPath, destination);
    var contentPathSuffix = "lib/uap10.0/";
    var filesMask = tempContentPath + contentPathSuffix + '*';
    Information($"Moving SDK package, move from {filesMask} to {destination}");
    var files = GetFiles(filesMask);
    CleanDirectories(destination);
    MoveFiles(files, destination);
}
