// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEditor.Build;
using UnityEditor;
using UnityEngine;

public class CreateManifest
{
    public static void ZipFile(string sourceFile, string destinationFile)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "/c cd appcenter-loader-release",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.WaitForExit();

            var process2 = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "/c jar -cMf ../Assets/AppCenter/Plugins/Android/appcenter-loader-release.aar . ",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process2.Start();
            process2.WaitForExit();
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"cd appcenter-loader-release ; zip -r ../Assets/AppCenter/Plugins/Android/appcenter-loader-release.aar * \"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.WaitForExit();
        }
    }

    public static void UnzipFile(string sourceFile, string destinationFile, string root)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "/c md " + destinationFile,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.WaitForExit();

            var process2 = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "/c cd " + destinationFile,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process2.Start();
            process2.WaitForExit();

            var process3 = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "jar xf " + root + sourceFile,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process3.OutputDataReceived += Process3_OutputDataReceived; 
            process3.Start();
            process3.WaitForExit();
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"unzip " + sourceFile + " -d " + destinationFile + " \"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.WaitForExit();
        }
    }

    private static void Process3_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        UnityEngine.Debug.Log(e.Data.ToString());
    }

    public static void Create(AppCenterSettings settings, string root) {
        int lastSeparator = root.LastIndexOf('/');
        root = root.Substring(0, lastSeparator + 1);
        UnityEngine.Debug.Log(root);
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
        ZipFile(loaderFolder, loaderZipFile);
        Directory.Delete(loaderFolder, true);
    }
}
