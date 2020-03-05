// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

#addin nuget:?package=Cake.FileHelpers
#addin nuget:?package=Cake.AzureStorage

var UwpDllDependencies = new Dictionary<string, string> { 
                                  {"Push", "Newtonsoft.Json.dll"},
                                  {"Analytics", "Newtonsoft.Json.dll"},
                                  {"Core", "Newtonsoft.Json.dll"}
                              };

async Task ProcessDownloadDllDependency(string moniker, string destination) {
    var dllName = UwpDllDependencies[moniker];
    DownloadFile(SdkStorageUrl + dllName, destination + dllName);
}