BeforeDiscovery  {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../../dev/bin/TestHelper') -ErrorAction Stop;
    [TestHelper.FsCrawlJobTestData]$FsCrawlJobTestData = Import-FsCrawlJobTestData -LiteralPath (
        $PSScriptRoot | Join-Path -ChildPath 'Data/DirectoryContentTemplates/ContentInfo.xml'
    );
    $InputSetData = Expand-InputSetData -TestData $FsCrawlJobTestData;
}

BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
}

Describe "<Description> | Start-FsCrawlJob" -ForEach $InputSetData {
    BeforeAll {
        # [TestHelper.FsCrawlJobTestData]$FsCrawlJobTestData;
        # [string]$Description;
        # [PSCustomObject[]]$Roots = @{
        #       [int]$ID;
        #       [string[]]$TemplateNames;
        #       [string]$TempPath;
        # }
        # [TestHelper.InputSet]$InputSet;
        $Roots | ForEach-Object {
            $p = $_.Path;
            [System.IO.Directory]::CreateDirectory($p);
            $_.TemplateNames | ForEach-Object {
                Expand-Archive -Path ($PSScriptRoot | Join-Path -ChildPath "Data\DirectoryContentTemplates\$_") -DestinationPath $p -Force;
            }
        };
        $TestData = Expand-TestData -InputSet $InputSet;
    }
    It "<TestDescription> returns <Returns>" -ForEach $TestData  {
        # [string]$TestDescription;
        # [string]$Returns;
        # [string]$JobName;
        # [string[]]$RootPath;
        # [int?]$MaxDepth;
        # [int?]$MaxItems;
        $Job = $null;
        if ($null -ne $MaxDepth) {
            if ($null -ne $MaxItems) {
                $Job = Start-FsCrawlJob -Name $JobName -RootPath $RootPath -MaxDepth $MaxDepth -MaxItems $MaxItems;
            } else {
                $Job = Start-FsCrawlJob -Name $JobName -RootPath $RootPath -MaxDepth $MaxDepth;
            }
        } else {
            if ($null -ne $MaxItems) {
                $Job = Start-FsCrawlJob -Name $JobName -RootPath $RootPath -MaxItems $MaxItems;
            } else {
                $Job = Start-FsCrawlJob -Name $JobName -RootPath $RootPath;
            }
        }
        ($null -eq $Job) | Should -BeFalse;
        $Index = -1;
        do {
            ++$Index;
            $FsRoot = Receive-Job -Job $Job -Wait;
            ($null -eq $r) | Should -BeFalse;
            $FsRoot.MachineName | Should -BeNullOrEmpty -Not;
            $FsRoot.MachineIdentifier | Should -BeNullOrEmpty -Not;
            $Index | Should -BeLessThan $Expected.Count;
            [TestHelper.ExpectedResult]$e = $Expected[$Index];
            $Comparer = $FsRoot.GetNameComparer();
            $ExpectedItemsWithPath = @($e.Items | ForEach-Object {
                [PSCustomObject]@{
                    Path = $Roots[$Index].Path | Join-Path -ChildPath $_.Path;
                    Item = $_;
                }
            });
            $FsChildNodesWithPath = [FsInfoCat.Models.Crawl.FsChildNodeWithPath]::Create($FsRoot);
            $FsChildNodesWithPath.Count
            $FullNames | ForEach-Object {
                $FsChildNode = $r.FindByPath($_.Path);
                ($null -eq $FsChildNode) | Should -BeFalse;
                if ($_.Item -is [TestHelper.FileRef]) {
                    ($FsChildNode -is [FsInfoCat.Models.Crawl.FsFile]) | Should -BeTrue;
                    $f = [TestHelper.ContentFile]::FindFileByID($_.Item.FileID, $Roots[$Index].Templates);
                    ($null -eq $f) | Should -BeFalse;
                    $FsChildNode.Name | Should -Be $f.Name;
                    $FsChildNode.Length | Should -Be $f.Length;
                } else {
                    ($FsChildNode -is [FsInfoCat.Models.Crawl.FsDirectory]) | Should -BeTrue;
                    $f = [TestHelper.ContentFolder]::FindFolderByID($_.Item.FolderID, $Roots[$Index].Templates);
                    ($null -eq $f) | Should -BeFalse;
                    $f.Name | Should -Be $FsChildNode.Name;
                }
            }
        } while ($Job.HasMoreData);
        $Index | Should -Be $Expected.Count;
    }
    AfterAll {
        $Roots | ForEach-Object { [System.IO.Directory]::Delete($_.Path, $true) }
    }
}

