// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using System;
using System.Reflection;

public class ProjectCapabilityManagerWrapper
{
    private static readonly Type ProjectCapabilityManagerType;
    private object _capabilityManager;

    static ProjectCapabilityManagerWrapper()
    {
        var xcExtensionsAssembly = Assembly.Load("UnityEditor.iOS.Extensions.Xcode");
        if (xcExtensionsAssembly != null)
        {
            ProjectCapabilityManagerType = xcExtensionsAssembly.GetType("UnityEditor.iOS.Xcode.ProjectCapabilityManager");
        }
    }

    public void AddRemoteNotificationsToBackgroundModes()
    {
        var backgroundModesEnumType = ProjectCapabilityManagerType.Assembly.GetType("UnityEditor.iOS.Xcode.BackgroundModesOptions");
        var remoteNotifEnum = Enum.Parse(backgroundModesEnumType, "RemoteNotifications");
        ProjectCapabilityManagerType.GetMethod("AddBackgroundModes").Invoke(_capabilityManager, new object[] { remoteNotifEnum });
    }

    public static bool ProjectCapabilityManagerIsAvailable
    {
        get
        {
            return ProjectCapabilityManagerType != null;
        }
    }

    public ProjectCapabilityManagerWrapper(string projectPath, string targetName, string targetGuid)
    {
#if UNITY_2019_3_OR_NEWER
        _capabilityManager = ProjectCapabilityManagerType
            .GetConstructor(new[] { typeof(string), typeof(string), typeof(string), typeof(string) })
            .Invoke(new object[] { projectPath, targetName + ".entitlements", targetName, targetGuid });
#else
        _capabilityManager = ProjectCapabilityManagerType
            .GetConstructor(new[] { typeof(string), typeof(string), typeof(string)})
            .Invoke(new object[] { projectPath, targetName + ".entitlements", targetName });
#endif
    }

    public void WriteToFile()
    {
        ProjectCapabilityManagerType.GetMethod("WriteToFile").Invoke(_capabilityManager, null);
    }
}
