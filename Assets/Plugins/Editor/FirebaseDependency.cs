using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using GooglePlayServices;
using UnityEditor;
using UnityEngine;

/// <summary>
/// This file is used to define dependencies, and pass them along to a system
/// which can resolve dependencies.
/// </summary>
[InitializeOnLoad]
public class FirebaseDependency : AssetPostprocessor
{
    private static string EXECUTABLE_NAME_WINDOWS = "generate_xml_from_google_services_json.exe";
    private static string EXECUTABLE_NAME_GENERIC = "generate_xml_from_google_services_json.py";
    private static string EXECUTABLE_LOCATION = "Assets/Firebase/Editor";
    private static string GOOGLE_SERVICES_FILE_BASENAME = "google-services";
    private static string GOOGLE_SERVICES_INPUT_FILE = GOOGLE_SERVICES_FILE_BASENAME + ".json";
    private static string GOOGLE_SERVICES_OUTPUT_FILE = GOOGLE_SERVICES_FILE_BASENAME + ".xml";
    private static string GOOGLE_SERVICES_OUTPUT_DIRECTORY = "Assets/Plugins/Android/Firebase/res/values";
    private static string GOOGLE_SERVICES_OUTPUT_PATH = Path.Combine(GOOGLE_SERVICES_OUTPUT_DIRECTORY, GOOGLE_SERVICES_OUTPUT_FILE);

    /// <summary>
    /// This is the entry point for "InitializeOnLoad". It will register the
    /// dependencies with the dependency tracking system.
    /// </summary>
    static FirebaseDependency()
    {
        SetupDependencies();
    }

    static void SetupDependencies()
    {
#if UNITY_ANDROID
        
        // Setup the resolver using reflection as the module may not be
        // available at compile time.
        Type playServicesSupport = Google.VersionHandler.FindClass(
            "Google.JarResolver", "Google.JarResolver.PlayServicesSupport");
        if (playServicesSupport == null) {
            return;
        }

        object svcSupport = Google.VersionHandler.InvokeStaticMethod(
            playServicesSupport, "CreateInstance",
            new object[] {
                "FirebaseMessaging",
                EditorPrefs.GetString("AndroidSdkRoot"),
                "ProjectSettings"
            });

        Google.VersionHandler.InvokeInstanceMethod(
            svcSupport, "DependOn",
            new object[] {
                "com.google.firebase",
                "firebase-messaging",
                "11.0.0"
            },
            namedArgs: new Dictionary<string, object>() {
                { "packageIds",
                    new string[] {
                        "extra-google-m2repository",
                        "extra-android-m2repository"
                    }
                },
                { "repositories",
                    null
                }
            });
        Google.VersionHandler.InvokeInstanceMethod(
            svcSupport, "DependOn",
            new object[] {
                "com.google.firebase",
                "firebase-core",
                "11.0.0"
            },
            namedArgs: new Dictionary<string, object>() {
                { "packageIds",
                    new string[] {
                        "extra-google-m2repository",
                        "extra-android-m2repository"
                    }
                },
                { "repositories",
                    null
                }
            });
#endif
    }

    static List<string> ReadBundleIds(string googleServicesFile)
    {
        SortedDictionary<string, string> sortedDictionary = new SortedDictionary<string, string>();
        CommandLine.Result result = RunResourceGenerator("-i \"" + googleServicesFile + "\" -l");
        if (result.exitCode == 0)
        {
            foreach (string index in result.stdout.Split('\r', '\n'))
            {
                if (!string.IsNullOrEmpty(index))
                    sortedDictionary[index] = index;
            }
        }
        return new List<string>(sortedDictionary.Keys);
    }

    static SortedDictionary<string, List<string>> ReadBundleIdsFromGoogleServicesFiles()
    {
        SortedDictionary<string, List<string>> sortedDictionary = new SortedDictionary<string, List<string>>();
        foreach (string asset in AssetDatabase.FindAssets(GOOGLE_SERVICES_FILE_BASENAME))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(asset);
            if (Path.GetFileName(assetPath) == GOOGLE_SERVICES_INPUT_FILE)
                sortedDictionary[assetPath] = null;
        }
        foreach (string googleServicesFile in new List<string>(sortedDictionary.Keys))
            sortedDictionary[googleServicesFile] = ReadBundleIds(googleServicesFile);
        return sortedDictionary;
    }

    static string FindGoogleServicesFile()
    {
        var bundleIdsByConfigFile = ReadBundleIdsFromGoogleServicesFiles();
        var bundleId = UnityCompat.ApplicationId;
        if (bundleIdsByConfigFile.Count == 0)
        {
            return null;
        }
        string str = null;
        foreach (KeyValuePair<string, List<string>> keyValuePair in bundleIdsByConfigFile)
        {
            if (keyValuePair.Value.Contains(bundleId))
                str = keyValuePair.Key;
        }
        return str;
    }

    static string GetProjectDir()
    {
        return Path.Combine(Application.dataPath, "..");
    }

    static CommandLine.Result RunResourceGenerator(string arguments)
    {
        bool flag = Application.platform == RuntimePlatform.WindowsEditor;
        string str1 = Path.Combine(Path.Combine(GetProjectDir(), EXECUTABLE_LOCATION), !flag ? EXECUTABLE_NAME_GENERIC : EXECUTABLE_NAME_WINDOWS);
        string toolPath;
        string arguments1;
        if (flag)
        {
            toolPath = str1;
            arguments1 = arguments;
        }
        else
        {
            toolPath = "python";
            arguments1 = "\"" + str1 + "\" " + arguments;
        }
        try
        {
            return CommandLine.Run(toolPath, arguments1);
        }
        catch (Win32Exception ex)
        {
            Debug.LogException(ex);
            return new CommandLine.Result { exitCode = 1 };
        }
    }

    static void GenerateXmlResources(string googleServicesFile)
    {
        string projectDir = GetProjectDir();
        string path1 = Path.Combine(projectDir, GOOGLE_SERVICES_OUTPUT_DIRECTORY);
        if (!Directory.Exists(path1))
        {
            try
            {
                Directory.CreateDirectory(path1);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return;
            }
        }
        string str = Path.Combine(projectDir, googleServicesFile);
        string path2 = Path.Combine(projectDir, GOOGLE_SERVICES_OUTPUT_PATH);
        if (File.Exists(path2) && File.GetLastWriteTime(path2).CompareTo(File.GetLastWriteTime(str)) >= 0)
            return;
        RunResourceGenerator("-i \"" + str + "\" -o \"" + path2 + "\" -p \"" + UnityCompat.ApplicationId + "\"");
    }

    static void UpdateJson()
    {
#if UNITY_ANDROID
        string googleServicesFile = FindGoogleServicesFile();
        if (googleServicesFile == null)
            return;
        GenerateXmlResources(googleServicesFile);
#endif
    }

    /// <summary>
    /// Handle delayed loading of the dependency resolvers.
    /// </summary>
    static void OnPostprocessAllAssets(
        string[] importedAssets, string[] deletedAssets,
        string[] movedAssets, string[] movedFromPath)
    {
        foreach (string asset in importedAssets)
        {
            if (asset.Contains("JarResolver"))
            {
                SetupDependencies();
            }
            else if (Path.GetFileName(asset) == GOOGLE_SERVICES_INPUT_FILE)
            {
                UpdateJson();
            }
        }
    }
}
