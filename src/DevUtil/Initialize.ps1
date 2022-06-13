$PackagesPath = [Environment]::GetFolderPath([Environment+SpecialFolder]::UserProfile) | Join-Path -ChildPath '.nuget\packages';
$DepsPath = $PSScriptRoot | Join-Path -ChildPath 'DevUtil.deps.json';
#$DepsPath = 'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\DevUtil\bin\Debug\net6.0-windows\DevUtil.deps.json'
$DevUtilDeps = ((Get-Content -LiteralPath $DepsPath) | Out-String).Trim() | ConvertFrom-Json;
$NetCoreApp6 = $DevUtilDeps.targets.'.NETCoreApp,Version=v6.0';
if ($null -eq $NetCoreApp6) { throw "Failed to find targets.'.NETCoreApp,Version=v6.0' in $DepsPath" }
$Dependencies = $NetCoreApp6.'DevUtil/1.0.0'.dependencies;
if ($null -eq $Dependencies) { throw "Failed to find targets.'.NETCoreApp,Version=v6.0'.'DevUtil/1.0.0'.dependencies in $DepsPath" }
$Libraries = $DevUtilDeps.libraries;
if ($null -eq $Libraries) { throw "Failed to find libraries in $DepsPath" }
('Microsoft.EntityFrameworkCore', 'Microsoft.EntityFrameworkCore.Relational', 'Microsoft.EntityFrameworkCore.Abstractions',
    'Microsoft-WindowsAPICodePack-Shell') | ForEach-Object {
    $PropertyName = $Dependencies.($_);
    if ($null -eq $PropertyName) { throw "Failed to find targets.'.NETCoreApp,Version=v6.0'.'DevUtil/1.0.0'.dependencies.$_ in $DepsPath" }
    $PropertyName = "$_/$PropertyName";
    $LibPath = $Libraries.($PropertyName).path;
    if ($null -eq $LibPath) { throw "Failed to find libraries.'$PropertyName' in $DepsPath" }
    $LibPath = $PackagesPath | Join-Path -ChildPath $LibPath
    $Runtime = $NetCoreApp6.($PropertyName).runtime;
    if ($null -eq $Runtime) { throw "Failed to find targets.'.NETCoreApp,Version=v6.0'.'$PropertyName'.runtime in $DepsPath" }
    ($Runtime | Get-Member -MemberType NoteProperty) | ForEach-Object {
        Add-Type -Path ($LibPath | Join-Path -ChildPath $_.Name) -ErrorAction Stop;
    }
}
<#
('microsoft.entityframeworkcore\5.0.9\lib\netstandard2.1\Microsoft.EntityFrameworkCore.dll',
    'microsoft.extensions.hosting\5.0.0\lib\netstandard2.1\Microsoft.Extensions.Hosting.dll',
    'microsoft.extensions.hosting.abstractions\5.0.0\lib\netstandard2.1\Microsoft.Extensions.Hosting.Abstractions.dll',
    'microsoft.entityframeworkcore.relational\5.0.9\lib\netstandard2.1\Microsoft.EntityFrameworkCore.Relational.dll',
    'microsoft.entityframeworkcore.abstractions\5.0.9\lib\netstandard2.1\Microsoft.EntityFrameworkCore.Abstractions.dll',
    'microsoft.extensions.logging.abstractions\5.0.0\lib\netstandard2.0\Microsoft.Extensions.Logging.Abstractions.dll',
    'microsoft-windowsapicodepack-core\1.1.4\lib\netcoreapp3.1\Microsoft.WindowsAPICodePack.dll',
    'microsoft-windowsapicodepack-shell\1.1.4\lib\netcoreapp3.1\Microsoft.WindowsAPICodePack.Shell.dll') | ForEach-Object {
    Add-Type -Path ($PackagesPath | Join-Path -ChildPath $_) -ErrorAction Stop;
}
#>
