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
    var projectDir = projectPath == null ? null : Statics.Context.MakeAbsolute(Statics.Context.Directory(projectPath));
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
    if (System.IO.File.Exists(unityLogFile))
    {
        unityLogFile += "1";
    }
    var unityArgs = "-batchmode -quit -logFile " + unityLogFile;

    // If the command has an associated project, add it to the arguments
    if (projectDir != null)
    {
        unityArgs += " -projectPath " + projectDir;
    }

    unityArgs += " " + extraArgs;

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
            if (logProcess.WaitForExit(0) && (logProcess.GetExitCode() != 0))
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
    DeleteFileIfExists(unityLogFile);
    return result;
}

// appType usually "Puppet" or "Demo"
static string GetBuildFolder(string appType, string projectPath)
{
     return projectPath + "/" + Statics.TemporaryPrefix + appType + "Builds";
}

static void ExecuteUnityMethod(string buildMethodName, string buildTarget = null, string projectPath = ".")
{
    Statics.Context.Information("Executing method " + buildMethodName + ", this could take a while...");
    var command = "-executeMethod " + buildMethodName; 
    if (buildTarget != null)
    {
        command += " -buildTarget " + buildTarget;
    }
    var result = ExecuteUnityCommand(command, projectPath);
    if (result != 0)
    {
        throw new Exception("Failed to execute method " + buildMethodName + ".");
    }
}

// Copy files to a clean directory using string names instead of FilePath[] and DirectoryPath
static void CopyFiles(IEnumerable<string> files, string targetDirectory, bool clean = true)
{
    if (clean)
    {
        CleanDirectory(targetDirectory);
    }
    foreach (var file in files)
    {
        Statics.Context.CopyFile(file, targetDirectory + "/" + System.IO.Path.GetFileName(file));
    }
}

static void DeleteDirectoryIfExists(string directoryName)
{
    if (Statics.Context.DirectoryExists(directoryName))
    {
        Statics.Context.DeleteDirectory(directoryName, new DeleteDirectorySettings() { Recursive = true });
    }
}

static void DeleteFileIfExists(string fileName)
{
    try
    {
        if (Statics.Context.FileExists(fileName))
        {
            Statics.Context.DeleteFile(fileName);
        }
    }
    catch
    {
        Statics.Context.Information("Unable to delete file '" + fileName + "'.");
    }
}


static void CleanDirectory(string directoryName)
{
    DeleteDirectoryIfExists(directoryName);
    Statics.Context.CreateDirectory(directoryName);
}

void HandleError(Exception exception)
{
    RunTarget("clean");
    throw new Exception("Error occurred, see inner exception for details", exception);
}

// Remove all temporary files and folders
Task("RemoveTemporaries").Does(()=>
{
    DeleteFiles(Statics.TemporaryPrefix + "*");
    var dirs = GetDirectories(Statics.TemporaryPrefix + "*");
    foreach (var directory in dirs)
    {
        DeleteDirectory(directory, new DeleteDirectorySettings() { Recursive = true });
    }
});
