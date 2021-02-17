if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Win32NT) {
    #Add-Type -AssemblyName 'System.DirectoryServices' -ErrorAction Stop;
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
        $CimInstance = Get-CimInstance -Query 'SELECT * from Win32_UserAccount WHERE Name="Administrator"';
        if ($null -eq $CimInstance) {
            Write-Warning -Message 'Unable to get principal object';
        } else {
            if ([string]::IsNullOrWhiteSpace($CimInstance.SID)) {
                Write-Warning -Message 'Principal object does not have a security identifier';
            } else {
                try {
                    [System.Security.Principal.SecurityIdentifier]::new($CimInstance.SID).AccountDomainSid.ToString() | Write-Output;
                } catch {
                    Write-Warning -Message "Failed to parse machine SID: $_";
                }
            }
        }
        <#
        $DirectoryEntry = New-Object -TypeName 'System.DirectoryServices.DirectoryEntry' -ArgumentList "WinNT://$([Environment]::MachineName)/Administrator";
        try {
            [byte[]]$Bytes = $DirectoryEntry.InvokeGet("objectSID");
            $Sid = [System.Security.Principal.SecurityIdentifier]::new($Bytes, 0);
            $Sid.AccountDomainSid.ToString() | Write-Output;
        } finally {
            $DirectoryEntry.Dispose();
        }
        #>
    } else {
        $id = '' + (Get-Content -LiteralPath 'etc/machine-id' -Force);
        if ($id.Length -gt 0) {
            $id | Write-Output;
        } else {
            Write-Error -Message 'Unable to get machine id' -Category ObjectNotFound -ErrorId 'EtcMachineIdReadError' -CategoryReason 'Unable to read from etc/machine-id';
        }
    }
}

Function ConvertTo-FsVolumeInfo {
    [CmdletBinding()]
    [OutputType([FsInfoCat.Models.Crawl.FsRoot])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [Microsoft.Management.Infrastructure.CimInstance]$CimInstance,

        [switch]$Force
    )

    Process {
        $IsValid = $true;
        $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
        $FsRoot.DriveFormat = $CimInstance.FileSystem;
        if ([string]::IsNullOrWhiteSpace($FsRoot.DriveFormat)) {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlWarning]::new("Drive format was not specified",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Drive format was not specified" -Category InvalidResult -ErrorId 'NoDriveFormat' -TargetObject $CimInstance;
            }
        }
        $FsRoot.VolumeName = $CimInstance.VolumeName;
        if ([string]::IsNullOrWhiteSpace($FsRoot.VolumeName)) { $FsRoot.VolumeName = ''; }
        [System.UInt32]$vsn = 0;
        if ([System.UInt32]::TryParse($CimInstance.VolumeSerialNumber, [ref]$vsn)) {
            $FsRootIdentifier = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new($vsn);
        } else {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Unable to parse volume serial number",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Unable to parse volume serial number" -Category InvalidResult -ErrorId 'InvalidSerialNumber' -TargetObject $CimInstance;
            }
        }
        try {
            [System.IO.DriveType]$FsRoot.DriveType = $CimInstance.DriveType
        } catch {
            if ($Force.IsPresent) {
                if ($_ -is [System.Exception]) {
                    $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_, [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                } else {
                    if ($_.Exception -is [System.Exception]) {
                        $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_.Exception, [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                    } else {
                        $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new('' + $_, [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                    }
                }
            } else {
                $IsValid = $false;
                if ($_ -is [System.Exception]) {
                    Write-Error -Exception $_ -Message "Unable to discern drive type" -Category InvalidResult -ErrorId 'InvalidDriveType' -TargetObject $CimInstance;
                } else {
                    if ($_ -is [System.Management.Automation.ErrorRecord]) {
                        Write-Error -ErrorRecord $_ -CategoryReason "Invalid Drive Type";
                    } else {
                        Write-Error -Message "Unable to discern drive type: $_" -Category InvalidResult -ErrorId 'InvalidDriveType' -TargetObject $CimInstance;
                    }
                }
            }
        }
        if ($FsRoot.DriveType -eq [System.IO.DriveType]::NoRootDirectory) {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Incompatible drive type (no root directory)",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Incompatible drive type (no root directory)" -Category InvalidResult -ErrorId 'NoRootDirectory' -TargetObject $CimInstance;
            }
        } else {
            $CIM_Directory = $CimInstance | Get-CimAssociatedInstance -ResultClassName 'CIM_Directory';
            if ($null -eq $CIM_Directory -or [string]::IsNullOrWhiteSpace($CIM_Directory.Name)) {
                if ($Force.IsPresent) {
                    $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Could not determine root directory",
                        [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                } else {
                    $IsValid = $false;
                    Write-Error -Message "Could not determine root directory" -Category InvalidResult -ErrorId 'NoRootDirectory' -TargetObject $CimInstance;
                }
            } else {
                $FsRoot.RootPathName = $CIM_Directory.Name;
            }
        }
        if ($IsValid) { $FsRoot | Write-Output }
    }
}

Function Get-FsVolumeInfo {
    <#
    .SYNOPSIS
        Get filesystem volume information.

    .DESCRIPTION
        Gets information about system volumes.

    .EXAMPLE
        PS C:\> $VolumeInfo = Get-FsVolumeInfo -Path 'D:\';

    .OUTPUTS
        VolumeInfo object(s) that contain information about file system volumes
    #>
    [CmdletBinding()]
    [OutputType([FsInfoCat.Models.Crawl.FsRoot])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [string[]]$Path,

        [switch]$Force
    )

    if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Win32NT) {
        $LogicalDiskAndRoot = @($LogicalDiskCollection | ForEach-Object {
            $Directory = $_ | Get-CimAssociatedInstance -ResultClassName 'CIM_Directory';
            if ($null -ne $Directory -and -not [string]::IsNullOrWhiteSpace($Directory.Name)) {
            [PsCustomObject]@{
                RootPath = $Directory.Name;
                LogicalDisk = $_;
            };
        });
        if ($PSBoundParameters.ContainsKey('Path')) {
            if ($Force.IsPresent) {
                $Path | ForEach-Object {
                    $dr = $PathRoot = $null;
                    $p = $_;
                    try {
                        $PathRoot = [System.IO.Path]::GetPathRoot([System.IO.Path]::GetFullPath($p));
                        $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ceq $_.RootPath } | Select-Object -First 1;
                        if ($null -eq $dr) {
                            $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ieq $_.RootPath } | Select-Object -First 1;
                        }
                    } catch {
                        $dr = $PathRoot = $null;
                        $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
                        $FsRoot.RootPathName = $p;
                        if ($_ -is [System.Exception]) {
                            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                        } else {
                            if ($_.Exception -is [System.Exception]) {
                                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_.Exception, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                            } else {
                                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new('' + $_, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                            }
                        }
                        $FsRoot | Write-Output;
                    }
                    if ($null -ne $dr) {
                        ($dr | ConvertTo-FsVolumeInfo -Force) | Write-Output;
                    } else {
                        if ($null -ne $PathRoot) {
                            $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
                            $FsRoot.RootPathName = $PathRoot;
                            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("No matching volume found",
                                [FsInfoCat.Models.Crawl.MessageId]::PathNotFound));
                            $FsRoot | Write-Output;
                        }
                    }
                }
            } else {
                $Path | ForEach-Object {
                    $dr = $null;
                    $p = $_;
                    try {
                        $PathRoot = [System.IO.Path]::GetPathRoot([System.IO.Path]::GetFullPath($p));
                        $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ceq $_.RootPath } | Select-Object -First 1;
                        if ($null -eq $dr) {
                            $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ieq $_.RootPath } | Select-Object -First 1;
                        }
                    } catch {
                        if ($_ -is [System.Exception]) {
                            Write-Error -Exception $_ -Message "Unable to determine root path" -Category InvalidArgument -ErrorId 'InvalidRootPath' -TargetObject $p;
                        } else {
                            if ($_ -is [System.Management.Automation.ErrorRecord]) {
                                Write-Error -ErrorRecord $_ -CategoryReason "Invalid Root Path";
                            } else {
                                Write-Error -Message "Unable to discern drive type: $_" -Category InvalidArgument -ErrorId 'InvalidRootPath' -TargetObject $p;
                            }
                        }
                        $dr = $null;
                    }
                    if ($null -ne $dr) {
                        ($dr | ConvertTo-FsVolumeInfo) | Write-Output;
                    }
                }
            }
        } else {
            if ($Force.IsPresent) {
                ((Get-CimInstance -ClassName 'CIM_LogicalDisk') | ConvertTo-FsVolumeInfo -Force) | Write-Output;
            } else {
                ((Get-CimInstance -ClassName 'CIM_LogicalDisk') | ConvertTo-FsVolumeInfo) | Write-Output;
            }
        }
    } else {
        [System.IO.DriveInfo[]]$Drives = [System.IO.DriveInfo]::GetDrives();
        $BlockDevices = (
            (lsblk -a -b -f -J -l -o NAME,LABEL,MOUNTPOINT,SIZE,FSTYPE,UUID,SERIAL,TYPE) | Out-String | ConvertFrom-Json
        ).BlockDevices | Where-Object { $_.type -eq 'part' }

        ((lsblk -a -b -f -J -l -o NAME,LABEL,MOUNTPOINT,SIZE,FSTYPE,UUID,SERIAL,TYPE) | Out-String | ConvertFrom-Json).BlockDevices | Where-Object { $_.type -eq 'part' }
        <#
{
   "blockdevices": [
      {"name": "loop0", "label": null, "mountpoint": "/snap/code/55", "size": "157016064", "fstype": "squashfs", "uuid": null, "serial": null, "type": "loop"},
      {"name": "loop1", "label": null, "mountpoint": "/snap/core/10583", "size": "102637568", "fstype": "squashfs", "uuid": null, "serial": null, "type": "loop"},
      {"name": "loop2", "label": null, "mountpoint": "/snap/code/52", "size": "150798336", "fstype": "squashfs", "uuid": null, "serial": null, "type": "loop"},
      {"name": "loop3", "label": null, "mountpoint": "/snap/core/10823", "size": "103129088", "fstype": "squashfs", "uuid": null, "serial": null, "type": "loop"},
      {"name": "loop4", "label": null, "mountpoint": null, "size": null, "fstype": null, "uuid": null, "serial": null, "type": "loop"},
      {"name": "loop5", "label": null, "mountpoint": null, "size": null, "fstype": null, "uuid": null, "serial": null, "type": "loop"},
      {"name": "loop6", "label": null, "mountpoint": null, "size": null, "fstype": null, "uuid": null, "serial": null, "type": "loop"},
      {"name": "loop7", "label": null, "mountpoint": null, "size": null, "fstype": null, "uuid": null, "serial": null, "type": "loop"},
      {"name": "sda", "label": null, "mountpoint": null, "size": "32213303296", "fstype": null, "uuid": null, "serial": "60022480723f03e4f52d65852314f69b", "type": "disk"},
      {"name": "sda1", "label": "cloudimg-rootfs", "mountpoint": "/", "size": "32096894464", "fstype": "ext4", "uuid": "3756934c-31d3-413c-8df9-5b7c7b1a4451", "serial": null, "type": "part"},
      {"name": "sda14", "label": null, "mountpoint": null, "size": "4194304", "fstype": null, "uuid": null, "serial": null, "type": "part"},
      {"name": "sda15", "label": "UEFI", "mountpoint": "/boot/efi", "size": "111149056", "fstype": "vfat", "uuid": "B38E-A2BF", "serial": null, "type": "part"},
      {"name": "sdb", "label": null, "mountpoint": null, "size": "8589934592", "fstype": null, "uuid": null, "serial": "60022480bf8b95f23da9e9c454906355", "type": "disk"},
      {"name": "sdb1", "label": null, "mountpoint": "/mnt", "size": "8587837440", "fstype": "ext4", "uuid": "3028dce3-2601-4cde-9774-f955c8bd0fc7", "serial": null, "type": "part"}
   ]
}
   ]
}

        #>
    }
}
