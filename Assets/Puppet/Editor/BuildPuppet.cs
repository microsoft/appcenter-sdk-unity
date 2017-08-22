
using System.IO;
using UnityEditor;

public class BuildPuppet
{
    public static void BuildPuppetSceneAndroidMono()
    {
        CreateGoogleServicesJsonIfNotPresent();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
        BuildPuppetScene(BuildTarget.Android, "PuppetBuilds/AndroidMonoBuild");
    }

    public static void BuildPuppetSceneAndroidIl2CPP()
    {
        CreateGoogleServicesJsonIfNotPresent();
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.Android, "PuppetBuilds/AndroidIL2CPPBuild");
    }

    public static void BuildPuppetSceneIosMono()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.Mono2x);
        BuildPuppetScene(BuildTarget.iOS, "PuppetBuilds/iOSMonoBuild");
    }

    public static void BuildPuppetSceneIosIl2CPP()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.iOS, "PuppetBuilds/iOSIL2CPPBuild");
    }

    public static void BuildPuppetSceneWsaNet()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.WinRTDotNET);
        BuildPuppetScene(BuildTarget.WSAPlayer, "PuppetBuilds/WSANetBuild");
    }

    public static void BuildPuppetSceneWsaIl2CPP()
    {
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.WSA, ScriptingImplementation.IL2CPP);
        BuildPuppetScene(BuildTarget.WSAPlayer, "PuppetBuilds/WSAIL2CPPBuild");
    }

    private static void BuildPuppetScene(BuildTarget target, string outputDir)
    {
        string[] puppetScene = { "Assets/Puppet/PuppetScene.unity" };
        var options = new BuildPlayerOptions
        {
            scenes = puppetScene,
            options = BuildOptions.StrictMode,
            locationPathName = outputDir,
            target = target
        };
        BuildPipeline.BuildPlayer(options);
    }

    private static void CreateGoogleServicesJsonIfNotPresent()
    {
        if (File.Exists("Assets/google-services.json"))
        {
            return;
        }
        File.Move("Assets/google-services-placeholder.json", "Assets/google-services.json");
        var importer = AssetImporter.GetAtPath("Assets/google-services.json");
        importer.SaveAndReimport();
    }
}
