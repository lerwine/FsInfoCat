class MountOptions {
    [bool]$AllowOther = $false;
    [bool]$Async = $false;
    [bool]$ATime = $false;
    [bool]$NoATime = $false;
    [bool]$Auto = $false;
    [bool]$NoAuto = $false;
    [bool]$Bind = $false;
    [bool]$Dev = $false;
    [bool]$NoDev = $false;
    [bool]$DirATime = $false;
    [bool]$NoDirATime = $false;
    [bool]$DirSync = $false;
    [bool]$Discard = $false;
    [bool]$Exec = $false;
    [bool]$NoExec = $false;
    [bool]$NoForceGID = $false;
    [bool]$NoForceUID = $false;
    [bool]$Group = $false;
    [bool]$IVersion = $false;
    [bool]$NoIVersion = $false;
    [bool]$LazyTime = $false;
    [bool]$NoLazyTime = $false;
    [bool]$Loud = $false;
    [bool]$Mand = $false;
    [bool]$NoMand = $false;
    [bool]$MapPosix = $false;
    [bool]$NetDev = $false;
    [bool]$NoNetDev = $false;
    [bool]$Owner = $false;
    [bool]$Private = $false;
    [bool]$RelATime = $false;
    [bool]$NoRelATime = $false;
    [bool]$Remount = $false;
    [bool]$ReadOnly = $false;
    [bool]$RPrivate = $false;
    [bool]$RShared = $false;
    [bool]$RSlave = $false;
    [bool]$RunBindable = $false;
    [bool]$ReadWrite = $false;
    [bool]$ServerINo = $false;
    [bool]$Shared = $false;
    [bool]$Silent = $false;
    [bool]$Slave = $false;
    [bool]$StrictATime = $false;
    [bool]$NoStrictATime = $false;
    [bool]$SUID = $false;
    [bool]$NoSUID = $false;
    [bool]$Sync = $false;
    [bool]$Unbindable = $false;
    [bool]$Unhide = $false;
    [bool]$NoUnix = $false;
    [bool]$User = $false;
    [bool]$NoUser = $false;
    [bool]$Users = $false;
    [System.Collections.ObjectModel.Collection[string]] $AllBinaryNames = [System.Collections.ObjectModel.Collection[string]]::new();
    [System.Collections.Generic.Dictionary[string, string]]$KeyValuePairs = [System.Collections.Generic.Dictionary[string, string]]::new();

    MountOptions([string]$csv) {
        $this.AllBinaryNames =
        if ([string]::IsNullOrWhiteSpace($csv)) { return }
        [string[]]$Cells = $csv.Split(',');
        foreach ($c in $Cells) {
            $i = $c.IndexOf('=');
            if ($i -lt 0) {
                if ($this.AllBinaryNames -cnotcontains $c) {
                    $this.AllBinaryNames.Add($c);
                }
                switch ($c) {
                    'allow_other' { $this.AllowOther = True; break; }
                    'async' { $this.Async = True; break; }
                    'atime' { $this.ATime = True; break; }
                    'noatime' { $this.NoATime = True; break; }
                    'auto' { $this.Auto = True; break; }
                    'noauto' { $this.NoAuto = True; break; }
                    'bind' { $this.Bind = True; break; }
                    'dev' { $this.Dev = True; break; }
                    'nodev' { $this.NoDev = True; break; }
                    'diratime' { $this.DirATime = True; break; }
                    'nodiratime' { $this.NoDirATime = True; break; }
                    'dirsync' { $this.DirSync = True; break; }
                    'discard' { $this.Discard = True; break; }
                    'exec' { $this.Exec = True; break; }
                    'noexec' { $this.NoExec = True; break; }
                    'noforcegid' { $this.NoForceGID = True; break; }
                    'noforceuid' { $this.NoForceUID = True; break; }
                    'group' { $this.Group = True; break; }
                    'iversion' { $this.IVersion = True; break; }
                    'noiversion' { $this.NoIVersion = True; break; }
                    'lazytime' { $this.LazyTime = True; break; }
                    'nolazytime' { $this.NoLazyTime = True; break; }
                    'loud' { $this.Loud = True; break; }
                    'mand' { $this.Mand = True; break; }
                    'nomand' { $this.NoMand = True; break; }
                    'mapposix' { $this.MapPosix = True; break; }
                    '_netdev' { $this.NetDev = True; break; }
                    'no_netdev' { $this.NoNetDev = True; break; }
                    'owner' { $this.Owner = True; break; }
                    'private' { $this.Private = True; break; }
                    'relatime' { $this.RelATime = True; break; }
                    'norelatime' { $this.NoRelATime = True; break; }
                    'remount' { $this.Remount = True; break; }
                    'ro' { $this.ReadOnly = True; break; }
                    'rprivate' { $this.RPrivate = True; break; }
                    'rshared' { $this.RShared = True; break; }
                    'rslave' { $this.RSlave = True; break; }
                    'runbindable' { $this.RunBindable = True; break; }
                    'rw' { $this.ReadWrite = True; break; }
                    'serverino' { $this.ServerINo = True; break; }
                    'shared' { $this.Shared = True; break; }
                    'silent' { $this.Silent = True; break; }
                    'slave' { $this.Slave = True; break; }
                    'strictatime' { $this.StrictATime = True; break; }
                    'nostrictatime' { $this.NoStrictATime = True; break; }
                    'suid' { $this.SUID = True; break; }
                    'nosuid' { $this.NoSUID = True; break; }
                    'sync' { $this.Sync = True; break; }
                    'unbindable' { $this.Unbindable = True; break; }
                    'unhide' { $this.Unhide = True; break; }
                    'nounix' { $this.NoUnix = True; break; }
                    'user' { $this.User = True; break; }
                    'nouser' { $this.NoUser = True; break; }
                    'users' { $this.Users = True; break; }
                    'defaults' {

                        'suid, dev, exec, auto, nouser, and async'
                    }
                }
            } else {
                $k = $c.Substring(0, $i);
                if (-not $this.KeyValuePairs.ContainsKey($k)) {
                    $this.KeyValuePairs.Add($k, $c.Substring($i + 1));
                }
            }
        }
    }
    [string] ToString() {
        if ($this.AllBinaryNames.Count -gt 0) {
            if ($this.KeyValuePairs.Count -gt 0) {
                return '"' + ((@($this.AllBinaryNames) + (@($this.KeyValuePairs.Keys) | ForEach-Object { "$_=$($this.KeyValuePairs[$_])" })) -join ',') + '"';
            }
            return '"' + (@($this.AllBinaryNames) -join ',') + '"';
        }
        if ($this.KeyValuePairs.Count -gt 0) {
            return '"' + ((@($this.KeyValuePairs.Keys) | ForEach-Object { "$_=$($this.KeyValuePairs[$_])" }) -join ',') + '"';
        }
        return '""';
    }
}

class ProcSelfMounts : MountOptions {
    [string]$Device;
    [string]$Target;
    [string]$FsType;
    [MountOptions]$Options;
    ProcSelfMounts([string]$csv) : base($csv) { }
    [string] ToString() {
        if ($null -eq $this.Options) {
            return "`"$($this.Device) $($this.Device) $($this.Device) `"";
        }
        return "`"$($this.Device) $($this.Device) $($this.Device) $($this.Options.ToString().Substring(1))";
    }
}

[string[]]$FileLines = [System.IO.File]::ReadAllLines('/proc/self/mounts');
$ProcSelfMounts = @($FileLines | ForEach-Object {
    if (-not [string]::IsNullOrWhiteSpace($_)) {
        [string[]]$Cells = $_ -split '\s';
        $m = [ProcSelfMounts]::new($Cells[3]);
        $m.Device = $Cells[0];
        $m.Target = $Cells[1];
        $m.FsType = $Cells[2];
        $m | Write-Output;
    }
});
$ProcSelfMounts | Out-GridView;

class ProcSelfMountInfo {
    [string]$MountId;
    [string]$DeviceId;
    [string]$MajorDevNo;
    [string]$MinorDevNo;
    [string]$FsRoot;
    [string]$Target;
    [string]$OptFields;
    [string]$FsType;
    [string]$Device;
}

[string[]]$FileLines = [System.IO.File]::ReadAllLines('/proc/self/mountinfo');
$ProcSelfMountInfo = @($FileLines | ForEach-Object {
    if (-not [string]::IsNullOrWhiteSpace($_)) {
        $m = [ProcSelfMountInfo]::new();
        if ($_ -match '^([^\s-]\S*(?:\s[^\s-]\S*)+)\s-\s(\S.+)$') {
            [string[]]$Cells = $Matches[1] -split '\s';
            $m.MountId = $Cells[0];
            $m.DeviceId = $Cells[1];
            ($major, $minor) = $Cells[2].Split(':', 2);
            $m.MajorDevNo = $major;
            $m.MinorDevNo = $minor;
            $m.FsRoot = $Cells[3];
            $m.Target = $Cells[4];
            $m.OptFields = $Cells[6];
            [string[]]$Cells = $Matches[2] -split '\s';
            $m.FsType = $Cells[0];
            $m.Device = $Cells[1];
        }
        $m | Write-Output;
    }
});
$ProcSelfMountInfo | Out-GridView;

class FindMntResult {
    [string]$Source;
    [string]$Target;
    [string]$FsType;
    [string]$Options;
    [string]$VfsOptions;
    [string]$FsOptions;
    [string]$Label;
    [string]$UUID;
    [string]$PartLabel;
    [string]$PartUUID;
    [int]$MajorDevNo;
    [int]$MinorDevNo;
    [long]$Size;
    [long]$Avail;
    [long]$Used;
    [System.Nullable[double]]$UsePct;
    [string]$FsRoot;
    [System.UInt32]$TaskID;
    [System.UInt32]$MountID;
    [string]$OptFields;
    [string]$Propagation;
    [string]$Freq;
    [string]$PassNo;
}
