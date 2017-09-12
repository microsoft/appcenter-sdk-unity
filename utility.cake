// This contains various utilities that are or can be used by multiple cake scripts.

// Prefix for temporary intermediates that are created by this script
var TemporaryPrefix = "CAKE_SCRIPT_TEMP";

// Location of puppet application builds
 var PuppetBuildsFolder = "PuppetBuilds";

static int ExecuteUnityCommand(string extraArgs, ICakeContext context)
{
    var projectDir = context.MakeAbsolute(context.Directory("."));
    var unityPath = context.EnvironmentVariable("UNITY_PATH");

    // If environment variable is not set, use default locations
    if (unityPath == null)
    {
        if (context.IsRunningOnUnix())
        {
            unityPath = "/Applications/Unity/Unity.app/Contents/MacOS/Unity";
        }
        else
        {
            unityPath = "C:\\Program Files\\Unity\\Editor\\Unity.exe";
        }
    }

    // Unity log file
    var unityLogFile = "CAKE_SCRIPT_TEMPunity_build_log.log";
    var unityArgs = "-batchmode -quit -logFile " + unityLogFile + " -projectPath " + projectDir + " " + extraArgs;
    System.IO.File.Create(unityLogFile).Dispose();
    var logExec = "powershell.exe";
    var logArgs = "Get-Content -Path " + unityLogFile + " -Wait";
    if (context.IsRunningOnUnix())
    {
        logExec = "tail";
        logArgs = "-f " + unityLogFile;
    }
    int result = 0;
    using (var unityProcess = context.StartAndReturnProcess(unityPath, new ProcessSettings{ Arguments = unityArgs }))
    {
        using (var logProcess = context.StartAndReturnProcess(logExec, new ProcessSettings{ Arguments = logArgs, RedirectStandardError = true}))
        {
            unityProcess.WaitForExit();
            result = unityProcess.GetExitCode();
            if (logProcess.WaitForExit(0) &&
                logProcess.GetExitCode() != 0)
            {
                context.Warning("There was an error logging, but command still executed.");
            }
            else try
            {
                logProcess.Kill();
            }
            catch
            {
                // Log process was stopped right after checking
            }
        }
    }
    if (System.IO.File.Exists(unityLogFile))
    {
        context.DeleteFile(unityLogFile);
    }
    return result;
}

void ExecuteUnityMethod(string buildMethodName, string buildTarget)
{
    Information("Executing method " + buildMethodName + ", this could take a while...");
    var command = "-executeMethod " + buildMethodName + " -buildTarget " + buildTarget;
    var result = ExecuteUnityCommand(command, Context);
    if (result != 0)
    {
        throw new Exception("Failed to execute method " + buildMethodName + ".");
    }
}

// Copy files to a clean directory using string names instead of FilePath[] and DirectoryPath
void CopyFiles(IEnumerable<string> files, string targetDirectory, bool clean = true)
{
    if (clean)
    {
        CleanDirectory(targetDirectory);
    }
    foreach (var file in files)
    {
        CopyFile(file, targetDirectory + "/" + System.IO.Path.GetFileName(file));
    }
}

void DeleteDirectoryIfExists(string directoryName)
{
    if (DirectoryExists(directoryName))
    {
        DeleteDirectory(directoryName, true);
    }
}

void CleanDirectory(string directoryName)
{
    DeleteDirectoryIfExists(directoryName);
    CreateDirectory(directoryName);
}

void HandleError(Exception exception)
{
    RunTarget("clean");
    throw exception;
}

// Remove all temporary files and folders
Task("RemoveTemporaries").Does(()=>
{
    DeleteFiles(TemporaryPrefix + "*");
    var dirs = GetDirectories(TemporaryPrefix + "*");
    foreach (var directory in dirs)
    {
        DeleteDirectory(directory, true);
    }
    CleanDirectory(PuppetBuildsFolder);
    DeleteFiles("./nuget/*.temp.nuspec");
});
