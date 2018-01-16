# Installs Unity and required components for WSA .NET and IL2CPP builds

[CmdletBinding()]
Param(
    # Path to install unity and tools to
    [string]$InstallPath
)

Write-Host "Installing Unity Editor and tools to directory $(InstallPath)"

# Download links for Unity 2017.3.0f3, editor and support
$WindowsUnityEditorUrl = "https://download.unity3d.com/download_unity/a9f86dcd79df/Windows64EditorInstaller/UnitySetup64-2017.3.0f3.exe?_ga=2.69975952.1053985475.1515441432-1315763065.1501872160"
$WSADotNetSupportWindowsUrl = "https://download.unity3d.com/download_unity/a9f86dcd79df/TargetSupportInstaller/UnitySetup-Metro-Support-for-Editor-2017.3.0f3.exe?_ga=2.69975952.1053985475.1515441432-1315763065.1501872160"
$WSAIl2CppSupportWindowsUrl = "https://download.unity3d.com/download_unity/a9f86dcd79df/TargetSupportInstaller/UnitySetup-UWP-IL2CPP-Support-for-Editor-2017.3.0f3.exe?_ga=2.216707094.1053985475.1515441432-1315763065.1501872160"

#install editor
$path = (Get-Location).Path + "\UnitySetup64.exe";
Write-Host "Downloading Unity Editor..."
(New-Object System.Net.WebClient).DownloadFile($WindowsUnityEditorUrl, $path) 
Write-Host "Installing Unity Editor..."
./UnitySetup64.exe /S /D=$InstallPath

# Install support tools (.NET)
$path = (Get-Location).Path + "\UnityNetSupport.exe";
Write-Host "Downloading Unity .NET support..."
(New-Object System.Net.WebClient).DownloadFile($WSADotNetSupportWindowsUrl, $path) 
Write-Host "Installing Unity .NET support..."
./UnityNetSupport.exe /S /D=$InstallPath

#install support tools (IL2CPP)
$path = (Get-Location).Path + "\UnityIl2CppSupport.exe";
Write-Host "Downloading Unity IL2CPP support..."
(New-Object System.Net.WebClient).DownloadFile($WSAIl2CppSupportWindowsUrl, $path) 
Write-Host "Installing Unity IL2CPP support..."
./UnityIl2CppSupport.exe /S /D=$InstallPath
