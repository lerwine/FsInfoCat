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

class FsLogicalVolume {
    [string]$RootPath;
    [string]$VolumeName;
    [string]$VolumeId;
    [string]$FsName;
    [System.IO.DriveType]$DriveType;
}

class Mounts {
    [string]$Device;
    [string]$MountPoint;
    [string]$FsType;
    [bool]$ReadOnly = $false;
    [bool]$ReadWrite = $false;
    [bool]$RelATime = $false;
    [bool]$NoExec = $false;
    [bool]$NoSuid = $false;
    [bool]$NoDev = $false;
    [bool]$Discard = $false;
    [bool]$AllowOther = $false;
    [bool]$NoForceUid = $false;
    [bool]$NoForceGid = $false;
    [bool]$NoUnix = $false;
    [bool]$ServerIno = $false;
    [bool]$MapPosix = $false;
    [System.Collections.Generic.Dictionary[string,string]]$Options = [System.Collections.Generic.Dictionary[string,string]]::new();
}

[string[]]$FileLines = [System.IO.File]::ReadAllLines('/proc/mounts');
$Mounts = @($FileLines | ForEach-Object {
    if (-not [string]::IsNullOrWhiteSpace($_)) {
        [string[]]$Cells = $_ -split '\s';
        $m = [Mounts]::new();
        $m.Device = $Cells[0];
        $m.MountPoint = $Cells[1];
        $m.FsType = $Cells[2];
        if (-not [string]::IsNullOrWhiteSpace($Cells[3])) {
            [string[]]$Cells = $Cells[3].Split(',');
            foreach ($c in $Cells) {
                switch ($c) {
                    'ro' { $m.ReadOnly = $true; break; }
                    'rw' { $m.ReadOnly = $true; break; }
                    'relatime' { $m.ReadOnly = $true; break; }
                    'nosuid' { $m.ReadOnly = $true; break; }
                    'noexec' { $m.NoExec = $true; break; }
                    'nodev' { $m.NoDev = $true; break; }
                    'discard' { $m.Discard = $true; break; }
                    'allow_other' { $m.AllowOther = $true; break; }
                    'noforceuid' { $m.NoForceUid = $true; break; }
                    'noforcegid' { $m.NoForceGid = $true; break; }
                    'nounix' { $m.NoUnix = $true; break; }
                    'serverino' { $m.ServerIno = $true; break; }
                    'mapposix' { $m.MapPosix = $true; break; }
                    default {
                        $i = $c.IndexOf('=');
                        if ($i -lt 0) {
                            if (-not $m.Options.ContainsKey($c)) {
                                $m.Options.Add($c, $null);
                            }
                        } else {
                            $k = $c.Substring(0, $i);
                            if (-not $m.Options.ContainsKey($k)) {
                                $m.Options.Add($k, $c.Substring($i + 1));
                            }
                        }
                        break;
                    }
                }
            }
        }
        $m | Write-Output;
    }
});
$Mounts


class MountInfo {
    [string]$MountId;
    [string]$DeviceId;
    [string]$Major;
    [string]$Minor;
    [string]$FsRoot;
    [string]$MountPoint;
    [string]$Unknown;
    [string]$FsType;
    [string]$Device;
}

[string[]]$FileLines = [System.IO.File]::ReadAllLines('/proc/mountinfo');
$MountInfo = @($FileLines | ForEach-Object {
    if (-not [string]::IsNullOrWhiteSpace($_)) {
        $m = [MountInfo]::new();
        if ($_ -match '^([^\s-]\S*(?:\s[^\s-]\S*)+)\s-\s(\S.+)$') {
            [string[]]$Cells = $Matches[1] -split '\s';
            $m.MountId = $Cells[0];
            $m.DeviceId = $Cells[1];
            ($major, $minor) = $Cells[2].Split(':', 2);
            $m.Major = $major;
            $m.Minor = $minor;
            $m.FsRoot = $Cells[3];
            $m.MountPoint = $Cells[4];
            $m.Unknown = $Cells[6];
            [string[]]$Cells = $Matches[2] -split '\s';
            $m.FsType = $Cells[0];
            $m.Device = $Cells[1];
        }
        $m | Write-Output;
    }
});
$MountInfo

Get-Command -Name 'lsblk';

if ([System.Environment]::OSVersion.Platform -eq [System.PlatformID]::Win32NT) {
    Function Get-FsLogicalVolume {
        [CmdletBinding()]
        [OutputType([FsLogicalVolume])]
        Param()
        
        ((Get-CimInstance -ClassName 'CIM_LogicalDisk') | ForEach-Object {
            $Directory = $_ | Get-CimAssociatedInstance -ResultClassName 'CIM_Directory';
            if ($null -ne $Directory -and -not [string]::IsNullOrWhiteSpace($Directory.Name)) {
                $DriveType = [System.IO.DriveType]::Unknown;
                try {
                    $DriveType = ([System.IO.DriveType]($_.DriveType));
                } catch { }
                [FsLogicalVolume]@{
                    RootPath = $Directory.Name;
                    VolumeName = $_.VolumeName;
                    VolumeId = $_.VolumeSerialNumber;
                    FsName = $_.FileSystem;
                    DriveType = $DriveType;
                } | Write-Output;
            }
        });
    }
} else {
    $Script:IgnoreFsType = @('sysfs', 'proc', 'devtmpfs', 'devpts', 'tmpfs', 'cgroup', 'cgroup2', 'pstore', 'autofs', 'mqueue',
        'debugfs', 'hugetlbfs', 'fusectl', 'configfs', 'squashfs', 'fuse', 'binfmt_misc');

    Function Get-FsLogicalVolume {
        [CmdletBinding()]
        [OutputType([FsLogicalVolume])]
        Param()
        
        $MountInfo = [MountInfo]::Load('/proc/mount');
        [System.IO.File]::ReadAllLines('/proc/mounts') | ForEach-Object { $Values = $_ -split '\s'; $Values[2] }
        @((
            #(lsblk -a -b -f -J -l -o LABEL,MOUNTPOINT,FSTYPE,UUID,TYPE,RO,RM,HOTPLUG) | Out-String | ConvertFrom-Json

            # TODO: Can add RO or RM?
            (findmnt -A -b -J -l -R -o SOURCE,TARGET,FSTYPE,OPTIONS,LABEL,UUID) | Out-String | ConvertFrom-Json
        ).blockDevices | Where-Object {
            $t = '' + $_.fstype;
            $i = $t.IndexOf('.');
            if ($i -lt 0) {
                $Script:IgnoreFsType -notcontains $t
            } else {
                $Script:IgnoreFsType -notcontains $t.Substring(0, $i);
            }
        }) | ForEach-Object {
            $DriveType = [System.IO.DriveType]::Unknown;
            switch ($_.fstype) {
                'cifs' {
                    $DriveType = [System.IO.DriveType]::Network;
                    break;
                }
                'smbfs' {
                    $DriveType = [System.IO.DriveType]::Network;
                    break;
                }
                'nfs' {
                    $DriveType = [System.IO.DriveType]::Network;
                    break;
                }
                'ntfs' {
                    $DriveType = [System.IO.DriveType]::Network;
                    break;
                }
                default {
                    if ($_.hotplug -ne '0') {
                        $DriveType = [System.IO.DriveType]::Removable;
                    } else {
                        if ($_.rm -ne '0') {
                            if ($_.ro -ne '0' -or $_.fstype -eq 'iso9660') {
                                $DriveType = [System.IO.DriveType]::CDRom;
                            } else {
                                $DriveType = [System.IO.DriveType]::Removable;
                            }
                        } else {
                            #if ($_.type -eq 'part') {
                            #}
                            $DriveType = [System.IO.DriveType]::Fixed;
                        }
                    }
                    break;
                }
            }
            [FsLogicalVolume]@{
                RootPath = $_.mountpoint;
                VolumeName = $_.label;
                VolumeId = $_.uuid;
                FsName = $_.fstype;
                DriveType = $DriveType;
            } | Write-Output;
        }
    }
}

enum FsType {
adfs; affs; autofs; coda; coherent; cramfs; devpts; efs; ext2; ext3; hfs; hpfs; iso9660; jfs; minix; msdos; ncpfs; nfs; ntfs; proc; qnx4; reiserfs;
romfs; smbfs; sysv; tmpfs; udf; ufs; umsdos; vfat; xenix; xfs
}
<#
name       : sdb1
label      : 
mountpoint : /mnt
fstype     : ext4
uuid       : 3028dce3-2601-4cde-9774-f955c8bd0fc7
type       : part
ro         : 0
rm         : 0
hotplug    : 0


findmnt -A -b -J -l -R -o SOURCE,TARGET,FSTYPE,OPTIONS,LABEL,UUID,MAJ:MIN



sudo mkdir /mnt/testazureshare
if [ ! -d "/etc/smbcredentials" ]; then
sudo mkdir /etc/smbcredentials
fi
if [ ! -f "/etc/smbcredentials/servicenowdiag479.cred" ]; then
    sudo bash -c 'echo "username=servicenowdiag479" >> /etc/smbcredentials/servicenowdiag479.cred'
    sudo bash -c 'echo "password=jFpbf9ilT+uDN1sQYY6ClGXzrX7xjFwSd8nmg1AIMCA7AzDadASW51CBKVfcpivqf0cvFP7Yjq0ER/fyxZ25KQ==" >> /etc/smbcredentials/servicenowdiag479.cred'
fi
sudo chmod 600 /etc/smbcredentials/servicenowdiag479.cred

sudo bash -c 'echo "//servicenowdiag479.file.core.windows.net/testazureshare /mnt/testazureshare cifs nofail,vers=3.0,credentials=/etc/smbcredentials/servicenowdiag479.cred,dir_mode=0777,file_mode=0777,serverino" >> /etc/fstab'
sudo mount -t cifs //servicenowdiag479.file.core.windows.net/testazureshare /mnt/testazureshare -o vers=3.0,credentials=/etc/smbcredentials/servicenowdiag479.cred,dir_mode=0777,file_mode=0777,serverino

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
#>
Function ConvertTo-FsVolumeInfo {
    [CmdletBinding()]
    [OutputType([FsInfoCat.Models.Crawl.FsRoot])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [FsLogicalVolume]$LogicalVolume,

        [switch]$Force
    )

    Process {
        $IsValid = $true;
        $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
        $FsRoot.DriveFormat = $LogicalVolume.FsName;
        if ([string]::IsNullOrWhiteSpace($FsRoot.DriveFormat)) {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlWarning]::new("Drive format was not specified",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Drive format was not specified" -Category InvalidResult -ErrorId 'NoDriveFormat' -TargetObject $LogicalVolume;
            }
        }
        $FsRoot.VolumeName = $LogicalVolume.VolumeName;
        if ([string]::IsNullOrWhiteSpace($FsRoot.VolumeName)) { $FsRoot.VolumeName = ''; }
        $VolumeIdentifier = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new([Guid]::Empty);
        if (-not [FsInfoCat.Models.Volumes.VolumeIdentifier]::TryCreate($LogicalVolume.VolumeId, [ref]$VolumeIdentifier)) {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Unable to parse volume serial number",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Unable to parse volume serial number" -Category InvalidResult -ErrorId 'InvalidSerialNumber' -TargetObject $LogicalVolume;
            }
        }
        $FsRoot.Identifier = $VolumeIdentifier;
        try {
            [System.IO.DriveType]$FsRoot.DriveType = $LogicalVolume.DriveType;
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
                    Write-Error -Exception $_ -Message "Unable to discern drive type" -Category InvalidResult -ErrorId 'InvalidDriveType' -TargetObject $LogicalVolume;
                } else {
                    if ($_ -is [System.Management.Automation.ErrorRecord]) {
                        Write-Error -ErrorRecord $_ -CategoryReason "Invalid Drive Type";
                    } else {
                        Write-Error -Message "Unable to discern drive type: $_" -Category InvalidResult -ErrorId 'InvalidDriveType' -TargetObject $LogicalVolume;
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
                Write-Error -Message "Incompatible drive type (no root directory)" -Category InvalidResult -ErrorId 'NoRootDirectory' -TargetObject $LogicalVolume;
            }
        } else {
            if ([string]::IsNullOrWhiteSpace($LogicalVolume.RootPath)) {
                if ($Force.IsPresent) {
                    $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Could not determine root directory",
                        [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                } else {
                    $IsValid = $false;
                    Write-Error -Message "Could not determine root directory" -Category InvalidResult -ErrorId 'NoRootDirectory' -TargetObject $LogicalVolume;
                }
            } else {
                $FsRoot.RootPathName = $LogicalVolume.RootPath;
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
            }
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
        if ($PSBoundParameters.ContainsKey('Path')) {
            if ($Force.IsPresent) {
                $Path | ForEach-Object {
                    $dr = $PathRoot = $null;
                    $p = $_;
                    try {
                        # TODO: Create Get-FsPathRoot function
                        $PathRoot = [System.IO.Path]::GetFullPath($p) | Get-FsPathRoot;
                        $dr = $BlockDevices | Where-Object { $PathRoot -ceq $_.mountpoint } | Select-Object -First 1;
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
                        # TODO: Modify ConvertTo-FsVolumeInfo for linux to accept JSON ojbect. Maybe create a custom Select-Object?
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
                        $PathRoot = [System.IO.Path]::GetFullPath($p) | Get-FsPathRoot;
                        $dr = $BlockDevices | Where-Object { $PathRoot -ceq $_.mountpoint } | Select-Object -First 1;
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
                ($BlockDevices | ConvertTo-FsVolumeInfo -Force) | Write-Output;
            } else {
                ($BlockDevices | ConvertTo-FsVolumeInfo) | Write-Output;
            }
        }
        <#
        ((Get-PSDrive -PSProvider FileSystem) | Select-Object -Property 'Name', 'Root', 'VolumeSeparatedByColon', 'Description') | Out-GridView;
        ([System.IO.DriveInfo]::GetDrives() | Where-Object { $_.DriveType -ne [System.IO.DriveType]::Ram } | Select-Object -Property 'DriveFormat', 'DriveType', 'Name', 'RootDirectory', 'VolumeLabel') | Out-GridView -Title 'DriveInfo'
        ((lsblk -a -b -f -I 8 -J -l -o NAME,LABEL,MOUNTPOINT,FSTYPE,UUID,TYPE,PARTLABEL) | Out-String | ConvertFrom-Json).blockDevices | Out-GridView -Title 'lsblk'
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
