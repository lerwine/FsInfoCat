if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Win32NT) {
    Add-Type -AssemblyName 'System.DirectoryServices' -ErrorAction Stop;
}

Function ConvertTo-PasswordHash {
    [CmdletBinding()]
    [OutputType([FsInfoCat.Util.PwHash])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString]
        [AllowNull]
        [string]$RawPwd,
        [System.UInt64]$Salt
    )
    Process {
        if ($PSBoundParameters.ContainsKey('Salt')) {
            [FsInfoCat.Util.PwHash]::Create($RawPwd, $Salt);
        } else {
            [FsInfoCat.Util.PwHash]::Create($RawPwd);
        }
    }
}

Function Test-PasswordHash {
    [CmdletBinding(DefaultParameterSetName = "String")]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = "String")]
        [AllowEmptyString]
        [AllowNull]
        [string]$RawPwd,
        [Parameter(Mandatory = $true, ParameterSetName = "PwHash")]
        [AllowNull]
        [Nullable[FsInfoCat.Util.PwHash]]$PwHash,
        [Parameter(Mandatory = $true, ParameterSetName = "String")]
        [AllowEmptyString]
        [AllowNull]
        [string]$EncodedPwHash
    )

    Begin {
        $PwHashObj = $null;
        if ($PSBoundParameters.ContainsKey('String')) {
            $PwHashObj = [FsInfoCat.Util.PwHash]::Import($EncodedPwHash);
        } else {
            $PwHashObj = $PwHash;
        }
    }
    Process {
        if ([string]::IsNullOrEmpty($RawPwd)) {
            ($null -eq $PwHashObj) | Write-Output;
        } else {
            ($null -ne $PwHashObj -and $PwHashObj.Test($RawPwd)) | Write-Output;
        }
    }
}

Function Get-LocalMachineIdentifier {
    <#
    .SYNOPSIS
        Get unique identifier for local machine.

    .DESCRIPTION
        Gets the SID or UUID that will be used as the unique identifier of the current host machine.

    .EXAMPLE
        PS C:\> $id = Get-LocalMachineIdentifier

    .OUTPUTS
        The string value of the SID or UUID that uniquely identifies the current host machine.
    #>
    [CmdletBinding()]
    Param()

    if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Win32NT) {
        $DirectoryEntry = New-Object -TypeName 'System.DirectoryServices.DirectoryEntry' -ArgumentList "WinNT://$([Environment]::MachineName)/Administrator";
        try {
            [byte[]]$Bytes = $DirectoryEntry.InvokeGet("objectSID");
            $Sid = [System.Security.Principal.SecurityIdentifier]::new($Bytes, 0);
            $Sid.AccountDomainSid.ToString() | Write-Output;
        } finally {
            $DirectoryEntry.Dispose();
        }
    } else {
        $id = '' + (Get-Content -LiteralPath 'etc/machine-id' -Force);
        if ($id.Length -gt 0) {
            $id | Write-Output;
        } else {
            Write-Error -Message 'Unable to get machine id' -Category ObjectNotFound -ErrorId 'EtcMachineIdReadError' -CategoryReason 'Unable to read from etc/machine-id';
        }
    }
}

Function Get-InitializationQueries {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$RawAdminPwd
    )
    $PwHash = (ConvertTo-PasswordHash -RawPwd $RawAdminPwd).ToString();
    $HostDeviceID = [Guid]::NewGuid().ToString('d');
    $MachineIdentifier = Get-LocalMachineSID;
    $MachineName = [System.Environment]::MachineName;
    @"
-- Insert this query immediately before the 'CREATE TABLE dbo.Account' statement
INSERT INTO dbo.UserCredential (AccountID, PwHash, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
    Values ('00000000-0000-0000-0000-000000000000', '$PwHash',
        @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000');

-- The next query should be executed at the end of the setup script.
INSERT INTO dbo.HostDevice (HostDeviceID, DisplayName, MachineIdentifer, MachineName, IsWindows, AllowCrawl, IsInactive, Notes,
        CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
    VALUES('$HostDeviceID'', '', '$MachineIdentifier', '$MachineName', 1, 1, 0, '',
        @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000');
"@ | Write-Host;
}
