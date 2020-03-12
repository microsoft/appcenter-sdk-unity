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

const string ExternalsUwpFolder = "externals/uwp/";
const PackageSaveMode packageSaveMode = PackageSaveMode.Defaultv3;

/*
 * This list includes all transitive dependencies required for the SDK.
 * In case of any change or version bump this list MUST be changed manually.
 */
var UwpIL2CPPDependencies = new [] {
    /* #      */ new NuGetDependency("Newtonsoft.Json", "12.0.2"),
    /* #      */ new NuGetDependency("SQLitePCLRaw.bundle_green", "2.0.2"),
    /* ├─#    */ new NuGetDependency("SQLitePCLRaw.core", "2.0.2"),
    /* | └─#  */ new NuGetDependency("System.Memory", "4.5.3"),
    /* |   ├─ */ new NuGetDependency("System.Buffers", "4.4.0"),
    /* |   └─ */ new NuGetDependency("System.Runtime.CompilerServices.Unsafe", "4.5.2"),
    /* ├───── */ new NuGetDependency("SQLitePCLRaw.lib.e_sqlite3", "2.0.2", "UAP10.0"),
    /* └───── */ new NuGetDependency("SQLitePCLRaw.provider.e_sqlite3", "2.0.2")
};

class NuGetDependency
{
    public string Name { get; set; }
    public NuGetVersion Version { get; set; }
    public NuGetFramework Framework { get; set; }

    public NuGetDependency(string name, string version) : this(name, version, ".NETStandard,Version=v2.0")
    {
    }

    public NuGetDependency(string name, string version, string framework)
    {
        Name = name;
        Version = NuGetVersion.Parse(version);
        Framework = NuGetFramework.Parse(framework);
    }

    public NuGetDependency(string name, NuGetVersion version, NuGetFramework framework) {
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

async Task<SourcePackageDependencyInfo> GetDependency(DependencyInfoResource dependencyResource, NuGetDependency dependency)
{
    Information($"Get dependency {dependency.Name} version {dependency.Version.ToString()}");
    return await dependencyResource.ResolvePackage(new PackageIdentity(dependency.Name, dependency.Version), dependency.Framework, new SourceCacheContext(), NullLogger.Instance, CancellationToken.None);
}

async Task ProcessDependency(NuGetDependency dependency, string destination)
{
    var package = await GetDependency(GetDefaultDependencyResource(), dependency);
    var uri = package.DownloadUri.ToString();
    Debug($"Downloading {package.Id} from {uri}");
    var path = ExternalsUwpFolder + dependency.Name + ".nupkg";
    DownloadFile(uri, path);
    Debug($"Extract NuGet package: {dependency.Name}");
    var tempPackageFolder = ExternalsUwpFolder + dependency.Name;
    Unzip(path, tempPackageFolder);
    ProcessPackageAssemblies(dependency, tempPackageFolder, destination);
}

FilePathCollection ResolveDllFiles(string tempContentPath, NuGetFramework frameworkName)
{
    var destinationPath = $"{tempContentPath}/lib/{frameworkName.GetShortFolderName()}/*.dll";
    var files = GetFiles(destinationPath);
    if (files.Any())
    {
        return files;
    }
    Warning($"Haven't found managed binaries in {tempContentPath}");
    return null;
}

void MoveAssembliesByArchitecture(string tempContentPath, string destination)
{
    var runtimesPath = tempContentPath + "/runtimes";
    if (!DirectoryExists(runtimesPath))
    {
        return;
    }
    Debug($"Move assemblies by architecture from {tempContentPath} to {destination}");
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
                Debug($"Moved native binary file {nativeFile} to {targetArchPath}");
            }
            else
            {
                Information($"Native binary file {nativeFile} already exists.");
            }
        }
    }
}

void MoveAssemblies(NuGetDependency package, string tempContentPath, string destination)
{
    Debug($"Move assemblies for {package.Name} from {tempContentPath} to {destination}.");
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
        Debug($"Moving {matchingFile.FullPath} to {targetPath}");
    }
}

void ProcessPackageAssemblies(NuGetDependency package, string tempContentPath, string destination)
{
    Debug($"Process {package.Name} assemblies. Temp content path is {tempContentPath}; Destination is {destination}");
    MoveAssembliesByArchitecture(tempContentPath, destination);
    MoveAssemblies(package, tempContentPath, destination);
}

void ProcessAppCenterAssemblies(string tempContentPath, string destination)
{
    MoveAssembliesByArchitecture(tempContentPath, destination);
    var contentPathSuffix = "lib/uap10.0/";
    var filesMask = tempContentPath + contentPathSuffix + '*';
    Information($"Moving SDK package, move from {filesMask} to {destination}");
    var files = GetFiles(filesMask);
    CleanDirectories(destination);
    MoveFiles(files, destination);
}
