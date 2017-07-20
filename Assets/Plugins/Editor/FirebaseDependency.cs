using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        string output = RunResourceGenerator("-i \"" + googleServicesFile + "\" -l");
        if (output != null)
        {
            foreach (string index in output.Split('\r', '\n'))
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
        var bundleId = GetApplicationId();
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
        if (str == null)
        {
            Debug.LogError(string.Format("Project Bundle ID {0} does not match any bundle IDs in your google-services.json files\n" +
                          "This will result in an app that will fail to initialize.\n\nAvailable Bundle IDs:\n{1}",
                bundleId, string.Join("\n", bundleIdsByConfigFile.SelectMany(i => i.Value).ToArray())));
        }
        return str;
    }

    static string GetProjectDir()
    {
        return Path.Combine(Application.dataPath, "..");
    }

    static string GetApplicationId()
    {
#if UNITY_5_6_OR_NEWER
        return PlayerSettings.applicationIdentifier;
#else
        return PlayerSettings.bundleIdentifier;
#endif
    }

    static string RunResourceGenerator(string arguments)
    {
        string command;
        string executable;
        string arguments1;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            executable = Path.Combine(Path.Combine(GetProjectDir(), EXECUTABLE_LOCATION), EXECUTABLE_NAME_WINDOWS);
            command = executable;
            arguments1 = arguments;
        }
        else
        {
            executable = Path.Combine(Path.Combine(GetProjectDir(), EXECUTABLE_LOCATION), EXECUTABLE_NAME_GENERIC);
            command = "python";
            arguments1 = "\"" + executable + "\" " + arguments;
        }
        try
        {
            var buildProcess = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = arguments1,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            buildProcess.Start();
            buildProcess.WaitForExit();
            if (buildProcess.ExitCode == 0)
            {
                return buildProcess.StandardOutput.ReadToEnd();
            }
            else
            {
                string error = buildProcess.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                    Debug.LogError(error);
                return null;
            }
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            return null;
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
        RunResourceGenerator("-i \"" + str + "\" -o \"" + path2 + "\" -p \"" + GetApplicationId() + "\"");
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
