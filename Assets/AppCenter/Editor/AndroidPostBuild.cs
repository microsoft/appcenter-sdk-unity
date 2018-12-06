using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.Android;
using UnityEngine;

namespace Assets.AppCenter.Editor
{
    class AndroidPostBuild : IPostGenerateGradleAndroidProject
    {
        public int callbackOrder { get { return 0; } }

        public void OnPostGenerateGradleAndroidProject(string path)
        {
            Debug.Log("fdsfs");
            MoveGoogleJsonFile(path);
            SwapGoogleAndJcenter(path);
            InjectFirebaseDependencies(path);
        }        

        public static void MoveGoogleJsonFile(string pathToBuiltProject)
        {
            var sourcePath = "Assets\\google-services.json";
            if (!File.Exists(sourcePath))
            {
                Debug.LogError("Please add google-services.json file to Assets folder in order for Push service to work!");
            }
            else
            {
                var destPath = Path.Combine(pathToBuiltProject, "google-services.json");
                File.Copy(sourcePath, destPath, true);
            }
        }

        public static void InjectFirebaseDependencies(string pathToBuiltProject)
        {
            string[] regexPatterns = new string[] 
            {
                "jcenter\\(\\)",
                "google\\(\\)",
                "com.android.tools.build:gradle:[0-9.]*'",
                "flatDir",
                "unity-android-resources'\\)",
                "com.android.application'"
            };

            string[] codePartsToInsert = 
                {
                "",
                "\njcenter()",
                "\nclasspath 'com.google.gms:google-services:4.0.1'\n",
                "\ngoogle()\njcenter()\n",
                "\napi 'com.google.firebase:firebase-core:16.0.1'\napi 'com.google.firebase:firebase-messaging:17.0.0'\n",
                "\napply plugin: 'com.google.gms.google-services'"
            };

            int[] indexesOfItemsToBeInsertedAtStart = { 3 };
            int[] indexesOfItemsToBeReplacedFully = { 0 };

            var appFilePath = Path.Combine(pathToBuiltProject, "build.gradle");
            ReplaceCodeParts(
                appFilePath,
                regexPatterns,
                codePartsToInsert,
                indexesOfItemsToBeReplacedFully,
                indexesOfItemsToBeInsertedAtStart
            );
        }

        private static void ReplaceCodeParts(string appFilePath, string[] regexPatterns, string[] codePartsToInsert,
        int[] indexesOfItemsToBeReplacedFully, int[] indexesOfItemsToBeInsertedAtStart)
        {
            var fileText = File.ReadAllText(appFilePath);
            for (var i = 0; i < regexPatterns.Length; i++)
            {
                var regex = new Regex(regexPatterns[i]);
                var matches = regex.Match(fileText);
                if (matches.Success)
                {
                    var codeToReplace = matches.ToString();
                    var codeToInsert = "";
                    if (indexesOfItemsToBeReplacedFully.Contains(i))
                    {
                        codeToInsert = codePartsToInsert[i];
                    }
                    else
                    {
                        codeToInsert = indexesOfItemsToBeInsertedAtStart.Contains(i) ? (codePartsToInsert[i] + codeToReplace) : (codeToReplace + codePartsToInsert[i]);
                    }
                    fileText = fileText.Replace(codeToReplace, codeToInsert);
                }
                else
                {
                    // TODO Update documentation link
                    Debug.LogError("Unable to automatically modify file '" + appFilePath + "'. For App Center Push to work properly, " +
                        "please follow troubleshooting instructions at https://docs.microsoft.com/en-us/mobile-center/sdk/troubleshooting/unity");
                    return;
                }
            }
            File.WriteAllText(appFilePath, fileText);
        }

        public static void SwapGoogleAndJcenter(string pathToBuiltProject)
        {
            string[] regexPatterns = new string[] 
            {
                "jcenter\\(\\)",
                "google\\(\\)"
            };

            string[] codePartsToInsert = new string[]
            {
                "",
                "\njcenter()"
            };

            int[] indexesOfItemsToBeInsertedAtStart = { };
            int[] indexesOfItemsToBeReplacedFully = { 0 };

            var appFilePath = Path.Combine(pathToBuiltProject, "unity-android-resources/build.gradle");
            ReplaceCodeParts(
                appFilePath, 
                regexPatterns, 
                codePartsToInsert, 
                indexesOfItemsToBeReplacedFully, 
                indexesOfItemsToBeInsertedAtStart
            );
        }
    }
}
