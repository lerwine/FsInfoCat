$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
$ErrorActionPreference = [System.Management.Automation.ActionPreference]::Continue;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$DebugPreference = [System.Management.Automation.ActionPreference]::SilentlyContinue;
$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
$PSModuleAutoLoadingPreference = [System.Management.Automation.ActionPreference]::Continue;
$ProgressPreference = [System.Management.Automation.ActionPreference]::Continue;

Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../Setup/bin/FsInfoCat') -ErrorAction Stop;

$FsLogicalVolumeCollection = @(Get-FsLogicalVolume);
$FsLogicalVolumeCollection;
Get-FsLogicalVolume | Register-FsLogicalVolume -PassThru;
