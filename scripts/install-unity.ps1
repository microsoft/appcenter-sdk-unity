# Installs Unity and required components for WSA .NET and IL2CPP builds

Write-Host "Installing Unity Editor and tools"

# Download links for Unity editor and support
$WindowsUnityEditorUrl = "https://netstorage.unity3d.com/unity/2207421190e9/Windows64EditorInstaller/UnitySetup64-2018.2.9f1.exe"
$WSADotNetSupportWindowsUrl = "https://netstorage.unity3d.com/unity/2207421190e9/TargetSupportInstaller/UnitySetup-UWP-.NET-Support-for-Editor-2018.2.9f1.exe"
$WSAIl2CppSupportWindowsUrl = "https://netstorage.unity3d.com/unity/2207421190e9/TargetSupportInstaller/UnitySetup-UWP-IL2CPP-Support-for-Editor-2018.2.9f1.exe"

# Install editor
$path = (Get-Location).Path + "\UnitySetup64.exe";
Write-Host "Downloading Unity Editor..."
(New-Object System.Net.WebClient).DownloadFile($WindowsUnityEditorUrl, $path) 
Write-Host "Installing Unity Editor..."
./UnitySetup64.exe /S

# Install support tools (.NET)
$path = (Get-Location).Path + "\UnityNetSupport.exe";
Write-Host "Downloading Unity .NET support..."
(New-Object System.Net.WebClient).DownloadFile($WSADotNetSupportWindowsUrl, $path) 
Write-Host "Installing Unity .NET support..."
./UnityNetSupport.exe /S

# Install support tools (IL2CPP)
$path = (Get-Location).Path + "\UnityIl2CppSupport.exe";
Write-Host "Downloading Unity IL2CPP support..."
(New-Object System.Net.WebClient).DownloadFile($WSAIl2CppSupportWindowsUrl, $path) 
Write-Host "Installing Unity IL2CPP support..."
./UnityIl2CppSupport.exe /S
