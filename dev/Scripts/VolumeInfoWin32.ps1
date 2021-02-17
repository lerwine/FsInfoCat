Import-Module -Name 'C:\Users\lerwi\Git\FsInfoCat\Setup\bin\FsInfoCat' -ErrorAction Stop;
Import-Module -Name 'Microsoft.PowerShell.Management' -ErrorAction Stop;
#$LogicalDisks = @(Get-CimInstance -ClassName 'Win32_LogicalDisk');
$CIM_LogicalDisk = @(Get-CimInstance -ClassName 'CIM_LogicalDisk');
<#
@(Get-CimInstance -ClassName 'CIM_LogicalDisk') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_LogicalDisk $_";
}
@(Get-CimInstance -ClassName 'CIM_Mount') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_Mount $_";
}
@(Get-CimInstance -ClassName 'CIM_NFS') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_NFS $_";
}
@(Get-CimInstance -ClassName 'CIM_ComputerSystem') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_ComputerSystem $_";
}
@(Get-CimInstance -ClassName 'CIM_OperatingSystem') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_OperatingSystem $_";
}
@(Get-CimInstance -ClassName 'CIM_System') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_System $_";
}
@(Get-CimInstance -ClassName 'CIM_VolumeSet') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_VolumeSet $_";
}
@(Get-CimInstance -ClassName 'CIM_InstalledOS') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_InstalledOS $_";
}
@(Get-CimInstance -ClassName 'CIM_LogicalIdentity') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "CIM_LogicalIdentity $_";
}
@(Get-CimInstance -ClassName 'Win32_SystemAccount') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "Win32_SystemAccount $_";
}
Get-CimInstance -Query 'SELECT * from Win32_UserAccount WHERE Name="Administrator"';

@(Get-CimInstance -ClassName 'Win32_UserAccount') | ForEach-Object {
    @($_.CimInstanceProperties) | Out-GridView -Title "Win32_UserAccount $_";
}
$LogicalDisks | Out-GridView -Title $LogicalDisks[0].ClassPath;
$LogicalDisks | ForEach-Object { ($_.Properties | Select-Object -Property 'IsArray', 'Name', 'Origin', 'Type', 'Value') | Out-GridView -Title "($($_.__PATH)).Properties" }
$LogicalDisks | ForEach-Object { ($_.GetRelationships() | Select-Object -Property 'GroupComponent', 'PartComponent') | Out-GridView -Title "($($_.__PATH)).Relationships" }
($LogicalDisks.Properties  | Select-Object -First 1) | Get-Member
#>

@((Get-CimInstance -Query 'SELECT * FROM CIM_Directory WHERE Name="c:\\"').CimInstanceProperties) | Out-GridView -Title "CIM_Directory";

$LogicalDisks | ForEach-Object {
    $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
    $FsRoot.DriveFormat = $_.FileSystem;
    if ([string]::IsNullOrWhiteSpace($FsRoot.DriveFormat)) {
        $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlWarning]::new("Drive format was not specified",
            [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
    }
    $FsRoot.VolumeName = $_.VolumeName;
    if ([string]::IsNullOrWhiteSpace($FsRoot.VolumeName)) { $FsRoot.VolumeName = ''; }
    [System.UInt32]$vsn = 0;
    if ([System.UInt32]::TryParse($_.VolumeSerialNumber, [ref]$vsn)) {
        $FsRootIdentifier = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new($vsn);
    } else {
        $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Unable to parse volume serial number",
            [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
    }
    try {
        [System.IO.DriveType]$FsRoot.DriveType = $_.DriveType
    } catch {
        if ($_ -is [System.Exception]) {
            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_, [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
        }
    }
    if ($FsRoot.DriveType -eq [System.IO.DriveType]::NoRootDirectory) {
        $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Incompatible drive type (no root directory)",
            [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
    } else {
        $CIM_Directory = $_ | Get-CimAssociatedInstance -ResultClassName 'CIM_Directory';
        if ($null -eq $CIM_Directory -or [string]::IsNullOrWhiteSpace($CIM_Directory.Name)) {
            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Could not determine root directory",
                [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
        } else {
            $FsRoot.RootPathName = $CIM_Directory.Name;
        }
    }
    $FsRoot | Write-Output;
}
<#
 @($ld.GetRelated('Win32_Directory')) | ForEach-Object { ($_.Properties | Select-Object -Property 'IsArray', 'Name', 'Origin', 'Type', 'Value') | Out-GridView -Title "($($_.__PATH)).Properties" }

$R = @($LogicalDisks[0].GetRelationships());


$LogicalDisks | ForEach-Object {
    $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
    $FsRoot.CaseSensitive = flags.HasFlag(FsInfoCat.FileSystemFeature.CasePreservedNames);
    $FsRoot.DriveFormat = $_.FileSystem;
    [System.UInt32]$vsn = 0;
    if ([System.UInt32]::TryParse($ld.VolumeSerialNumber, [ref]$vsn)) {
        $FsRootIdentifier = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new($vsn);
    } else {
        $FsRoot.Errors.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Unable to parse volume serial number"));
    }
    $Win32_Directory = $_.GetRelated('Win32_Directory');
    if ($null -ne $Win32_Directory) { $FsRoot.RootPathName = $Win32_Directory.Name }
    $FsRoot.VolumeName = $_.VolumeName;


[System.IO.DriveType]$t = [System.IO.DriveType]::Unknown;
$t
$t.ToString('f')
[System.IO.DriveType]::GetValues([System.IO.DriveType]) | Where-Object {
    [int]$I = $_;
    $LogicalDisks[0].DriveType -eq 
}
#>
# \\DESKTOP-G10FC12\root\cimv2:Win32_ComputerSystem.Name="DESKTOP-G10FC12"	\\DESKTOP-G10FC12\root\cimv2:Win32_LogicalDisk.DeviceID="C:"	
