if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Win32NT) {
    Add-Type -AssemblyName 'System.DirectoryServices' -ErrorAction Stop;
    Import-Module -Name 'Microsoft.PowerShell.Management' -ErrorAction Stop;
}

Function ConvertTo-PasswordHash {
    <#
    .SYNOPSIS
        Creates a password hash code.

    .DESCRIPTION
        Creates an object that contains a SHA512 password hash.

    .EXAMPLE
        PS C:\> $PwHash = ConvertTo-PasswordHash -RawPwd 'myPassword';
        Generates a cryptographically random salt and hashes the raw password.

    .INPUTS
        Raw password to be converted to a SHA512 hash.

    .OUTPUTS
        A FsInfoCat.Util.PwHash object representing the password hash.
    #>
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
    <#
    .SYNOPSIS
        Checks to see if a raw password matches a password hash.

    .DESCRIPTION
        Tests whether the provided raw password is a match of the same password that was used to create the SHA512 password hash.

    .EXAMPLE
        PS C:\> $Success = $myPw | Test-PasswordHash -PwHash $PwHash;

    .INPUTS
        The raw password to test.

    .OUTPUTS
        true if the raw password is a match; otherwise, false.
    #>
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

Function Get-FsVolumeInformation {
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
    Param(
        # Specifies one or more paths to file system volumes. Wildcards are permitted.
        [string[]]$Path,
        
        # Specifies one or more literal paths to file system volumes (no wildcards).
        [string[]]Literal$Path
    )

}
