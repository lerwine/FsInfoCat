Param(
    # Path to FSInfoCat module, relative to the current script.
    [string]$FsInfoCatModulePath = '..\Setup\bin\FSInfoCat',
    
    # Path to the published and packaged FSInfoCat modules.
    [string]$FsInfoCatDistroPath = '..\Distro\PS',

    # Path to the published and packaged FSInfoCat modules.
    [string]$WebProjectFile = '..\..\src\FsInfoCat.Web\FsInfoCat.Web.csproj',

    [string]$ProdAppSettingsFile = '..\..\src\FsInfoCat.Web\appsettings.json',

    [string]$DevAppSettingsFile = '..\..\src\FsInfoCat.Web\appsettings.Development.json'
)

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

$FsInfoCatModulePath = $PSScriptRoot | Join-Path -ChildPath $FsInfoCatModulePath;
$FsInfoCatDistroPath = $PSScriptRoot | Join-Path -ChildPath $FsInfoCatDistroPath;
$WebProjectFile = $PSScriptRoot | Join-Path -ChildPath $WebProjectFile;
$ProdAppSettingsFile = $PSScriptRoot | Join-Path -ChildPath $ProdAppSettingsFile;
$DevAppSettingsFile = $PSScriptRoot | Join-Path -ChildPath $DevAppSettingsFile;

<#
if (-not (($FsInfoCatModulePath | Test-Path -PathType Container) -and $null -ne (Import-Module -Name $FsInfoCatModulePath -ErrorAction Ignore -PassThru))) {
    $ArchivePath = $FsInfoCatDistroPath | Join-Path -ChildPath 'FSInfoCat-Windows-netcoreapp31.zip';
    if ($PSVersionTable.PSEdition -eq 'Desktop') {
        $ArchivePath = $FsInfoCatDistroPath | Join-Path -ChildPath 'FSInfoCat-Windows-net461.zip';
    } else {
        if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Unix) {
            $ArchivePath = $FsInfoCatDistroPath | Join-Path -ChildPath 'FSInfoCat-Linux-netcoreapp31.zip';
        } else {
            if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::MacOSX) {
                $ArchivePath = $FsInfoCatDistroPath | Join-Path -ChildPath 'FSInfoCat-OSX-netcoreapp31.zip';
            }
        }
    }
    Expand-Archive -Path ($PSScriptRoot | Join-Path -ChildPath $ArchivePath) -DestinationPath $FsInfoCatModulePath -ErrorAction Stop;
    Import-Module -Name $FsInfoCatModulePath -ErrorAction Stop;
}
#>
$ProdAppSettingsJson = $null;
$ProdAppSettingsJson = [System.IO.File]::ReadAllText($ProdAppSettingsFile) | ConvertFrom-Json -ErrorAction Continue;
if ($null -eq $ProdAppSettingsJson) {
    Write-Warning -Message 'Failed to load web application production settings';
    return;
}

if ($null -eq $ProdAppSettingsJson.ConnectionStrings) {
    $ProdAppSettingsJson | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value (New-Object -TypeName 'System.Management.Automation.PSObject');
}

if ($null -eq $ProdAppSettingsJson.ConnectionStrings.FsInfoCat) {
    $ProdAppSettingsJson.ConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value 'Server=tcp:c868dbserver.database.windows.net,1433;Initial Catalog=FsInfoCat;Persist Security Info=False;User ID=fsinfocatadmin;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;';
}

$ProdSqlConnectionStringBuilder = $null;
$ProdSqlConnectionStringBuilder = New-Object -TypeName 'System.Data.SqlClient.SqlConnectionStringBuilder' -ArgumentList $ProdAppSettingsJson.ConnectionStrings.FsInfoCat -ErrorAction Continue;
if ($null -eq $ProdSqlConnectionStringBuilder) {
    Write-Warning -Message 'Failed to parse production connection string';
    return;
}

$DevAppSettingsJson = $null;
$DevAppSettingsJson = [System.IO.File]::ReadAllText($DevAppSettingsFile) | ConvertFrom-Json -ErrorAction Continue;
if ($null -eq $DevAppSettingsJson) {
    Write-Warning -Message 'Failed to load web application dev settings';
    return;
}

if ($null -eq $DevAppSettingsJson.ConnectionStrings) {
    $DevAppSettingsJson | Add-Member -MemberType NoteProperty -Name 'ConnectionStrings' -Value (New-Object -TypeName 'System.Management.Automation.PSObject');
}

if ($null -eq $DevAppSettingsJson.ConnectionStrings.FsInfoCat) {
    $DevAppSettingsJson.ConnectionStrings | Add-Member -MemberType NoteProperty -Name 'FsInfoCat' -Value $ProdSqlConnectionStringBuilder.ConnectionString;
}

$DevSqlConnectionStringBuilder = $null;
$DevSqlConnectionStringBuilder = New-Object -TypeName 'System.Data.SqlClient.SqlConnectionStringBuilder' -ArgumentList $DevAppSettingsJson.ConnectionStrings.FsInfoCat -ErrorAction Continue;
if ($null -eq $DevSqlConnectionStringBuilder) {
    Write-Warning -Message 'Failed to parse dev connection string';
    return;
}

@"
ApplicationName = '$($DevSqlConnectionStringBuilder.ApplicationName)';
Authentication = '$($DevSqlConnectionStringBuilder.Authentication)';
ConnectTimeout = '$($DevSqlConnectionStringBuilder.ConnectTimeout)';
DataSource = '$($DevSqlConnectionStringBuilder.DataSource)';
InitialCatalog = '$($DevSqlConnectionStringBuilder.InitialCatalog)';
PersistSecurityInfo = '$($DevSqlConnectionStringBuilder.PersistSecurityInfo)';
UserID = '$($DevSqlConnectionStringBuilder.UserID)';
MultipleActiveResultSets = '$($DevSqlConnectionStringBuilder.MultipleActiveResultSets)';
Encrypt = '$($DevSqlConnectionStringBuilder.Encrypt)';
TrustServerCertificate = '$($DevSqlConnectionStringBuilder.TrustServerCertificate)';
AsynchronousProcessing = '$($DevSqlConnectionStringBuilder.AsynchronousProcessing)';
ConnectionReset = '$($DevSqlConnectionStringBuilder.ConnectionReset)';
ConnectRetryCount = '$($DevSqlConnectionStringBuilder.ConnectRetryCount)';
ConnectRetryInterval = '$($DevSqlConnectionStringBuilder.ConnectRetryInterval)';
IntegratedSecurity = '$($DevSqlConnectionStringBuilder.IntegratedSecurity)';
LoadBalanceTimeout = '$($DevSqlConnectionStringBuilder.LoadBalanceTimeout)';
MaxPoolSize = '$($DevSqlConnectionStringBuilder.MaxPoolSize)';
MinPoolSize = '$($DevSqlConnectionStringBuilder.MinPoolSize)';
MultiSubnetFailover = '$($DevSqlConnectionStringBuilder.MultiSubnetFailover)';
NetworkLibrary = '$($DevSqlConnectionStringBuilder.NetworkLibrary)';
PacketSize = '$($DevSqlConnectionStringBuilder.PacketSize)';
PoolBlockingPeriod = '$($DevSqlConnectionStringBuilder.PoolBlockingPeriod)';
Pooling = '$($DevSqlConnectionStringBuilder.Pooling)';
Replication = '$($DevSqlConnectionStringBuilder.Replication)';
TransactionBinding = '$($DevSqlConnectionStringBuilder.TransactionBinding)';
TransparentNetworkIPResolution = '$($DevSqlConnectionStringBuilder.TransparentNetworkIPResolution)';
Enlist = '$($DevSqlConnectionStringBuilder.Enlist)';
FailoverPartner = '$($DevSqlConnectionStringBuilder.FailoverPartner)';
TypeSystemVersion = '$($DevSqlConnectionStringBuilder.TypeSystemVersion)';
UserInstance = '$($DevSqlConnectionStringBuilder.UserInstance)';
WorkstationID = '$($DevSqlConnectionStringBuilder.WorkstationID)';
"@


<#
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