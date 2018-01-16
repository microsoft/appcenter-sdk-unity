$WindowsUnityEditorUrl = "https://download.unity3d.com/download_unity/a9f86dcd79df/Windows64EditorInstaller/UnitySetup64-2017.3.0f3.exe?_ga=2.69975952.1053985475.1515441432-1315763065.1501872160"
$WSADotNetSupportWindowsUrl = "https://download.unity3d.com/download_unity/a9f86dcd79df/TargetSupportInstaller/UnitySetup-Metro-Support-for-Editor-2017.3.0f3.exe?_ga=2.69975952.1053985475.1515441432-1315763065.1501872160";

$InstallPath = (Get-Location).Path + "\Unity"

#install editor
$path = (Get-Location).Path + "\UnitySetup64.exe";
Write-Host "Downloading Unity Editor..."
(New-Object System.Net.WebClient).DownloadFile($WindowsUnityEditorUrl, $path) 
Write-Host "Installing Unity Editor..."
./UnitySetup64.exe /S /D=$InstallPath

#install support (.net)
$path = (Get-Location).Path + "\UnityNetSupport.exe";
Write-Host "Downloading Unity .NET support..."
(New-Object System.Net.WebClient).DownloadFile($WSADotNetSupportWindowsUrl, $path) 
Write-Host "Installing Unity .NET support..."
./UnityNetSupport.exe /S /D=$InstallPath
