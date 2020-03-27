# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT license.

param
(
    [Parameter(Position=0, Mandatory = $false, HelpMessage="Source folder", ValueFromPipeline = $true)] 
    $Source,
    [Parameter(Position=1, Mandatory = $false, HelpMessage="Destination file path", ValueFromPipeline = $true)] 
    $Destination
)

Try
{
    Add-Type -assembly "system.io.compression.filesystem"
    [io.compression.zipfile]::CreateFromDirectory($Source, $Destination)
}
Catch
{
    $Exc = $_.Exception.Message
    Write-Error "File $Destination was not created. Error: $Exc"
}
