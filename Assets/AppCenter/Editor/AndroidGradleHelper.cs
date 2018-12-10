using Microsoft.AppCenter.Unity;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class AndroidGradleHelper
{
    private const string GOOGLE_SERVICES_VERSION = "4.0.1";
    public static bool MoveCustomGradleScript(string pathToBuiltProject)
    {
        var appAdditionsFolder = AppCenterSettingsContext.AppCenterPath + "/AppCenter/Plugins/Android/Push/AppAdditions";
        var sourcePath = Path.Combine(appAdditionsFolder, "appcenterpush.gradle");
        if (!File.Exists(sourcePath))
        {
            Debug.LogError(sourcePath + " could not be found. It needs to exist in order for Push Push service to work!");
            return false;
        }
        var destPath = Path.Combine(pathToBuiltProject, "appcenterpush.gradle");
        File.Copy(sourcePath, destPath, true);
        return true;
    }

    public static bool MoveGoogleJsonFile(string pathToBuiltProject)
    {
        var sourcePath = "Assets\\google-services.json";
        if (!File.Exists(sourcePath))
        {
            Debug.LogError("Please add google-services.json file to Assets folder in order for Push service to work!");
            return false;
        }
        else
        {
            var destPath = Path.Combine(pathToBuiltProject, "google-services.json");
            File.Copy(sourcePath, destPath, true);
        }
        return true;
    }

    public static bool InjectFirebaseDependencies(string pathToBuiltProject)
    {
        string[] regexPatterns = new string[]
        {
            "com.android.tools.build:gradle:[0-9.]*'",
            "com.android.application'"
        };

        string[] codePartsToInsert =
        {
            string.Format("{0}classpath 'com.google.gms:google-services:{1}'{0}", Environment.NewLine, GOOGLE_SERVICES_VERSION),
            string.Format("{0}apply from: 'appcenterpush.gradle'",  Environment.NewLine)
        };

        var appFilePath = Path.Combine(pathToBuiltProject, "build.gradle");
        return ReplaceCodeParts(
            appFilePath,
            regexPatterns,
            codePartsToInsert,
            false,
            true
        );
    }

    public static void SwapGoogleAndJcenter(string appFilePath)
    {
        string[] regexPatterns = new string[]
        {
            "google\\(\\)",
            "jcenter\\(\\)"
        };

        string[] codePartsToInsert = new string[]
        {
            "",
            string.Format("google(){0}jcenter()", Environment.NewLine)
        };

        // On lower Unity versions, generated gradle doesn't contain google() repository.
        // That's why we do the replace in a silent way.
        ReplaceCodeParts(
            appFilePath,
            regexPatterns,
            codePartsToInsert,
            true,
            false
        );
    }

    private static bool ReplaceCodeParts(string appFilePath, string[] regexPatterns, string[] codePartsToInsert, bool replaceFully, bool forceCrash)
    {
        var fileText = File.ReadAllText(appFilePath);
        for (var i = 0; i < regexPatterns.Length; i++)
        {
            var regex = new Regex(regexPatterns[i]);
            var matches = regex.Match(fileText);
            if (matches.Success)
            {
                var codeToReplace = matches.ToString();
                var codeToInsert = replaceFully ? codePartsToInsert[i] : (codeToReplace + codePartsToInsert[i]);
                fileText = fileText.Replace(codeToReplace, codeToInsert);
            }
            else
            {
                var errorString = "Unable to automatically modify file '" + appFilePath + "'. For App Center Push to work properly, " +
                        "please follow troubleshooting instructions at https://docs.microsoft.com/en-us/mobile-center/sdk/troubleshooting/unity";
                if (forceCrash)
                {
                    // TODO Update documentation link
                    Debug.LogError(errorString);
                    return false;
                }
            }
        }
        File.WriteAllText(appFilePath, fileText);
        return true;
    }
}

