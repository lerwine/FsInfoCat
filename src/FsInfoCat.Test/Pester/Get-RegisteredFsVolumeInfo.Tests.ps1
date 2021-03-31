BeforeDiscovery  {
    $DataDirectory = $PSScriptRoot | Join-Path -ChildPath 'Data';
    [System.Xml.XmlDocument]$TestData = New-Object -TypeName 'System.Xml.XmlDocument';
    $TestData.Load(($DataDirectory | Join-Path -ChildPath 'TestLogicalVolumeData.xml'));
}


BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
    @($TestContentInfo.SelectNodes('/TestData/FsLogicalVolume')) | ForEach-Object {
        switch ('' + $_.Registered.OptionalSwitch)
        {
            'CaseSensitive' {
                Register-FsLogicalVolume -LogicalVolume $LogicalVolume -CaseSensitive -PassThru;
                break;
            }
            'IgnoreCase' {
                Register-FsLogicalVolume -LogicalVolume $LogicalVolume -IgnoreCase -PassThru;
                break;
            }
            default {
                Register-FsLogicalVolume -LogicalVolume $LogicalVolume -CaseSensitive -PassThru;
                break;
            }
        }
    }
}

Describe "Get-RegisteredFsVolumeInfo -LogicalVolume '<LogicalVolumeParamString>'<OptionalSwitches> -PassThru" -ForEach @(
    @(@($TestContentInfo.SelectNodes('/TestData/FsLogicalVolume')) | ForEach-Object {
        @{
            RootPath = '' + $_.RootPath;
            VolumeName = '' + $_.VolumeName;
            Identifier = '' + $_.Registered.Identifier;
        }
    })) {
    It "Returns <Returns>"  {
        $ActualResult = Get-RegisteredFsVolumeInfo -Path $RootPath;
        $ActualResult | Should -Not -Be $null;
        $ActualResult | Should -Not -BeOfType FsInfoCat.Models.Volumes.IVolumeInfo;

        $ActualResult = Get-RegisteredFsVolumeInfo -LiteralPath $RootPath;
        $ActualResult | Should -Not -Be $null;
        $ActualResult | Should -Not -BeOfType FsInfoCat.Models.Volumes.IVolumeInfo;

        $ActualResult = Get-RegisteredFsVolumeInfo -RootPathName $RootPath;
        $ActualResult | Should -Not -Be $null;
        $ActualResult | Should -Not -BeOfType FsInfoCat.Models.Volumes.IVolumeInfo;

        $ActualResult = Get-RegisteredFsVolumeInfo -VolumeName $VolumeName;
        $ActualResult | Should -Not -Be $null;
        $ActualResult | Should -Not -BeOfType FsInfoCat.Models.Volumes.IVolumeInfo;

        $ActualResult = Get-RegisteredFsVolumeInfo -Identifier $Identifier;
        $ActualResult | Should -Not -Be $null;
        $ActualResult | Should -Not -BeOfType FsInfoCat.Models.Volumes.IVolumeInfo;
    }
}
