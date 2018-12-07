using System.IO;
using System.Text.RegularExpressions;
#if UNITY_2018_2_OR_NEWER
using UnityEditor.Android;
#endif
using UnityEditor;
using UnityEngine;

#if UNITY_2018_2_OR_NEWER
public class AndroidPostBuild : IPostGenerateGradleAndroidProject
#else
public class AndroidPostBuild
#endif
{
    private const string GOOGLE_SERVICES_VERSION = "4.0.1";
    private const string FIREBASE_CORE_VERSION = "16.0.1";
    private const string FIREBASE_MESSAGING_VERSION = "17.0.0";

    public int callbackOrder { get { return 0; } }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
        if (settings.UsePush)
        {
            OnAndroidPostBuild(path);
        }
    }

    public static void OnAndroidPostBuild(string path)
    {
        if (EditorUserBuildSettings.exportAsGoogleAndroidProject)
        {
            var gradleFilePath = Path.Combine(path, "build.gradle");
            // On older versions of Unity, there's a bug where path to project is determined wrong.
            // Reference: https://issuetracker.unity3d.com/issues/android-ipostgenerategradleandroidproject-dot-onpostgenerategradleandroidproject-returns-incorrect-path-when-exporting-a-project
            if (!File.Exists(gradleFilePath))
            {
                var dirInfo = new DirectoryInfo(path);
                var dirs = dirInfo.GetDirectories();
                if (dirs.Length > 0)
                {
                    path = dirs[0].FullName;
                }
            }
        }

        if (!MoveGoogleJsonFile(path))
        {
            return;
        }
        MoveCustomGradleScript(path);
        if (!InjectFirebaseDependencies(path))
        {
            return;
        }
        var appFilePath = Path.Combine(path, "unity-android-resources\\build.gradle");
        SwapGoogleAndJcenter(appFilePath);
        appFilePath = Path.Combine(path, "build.gradle");
        SwapGoogleAndJcenter(appFilePath);
    }

    public static void MoveCustomGradleScript(string pathToBuiltProject)
    {
        var appAdditionsFolder = AppCenterSettingsContext.AppCenterPath + "/AppCenter/Plugins/Android/Push/AppAdditions";
        var sourcePath = Path.Combine(appAdditionsFolder, "appcenterpush.gradle");
        var destPath = Path.Combine(pathToBuiltProject, "appcenterpush.gradle");
        File.Copy(sourcePath, destPath, true);
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
                string.Format("\nclasspath 'com.google.gms:google-services:{0}'\n", GOOGLE_SERVICES_VERSION),
                "\napply from: 'appcenterpush.gradle'"
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
            "google()\njcenter()"
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

