if ($null -ne (Get-Module -Name 'FsInfoCat')) { Remove-Module -Name 'FsInfoCat' }
$Path = $PSScriptRoot | Join-Path -ChildPath 'src\FsInfoCat.PsDesktop\bin\Debug\FsInfoCat.psm1';
Import-Module -Name $Path -ErrorAction Stop;
Get-InitializationQueries
