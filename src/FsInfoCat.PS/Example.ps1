$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
$ErrorActionPreference = [System.Management.Automation.ActionPreference]::Continue;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$DebugPreference = [System.Management.Automation.ActionPreference]::Continue;
$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
$PSModuleAutoLoadingPreference = [System.Management.Automation.ActionPreference]::Continue;
$ProgressPreference = [System.Management.Automation.ActionPreference]::Continue;

if ($null -ne (Import-Module -Name (($PSScriptRoot | Join-Path -ChildPath '..\..\Debug\Windows\FsInfoCat')))) {
    $PwHash = [FsInfoCat.Util.PwHash]::Import('XrVxbJmeSwCzNtQp6PpepBE8WUh1nfhPGlQCBoIOaS6ho6+7UH/lGnJgH0w2hF2oFzxDrQ6liVHWDldtFawAetBTWV+jCsFG');
    if ($null -eq $PwHash) {
        Write-Error -Message 'Error parsing base-64 hash' -Category InvalidResult -ErrorId 'PwHash' -ErrorAction Stop;
    }
}
Start-FsCrawlJob -RootPath '.'