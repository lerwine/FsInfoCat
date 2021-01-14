$FsInfoCatModulePath = $PSScriptRoot | Join-Path -ChildPath '../Setup/bin/FSInfoCat';

if (-not (($FsInfoCatModulePath | Test-Path -PathType Container) -and $null -ne (Import-Module -Name $FsInfoCatModulePath -ErrorAction Ignore -PassThru))) {
    $ArchivePath = '../Distro/PS/FSInfoCat-Windows-netcoreapp31.zip';
    if ($PSVersionTable.PSEdition -eq 'Desktop') {
        $ArchivePath = '../Distro/PS/FSInfoCat-Windows-net461.zip';
    } else {
        if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Unix) {
            $ArchivePath = '../Distro/PS/FSInfoCat-Linux-netcoreapp31.zip';
        } else {
            if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::MacOSX) {
                $ArchivePath = '../Distro/PS/FSInfoCat-OSX-netcoreapp31.zip';
            }
        }
    }
    Expand-Archive -Path ($PSScriptRoot | Join-Path -ChildPath $ArchivePath) -DestinationPath $FsInfoCatModulePath -ErrorAction Stop;
    Import-Module -Name $FsInfoCatModulePath -ErrorAction Stop;
}

$SqlConnectionStringBuilder = New-Object -TypeName 'System.Data.SqlClient.SqlConnectionStringBuilder' -Property @{
    Authentication = [System.Data.SqlClient.SqlAuthenticationMethod]::SqlPassword;
    ApplicationName = 'Initialize-FsInfoCatDb.ps1';
    WorkstationID = [System.Environment]::MachineName;
};
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