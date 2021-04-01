Param(
    [string]$AppSettingsJson,

    [ValidateSet("Production", "Testing", "Development", "Prod", "Test", "Dev")]
    [string]$Stage = "Production",

    [string]$DataSource = 'tcp:c868dbserver.database.windows.net,1433',
    
    [string]$InitialCatalog = 'FsInfoCat',
    
    [bool]$PersistSecurityInfo = $false,
    
    [string]$UserID = 'fsinfocatadmin',
    
    [bool]$MultipleActiveResultSets = $false,
    
    [bool]$Encrypt = $true,

    [bool]$TrustServerCertificate = $false,

    [int]$ConnectionTimeout = 30,

    [string]$ApplicationName = 'FsInfoCat'
)
$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
$ErrorActionPreference = [System.Management.Automation.ActionPreference]::Continue;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$DebugPreference = [System.Management.Automation.ActionPreference]::SilentlyContinue;
$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
$PSModuleAutoLoadingPreference = [System.Management.Automation.ActionPreference]::Continue;
$ProgressPreference = [System.Management.Automation.ActionPreference]::Continue;
&{
    $VarNames = @(Get-Variable | Select-Object -ExpandProperty 'Name');
    if (@(@('SetupScriptsFolder', 'FsInfoCatModuleName', 'FsInfoCatModulePath', 'SetupDistroFolder', 'FsInfoCatModuleDistroFile', 'WebProjectDirectory', 'ProdAppSettingsFile',
    'TestAppSettingsFile', 'DevAppSettingsFile', 'RepositoryRootPath') | Where-Object { $VarNames -inotcontains $_ }).Count -eq 0) { return }
    try {
        Set-Variable -Name 'RepositoryRootPath' -Value (($PSScriptRoot | Join-Path -ChildPath '../..') | Resolve-Path -ErrorAction Continue).ProviderPath -Scope Global -Option Constant;
        if ($null -ne $RepositoryRootPath) {
            Set-Variable -Name 'SetupScriptsFolder' -Value (($RepositoryRootPath | Join-Path -ChildPath 'Setup') | Join-Path -ChildPath 'Scripts') -Scope Global -Option Constant;
            Set-Variable -Name 'FsInfoCatModuleName' -Value 'FsInfoCat' -Scope Global -Option Constant;
            Set-Variable -Name 'FsInfoCatModulePath' -Value ((($RepositoryRootPath | Join-Path -ChildPath 'Setup') | Join-Path -ChildPath 'bin') | Join-Path -ChildPath $FsInfoCatModuleName) -Scope Global -Option Constant;
            Set-Variable -Name 'SetupDistroFolder' -Value (($RepositoryRootPath | Join-Path -ChildPath 'Setup') | Join-Path -ChildPath 'Distro') -Scope Global -Option Constant;
            Set-Variable -Name 'FsInfoCatModuleDistroFile' -Value ($SetupDistroFolder | Join-Path -ChildPath "$FsInfoCatModuleName.zip") -Scope Global -Option Constant;
            Set-Variable -Name 'WebProjectDirectory' -Value (($RepositoryRootPath | Join-Path -ChildPath 'src') | Join-Path -ChildPath 'FsInfoCat.Web') -Scope Script -Option Constant;
            Set-Variable -Name 'ProdAppSettingsFile' -Value ($WebProjectDirectory | Join-Path -ChildPath 'appsettings.json') -Scope Script -Option Constant;
            Set-Variable -Name 'TestAppSettingsFile' -Value ($WebProjectDirectory | Join-Path -ChildPath 'appsettings.Testing.json') -Scope Script -Option Constant;
            Set-Variable -Name 'DevAppSettingsFile' -Value ($WebProjectDirectory | Join-Path -ChildPath 'appsettings.Testing.json') -Scope Script -Option Constant;
        }
    } catch {
        Write-Error -ErrorRecord $Error[0] -CategoryReason "Failure setting up constant variables" `
            -RecommendedAction "Execute script from a new session and/or ensure there are no profile scripts which create conflicting variable names." -ErrorAction Continue;
        Write-Warning -Message "Cannot continue: Unable to initialize constant variables." ;
        Exit 1;
    }
    if ($null -eq $RepositoryRootPath) {
        Write-Warning -Message "Unable to resolve the root repository path. Ensure this script is being executed from the '$SetupScriptsFolder' subdirectory of the cloned source repository.";
        Exit 1;
    }
} | Out-Null;

# Change filesystem location to repository root.
try { Set-Location -LiteralPath $RepositoryRootPath -ErrorAction Stop }
catch {
    Write-Error -ErrorRecord $Error[0] -CategoryReason "Unable change to the root repository path '$RepositoryRootPath'" -ErrorAction Continue;
    Exit 2;
}

enum LogLevelValue {
    Critical;
    Error;
    Warning;
    Information;
    Trace;
    Debug;
    None;
}

class LogLevelSettingsData {
    [LogLevelValue]$Default;
    [LogLevelValue]$Microsoft;
    [LogLevelValue]$MicrosoftHostingLifetime;
    [PSCustomObject]$OriginalValues;
    
    LogLevelSettingsData() {
        $this.OriginalValues = [PSCustomObject]::new();
        $this.Default = [LogLevelValue]::Information;
        $this.Microsoft = [LogLevelValue]::Information;
        $this.MicrosoftHostingLifetime = [LogLevelValue]::Information;
    }
    LogLevelSettingsData([LogLevelSettingsData]$Source, [PSCustomObject]$OriginalValues) {
        if ($null -ne $Originalvalues) {
            $this.OriginalValues = $Originalvalues;
        } else {
            $this.OriginalValues = [PSCustomObject]::new();
        }
        if ($null -eq $Source) {
            [LogLevelValue]$Value = [LogLevelValue]::Information;
            if ($this.OriginalValues.Default -is [string] -and [Enum]::TryParse($this.OriginalValues.Default, [ref]$Value)) {
                $this.Default = $Value;
            } else {
                $this.Default = [LogLevelValue]::Information;
            }
            if ($this.OriginalValues.Microsoft -is [string] -and [Enum]::TryParse($this.OriginalValues.Microsoft, [ref]$Value)) {
                $this.Microsoft = $Value;
            } else {
                $this.Microsoft = $this.Default;
            }
            if ($this.OriginalValues.('Microsoft.Hosting.Lifetime') -is [string] -and [Enum]::TryParse($this.OriginalValues.('Microsoft.Hosting.Lifetime'), [ref]$Value)) {
                $this.MicrosoftHostingLifetime = $Value;
            } else {
                $this.MicrosoftHostingLifetime = $this.Microsoft;
            }
        } else {
            $this.Default = $Source.Default;
            $this.Microsoft = $Source.Microsoft;
            $this.MicrosoftHostingLifetime = $Source.MicrosoftHostingLifetime;
        }
    }
    [bool] IsChanged() {
        [LogLevelValue]$Value = [LogLevelValue]::Information;
        return -not ($this.OriginalValues.Default -is [string] -and [Enum]::TryParse($this.OriginalValues.Default, [ref]$Value) -and $Value -eq $this.Default `
            -and $this.OriginalValues.Microsoft -is [string] -and [Enum]::TryParse($this.OriginalValues.Microsoft, [ref]$Value) -and $Value -eq $this.Microsoft `
            -and $this.OriginalValues.('Microsoft.Hosting.Lifetime') -is [string] `
            -and [Enum]::TryParse($this.OriginalValues.('Microsoft.Hosting.Lifetime'), [ref]$Value) -and $Value -eq $this.MicrosoftHostingLifetime);
    }
    [void] ApplyChanges([PSCustomObject]$NewOriginal) {
        $NewOriginal | Add-Member -MemberType NoteProperty -Name 'Default' -Value ([Enum]::GetName([LogLevelValue], $this.Default));
        $NewOriginal | Add-Member -MemberType NoteProperty -Name 'Microsoft' -Value ([Enum]::GetName([LogLevelValue], $this.Microsoft));
        $NewOriginal | Add-Member -MemberType NoteProperty -Name 'Microsoft.Hosting.Lifetime' -Value ([Enum]::GetName([LogLevelValue], $this.MicrosoftHostingLifetime));
        $this.OriginalValues.PSObject.Properties | ForEach-Object {
            $n = $_.Name;
            switch ($n) {
                'Default' { break; }
                'Microsoft' { break; }
                'Microsoft.Hosting.Lifetime' { break; }
                default {
                    $NewOriginal | Add-Member -MemberType NoteProperty -Name $n -Value $_.Value;
                    break;
                }
            }
        }
        $this.OriginalValues = $NewOriginal;
    }
}

class LoggingSettingsData {
    [LogLevelSettingsData]$LogLevel;
    hidden [PSCustomObject]$OriginalValues;
    LoggingSettingsData() {
        $this.OriginalValues = [PSCustomObject]::new();
        $this.LogLevel = [LogLevelSettingsData]::new();
    }
    LoggingSettingsData([LoggingSettingsData]$Source, [PSCustomObject]$OriginalValues) {
        if ($null -ne $Originalvalues) {
            $this.OriginalValues = $Originalvalues;
        } else {
            $this.OriginalValues = [PSCustomObject]::new();
        }
        if ($null -eq $Source) {
            $this.LogLevel = [LogLevelSettingsData]::new($null, $this.OriginalValues.LogLevel);
        } else {
            if ($null -eq $Source.LogLevel) {
                $this.LogLevel = $null;
            } else {
                $this.LogLevel = [LogLevelSettingsData]::new($Source.LogLevel, $this.OriginalValues.LogLevel);
            }
        }
    }
    [bool] IsChanged() {
        if ($null -eq $this.LogLevel) { return $null -ne $this.OriginalValues.LogLevel }
        return $null -eq $this.OriginalValues.LogLevel -or $this.LogLevel.IsChanged();
    }
    [void] ApplyChanges([PSCustomObject]$NewOriginal) {
        $s = '';
        if ($null -ne $this.FsInfoCat) {
            try { $s = $this.FsInfoCat.ConnectionString }
            catch { $s = '' }
        }
        if ($null -ne $this.LogLevel) {
            $obj = [PSCustomObject]::new();
            $this.ConnectionStrings.ApplyChanges($obj);
            $NewOriginal | Add-Member -MemberType NoteProperty -Name 'LogLevel' -Value $obj;
        }
        $this.OriginalValues.PSObject.Properties | ForEach-Object {
            $n = $_.Name;
            if ($n -ne 'LogLevel') {
                $NewOriginal | Add-Member -MemberType NoteProperty -Name $n -Value $_.Value;
            }
        }
        $this.OriginalValues = $NewOriginal;
    }
}

class ConnectionStringsSettingsData {
    [System.Data.SqlClient.SqlConnectionStringBuilder]$FsInfoCat;
    hidden [PSCustomObject]$OriginalValues;
    
    ConnectionStringsSettingsData() {
        $this.OriginalValues = [PSCustomObject]::new();
        $this.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
    }
    ConnectionStringsSettingsData([ConnectionStringsSettingsData]$Source, [PSCustomObject]$OriginalValues) {
        if ($null -ne $Originalvalues) {
            $this.OriginalValues = $Originalvalues;
        } else {
            $this.OriginalValues = [PSCustomObject]::new();
        }
        if ($null -eq $Source) {
            if ($this.OriginalValues.FsInfoCat -is [string] -and $this.OriginalValues.FsInfoCat.Trim().Length -gt 0) {
                try {
                    $this.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new($this.OriginalValues.FsInfoCat);
                } catch {
                    $this.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
                }
            }
        } else {
            $this.FsInfoCat = $Source.FsInfoCat;
        }
    }
    [bool] IsChanged() {
        if ($null -eq $this.FsInfoCat) { return $null -ne $this.OriginalValues.FsInfoCat }
        $s = '';
        try { $s = $this.FsInfoCat.ConnectionString }
        catch { $s = '' }
        if ([string]::IsNullOrWhiteSpace($s)) { return $this.OriginalValues.FsInfoCat -isnot [string] -or [string]::IsNullOrWhiteSpace($this.OriginalValues.FsInfoCat) }
        return $s -ieq $this.OriginalValues.FsInfoCat;
    }
    [void] ApplyChanges([PSCustomObject]$NewOriginal) {
        $s = '';
        if ($null -ne $this.FsInfoCat) {
            try { $s = $this.FsInfoCat.ConnectionString }
            catch { $s = '' }
        }
        if (-not [string]::IsNullOrWhiteSpace($s)) {
            $NewOriginal | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $s;
        }
        $this.OriginalValues.PSObject.Properties | ForEach-Object {
            $n = $_.Name;
            if ($n -ne 'FsInfoCat') {
                $NewOriginal | Add-Member -MemberType NoteProperty -Name $n -Value $_.Value;
            }
        }
        $this.OriginalValues = $NewOriginal;
    }
}

class AppSettingsData {
    hidden [string]$Path;
    [LoggingSettingsData]$Logging;
    [string]$AllowedHosts;
    [ConnectionStringsSettingsData]$ConnectionStrings;
    hidden [PSCustomObject]$OriginalValues;
    AppSettingsData([PSCustomObject]$Source) {
        if ($null -eq $Source) {
            $this.AllowedHosts = $null;
            $this.OriginalValues = [PSCustomObject]::new();
        } else {
            if ($Source.AllowedHosts -is [string]) {
                $this.AllowedHosts = $Source.AllowedHosts;
            }
            $this.OriginalValues = $Source;
            $this.Logging = [LoggingSettingsData]::new($null, $this.OriginalValues.Logging);
            $this.ConnectionStrings = [ConnectionStringsSettingsData]::new($null, $this.OriginalValues.ConnectionStrings);
        }
    }

    [AppSettingsData] Clone() {
        $Result = [AppSettingsData]::new((($this.OriginalValues | ConvertTo-Json) | ConvertFrom-Json));
        $Result.Path = $this.Path;
        $Result.Logging = [LoggingSettingsData]::new($this.Logging, $this.OriginalValues.Logging);
        $Result.ConnectionStrings = [ConnectionStringsSettingsData]::new($this.ConnectionStrings, $this.OriginalValues.ConnectionStrings);
        $Result.AllowedHosts = $this.AllowedHosts;
        return $Result;
    }
    
    [bool] IsChanged() {
        if ([string]::IsNullOrWhiteSpace($this.Path)) { return $true }
        if ($null -eq $this.Logging) {
            if ($null -ne $this.OriginalValues.Logging) { return $true }
        } else {
            if ($null -eq $this.OriginalValues.Logging -or $this.Logging.IsChanged()) { return $true }
        }
        if ($null -eq $this.ConnectionStrings) {
            if ($null -ne $this.OriginalValues.ConnectionStrings) { return $true }
        } else {
            if ($null -eq $this.OriginalValues.ConnectionStrings -or $this.ConnectionStrings.IsChanged()) { return $true }
        }
        $s = $null;
        if ($this.OriginalValues.AllowedHosts -is [string]) { $s = $this.OriginalValues.AllowedHosts }
        if ([string]::IsNullOrWhiteSpace($s)) { return [string]::IsNullOrWhiteSpace($this.AllowedHosts) }
        return $this.AllowedHosts -eq $s;
    }

    [string] GetPath() { return $this.Path }
    
    [void] Save() { $this.Save($null); }

    [void] Save([string]$SaveAs) {
        if ([string]::IsNullOrWhiteSpace($SaveAs)) {
            if ([string]::IsNullOrWhiteSpace($this.Path)) {
                Write-Error -Message 'Path must be specified for settings that have never been saved or loaded.' -Category InvalidOperation -ErrorId 'NoSavePath' -TargetObject $this;
                return;
            }
            $SaveAs = $this.Path;
        }
        $NewOriginal = [PSCustomObject]::new();
        if ($null -ne $this.Logging) {
            $obj = [PSCustomObject]::new();
            $this.ConnectionStrings.ApplyChanges($obj);
            $NewOriginal | Add-Member -MemberType NoteProperty -Name 'Logging' -Value $obj;
        }
        if (-not [string]::IsNullOrWhiteSpace($this.AllowedHosts)) {
            $NewOriginal | Add-Member -MemberType NoteProperty -Name 'AllowedHosts' -Value $this.AllowedHosts;
        }
        if ($null -ne $this.ConnectionStrings) {
            $obj = [PSCustomObject]::new();
            $this.Logging.ApplyChanges($obj);
            $NewOriginal | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $obj;
        }
        $this.OriginalValues.PSObject.Properties | ForEach-Object {
            $n = $_.Name;
            switch ($n) {
                'ConnectionStrings' { break; }
                'Logging' { break; }
                'AllowedHosts' { break; }
                default {
                    $NewOriginal | Add-Member -MemberType NoteProperty -Name $n -Value $_.Value;
                    break;
                }
            }
        }
        [System.IO.File]::WriteAllText($SaveAs, ($NewOriginal | ConvertTo-Json), [System.Text.UTF8Encoding]::new($true, $false));
        $this.OriginalValues = $NewOriginal;
    }

    static [AppSettingsData] Load([string]$Path) {
        if ($Path | Test-Path) {
            $o = [System.IO.File]::ReadAllText($Path) | ConvertFrom-Json;
            $Result = $null;
            if ($null -ne $o) {
                $Result = [AppSettingsData]::new($o);
                if ($null -ne $Result) {
                    $Result.Path = $Path;
                }
            }
        } else {
            $Result = [AppSettingsData]::new($null);
            $Result.Path = $Path;
        }
        return $Result; 
    }
}

$FsInfoCatModulePath
# Load FsInfoCat PowerShell module.
if ($null -eq (Get-Module -Name $FsInfoCatModuleName)) {
    if (-not ($FsInfoCatModulePath | Test-Path -PathType Container)) {
        if ($FsInfoCatModulePath | Test-Path) {
            Write-Error -Message "Unable to load FsInfoCat PowerShell module" -Category ResourceUnavailable -ErrorId 'UnexpectedFile' `
                -TargetObject ($RepositoryRootPath | Join-Path -ChildPath $FsInfoCatModulePath) -RecommendedAction "Move or delete file $FsInfoCatModulePath" `
                -CategoryReason "Expected subdirectory at path '$FsInfoCatModulePath'; Actual item was a file.";
                Exit 2;
        }
        if ($FsInfoCatModuleDistroFile | Test-Path -PathType Leaf) {
            try { Expand-Archive -Path $FsInfoCatModuleDistroFile -DestinationPath $FsInfoCatModulePath -ErrorAction Stop }
            catch {
                Write-Error -ErrorRecord $Error[0] -CategoryReason "Unable extract FsInfoCat PowerShell module to '$FsInfoCatModulePath'" -ErrorAction Continue `
                    -RecommendedAction "Manually unpack distributable archive '$FsInfoCatModuleDistroFile' into subdirectory '$FsInfoCatModulePath' before executing this script.";
                Exit 2;
            }
            if (-not ($FsInfoCatModulePath | Test-Path -PathType Container)) {
                Write-Error -Message "Failed to exract FsInfoCat PowerShell module distributable" `
                    -CategoryReason "Subdirectory '$FsInfoCatModulePath' not found after attempt to expand contents of '$FsInfoCatModuleDistroFile'." -ErrorAction Continue `
                    -RecommendedAction "Manually unpack distributable archive '$FsInfoCatModuleDistroFile' into subdirectory '$FsInfoCatModulePath' before executing this script.";
                Exit 2;
            }
        } else {
            if ($FsInfoCatModuleDistroFile | Test-Path) {
                Write-Error -Message "Unable to unpack contents of FsInfoCat PowerShell module distributable" -Category ResourceUnavailable -ErrorId 'UnexpectedSubdirectory' `
                    -TargetObject ($RepositoryRootPath | Join-Path -ChildPath $FsInfoCatModuleDistroFile) -RecommendedAction "Move or delete subdirectory $FsInfoCatModuleDistroFile" `
                    -CategoryReason "Expected archive file at path '$FsInfoCatModuleDistroFile'; Actual item was a subdirectory." -ErrorAction Continue;
            } else {
                Write-Error -Message "Unable to unpack contents of FsInfoCat PowerShell module distributable" -Category ObjectNotFound -ErrorId 'NoPsModuleDistributable' `
                    -TargetObject ($RepositoryRootPath | Join-Path -ChildPath $FsInfoCatModuleDistroFile) `
                    -RecommendedAction "Rebuild PowerShell module to create the distributable ZIP file." `
                    -CategoryReason "Archive file not found at '$FsInfoCatModuleDistroFile'." -ErrorAction Continue;
            }
            Exit 2;
        }
    }
    try { Import-Module -Name $FsInfoCatModulePath -ErrorAction Stop }
    catch {
        Write-Error -ErrorRecord $Error[0] -CategoryReason "Unable import module '$FsInfoCatModulePath'." `
            -RecommendedAction "Manually import module '$FsInfoCatModulePath' before executing this script" -ErrorAction Continue;
        Exit 1;
    }
}

Function Find-MsBuildBinFolder {
    [CmdletBinding()]
    Param()

    if ($null -eq $Script:__Find_MsBuild_YearRegex) {
        $Script:__Find_MsBuild_YearRegex = [System.Text.RegularExpressions.Regex]::new('^2[01]\d\d$');
        $Script:__Find_MsBuild_VersionRegex = [System.Text.RegularExpressions.Regex]::new('^v?(?<major>\d{1,9})(\.(?<minor>\d{1,9})(\.(?<build>\d{1,9})(\.(?<revision>\d{1,9}))?)?)?$');
    }

    (@(@((&{
        (((${Env:ProgramFiles(x86)}, $Env:ProgramFiles) | ForEach-Object { $_ | Join-Path -ChildPath 'Microsoft Visual Studio' } | Where-Object { $_ | Test-Path -PathType Container } | Get-ChildItem -Directory) | ForEach-Object {
            New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{ Match = $Script:__Find_MsBuild_YearRegex.Match($_.Name); RootPath = $_.FullName }
        } | Where-Object { $_.Match.Success } | ForEach-Object {
            $Item = $_;
            (('BuildTools', 'Enterprise', 'Professional', 'Community') | ForEach-Object { ($Item.RootPath | Join-Path -ChildPath $_) | Join-Path -ChildPath 'MSBuild' } | Where-Object { $_ | Test-Path -PathType Container } | Get-ChildItem -Directory) | ForEach-Object {
                New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                    Year = [int]::Parse($Item.Match.Value);
                    Match = $Script:__Find_MsBuild_VersionRegex.Match($_.Name);
                    Name = $_.Name;
                    BasePath = $_.FullName;
                    Level = 2;
                };
            }
        }) | Where-Object { $_.Match.Success -or $_.Name -eq 'Current' }
        (($Env:ProgramFiles | Join-Path -ChildPath 'MSBuild') | Get-ChildItem -Directory) | ForEach-Object {
            New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                Match = $Script:__Find_MsBuild_VersionRegex.Match($_.Name);
                Name = $_.Name;
                BasePath = $_.FullName;
                Level = 1;
            };
        } | Where-Object { $_.Match.Success }
    }) | ForEach-Object { $_ | Add-Member -MemberType NoteProperty -Name 'Path' -Value ($_.BasePath | Join-Path -ChildPath 'Bin') -PassThru }) + @(
        (($env:WinDir | Join-Path -ChildPath 'Microsoft.NET\Framework') | Get-ChildItem -Directory) | ForEach-Object {
            New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
                Match = $Script:__Find_MsBuild_VersionRegex.Match($_.Name);
                Name = $_.Name;
                Path = $_.FullName;
                Level = 0;
            };
        } | Where-Object { $_.Match.Success }
    )) | Where-Object { ($_.Path | Join-Path -ChildPath 'Microsoft.Build.Engine.dll') | Test-Path -PathType Leaf } | ForEach-Object {
        if ($_.Match.Success) {
            if ($_.Match.Groups['minor'].Success) {
                if ($_.Match.Groups['build'].Success) {
                    if ($_.Match.Groups['revision'].Success) {
                        $_ | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new([int]::Parse($_.Match.Groups['major'].Value), [int]::Parse($_.Match.Groups['minor'].Value), [int]::Parse($_.Match.Groups['build'].Value), [int]::Parse($_.Match.Groups['revision'].Value))) -PassThru;
                    } else {
                        $_ | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new([int]::Parse($_.Match.Groups['major'].Value), [int]::Parse($_.Match.Groups['minor'].Value), [int]::Parse($_.Match.Groups['build'].Value))) -PassThru;
                    }
                } else {
                    $_ | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new([int]::Parse($_.Match.Groups['major'].Value), [int]::Parse($_.Match.Groups['minor'].Value))) -PassThru;
                }
            } else {
                $_ | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new([int]::Parse($_.Match.Groups['major'].Value), 0)) -PassThru;
            }
        } else {
            $Item = $_;
            switch ($Item.Year) {
                2019 { $Item | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new(16, 0)) -PassThru; break; }
                2017 { $Item | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new(15, 0)) -PassThru; break; }
                default {
                    if ($Item.Year -gt 2019) {
                        $Item | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new($Item.Year, 0)) -PassThru;
                    } else {
                        $Item | Add-Member -MemberType NoteProperty -Name 'Version' -Value ([Version]::new(0, 0, 0, 0)) -PassThru;
                    }
                }
            }
        }
    } | Where-Object { $_.Version.Major -ge 4 } | Sort-Object -Property 'Level', 'Version' -Descending) | Select-Object -First 1 -ExpandProperty 'Path';
}

$ProdAppSettings = $TestAppSettings = $DevAppSettings = $null;

if ($ProdAppSettingsFile | Test-Path) {
    $ProdAppSettings = [AppSettingsData]::Load($ProdAppSettingsFile);
    if ($TestAppSettingsFile | Test-Path) {
        $TestAppSettings = [AppSettingsData]::Load($TestAppSettingsFile);
    } else {
        $TestAppSettings = $ProdAppSettings.Clone();
    }
    if ($DevAppSettingsFile | Test-Path) {
        $DevAppSettings = [AppSettingsData]::Load($DevAppSettingsFile);
    } else {
        $DevAppSettings = $TestAppSettings.Clone();
    }
} else {
    if ($TestAppSettingsFile | Test-Path) {
        $TestAppSettings = [AppSettingsData]::Load($TestAppSettingsFile);
        if ($DevAppSettingsFile | Test-Path) {
            $DevAppSettings = [AppSettingsData]::Load($DevAppSettingsFile);
        } else {
            $DevAppSettings = $TestAppSettings.Clone();
        }
    } else {
        $DevAppSettings = [AppSettingsData]::Load($DevAppSettingsFile);
        $TestAppSettings = $DevAppSettings.Clone();
    }
    $ProdAppSettings = $TestAppSettings.Clone();
}

if ($null -eq $DevAppSettings.ConnectionStrings) {
    if ($null -eq $TestAppSettings.ConnectionStrings) {
        if ($null -eq $ProdAppSettings.ConnectionStrings) { $ProdAppSettings.ConnectionStrings = [ConnectionStringsSettingsData]::new() }
        $TestAppSettings.ConnectionStrings = [ConnectionStringsSettingsData]::new($ProdAppSettings.ConnectionStrings, $null);
    } else {
        if ($null -eq $ProdAppSettings.ConnectionStrings) { $ProdAppSettings.ConnectionStrings = [ConnectionStringsSettingsData]::new($TestAppSettings.ConnectionStrings, $null) }
    }
    $DevAppSettings.ConnectionStrings = [ConnectionStringsSettingsData]::new($TestAppSettings.ConnectionStrings, $null);
} else {
    if ($null -eq $TestAppSettings.ConnectionStrings) {
        if ($null -eq $ProdAppSettings.ConnectionStrings) { $ProdAppSettings.ConnectionStrings = [ConnectionStringsSettingsData]::new($DevAppSettings.ConnectionStrings, $null) }
        $TestAppSettings.ConnectionStrings = [ConnectionStringsSettingsData]::new($ProdAppSettings.ConnectionStrings, $null);
    } else {
        if ($null -eq $ProdAppSettings.ConnectionStrings) { $ProdAppSettings.ConnectionStrings = [ConnectionStringsSettingsData]::new($TestAppSettings.ConnectionStrings, $null) }
    }
}

$csb = $DevAppSettings.ConnectionStrings.FsInfoCat;
if ($null -eq $csb) {
    $csb = $TestAppSettings.ConnectionStrings.FsInfoCat;
    if ($null -eq $csb) {
        $csb = $ProdAppSettings.ConnectionStrings.FsInfoCat;
        if ($null -eq $csb) {
            $ProdAppSettings.ConnectionStrings.FsInfoCat = $csb = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
        }
        $TestAppSettings.ConnectionStrings.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
        foreach ($k in $csb.Keys) { if ($csb.ContainsKey($_)) { $TestAppSettings.ConnectionStrings.FsInfoCat[$k] = $csb[$k] } }
    } else {
        if ($null -eq $ProdAppSettings.ConnectionStrings) {
            $ProdAppSettings.ConnectionStrings.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
            foreach ($k in $csb.Keys) { if ($csb.ContainsKey($_)) { $ProdAppSettings.ConnectionStrings.FsInfoCat[$k] = $csb[$k] } }
        }
    }
    $DevAppSettings.ConnectionStrings.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
    foreach ($k in $csb.Keys) { if ($csb.ContainsKey($_)) { $DevAppSettings.ConnectionStrings.FsInfoCat[$k] = $csb[$k] } }
} else {
    if ($null -eq $TestAppSettings.ConnectionStrings.FsInfoCat) {
        if ($null -eq $ProdAppSettings.ConnectionStrings) {
            $ProdAppSettings.ConnectionStrings.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
            foreach ($k in $csb.Keys) { if ($csb.ContainsKey($_)) { $ProdAppSettings.ConnectionStrings.FsInfoCat[$k] = $csb[$k] } }
        } else {
            $csb = $ProdAppSettings.ConnectionStrings.FsInfoCat;
        }
        $TestAppSettings.ConnectionStrings.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
        foreach ($k in $csb.Keys) { if ($csb.ContainsKey($_)) { $TestAppSettings.ConnectionStrings.FsInfoCat[$k] = $csb[$k] } }
    } else {
        if ($null -eq $ProdAppSettings.ConnectionStrings) {
            $ProdAppSettings.ConnectionStrings.FsInfoCat = [System.Data.SqlClient.SqlConnectionStringBuilder]::new();
            foreach ($k in $csb.Keys) { if ($csb.ContainsKey($_)) { $ProdAppSettings.ConnectionStrings.FsInfoCat[$k] = $csb[$k] } }
        }
    }
}

$SqlConnectionStringBuilder = $null;
switch ($Stage) {
    { $_ -ieq 'Testing' -or $_ -ieq 'Test' } {
        $SqlConnectionStringBuilder = $TestAppSettings.ConnectionStrings.FsInfoCat;
        break;
    }
    { $_ -ieq 'Development' -or $_ -ieq 'Dev' } {
        $SqlConnectionStringBuilder = $DevAppSettings.ConnectionStrings.FsInfoCat;
        break;
    }
    default {
        $SqlConnectionStringBuilder = $ProdAppSettings.ConnectionStrings.FsInfoCat;
        break;
    }
}

if ($SqlConnectionStringBuilder.Count -eq 0) {
    $SqlConnectionStringBuilder.DataSource = $DataSource;
    $SqlConnectionStringBuilder.InitialCatalog = $InitialCatalog;
    $SqlConnectionStringBuilder.PersistSecurityInfo = $PersistSecurityInfo;
    $SqlConnectionStringBuilder.UserID = $UserID;
    $SqlConnectionStringBuilder.MultipleActiveResultSets = $MultipleActiveResultSets;
    $SqlConnectionStringBuilder.Encrypt = $Encrypt;
    $SqlConnectionStringBuilder.TrustServerCertificate = $TrustServerCertificate;
    $SqlConnectionStringBuilder.ConnectionTimeout = $ConnectionTimeout;
    $SqlConnectionStringBuilder.ApplicationName = $ApplicationName;
} else {
    if ($PSBoundParameters.ContainsKey('DataSource')) { $SqlConnectionStringBuilder.DataSource = $DataSource }
    if ($PSBoundParameters.ContainsKey('InitialCatalog')) { $SqlConnectionStringBuilder.InitialCatalog = $InitialCatalog }
    if ($PSBoundParameters.ContainsKey('PersistSecurityInfo')) { $SqlConnectionStringBuilder.PersistSecurityInfo = $PersistSecurityInfo }
    if ($PSBoundParameters.ContainsKey('UserID')) { $SqlConnectionStringBuilder.UserID = $UserID }
    if ($PSBoundParameters.ContainsKey('MultipleActiveResultSets')) { $SqlConnectionStringBuilder.MultipleActiveResultSets = $MultipleActiveResultSets }
    if ($PSBoundParameters.ContainsKey('Encrypt')) { $SqlConnectionStringBuilder.Encrypt = $Encrypt }
    if ($PSBoundParameters.ContainsKey('TrustServerCertificate')) { $SqlConnectionStringBuilder.TrustServerCertificate = $TrustServerCertificate }
    if ($PSBoundParameters.ContainsKey('ConnectionTimeout')) { $SqlConnectionStringBuilder.ConnectionTimeout = $ConnectionTimeout }
    if ($PSBoundParameters.ContainsKey('ApplicationName')) { $SqlConnectionStringBuilder.ApplicationName = $ApplicationName }
}

Write-Information -MessageData @"
Current SQL connection properties:
    ApplicationName = '$($SqlConnectionStringBuilder.ApplicationName)';
    Authentication = '$($SqlConnectionStringBuilder.Authentication)';
    ConnectTimeout = '$($SqlConnectionStringBuilder.ConnectTimeout)';
    DataSource = '$($SqlConnectionStringBuilder.DataSource)';
    InitialCatalog = '$($SqlConnectionStringBuilder.InitialCatalog)';
    PersistSecurityInfo = '$($SqlConnectionStringBuilder.PersistSecurityInfo)';
    UserID = '$($SqlConnectionStringBuilder.UserID)';
    MultipleActiveResultSets = '$($SqlConnectionStringBuilder.MultipleActiveResultSets)';
    Encrypt = '$($SqlConnectionStringBuilder.Encrypt)';
    TrustServerCertificate = '$($SqlConnectionStringBuilder.TrustServerCertificate)';
    AsynchronousProcessing = '$($SqlConnectionStringBuilder.AsynchronousProcessing)';
    ConnectionReset = '$($SqlConnectionStringBuilder.ConnectionReset)';
    ConnectRetryCount = '$($SqlConnectionStringBuilder.ConnectRetryCount)';
    ConnectRetryInterval = '$($SqlConnectionStringBuilder.ConnectRetryInterval)';
    IntegratedSecurity = '$($SqlConnectionStringBuilder.IntegratedSecurity)';
    LoadBalanceTimeout = '$($SqlConnectionStringBuilder.LoadBalanceTimeout)';
    MaxPoolSize = '$($SqlConnectionStringBuilder.MaxPoolSize)';
    MinPoolSize = '$($SqlConnectionStringBuilder.MinPoolSize)';
    MultiSubnetFailover = '$($SqlConnectionStringBuilder.MultiSubnetFailover)';
    NetworkLibrary = '$($SqlConnectionStringBuilder.NetworkLibrary)';
    PacketSize = '$($SqlConnectionStringBuilder.PacketSize)';
    PoolBlockingPeriod = '$($SqlConnectionStringBuilder.PoolBlockingPeriod)';
    Pooling = '$($SqlConnectionStringBuilder.Pooling)';
    Replication = '$($SqlConnectionStringBuilder.Replication)';
    TransactionBinding = '$($SqlConnectionStringBuilder.TransactionBinding)';
    TransparentNetworkIPResolution = '$($SqlConnectionStringBuilder.TransparentNetworkIPResolution)';
    Enlist = '$($SqlConnectionStringBuilder.Enlist)';
    FailoverPartner = '$($SqlConnectionStringBuilder.FailoverPartner)';
    TypeSystemVersion = '$($SqlConnectionStringBuilder.TypeSystemVersion)';
    UserInstance = '$($SqlConnectionStringBuilder.UserInstance)';
    WorkstationID = '$($SqlConnectionStringBuilder.WorkstationID)';
"@

[System.Management.Automation.PSCredential]$AppCredentials = $null;
$BaseMessage = $Message = "Please provide the login and password that will be used to log into the application for administrative purposes.`nNote: Login name and password is case-sensitive";
do {
    $AppCredentials = Get-Credential -UserName $SqlConnectionStringBuilder.UserID -Message $Message;
    if ($null -eq $AppCredentials) { return }
    if ([string]::IsNullOrWhiteSpace($AppCredentials.UserName)) {
        $Message = "ERROR: User name cannot be empty!`n`n$BaseMessage";
        Write-Warning -Message 'User name cannot be empty!' -WarningAction Continue;
        $AppCredentials = $null;
    } else {
        if ($AppCredentials.UserName.Trim() -notmatch '^[a-zA-Z][a-zA-Z\d]*$') {
            $Message = "ERROR: User name must start with a letter and can only be followed by letters and numbers!`n`n$BaseMessage";
            Write-Warning -Message 'User name must start with a letter and can only be followed by letters and numbers!' -WarningAction Continue;
            $AppCredentials = $null;
        } else {
            if ([FsInfoCat.Util.PwHash]::PasswordValidationRegex.IsMatch($AppCredentials.GetNetworkCredential().Password)) {
                $ConfirmCredentials = Get-Credential -UserName $AppCredentials.UserName -Message 'Please Re-enter the application administrative password.';
                if ($null -eq $ConfirmCredentials) { return }
                if ($AppCredentials.GetNetworkCredential().Password -cne $ConfirmCredentials.GetNetworkCredential().Password) {
                    $Message = "ERROR: Password and confirmation do not match!`n`n$BaseMessage";
                    Write-Warning -Message 'Password and confirmation do not match!' -WarningAction Continue;
                    $AppCredentials = $null;
                } else {
                    if ($AppCredentials.UserName -cne $ConfirmCredentials.UserName) {
                        $Message = "ERROR: User name in confirmation does not match the original user name!`n`n$BaseMessage";
                        Write-Warning -Message 'User name in confirmation does not match the original user name!' -WarningAction Continue;
                        $AppCredentials = $null;
                    }
                }
            } else {
                $Message = "ERROR: $([FsInfoCat.Util.PwHash]::PASSWORD_VALIDATION_MESSAGE)`n`n$BaseMessage";
                Write-Warning -Message ([FsInfoCat.Util.PwHash]::PASSWORD_VALIDATION_MESSAGE) -WarningAction Continue;
                $AppCredentials = $null;
            }
        }
    }
} while ($null -eq $AppCredentials);

$PwHash = [FsInfoCat.Util.PwHash]::Create($AppCredentials.GetNetworkCredential().Password);
$DbCredentials.Password.MakeReadOnly();
$SqlCredential = New-Object -TypeName 'System.Data.SqlClient.SqlCredential' -ArgumentList $DbCredentials.UserName, $DbCredentials.Password -ErrorAction Stop;
$DbCommands = [System.IO.File]::ReadAllText(($PSScriptRoot | Join-Path -ChildPath 'CreateDb.json')) | ConvertFrom-Json;
#<#
if ($SqlConnectionStringBuilder.ContainsKey('UserID')) { $SqlConnectionStringBuilder.Remove('UserID') }
if ($SqlConnectionStringBuilder.ContainsKey('User ID')) { $SqlConnectionStringBuilder.Remove('User ID') }
$SqlConnection = New-Object -TypeName 'System.Data.SqlClient.SqlConnection' -ArgumentList $SqlConnectionStringBuilder.ConnectionString, $SqlCredential -ErrorAction Stop;
$SqlConnection.Open();
if ($SqlConnection.State -ne [System.Data.ConnectionState]::Open) {
    $SqlConnection.Dispose();
    Write-Warning -Message "Unable to open database connection to $($SqlConnectionStringBuilder.ConnectionString)" -WarningAction Continue;
    return;
}
$HostDeviceID = [Guid]::NewGuid();
try {
    $DbCommands | ForEach-Object {
        $Status = $_.Message;
        $_.Commands | ForEach-Object {
            $SqlCommand = $SqlConnection.CreateCommand();
            try {
                if ($_ -is [string]) {
                    Write-Progress -Activity 'Initializing database' -Status $Message -CurrentOperation $_;
                    $SqlCommand.CommandText = $_;
                } else {
                    Write-Progress -Activity 'Initializing database' -Status $Message -CurrentOperation $_.Text;
                    $SqlCommand.CommandText = $_.Text;
                    $_.Parameters | ForEach-Object {
                        switch ($_) {
                            'CreatedOn' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('CreatedOn', [System.Data.SqlDbType]::DateTime).Value = [DateTime]::Now;
                                break;
                            }
                            'PwHash' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('PwHash', [System.Data.SqlDbType]::NVarChar).Value = $PwHash.ToString();
                                break;
                            }
                            'LoginName' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('LoginName', [System.Data.SqlDbType]::NVarChar).Value = $AppCredentials.UserName;
                                break;
                            }
                            'HostDeviceID' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('HostDeviceID', [System.Data.SqlDbType]::UniqueIdentifier).Value = $HostDeviceID;
                                break;
                            }
                            'MachineIdentifer' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('MachineIdentifer', [System.Data.SqlDbType]::NVarChar).Value = Get-LocalMachineIdentifier;
                                break;
                            }
                            'MachineName' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('MachineName', [System.Data.SqlDbType]::NVarChar).Value = [Environment]::MachineName;
                                break;
                            }
                            'IsWindows' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('IsWindows', [System.Data.SqlDbType]::Bit).Value = [Environment]::OSVersion.Platform -eq [PlatformID]::Win32NT;
                                break;
                            }
                        }
                    }
                }
                $SqlCOmmand.ExecuteNonQuery();
            } finally { $SqlCommand.Dispose(); }
        }
    }
} finally {
    $SqlConnection.Dispose();
    Write-Progress -Activity 'Initializing database' -Status 'Finished' -Completed;
}

#>
