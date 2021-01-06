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

Function Get-InitializationQueries {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$RawAdminPwd
    )
    $PwHash = (ConvertTo-PasswordHash -RawPwd $RawAdminPwd).ToString();
    $HostDeviceID = [Guid]::NewGuid().ToString('d');
    $MachineIdentifier = [FsInfoCat.PsDesktop.FsInfoCatUtil]::GetLocalMachineSID();
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
