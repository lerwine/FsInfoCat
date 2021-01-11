$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
$ErrorActionPreference = [System.Management.Automation.ActionPreference]::Continue;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$DebugPreference = [System.Management.Automation.ActionPreference]::Continue;
$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
$PSModuleAutoLoadingPreference = [System.Management.Automation.ActionPreference]::Continue;
$ProgressPreference = [System.Management.Automation.ActionPreference]::Continue;

if ($null -eq (Import-Module -Name (($PSScriptRoot | Join-Path -ChildPath '../../../Debug/Desktop/FsInfoCat')) -PassThru)) { return }
$FsCrawlJob = Start-FsCrawlJob -RootPath '.';
$Result = Receive-Job -Job $FsCrawlJob -Wait;

<#

$Error[0].Exception.StackTrace
$Error[0].Exception.FileName
$Error[0].Exception.FusionLog

#>
