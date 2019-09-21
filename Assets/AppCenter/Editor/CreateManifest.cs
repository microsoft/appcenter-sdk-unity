// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;

public class CreateManifest
{
    public static void ZipFile(string sourceFile, string destinationFile)
    {
        var stringBuilder = new StringBuilder();
        var args = "";
        var processName = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            args = stringBuilder
                .Append("/c powershell")
                .Append(" -File \"")
                .Append(AppCenterSettingsContext.AppCenterPath)
                .Append("/AppCenter/Plugins/Android/Utility/archive.ps1 \"")
                .Append(" -Source ")
                .Append(sourceFile)
                .Append(" -Destination ")
                .Append(destinationFile)
                .ToString();
            processName = "cmd";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            args = stringBuilder
                .Append("-c \"cd ")
                .Append(sourceFile)
                .Append(" ; zip -r ")
                .Append("../")
                .Append(destinationFile)
                .Append(" * \"")
                .ToString();
            processName = "/bin/bash";
        }
        if (processName.Length > 0)
        {
            ExecuteProcess(processName, args);
        }
    }

    private static void ExecuteProcess(string processName, string args)
    {
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = processName,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        if (output.Length > 0 || error.Length > 0)
        {
            UnityEngine.Debug.Log(output + error);
        }
    }

    public static void UnzipFile(string sourceFile, string destinationFile)
    {
        var stringBuilder = new StringBuilder();
        var args = "";
        var processName = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            args = stringBuilder
                .Append("/c powershell")
                .Append(" -File \"")
                .Append(AppCenterSettingsContext.AppCenterPath)
                .Append("/AppCenter/Plugins/Android/Utility/unarchive.ps1 \"")
                .Append(" -Source ")
                .Append(sourceFile)
                .Append(" -Destination ")
                .Append(destinationFile)
                .ToString();
            processName = "cmd";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            args = stringBuilder
                   .Append("-c \"unzip ")
                   .Append(sourceFile)
                   .Append(" -d ")
                   .Append(destinationFile)
                   .Append(" \"")
                   .ToString();
            processName = "/bin/bash";
        }
        if (processName.Length > 0)
        {
            ExecuteProcess(processName, args);
        }
    }

    public static void Create(AppCenterSettings settings)
    {
        var loaderZipFile = AppCenterSettingsContext.AppCenterPath + "/AppCenter/Plugins/Android/appcenter-loader-release.aar";
        var loaderFolder = "appcenter-loader-release";
        var manifestPath = "appcenter-loader-release/AndroidManifest.xml";
        var manifestMetafile = "appcenter-loader-release/AndroidManifest.xml.meta";

        if (!File.Exists(loaderZipFile))
        {
            UnityEngine.Debug.LogWarning("Failed to load dependency file appcenter-loader-release.aar");
            return;
        }

        // Delete unzipeed directory if it already exists.
        if (Directory.Exists(loaderFolder))
        {
            Directory.Delete(loaderFolder, true);
        }

        UnzipFile(loaderZipFile, loaderFolder);
        if (!Directory.Exists(loaderFolder))
        {
            UnityEngine.Debug.LogWarning("Unzipping loader folder failed.");
            return;
        }

        var xmlFile = XDocument.Load(manifestPath);

        var applicationElements = xmlFile.Root.Elements().Where(element => element.Name.LocalName == "application").ToList();
        if (applicationElements.Count == 0)
        {
            UnityEngine.Debug.LogWarning("Incorrect format of AndroidManifest.xml of AppCenterLoader");
            Directory.Delete(loaderFolder, true);
            return;
        }
        var activityElements = applicationElements[0].Elements().Where(element => element.Name.LocalName == "activity").ToList();

        // Delete the unzipped folder if the activity element already exists in the AndroidManifest.xml file
        if (activityElements.Count == 1)
        {
            Directory.Delete(loaderFolder, true);
            return;
        }

        var intentElement = new XElement("intent-filter");
        XNamespace ns = "http://schemas.android.com/apk/res/android";
        var activityElement = new XElement("activity");
        activityElement.SetAttributeValue(ns + "name", "com.microsoft.identity.client.BrowserTabActivity");
        var actionElement = new XElement("action");
        actionElement.SetAttributeValue(ns + "name", "android.intent.action.VIEW");
        var categoryElement1 = new XElement("category");
        categoryElement1.SetAttributeValue(ns + "name", "android.intent.category.DEFAULT");
        var categoryElement2 = new XElement("category");
        categoryElement2.SetAttributeValue(ns + "name", "android.intent.category.BROWSABLE");
        var dataElement = new XElement("data");
        dataElement.SetAttributeValue(ns + "host", "auth");
        dataElement.SetAttributeValue(ns + "scheme", "msal" + settings.AndroidAppSecret);

        intentElement.Add(actionElement);
        intentElement.Add(categoryElement1);
        intentElement.Add(categoryElement2);
        intentElement.Add(dataElement);
        activityElement.Add(intentElement);
        applicationElements[0].Add(activityElement);
        xmlFile.Save(manifestPath);

        // Delete the AndroidManifest.xml.meta file if generated
        if (File.Exists(manifestMetafile))
        {
            File.Delete(manifestMetafile);
        }

        // Delete the original aar file and zipped the extracted folder to generate a new one.
        File.Delete(loaderZipFile);
        ZipFile(loaderFolder, loaderZipFile);
        Directory.Delete(loaderFolder, true);
    }
}
