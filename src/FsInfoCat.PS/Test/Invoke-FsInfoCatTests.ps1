Param(
    [Parameter(Mandatory = $true)]
    [string]$FsInfoCatModulePath
)

if ($null -eq (Get-Module -Name 'Paster') -and $null -eq (Import-Module -Name 'Pester' -ErrorAction Continue -PassThru)) {
    Write-Error -Message 'Unable to import the Pester Module. Perhaps it is not installed?l' -Category NotInstalled -ErrorId 'CannotImportPester' `
        -RecommendedAction "Execute the following command to install the module:`nInstall-Module -Name Pester" -ErrorAction Stop;
}

$container = New-PesterContainer -Path '*.Tests.ps1' -Data @{
    FsInfoCatModulePath = $FsInfoCatModulePath;
};

Invoke-Pester -Container $container -Output Detailed;
