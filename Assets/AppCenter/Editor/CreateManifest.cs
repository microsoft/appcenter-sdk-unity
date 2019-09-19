// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using System.Text;

public class CreateManifest
{
    public static void ZipFile(string sourceFile, string destinationFile, string root)
    {
        var stringBuilder = new StringBuilder();
        var args = "";
        var processName = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            args = stringBuilder
                .Append("/c powershell")
                .Append(" -File \"")
                .Append(root)
                .Append("Assets/AppCenter/Plugins/Android/Utility/archive.ps1 \"")
                .Append(" -Source ")
                .Append(root)
                .Append(sourceFile)
                .Append(" -Destination ")
                .Append(root)
                .Append(destinationFile)
                .ToString();
            processName = "cmd";
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            args = stringBuilder
                .Append("-c \"cd ")
                .Append(destinationFile)
                .Append(" ; zip -r ../")
                .Append(sourceFile)
                .Append(" * \"")
                .ToString();
            processName = "/bin/bash";
        }
        ExecuteProcess(processName, args);
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

    public static void UnzipFile(string sourceFile, string destinationFile, string root)
    {
        var stringBuilder = new StringBuilder();
        var args = "";
        var processName = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            stringBuilder = new StringBuilder();
            args = stringBuilder
                .Append("/c powershell")
                .Append(" -File \"")
                .Append(root)
                .Append("Assets/AppCenter/Plugins/Android/Utility/unarchive.ps1 \"")
                .Append(" -Source ")
                .Append(root)
                .Append(sourceFile)
                .Append(" -Destination ")
                .Append(root)
                .Append(destinationFile)
                .ToString();
            processName = "cmd";
;        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            stringBuilder = new StringBuilder();
            args = stringBuilder
                   .Append("-c \"unzip ")
                   .Append(sourceFile)
                   .Append(" -d ")
                   .Append(destinationFile)
                   .Append(" \"")
                   .ToString();
            processName = "/bin/bash";
        }
        ExecuteProcess(processName, args);
    }

    public static void Create(AppCenterSettings settings, string root)
    {
        int lastSeparator = root.LastIndexOf('/');
        root = root.Substring(0, lastSeparator + 1);
        string loaderZipFile = "Assets/AppCenter/Plugins/Android/appcenter-loader-release.aar";
        string loaderFolder = "appcenter-loader-release";
        string manifestPath = "appcenter-loader-release/AndroidManifest.xml";
        string manifestMetafile = "appcenter-loader-release/AndroidManifest.xml.meta";
        
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

        UnzipFile(loaderZipFile, loaderFolder, root);
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

        var intentelement = new XElement("intent-filter");
        XNamespace ns = "http://schemas.android.com/apk/res/android";
        var activityElement = new XElement("activity");
        activityElement.SetAttributeValue(ns+"name", "com.microsoft.identity.client.BrowserTabActivity");
        var actionElement = new XElement("action");
        actionElement.SetAttributeValue(ns+"name", "android.intent.action.VIEW");
        var categoryElement1 = new XElement("category");
        categoryElement1.SetAttributeValue(ns+"name", "android.intent.category.DEFAULT");
        var categoryElement2 = new XElement("category");
        categoryElement2.SetAttributeValue(ns+"name", "android.intent.category.BROWSABLE");
        var dataElement = new XElement("data");
        dataElement.SetAttributeValue(ns+"host", "auth");
        dataElement.SetAttributeValue(ns+"scheme", "msal" + settings.AndroidAppSecret);

        intentelement.Add(actionElement);
        intentelement.Add(categoryElement1);
        intentelement.Add(categoryElement2);
        intentelement.Add(dataElement);
        activityElement.Add(intentelement);
        applicationElements[0].Add(activityElement);
        xmlFile.Save(manifestPath);
      
        // Delete the AndroidManifest.xml.meta file if generated
        if (File.Exists(manifestMetafile))
        {    
            File.Delete(manifestMetafile);    
        }    

        // Delete the original aar file and zipped the extracted folder to generate a new one.
        File.Delete(loaderZipFile);
        ZipFile(loaderFolder, loaderZipFile, root);
        Directory.Delete(loaderFolder, true);
    }
}
