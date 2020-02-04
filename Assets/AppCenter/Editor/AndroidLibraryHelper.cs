// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System.Diagnostics;
using System.Text;
using UnityEngine;

public static class AndroidLibraryHelper
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
                .Append(" -executionpolicy bypass")
                .Append(" -File \"")
                .Append(AppCenterSettingsContext.AppCenterPath)
                .Append("/Plugins/Android/Utility/archive.ps1 \"")
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

    public static void UnzipFile(string sourceFile, string destinationFile)
    {
        var stringBuilder = new StringBuilder();
        var args = "";
        var processName = "";
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            args = stringBuilder
                .Append("/c powershell")
                .Append(" -executionpolicy bypass")
                .Append(" -File \"")
                .Append(AppCenterSettingsContext.AppCenterPath)
                .Append("/Plugins/Android/Utility/unarchive.ps1 \"")
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
}
