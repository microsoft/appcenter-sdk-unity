# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.

param
(
    [Parameter(Position=0, Mandatory = $false, HelpMessage="Source file", ValueFromPipeline = $true)] 
    $Source,
    [Parameter(Position=1, Mandatory = $false, HelpMessage="Destination path", ValueFromPipeline = $true)] 
    $Destination
)

New-Item -ItemType directory -Path $Destination

Try
{
    Add-Type -assembly "system.io.compression.filesystem"
    [io.compression.zipfile]::ExtractToDirectory($Source, $Destination)
}
Catch
{
    $Exc = $_.Exception.Message
    Write-Error "Folder $Destination was not created. Error: $Exc"
}
