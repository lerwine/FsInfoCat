$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
$ErrorActionPreference = [System.Management.Automation.ActionPreference]::Continue;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$DebugPreference = [System.Management.Automation.ActionPreference]::Continue;
$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
$PSModuleAutoLoadingPreference = [System.Management.Automation.ActionPreference]::Continue;
$ProgressPreference = [System.Management.Automation.ActionPreference]::Continue;

Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
$FsCrawlJob = Start-FsCrawlJob -RootPath '../..' -ErrorAction Stop;
while ($FsCrawlJob.HasMoreData) {
    Receive-Job -Job $FsCrawlJob -Wait -ErrorAction Stop;
}

<#

$Error[0].Exception.StackTrace
$Error[0].Exception.FileName
$Error[0].Exception.FusionLog

#>
