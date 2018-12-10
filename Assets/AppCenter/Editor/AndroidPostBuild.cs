using Microsoft.AppCenter.Unity;
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
    public int callbackOrder { get { return 0; } }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        var settings = AppCenterSettingsContext.SettingsInstance;
        if (settings.UsePush && AppCenter.Push != null)
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

        if (!AndroidGradleHelper.MoveGoogleJsonFile(path))
        {
            return;
        }
        AndroidGradleHelper.MoveCustomGradleScript(path);
        if (!AndroidGradleHelper.InjectFirebaseDependencies(path))
        {
            return;
        }
        var resourceGradleFilePath = Path.Combine(path, "unity-android-resources\\build.gradle");
        AndroidGradleHelper.SwapGoogleAndJcenter(resourceGradleFilePath);
        var mainGradleFilePath = Path.Combine(path, "build.gradle");
        AndroidGradleHelper.SwapGoogleAndJcenter(mainGradleFilePath);
    }   
}

