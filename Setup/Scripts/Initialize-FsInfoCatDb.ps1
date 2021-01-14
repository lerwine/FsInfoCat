$FsInfoCatModulePath = $PSScriptRoot | Join-Path -ChildPath '../Setup/bin/FSInfoCat';

if (-not (($FsInfoCatModulePath | Test-Path -PathType Container) -and $null -ne (Import-Module -Name $FsInfoCatModulePath -ErrorAction Ignore -PassThru))) {
    $ArchivePath = '../Setup/Distro/PS/FSInfoCat-Windows-netcoreapp31.zip';
    if ($PSVersionTable.PSEdition -eq 'Desktop') {
        $ArchivePath = '../Setup/Distro/PS/FSInfoCat-Windows-net461.zip';
    } else {
        if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Unix) {
            $ArchivePath = '../Setup/Distro/PS/FSInfoCat-Linux-netcoreapp31.zip';
        } else {
            if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::MacOSX) {
                $ArchivePath = '../Setup/Distro/PS/FSInfoCat-OSX-netcoreapp31.zip';
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
    $AppCredentials = Get-Credential -UserName 'admin' -Message $Message;
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

$DbCredentials.Password.MakeReadOnly();
$SqlCredential = New-Object -TypeName 'System.Data.SqlClient.SqlCredential' -ArgumentList $DbCredentials.UserName, $DbCredentials.Password -ErrorAction Stop;

$SqlConnection = New-Object -TypeName 'System.Data.SqlClient.SqlConnection' -ArgumentList $SqlConnectionStringBuilder.ConnectionString, $SqlCredential -ErrorAction Stop;
$SqlConnection.Open();
if ($SqlConnection.State -ne [System.Data.ConnectionState]::Open) {
    $SqlConnection.Dispose();
    Write-Warning -Message "Unable to open database connection to $($SqlConnectionStringBuilder.ConnectionString)" -WarningAction Continue;
    return;
}
try {
    @('Volume', 'HostContributor', 'HostDevice', 'UserCredential', 'Account') | ForEach-Object {
        $SqlCommand = $SqlConnection.CreateCommand();
        try {
            $SqlCommand.CommandText = "IF OBJECT_ID('dbo.$_', 'U') IS NOT NULL DROP TABLE dbo.$_";
            $SqlCOmmand.ExecuteNonQuery();
        } finally { $SqlCommand.Dispose(); }
    }
    $SqlCommand = $SqlConnection.CreateCommand();
    try {
        $SqlCommand.CommandText = @'
CREATE TABLE dbo.UserCredential
(
    AccountID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    PwHash NCHAR(96) NOT NULL,
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_UserCredential PRIMARY KEY CLUSTERED
    (
        AccountID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
'@;
        $SqlCOmmand.ExecuteNonQuery();
    } finally { $SqlCommand.Dispose(); }
    $SqlCommand = $SqlConnection.CreateCommand();
    try {
        $PwHash = [FsInfoCat.Util.PwHash]::Create($AppCredentials.GetNetworkCredential().Password);
        $SqlCommand.CommandText = @"
INSERT INTO dbo.UserCredential (AccountID, PwHash, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
    Values ('00000000-0000-0000-0000-000000000000', '$PwHash',
        @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000')
"@;
        $SqlCOmmand.ExecuteNonQuery();
    } finally { $SqlCommand.Dispose(); }
    $SqlCommand = $SqlConnection.CreateCommand();
    try {
        $SqlCommand.CommandText = @'
CREATE TABLE dbo.Account
(
    AccountID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    DisplayName NVARCHAR(128) NOT NULL DEFAULT '',
    LoginName NVARCHAR(32) NOT NULL,
    [Role] TINYINT NOT NULL,
    Notes NTEXT NOT NULL DEFAULT '',
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_Account PRIMARY KEY CLUSTERED
    (
        AccountID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
'@;
        $SqlCOmmand.ExecuteNonQuery();
    } finally { $SqlCommand.Dispose(); }
    $SqlCommand = $SqlConnection.CreateCommand();
    try {
        $PwHash = [FsInfoCat.Util.PwHash]::Create($AppCredentials.GetNetworkCredential().Password);
        $SqlCommand.CommandText = @"
INSERT INTO dbo.Account (AccountID, DisplayName, LoginName, [Role], Notes, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
    Values ('00000000-0000-0000-0000-000000000000', 'FS InfoCat Administrator', '$($AppCredentials.UserName)', 4, '',
        @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000')
"@;
        $SqlCOmmand.ExecuteNonQuery();
    } finally { $SqlCommand.Dispose(); }
    @(
        @'
ALTER TABLE dbo.Account WITH CHECK ADD CONSTRAINT FK_Account_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.Account CHECK CONSTRAINT FK_Account_CreatedBy
'@, @'
ALTER TABLE dbo.Account WITH CHECK ADD CONSTRAINT FK_Account_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.Account CHECK CONSTRAINT FK_Account_ModifiedBy
'@, @'
CREATE UNIQUE INDEX IDX_Account_LoginName ON dbo.Account (LoginName)
'@, @'
ALTER TABLE dbo.UserCredential WITH CHECK ADD CONSTRAINT FK_UserCredential_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.UserCredential CHECK CONSTRAINT FK_UserCredential_CreatedBy
'@, @'
ALTER TABLE dbo.UserCredential WITH CHECK ADD CONSTRAINT FK_UserCredential_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.UserCredential CHECK CONSTRAINT FK_UserCredential_ModifiedBy
'@, @'
CREATE TABLE dbo.HostDevice
(
    HostDeviceID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    DisplayName NVARCHAR(128) NOT NULL DEFAULT '',
    MachineIdentifer NVARCHAR(128) NOT NULL,
    MachineName NVARCHAR(128) NOT NULL,
    IsWindows BIT NOT NULL DEFAULT 0,
    AllowCrawl BIT NOT NULL DEFAULT 0,
    IsInactive BIT NOT NULL DEFAULT 0,
    Notes NTEXT NOT NULL DEFAULT '',
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_HostDevice PRIMARY KEY CLUSTERED
    (
        HostDeviceID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    CONSTRAINT AK_HostDevice_MachineIdentifier UNIQUE(DisplayName),
    CONSTRAINT AK_HostDevice_MachineIdentifer UNIQUE(MachineIdentifer),
    CONSTRAINT AK_HostDevice_MachineName UNIQUE(MachineName)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
'@, @'
ALTER TABLE dbo.HostDevice WITH CHECK ADD CONSTRAINT FK_HostDevice_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.HostDevice CHECK CONSTRAINT FK_HostDevice_CreatedBy
'@, @'
ALTER TABLE dbo.HostDevice WITH CHECK ADD CONSTRAINT FK_HostDevice_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.HostDevice CHECK CONSTRAINT FK_HostDevice_ModifiedBy
'@, @'
CREATE UNIQUE INDEX IDX_HostDevice_MachineIdentifer ON dbo.HostDevice (MachineIdentifer)
'@, @'
'@, @'
CREATE TABLE dbo.HostContributor
(
    HostDeviceID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    AccountID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT PK_HostContributor PRIMARY KEY CLUSTERED
    (
        HostDeviceID ASC,
        AccountID
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    CONSTRAINT AK_HostContributor UNIQUE(HostDeviceID, AccountID)
) ON [PRIMARY]
'@, @'
ALTER TABLE dbo.HostContributor WITH CHECK ADD CONSTRAINT FK_HostContributor_Host FOREIGN KEY(HostDeviceID)
    REFERENCES dbo.HostDevice (HostDeviceID)
    ON DELETE CASCADE
'@, @'
ALTER TABLE dbo.HostContributor CHECK CONSTRAINT FK_HostContributor_Host
'@, @'
ALTER TABLE dbo.HostContributor WITH CHECK ADD CONSTRAINT FK_HostContributor_Account FOREIGN KEY(AccountID)
    REFERENCES dbo.Account (AccountID)
    ON DELETE CASCADE
'@, @'
ALTER TABLE dbo.HostContributor CHECK CONSTRAINT FK_HostContributor_Account
'@, @'
ALTER TABLE dbo.HostContributor WITH CHECK ADD CONSTRAINT FK_HostContributor_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.HostContributor CHECK CONSTRAINT FK_HostContributor_CreatedBy
'@, @'
CREATE TABLE dbo.Volume
(
    VolumeID UNIQUEIDENTIFIER NOT NULL, -- primary key column
    HostDeviceID UNIQUEIDENTIFIER NULL,
    CreatedOn DATETIME NOT NULL,
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    ModifiedOn DATETIME NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER NOT NULL,
    DisplayName NVARCHAR(128) NOT NULL DEFAULT '',
    RootPathName NVARCHAR(1024) NOT NULL,
    FileSystemName NVARCHAR(256) NOT NULL,
    VolumeName NVARCHAR(128) NOT NULL,
    SerialNumber BIGINT NOT NULL,
    MaxNameLength BIGINT NOT NULL,
    Flags INT NOT NULL,
    IsInactive BIT NOT NULL DEFAULT 0,
    Notes NTEXT NOT NULL DEFAULT '',
    CONSTRAINT PK_Volume PRIMARY KEY CLUSTERED
    (
        VolumeID ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
'@, @'
ALTER TABLE dbo.Volume WITH CHECK ADD CONSTRAINT FK_Volume_Host FOREIGN KEY(HostDeviceID)
    REFERENCES dbo.HostDevice (HostDeviceID)
    ON DELETE CASCADE
'@, @'
ALTER TABLE dbo.Volume CHECK CONSTRAINT FK_Volume_Host
'@, @'
ALTER TABLE dbo.Volume WITH CHECK ADD CONSTRAINT FK_Volume_CreatedBy FOREIGN KEY(CreatedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.Volume CHECK CONSTRAINT FK_Volume_CreatedBy
'@, @'
ALTER TABLE dbo.Volume WITH CHECK ADD CONSTRAINT FK_Volume_ModifiedBy FOREIGN KEY(ModifiedBy)
    REFERENCES dbo.Account (AccountID)
    ON DELETE NO ACTION
'@, @'
ALTER TABLE dbo.Volume CHECK CONSTRAINT FK_Volume_ModifiedBy
'@, @'
CREATE UNIQUE INDEX IDX_Volume_SerialNumber ON dbo.Volume (SerialNumber)
'@, @'
INSERT INTO dbo.HostDevice (HostDeviceID, DisplayName, MachineIdentifer, MachineName, IsWindows, AllowCrawl, IsInactive, Notes,
        CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
    VALUES('1fd51535-1bd1-4ab2-acd9-3e152f1da4e2', '', '$($HostDeviceRegRequest.MachineIdentifer)', '$($HostDeviceRegRequest.MachineName)', 1, 1, 0, '',
        @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000')
'@, @'
'@, @'
'@) | ForEach-Object {
        $SqlCommand = $SqlConnection.CreateCommand();
        try {
            $SqlCommand.CommandText = $_;
            $SqlCOmmand.ExecuteNonQuery();
        } finally { $SqlCommand.Dispose(); }
    }
} finally { $SqlConnection.Dispose(); }