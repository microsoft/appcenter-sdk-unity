#tool nuget:?package=XamarinComponent
#addin nuget:?package=Cake.Xamarin
#addin nuget:?package=Cake.FileHelpers
#addin nuget:?package=Newtonsoft.Json
#load "utility.cake"

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;

// Task Target for build
var Target = Argument("target", Argument("t", "Default"));

string ArchiveDirectory = "archives";
bool IsMandatory = false;
string DistributionGroup = "Private Release Script Group";
string Token = EnvironmentVariable("APP_CENTER_API_TOKEN");
string BaseUrl = "https://api.appcenter.ms";
ApplicationInfo CurrentApp = null;
string ProjectPath = "AppCenterDemoApp";
string BuildFolder = GetBuildFolder("Demo", ProjectPath);

public enum Environment
{
    Int,
    Prod
}

public enum Platform
{
    iOS,
    Android,
    UWP
}

public enum ScriptingBackend
{
    Il2Cpp,
    Mono,
    DotNet
}

public class ApplicationInfo
{
    public static string OutputDirectory;
    public Environment AppEnvironment { get; }
    public ScriptingBackend AppScriptingBackend { get; }
    public Platform AppPlatform { get; }
    public string AppOwner { get; }
    public string AppId { get; }
    public string BuildMethod { get; }
    public string IncrementVersionMethod { get; }

    public string AppPath
    {
        get
        {
            return OutputDirectory + "/" + AppId + "." + _appExtension;
        }
    }

    private string _appExtension = null;
    public ApplicationInfo(Environment environment, Platform platform, ScriptingBackend backend, string appOwner, string appId, string buildMethod = "", string incrementVersionMethod = "", string appExtension = "")
    {
        AppOwner = appOwner;
        AppId = appId;
        AppScriptingBackend = backend;
        AppEnvironment = environment;
        AppPlatform = platform;
        BuildMethod = buildMethod;
        IncrementVersionMethod = incrementVersionMethod;
        _appExtension = appExtension;
    }
}

ApplicationInfo.OutputDirectory = ArchiveDirectory;
IList<ApplicationInfo> Applications = new List<ApplicationInfo>
{
    new ApplicationInfo(Environment.Int, Platform.iOS, ScriptingBackend.Mono, "alchocro", "iOS-Unity-Puppet", "BuildPuppet.BuildPuppetSceneIosMonoDeviceSdk", "BuildPuppet.IncrementVersionNumber", "ipa"),
    new ApplicationInfo(Environment.Int, Platform.iOS, ScriptingBackend.Il2Cpp, "alchocro", "iOS-Unity-Puppet", "BuildPuppet.BuildPuppetSceneIosIl2CPPDeviceSdk", "BuildPuppet.IncrementVersionNumber", "ipa"),
    new ApplicationInfo(Environment.Int, Platform.Android, ScriptingBackend.Mono, "alchocro", "Unity-Android-Puppet", "BuildPuppet.BuildPuppetSceneAndroidMono", "BuildPuppet.IncrementVersionNumber", "apk"),
    new ApplicationInfo(Environment.Int, Platform.Android, ScriptingBackend.Il2Cpp, "alchocro", "Unity-Android-Puppet", "BuildPuppet.BuildPuppetSceneAndroidIl2CPP", "BuildPuppet.IncrementVersionNumber", "apk"),
    new ApplicationInfo(Environment.Int, Platform.UWP, ScriptingBackend.DotNet, "alchocro", "UWP-Unity-Puppet"),
    new ApplicationInfo(Environment.Int, Platform.UWP, ScriptingBackend.Il2Cpp, "alchocro", "UWP-Unity-Puppet"),
    new ApplicationInfo(Environment.Prod, Platform.iOS, ScriptingBackend.Mono, "mobile-center-sdk", "iOS-Unity-Demo-App", "BuildDemo.BuildDemoSceneIosMonoDeviceSdk", "BuildDemo.IncrementVersionNumber", "ipa"),
    new ApplicationInfo(Environment.Prod, Platform.iOS, ScriptingBackend.Il2Cpp, "mobile-center-sdk", "iOS-Unity-Demo-App", "BuildDemo.BuildDemoSceneIosIl2CPPDeviceSdk", "BuildDemo.IncrementVersionNumber", "ipa"),
    new ApplicationInfo(Environment.Prod, Platform.Android, ScriptingBackend.Mono, "mobile-center-sdk", "Android-Unity-Demo-App", "BuildDemo.BuildDemoSceneAndroidMono", "BuildDemo.IncrementVersionNumber", "apk"),
    new ApplicationInfo(Environment.Prod, Platform.Android, ScriptingBackend.Il2Cpp, "mobile-center-sdk", "Android-Unity-Demo-App", "BuildDemo.BuildDemoSceneAndroidIl2CPP", "BuildDemo.IncrementVersionNumber", "apk"),
    new ApplicationInfo(Environment.Prod, Platform.UWP, ScriptingBackend.DotNet, "mobile-center-sdk", "UWP-Unity-Demo-App"),
    new ApplicationInfo(Environment.Prod, Platform.UWP, ScriptingBackend.Il2Cpp, "mobile-center-sdk", "UWP-Unity-Demo-App")
};

Setup(context =>
{
    // Arguments:
    //  -Environment:       App "environment" ("prod" or "int") -- Default is "int"
    //  -Group:             Distribution group name -- Default is "Private Release Script Group"
    //  -Mandatory:         Should the release be mandatory ("true" or "false") -- Default is "false"
    //  -Platform:          ios, android, or uwp -- Default is "ios"
    //  -ScriptingBackend:  dotnet, mono, or il2cpp -- Default is "il2cpp"

    // Read arguments
    var environment = Environment.Prod;
    if (Argument("Environment", "int") == "int")
    {
        environment = Environment.Int;
        Token = EnvironmentVariable("APP_CENTER_INT_API_TOKEN");
        BaseUrl = "https://asgard-int.trafficmanager.net/api";
        ProjectPath = ".";
        BuildFolder = GetBuildFolder("Puppet", ProjectPath);
    }

    var platformString = Argument<string>("Platform", "ios");
    var platform = Platform.iOS;

    if (platformString == "android")
    {
        platform = Platform.Android;
    }
    else if (platformString == "uwp")
    {
        platform = Platform.UWP;
    }

    var backend = ScriptingBackend.Il2Cpp;
    var backendString = Argument("ScriptingBackend", "il2cpp");
    if (backendString == "dotnet")
    {
        backend = ScriptingBackend.DotNet;
    }
    else if (backendString == "mono")
    {
        backend = ScriptingBackend.Mono;
    }

    try
    {
        CurrentApp = (  from    app in Applications
                        where   app.AppPlatform == platform &&
                                app.AppEnvironment == environment &&
                                app.AppScriptingBackend == backend
                        select  app
        ).Single();
    }
    catch
    {
        Error("No application configuration exists with the given arguments.");
    }
    DistributionGroup = Argument<string>("Group", DistributionGroup);
    DistributionGroup = DistributionGroup.Replace('_', ' ');
    IsMandatory = Argument<bool>("Mandatory", false);
});

// Distribution Tasks

Task("CreateIosArchive").IsDependentOn("IncreaseIosVersion")
.Does(()=>
{
    ExecuteUnityMethod(CurrentApp.BuildMethod, "ios", ProjectPath);
    var xcodeProjectPath = GetDirectories(BuildFolder + "/*/*.xcodeproj").Single().FullPath;
    Information("Creating archive...");
    var archiveName = Statics.TemporaryPrefix + "iosArchive.xcarchive";
    StartProcess("xcodebuild", "-project \"" + xcodeProjectPath + "\" -configuration Release -scheme Unity-iPhone -archivePath \"" + archiveName + "\" archive");

    // Just create the empty plist file here so it doesn't cluttering the repo.
    var plistName = Statics.TemporaryPrefix + "exportoptions.plist";
    System.IO.File.WriteAllText(plistName,
        "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
        "<!DOCTYPE plist PUBLIC \"-//Apple//DTD PLIST 1.0//EN\" \"http://www.apple.com/DTDs/PropertyList-1.0.dtd\">" +
        "<plist version=\"1.0\"><dict></dict></plist>");
    Information("Creating ipa...");
    DeleteFileIfExists(CurrentApp.AppPath);
    StartProcess("xcodebuild", "-exportArchive -archivePath \"" + archiveName +
                    "\" -exportPath \"" + CurrentApp.AppPath +
                    "\" -exportOptionsPlist \"" + plistName + "\"");
    var ipaFile = GetFiles(CurrentApp.AppPath + "/*.ipa").Single();
    var temporaryIpaPath = ArchiveDirectory + "/" + Statics.TemporaryPrefix + "tempIpa.ipa";
    MoveFile(ipaFile, temporaryIpaPath);
    DeleteDirectoryIfExists(CurrentApp.AppPath);
    var temporaryIpaFile = File(temporaryIpaPath);
    MoveFile(temporaryIpaFile, CurrentApp.AppPath);
}).Finally(() => RunTarget("RemoveTemporaries"));

Task("CreateAndroidArchive").IsDependentOn("IncreaseAndroidVersion").Does(()=>
{
    ExecuteUnityMethod(CurrentApp.BuildMethod, "android", ProjectPath);
    var apkFile = GetFiles(BuildFolder + "/*.apk").Single();
    MoveFile(apkFile, CurrentApp.AppPath);
}).Finally(() => RunTarget("RemoveTemporaries"));

Task("IncreaseIosVersion").Does(()=>
{
    ExecuteUnityMethod(CurrentApp.IncrementVersionMethod, "ios", ProjectPath);
});

Task("IncreaseAndroidVersion").Does(()=>
{
    ExecuteUnityMethod(CurrentApp.IncrementVersionMethod, "android", ProjectPath);
});

Task("ReleaseApplication")
.Does(()=>
{
    if (CurrentApp.AppPlatform == Platform.iOS)
    {
        RunTarget("CreateIosArchive");
    }
    else if (CurrentApp.AppPlatform == Platform.Android)
    {
        RunTarget("CreateAndroidArchive");
    }
    else
    {
        Error("Cannot distribute for this platform.");
        return;
    }

    // Start the upload.
    Information("Initiating distribution process...");
    var startUploadUrl = GetApiUrl(BaseUrl, CurrentApp.AppOwner, CurrentApp.AppId, "release_uploads");
    var startUploadRequest = GetWebRequest(startUploadUrl, Token);
    var startUploadResponse = GetResponseJson(startUploadRequest);

    // Upload the file to the given endpoint. The label "ipa" is correct for all platforms.
    var uploadUrl = startUploadResponse["upload_url"].ToString();
    HttpUploadFile(uploadUrl, CurrentApp.AppPath, "ipa");

    // Commit the upload
    Information("Committing distribution...");
    var uploadId = startUploadResponse["upload_id"].ToString();
    var commitRequestUrl = startUploadUrl + "/" + uploadId;
    var commitRequest = GetWebRequest(commitRequestUrl, Token, "PATCH");
    AttachJsonPayload(commitRequest,
                        new JObject(
                            new JProperty("status", "committed")));
    var commitResponse = GetResponseJson(commitRequest);
    var releaseUrl = BaseUrl + "/" + commitResponse["release_url"].ToString();

    // Release the upload
    Information("Finalizing release...");
    var releaseRequest = GetWebRequest(releaseUrl, Token, "PATCH");
    var releaseNotes = "This release has been created by the script test-tools.cake.";
    AttachJsonPayload(releaseRequest,
                        new JObject(
                            new JProperty("destination_name", DistributionGroup),
                            new JProperty("release_notes", releaseNotes),
                            new JProperty("mandatory", IsMandatory.ToString().ToLower())));
    releaseRequest.GetResponse().Dispose();
    var mandatorySuffix = IsMandatory ? " as a mandatory update" : "";
    Information("Successfully released " + CurrentApp.AppOwner +
                    "/" + CurrentApp.AppId + " to group " +
                    DistributionGroup + mandatorySuffix + ".");
});

// Push tasks
Task("SendPushNotification")
.Does(()=>
{
    var name = "Test Notification";
    var title = "Test Notification";
    var timeSent = DateTime.Now.ToString();
    var body = "Notification sent from test script at " + timeSent + ".";
    var properties = new Dictionary<string, string> {{"time_sent", timeSent}};
    var notificationJson = new JObject(
                            new JProperty("notification_content",
                                new JObject(
                                    new JProperty("name", name),
                                    new JProperty("title", title),
                                    new JProperty("body", body),
                                    new JProperty("custom_data",
                                        new JObject(
                                            from key in properties.Keys
                                            select new JProperty(key, properties[key]))))));
    Information("Sending notification:\n" + notificationJson.ToString());
    var url = GetApiUrl(BaseUrl, CurrentApp.AppOwner, CurrentApp.AppId, "push/notifications");
    var request = GetWebRequest(url, Token);
    AttachJsonPayload(request, notificationJson);
    var responseJson = GetResponseJson(request);
    Information("Successfully sent push notification and received result:\n" + responseJson.ToString());
});

// Helper methods

string GetApiUrl(string baseUrl, string appOwner, string appId, string apiName)
{
    return string.Format("{0}/v0.1/apps/{1}/{2}/{3}", baseUrl, appOwner, appId, apiName);
}

JObject GetResponseJson(HttpWebRequest request)
{
    using (var response = request.GetResponse())
    using (var reader = new StreamReader(response.GetResponseStream()))
    {
        return JObject.Parse(reader.ReadToEnd());
    }
}

HttpWebRequest GetWebRequest(string url, string token, string method = "POST")
{
    Information(string.Format("About to call url '{0}'", url));
    var request = (HttpWebRequest)WebRequest.Create(url);
    request.Headers["X-API-Token"] = token;
    request.ContentType = "application/json";
    request.Accept = "application/json";
    request.Method = method;
    return request;
}

void AttachJsonPayload(HttpWebRequest request, JObject json)
{
    using (var stream = request.GetRequestStream())
    using (var sr = new StreamWriter(stream))
    {
        sr.Write(json.ToString());
    }
}

// Adapted from https://stackoverflow.com/questions/566462/upload-files-with-httpwebrequest-multipart-form-data/2996904#2996904
void HttpUploadFile(string url, string file, string paramName)
{
    Information(string.Format("Uploading {0} to {1}", file, url));
    var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
    byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
    var request = (HttpWebRequest)WebRequest.Create(url);
    request.ContentType = "multipart/form-data; boundary=" + boundary;
    request.Method = "POST";
    request.KeepAlive = true;
    using (var requestStream = request.GetRequestStream())
    {
        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
        var headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n\r\n";
        var header = string.Format(headerTemplate, paramName, file);
        byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);
        requestStream.Write(headerBytes, 0, headerBytes.Length);
        using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
        {
            byte[] buffer = new byte[4096];
            var bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
            }
        }
        byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        requestStream.Write(trailer, 0, trailer.Length);
    }
    request.GetResponse().Dispose();
    Information("File uploaded.");
}

Task("clean").IsDependentOn("RemoveTemporaries");

Task("Default").Does(()=>
{
    Error("Please run a specific target.");
});

RunTarget(Target);
