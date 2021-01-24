if ($null -eq (Get-Module -Name 'Paster') -and $null -eq (Import-Module -Name 'Pester' -ErrorAction Continue -PassThru)) {
    Write-Error -Message 'Unable to import the Pester Module. Perhaps it is not installed?' -Category NotInstalled -ErrorId 'CannotImportPester' `
        -RecommendedAction "Execute the following command to install the module:`nInstall-Module -Name Pester" -ErrorAction Stop;
}

$LogPath = $PSScriptRoot | Join-Path -ChildPath '../../../dev/log/FsInfoCat.Pester.log';
Set-Location -LiteralPath $PSScriptRoot;
Invoke-Pester -OutputFile $LogPath -OutputFormat NUnitXml;
