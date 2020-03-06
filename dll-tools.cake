// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#addin nuget:?package=Cake.FileHelpers
#addin nuget:?package=Cake.AzureStorage

async Task ProcessDownloadDllDependencies() {

    // Prepare destination
    var path = ExternalsFolder + "uwp.zip";
    var tempPackageFolder = ExternalsFolder;

    // Downloading files.
    Information($"Downloading UWP packages from {UwpUrl} to {path}.");
    DownloadFile(UwpUrl, path);

    // Unzipping files.
    Information($"Unzipping UWP packages from {path} to {tempPackageFolder}.");
    Unzip(path, tempPackageFolder);
    foreach (var module in AppCenterModules)
    {
        if (module.Moniker == "Distribute") 
        {
            Warning("Skipping 'Distribute' for UWP.");
            continue;
        }
        if (module.Moniker == "Crashes") 
        {
            Warning("Skipping 'Crashes' for UWP.");
            continue;
        }

        // Prepare folders.
        var destination = "Assets/AppCenter/Plugins/WSA/" + module.Moniker + "/";
        EnsureDirectoryExists(destination);
        DeleteFiles(destination + "*.dll");
        DeleteFiles(destination + "*.winmd");

        // Copy files.
        var files = GetFiles(ExternalsFolder + module.DotNetModule + ".dll");
        Information($"Copy module {module.DotNetModule}.");
        CopyFiles(files, destination);
    }
    DeleteFiles(ExternalsFolder);
}