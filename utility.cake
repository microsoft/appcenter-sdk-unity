// This file contains various utilities that are or can be used by multiple cake scripts.

// Static variables defined outside of a class can cause issues.
public class Statics
{
    // Cake context.
    public static ICakeContext Context { get; set; }

    // Prefix for temporary intermediates that are created by this script.
    public const string TemporaryPrefix = "CAKE_SCRIPT_TEMP";
}

// Can't reference Context within the class, so set value outside
Statics.Context = Context;

static int ExecuteUnityCommand(string extraArgs, string projectPath = ".")
{
    var projectDir = Statics.Context.MakeAbsolute(Statics.Context.Directory(projectPath));
    var unityPath = Statics.Context.EnvironmentVariable("UNITY_PATH");

    // If environment variable is not set, use default locations
    if (unityPath == null)
    {
        if (Statics.Context.IsRunningOnUnix())
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
    if (Statics.Context.IsRunningOnUnix())
    {
        logExec = "tail";
        logArgs = "-f " + unityLogFile;
    }
    int result = 0;
    using (var unityProcess = Statics.Context.StartAndReturnProcess(unityPath, new ProcessSettings{ Arguments = unityArgs }))
    {
        using (var logProcess = Statics.Context.StartAndReturnProcess(logExec, new ProcessSettings{ Arguments = logArgs, RedirectStandardError = true}))
        {
            unityProcess.WaitForExit();
            result = unityProcess.GetExitCode();
            if (logProcess.WaitForExit(0) &&
                logProcess.GetExitCode() != 0)
            {
                Statics.Context.Warning("There was an error logging, but command still executed.");
            }
            else
            {
                try
                {
                    logProcess.Kill();
                }
                catch
                {
                    // Log process was stopped right after checking
                }
            }
        }
    }
    if (System.IO.File.Exists(unityLogFile))
    {
        Statics.Context.DeleteFile(unityLogFile);
    }
    return result;
}

// appType usually "Puppet" or "Demo"
string GetBuildFolder(string appType)
{
     return Statics.TemporaryPrefix + appType + "Builds";
}

void ExecuteUnityMethod(string buildMethodName, string buildTarget, string projectPath = ".")
{
    Statics.Context.Information("Executing method " + buildMethodName + ", this could take a while...");
    var command = "-executeMethod " + buildMethodName + " -buildTarget " + buildTarget;
    var result = ExecuteUnityCommand(command, projectPath);
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
    DeleteFiles(Statics.TemporaryPrefix + "*");
    var dirs = GetDirectories(Statics.TemporaryPrefix + "*");
    foreach (var directory in dirs)
    {
        DeleteDirectory(directory, true);
    }
});
