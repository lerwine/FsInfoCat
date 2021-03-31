Param(
    [string]$AppSettingsJson,

    [ValidateSet("Production", "Testing", "Development", "Prod", "Test", "Dev")]
    [string]$Stage = "Production",

    [string]$DbConnectionString = 'Server=tcp:c868dbserver.database.windows.net,1433;Initial Catalog=FsInfoCat;Persist Security Info=False;User ID=fsinfocatadmin;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'
)

&{
    $VarNames = @(Get-Variable | Select-Object -ExpandProperty 'Name');
    if (@(@('SetupScriptsFolder', 'FsInfoCatModuleName', 'FsInfoCatModulePath', 'SetupDistroFolder', 'FsInfoCatModuleDistroFile', 'WebProjectDirectory', 'ProdAppSettingsFile',
    'TestAppSettingsFile', 'DevAppSettingsFile', 'RepositoryRootPath') | Where-Object { $VarNames -inotcontains $_ }).Count -eq 0) { return }
    try {
        Set-Variable -Name 'SetupScriptsFolder' -Value ('Setup' | Join-Path -ChildPath 'Scripts') -Scope Global -Option Constant;
        Set-Variable -Name 'FsInfoCatModuleName' -Value 'FsInfoCat' -Scope Global -Option Constant;
        Set-Variable -Name 'FsInfoCatModulePath' -Value (('Setup' | Join-Path -ChildPath 'bin') | Join-Path -ChildPath $FsInfoCatModuleName) -Scope Global -Option Constant;
        Set-Variable -Name 'SetupDistroFolder' -Value ('Setup' | Join-Path -ChildPath 'Distro') -Scope Global -Option Constant;
        Set-Variable -Name 'FsInfoCatModuleDistroFile' -Value ($SetupDistroFolder | Join-Path -ChildPath "$FsInfoCatModuleName.zip") -Scope Global -Option Constant;
        Set-Variable -Name 'WebProjectDirectory' -Value ('src' | Join-Path -ChildPath 'FsInfoCat.Web') -Scope Script -Option Constant;
        Set-Variable -Name 'ProdAppSettingsFile' -Value ($WebProjectDirectory | Join-Path -ChildPath 'appsettings.json') -Scope Script -Option Constant;
        Set-Variable -Name 'TestAppSettingsFile' -Value ($WebProjectDirectory | Join-Path -ChildPath 'appsettings.Testing.json') -Scope Script -Option Constant;
        Set-Variable -Name 'DevAppSettingsFile' -Value ($WebProjectDirectory | Join-Path -ChildPath 'appsettings.Testing.json') -Scope Script -Option Constant;
        Set-Variable -Name 'RepositoryRootPath' -Value (($PSScriptRoot | Join-Path -ChildPath '../..') | Resolve-Path -ErrorAction Continue).ProviderPath -Scope Global -Option Constant;
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
        Write-Error -ErrorRecord $Error[0] -Category ResourceUnavailable -ErrorId 'UnexpectedSubdirectory' `
            -TargetObject ($RepositoryRootPath | Join-Path -ChildPath $FsInfoCatModuleDistroFile) `
            -RecommendedAction "Manually import module '$FsInfoCatModulePath' before executing this script" `
            -CategoryReason "Unable import module '$FsInfoCatModulePath'." -ErrorAction Continue;
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

function Read-AppSettingsFile {
    [CmdletBinding(DefaultParameterSetName = 'Read')]
    param (
        [Parameter(ParameterSetName = 'Read')]
        [string]$Path,

        [Parameter(Mandatory = $true, ParameterSetName = 'Clone')]
        [PSCustomObject]$Clone
    )

    $AppSettings = $null;
    if ($PSBoundParameters.ContainsKey('Clone')) {
        $AppSettings = (($Clone | ConvertTo-Json) | ConvertFrom-Json);
    } else {
        if ($PSBoundParameters.ContainsKey('Path')) {
            if ($Path | Test-Path) {
                try { $AppSettings = [System.IO.File]::ReadAllText($Path) | ConvertFrom-Json -ErrorAction Stop }
                catch {
                    Write-Error -ErrorRecord $Error[0] -CategoryReason "Failure reading from website app settings file '$Path'" `
                        -RecommendedAction "Ensure '$Path' contains valid JSON and that security settings allow read/write access.";
                    return;
                }
            }
        } else {
            return [PSCustomObject]@{
                Logging = [PSCustomObject]@{
                    LogLevel = [PSCustomObject]@{
                        Default = 'Warning';
                        Microsoft = 'Warning';
                        'Microsoft.Hosting.Lifetime' = 'Warning';
                    };
                };
                AllowedHosts = '*'
            };
        }
    }
    if ($null -ne ($AppSettings | Get-Member -Name 'ConnectionStrings')) {
        $ConnectionStrings = $AppSettings.ConnectionStrings;
        if ($null -eq $ConnectionStrings) {
            Write-Error -Message 'Invalid ConnectionStrings Setting' -Category InvalidData  -ErrorId 'InvalidConnectionStringsSetting' -CategoryTargetName 'ConnectionStrings';
        } else {
            if ($ConnectionStrings -isnot [PSCustomObject]) {
                Write-Error -Message 'Invalid ConnectionStrings Setting' -Category InvalidData  -ErrorId 'InvalidConnectionStringsSetting' -TargetObject $ConnectionStrings -CategoryTargetName 'ConnectionStrings';
            } else {
                if ($null -ne ($ConnectionStrings | Get-Member -Name 'FsInfoCat')) {
                    $FsInfoCatConnectionString = $ConnectionStrings.FsInfoCat;
                    if ($null -eq $FsInfoCatConnectionString) {
                        Write-Error -Message 'Invalid ConnectionString Value' -Category InvalidData  -ErrorId 'InvalidConnectionStringValue' -CategoryTargetName 'FsInfoCat';
                    } else {
                        if ($FsInfoCatConnectionString -isnot [string] -or [string]::IsNullOrWhiteSpace($FsInfoCatConnectionString)) {
                            Write-Error -Message 'Invalid ConnectionString Value' -Category InvalidData  -ErrorId 'InvalidConnectionStringValue' -TargetObject $FsInfoCatConnectionString -CategoryTargetName 'ConnectionStrings';
                        }
                    }
                }
            }
        }
    }
    if ($null -ne ($AppSettings | Get-Member -Name 'Logging')) {
        $LoggingSetting = $AppSettings.Logging;
        if ($null -eq $LoggingSetting) {
            Write-Error -Message 'Invalid Logging Setting' -Category InvalidData  -ErrorId 'InvalidLoggingSetting' -CategoryTargetName 'Logging';
        } else {
            if ($LoggingSetting -isnot [PSCustomObject]) {
                Write-Error -Message 'Invalid Logging Setting' -Category InvalidData  -ErrorId 'InvalidLoggingSetting' -TargetObject $LoggingSetting -CategoryTargetName 'Logging';
            } else {
                if ($null -ne ($LoggingSetting | Get-Member -Name 'LogLevel')) {
                    $LogLevelSetting = $LoggingSetting.LogLevel;
                    if ($null -eq $LogLevelSetting) {
                        Write-Error -Message 'Invalid Log Level Setting' -Category InvalidData  -ErrorId 'InvalidLogLevelSetting' -CategoryTargetName 'LogLevel';
                    } else {
                        if ($LogLevelSetting -isnot [PSCustomObject]) {
                            Write-Error -Message 'Invalid Log Level Setting' -Category InvalidData  -ErrorId 'InvalidLogLevelSetting' -TargetObject $LogLevelSetting -CategoryTargetName 'LogLevel';
                        } else {
                            foreach ($PropertyName in @('Default', 'Microsoft', 'Microsoft.Hosting.Lifetime')) {
                                if ($null -ne ($LogLevelSetting | Get-Member -Name $PropertyName)) {
                                    $Value = $LogLevelSetting.($PropertyName);
                                    if ($null -eq $Value) {
                                        Write-Error -Message 'Invalid Log Level Value' -Category InvalidData  -ErrorId 'InvalidLogLevelValue' -CategoryTargetName 'Default';
                                    } else {
                                        if ($FsInfoCatConnectionString -isnot [string] -or [string]::IsNullOrWhiteSpace($Value) -or @('Critical', 'Error', 'Warning', 'Information', 'Trace', 'Debug', 'None') -inotcontains $Value) {
                                            Write-Error -Message 'Invalid Log Level Value' -Category InvalidData  -ErrorId 'InvalidLogLevelValue' -TargetObject $FsInfoCatConnectionString -CategoryTargetName 'Default';
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    AppSettings | Write-Output;
}

$ProdAppSettings = $OriginalProdAppSettings = Read-AppSettingsFile -Path $ProdAppSettingsFile;
$TestAppSettings = $OriginalTestAppSettings = Read-AppSettingsFile -Path $TestAppSettingsFile;
$DevAppSettings = $OriginalDevAppSettings = Read-AppSettingsFile -Path $DevAppSettingsFile;

if ($null -eq $ProdAppSettings) {
    if ($null -eq $TestAppSettings) {
        if ($null -eq $DevAppSettings) { $DevAppSettings = Read-AppSettingsFile }
        $TestAppSettings = Read-AppSettingsFile -Clone $DevAppSettings;
    } else {
        if ($null -eq $DevAppSettings) { $DevAppSettings = Read-AppSettingsFile -Clone $TestAppSettings }
    }
    $ProdAppSettings = Read-AppSettingsFile -Clone $TestAppSettings;
} else {
    if ($null -eq $TestAppSettings) {
        if ($null -eq $DevAppSettings) { $DevAppSettings = Read-AppSettingsFile -Clone $ProdAppSettings }
        $TestAppSettings = Read-AppSettingsFile -Clone $DevAppSettings;
    } else {
        if ($null -eq $DevAppSettings) { $DevAppSettings = Read-AppSettingsFile -Clone $TestAppSettings }
    }
}

$ProdConnectionStrings = $ProdAppSettings.ConnectionStrings;
$TestConnectionStrings = $TestAppSettings.ConnectionStrings;
$DevConnectionStrings = $DevAppSettings.ConnectionStrings;
if ($null -eq $ProdConnectionStrings) {
    if ($null -eq $TestConnectionStrings) {
        if ($null -eq $DevConnectionStrings) {
            $DevConnectionStrings = [PSCustomObject]@{ FsInfoCat = $DbConnectionString }
            $DevAppSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $DevConnectionStrings;
        }
        $TestConnectionStrings = ($DevConnectionStrings | ConvertTo-Json) | ConvertFrom-Json;
        $TestAppSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $TestConnectionStrings;
    } else {
        if ($null -eq $DevConnectionStrings) {
            $DevConnectionStrings = ($TestConnectionStrings | ConvertTo-Json) | ConvertFrom-Json;
            $DevAppSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $DevConnectionStrings;
        }
    }
    $ProdConnectionStrings = ($TestConnectionStrings | ConvertTo-Json) | ConvertFrom-Json;
    $ProdAppSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $ProdConnectionStrings;
} else {
    if ($null -eq $TestConnectionStrings) {
        if ($null -eq $DevConnectionStrings) {
            $DevConnectionStrings = ($ProdConnectionStrings | ConvertTo-Json) | ConvertFrom-Json;
            $DevAppSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $DevConnectionStrings;
        }
        $TestConnectionStrings = ($DevConnectionStrings | ConvertTo-Json) | ConvertFrom-Json;
        $TestAppSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $TestConnectionStrings;
    } else {
        if ($null -eq $DevConnectionStrings) {
            $DevConnectionStrings = ($TestConnectionStrings | ConvertTo-Json) | ConvertFrom-Json;
            $DevAppSettings | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value $DevConnectionStrings;
        }
    }
}

$TargetConnectionString = $DbConnectionString;
if ($PSBoundParameters.ContainsKey('DbConnectionString')) {
    switch ($Stage) {
        { $_ -ieq 'Testing' -or $_ -ieq 'Test' } {
            if ($TestConnectionStrings.FsInfoCat -ine $DbConnectionString) {
                $TestConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString -Force;
            }
            if ($null -eq $ProdConnectionStrings.FsInfoCat) {
                $ProdConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            if ($null -eq $DevConnectionStrings.FsInfoCat) {
                $DevConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            break;
        }
        { $_ -ieq 'Development' -or $_ -ieq 'Dev' } {
            if ($DevConnectionStrings.FsInfoCat -cne $DbConnectionString) {
                $DevConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            if ($null -eq $ProdConnectionStrings.FsInfoCat) {
                $ProdConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            if ($null -eq $TestConnectionStrings.FsInfoCat) {
                $TestConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            break;
        }
        default {
            if ($ProdConnectionStrings.FsInfoCat -cne $DbConnectionString) {
                $ProdConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            if ($null -eq $TestConnectionStrings.FsInfoCat) {
                $TestConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            if ($null -eq $DevConnectionStrings.FsInfoCat) {
                $DevConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            break;
        }
    }
} else {
    if ($null -eq $ProdConnectionStrings.FsInfoCat) {
        if ($null -eq $TestConnectionStrings.FsInfoCat) {
            if ($null -eq $DevConnectionStrings.FsInfoCat) {
                $DevConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DbConnectionString;
            }
            $TestConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DevConnectionStrings.FsInfoCat;
        } else {
            if ($null -eq $DevConnectionStrings.FsInfoCat) {
                $DevConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $TestConnectionStrings.FsInfoCat;
            }
        }
        $ProdConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $TestConnectionStrings.FsInfoCat;
    } else {
        if ($null -eq $TestConnectionStrings.FsInfoCat) {
            if ($null -eq $DevConnectionStrings.FsInfoCat) {
                $DevConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $ProdConnectionStrings.FsInfoCat;
            }
            $TestConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $DevConnectionStrings.FsInfoCat;
        } else {
            if ($null -eq $DevConnectionStrings.FsInfoCat) {
                $DevConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $TestConnectionStrings.FsInfoCat;
            }
        }
    }
    switch ($Stage) {
        { $_ -ieq 'Testing' -or $_ -ieq 'Test' } {
            $TargetConnectionString = $TestConnectionStrings.FsInfoCat;
            break;
        }
        { $_ -ieq 'Development' -or $_ -ieq 'Dev' } {
            $TargetConnectionString = $DevConnectionStrings.FsInfoCat;
            break;
        }
        default {
            $TargetConnectionString = $ProdConnectionStrings.FsInfoCat;
            break;
        }
    }
}

$SqlConnectionStringBuilderHashTable = @{};
foreach ($cs in @($ProdConnectionStrings.FsInfoCat, $TestConnectionStrings.FsInfoCat, $DevConnectionStrings.FsInfoCat)) {
    if (-not $SqlConnectionStringBuilderHashTable.ContainsKey($cs)) {
        $csb = $null;
        try { $csb = New-Object -TypeName 'System.Data.SqlClient.SqlConnectionStringBuilder' -cs $DbConnectionString -ErrorAction Stop }
        catch {
            Write-Error -ErrorRecord $Error[0] -CategoryReason "Unable parse connection string '$cs'";
        }
        $SqlConnectionStringBuilderHashTable[$cs] = $csb;
    }
}

$SqlConnectionStringBuilder = $null;
New-Object -TypeName 'System.Data.SqlClient.SqlConnectionStringBuilder' -ArgumentList $ProdAppSettingsJson.ConnectionStrings.FsInfoCat -ErrorAction Continue;
if (-not $SqlConnectionStringBuilderHashTable.ContainsKey($TargetConnectionString)) {
    Write-Warning -Message "Failed to parse $Stage connection string";
    return;
}
$SqlConnectionStringBuilder = $SqlConnectionStringBuilderHashTable[$TargetConnectionString];

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

if (-not $PSBoundParameters.ContainsKey('DbConnectionString')) {
    $SqlConnectionStringBuilder.PSBase.DataSource = ([string](Read-Host -Prompt 'Database server name'))
    if ([string]::IsNullOrWhiteSpace($SqlConnectionStringBuilder.PSBase.DataSource)) { return }
    $SqlConnectionStringBuilder.PSBase.InitialCatalog = Read-Host -Prompt 'Initial Catalog (db name)';
    if ([string]::IsNullOrWhiteSpace($SqlConnectionStringBuilder.PSBase.InitialCatalog)) { return }
    $DbCredentials = Get-Credential -Message 'Enter database login credentials';
    if ($null -eq $DbCredentials) { return }
    [System.Management.Automation.PSCredential]$AppCredentials = $null;
    $BaseMessage = $Message = "Please provide the login and password that will be used to log into the application for administrative purposes.`nNote: Login name and password is case-sensitive";
    do {
        $AppCredentials = Get-Credential -UserName 'fsinfocatadmin' -Message $Message;
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
}

$HostDeviceRegRequest = [FsInfoCat.Models.HostDevices.HostDeviceRegRequest]::CreateForLocal();
if ($null -eq $HostDeviceRegRequest) { return }
$PwHash = [FsInfoCat.Util.PwHash]::Create($AppCredentials.GetNetworkCredential().Password);
$DbCredentials.Password.MakeReadOnly();
$SqlCredential = New-Object -TypeName 'System.Data.SqlClient.SqlCredential' -ArgumentList $DbCredentials.UserName, $DbCredentials.Password -ErrorAction Stop;
$DbCommands = Import-Clixml -Path ($PSScriptRoot | Join-Path -ChildPath 'CreateDb.xml') -ErrorAction Stop;

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
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('MachineIdentifer', [System.Data.SqlDbType]::NVarChar).Value = $HostDeviceRegRequest.MachineIdentifer;
                                break;
                            }
                            'MachineName' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('MachineName', [System.Data.SqlDbType]::NVarChar).Value = $HostDeviceRegRequest.MachineName;
                                break;
                            }
                            'IsWindows' {
                                ([System.Data.SqlClient.SqlCommand]$SqlCommand).Parameters.Add('IsWindows', [System.Data.SqlDbType]::Bit).Value = $HostDeviceRegRequest.IsWindows;
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
