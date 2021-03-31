BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
}

Describe "Get-FsLogicalVolume" {
    It "Returns <FsLogicalVolume[]>"  {
        $LogicalVolumeArray = Get-FsLogicalVolume;
        Should -ActualValue $LogicalVolumeArray -Not -BeNullOrEmpty;
        for ($Index = 0; $Index -lt $LogicalVolumeArray.Length; $Index++)
        {
            $FsLogicalVolume = $LogicalVolumeArray[$Index];
            $FsLogicalVolume | Should -Not -BeNullOrEmpty -Because "`$LogicalVolumeArray[$Index]";
            $FsLogicalVolume | Should -BeOfType FsLogicalVolume;
            $FsLogicalVolume.RootPath | Should -Not -BeNullOrEmpty -Because "`$LogicalVolumeArray[$Index].RootPath";
            $BooleanResult = $FsLogicalVolume.RootPath | Should -Exist;
            $BooleanResult | Should -BeTrue -Because -Because "`$LogicalVolumeArray[$Index].RootPath | Test-Path";
            $FsLogicalVolume.VolumeName | Should -Not -Be $null -Because "`$LogicalVolumeArray[$Index].VolumeName";
            $FsLogicalVolume.VolumeId | Should -Not -BeNullOrEmpty -Because "`$LogicalVolumeArray[$Index].VolumeId";
            $FsLogicalVolume.FsName | Should -Not -BeNullOrEmpty -Because "`$LogicalVolumeArray[$Index].FsName";
        }
    }
}
