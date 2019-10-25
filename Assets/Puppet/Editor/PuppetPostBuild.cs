// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

#if UNITY_2018_1_OR_NEWER
public class PuppetPostBuild : IPostprocessBuildWithReport
#else
public class AppCenterPostBuild : IPostprocessBuild
#endif
{
    public int callbackOrder { get { return 0; } }

#if UNITY_2018_1_OR_NEWER
    public void OnPostprocessBuild(BuildReport report)
    {
        OnPostprocessBuild(report.summary.platform, report.summary.outputPath);
    }
#endif

    public void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS &&
            PBXProjectWrapper.PBXProjectIsAvailable &&
            PlistDocumentWrapper.PlistDocumentIsAvailable)
        {
            var pbxProject = new PBXProjectWrapper(pathToBuiltProject);

            // Update project.
            OnPostprocessProject(pbxProject);
            pbxProject.WriteToFile();

            // Update Info.plist.
            var settings = AppCenterSettingsContext.SettingsInstance;
            var infoPath = pathToBuiltProject + "/Info.plist";
            var info = new PlistDocumentWrapper(infoPath);
            var rootDict = info.GetRoot();
            var setStringMethod = rootDict.GetType().GetMethod("SetString");
            setStringMethod.Invoke(rootDict, new object[] { "NSCameraUsageDescription", "Select photo for attachment" });
            setStringMethod.Invoke(rootDict, new object[] { "NSPhotoLibraryUsageDescription", "Select photo for attachment" });
            info.WriteToFile();
        }
    }

    private static void OnPostprocessProject(PBXProjectWrapper project)
    {
        project.AddBuildProperty("OTHER_LDFLAGS", "-framework Photos");
    }
}
