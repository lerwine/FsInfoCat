BeforeDiscovery  {
    $DataDirectory = $PSScriptRoot | Join-Path -ChildPath 'Data';
    [System.Xml.XmlDocument]$TestData = New-Object -TypeName 'System.Xml.XmlDocument';
    $TestData.Load(($DataDirectory | Join-Path -ChildPath 'TestLogicalVolumeData.xml'));
}

BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
}

Describe "Register-FsLogicalVolume -LogicalVolume '<LogicalVolumeParamString>'<OptionalSwitches> -PassThru" -ForEach @(
    @($TestContentInfo.SelectNodes('/TestData/FsLogicalVolume')) | ForEach-Object {
        $LogicalVolume = [FsLogicalVolume]@{
            RootPath = '' + $_.RootPath;
            VolumeName = '' + $_.VolumeName;
            VolumeId = '' + $_.VolumeId;
            FsName = '' + $_.FsName;
            RemotePath = '' + $_.RemotePath;
            IsReadOnly = [System.Xml.XmlConvert]::ToBoolean(('' + $_.IsReadOnly));
            IsFixed = [System.Xml.XmlConvert]::ToBoolean(('' + $_.IsFixed));
            IsNetwork = [System.Xml.XmlConvert]::ToBoolean(('' + $_.IsNetwork));
            CaseSensitive = [System.Xml.XmlConvert]::ToBoolean(('' + $_.CaseSensitive));
        };
        $e = $_.SelectSingleNode('Registered');
        $Registered = @{
            VolumeIdentifier = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new(('' + $e.VolumeIdentifier));
            RootUri = [FsInfoCat.Util.FileUri]::new(('' + $e.RootUri), [FsInfoCat.PlatformType]::Windows);
            OptionalSwitch = '' + $e.OptionalSwitch;
        };
        switch ($Registered.OptionalSwitch) {
            'CaseSensitive' {
                @{
                    LogicalVolume = $LogicalVolume;
                    Registered = $Registered;
                    CaseSensitive = $true;
                }
                break;
            }
            'IgnoreCase' {
                @{
                    LogicalVolume = $LogicalVolume;
                    Registered = $Registered;
                    CaseSensitive = $false;
                }
                break;
            }
            default {
                @{
                    LogicalVolume = $LogicalVolume;
                    Registered = $Registered;
                    CaseSensitive = $LogicalVolume.CaseSensitive;
                }
                break;
            }
        }
    }) {
    It "Returns { RootUri = '<Registered.RootUri>'; RootPathName = '<LogicalVolume.RootPath>'; VolumeName = '<LogicalVolume.VolumeName>'; FsName = '<LogicalVolume.VolumeName>'; Identifier = '<Registered.Identifier>'; CaseSensitive = <CaseSensitive> }>"  {
        $ActualResult = $null;
        switch ($Registered.OptionalSwitch) {
            'CaseSensitive' {
                $ActualResult = Register-FsLogicalVolume -LogicalVolume $LogicalVolume -CaseSensitive -PassThru;
                break;
            }
            'IgnoreCase' {
                $ActualResult = Register-FsLogicalVolume -LogicalVolume $LogicalVolume -IgnoreCase -PassThru;
                break;
            }
            Default {
                $ActualResult = Register-FsLogicalVolume -LogicalVolume $LogicalVolume -PassThru;
                break;
            }
        }
        $ActualResult | Should -Not -Be $null;
        $ActualResult | Should -Not -BeOfType FsInfoCat.Models.Volumes.IVolumeInfo;
        $ActualResult.RootUri | Should -Be $RootUri;
        $ActualResult.RootPathName | Should -Be $LogicalVolume.RootPath;
        $ActualResult.VolumeName | Should -Be $LogicalVolume.VolumeName;
        $ActualResult.DriveFormat | Should -Be $LogicalVolume.FsName;
        $ActualResult.Identifier | Should -Be $LogicalVolume.VolumeIdentifier;
        $ActualResult.CaseSensitive | Should -Be $ExpectedCaseSensitive;
    }
}
