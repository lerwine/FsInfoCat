BeforeDiscovery  {
    $DataDirectory = $PSScriptRoot | Join-Path -ChildPath 'Data/DirectoryContentTemplates'
    #Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../../dev/bin/TestHelper') -ErrorAction Stop;
    [System.Xml.XmlDocument]$TestContentInfo = New-Object -TypeName 'System.Xml.XmlDocument';
    $TestContentInfo.Load(($DataDirectory | Join-Path -ChildPath 'ContentInfo.xml'));
    # [TestHelper.FsCrawlJobTestData]$FsCrawlJobTestData = Import-FsCrawlJobTestData -LiteralPath (
    #     $PSScriptRoot | Join-Path -ChildPath 'Data/DirectoryContentTemplates/ContentInfo.xml'
    # );
    # $InputSetData = Expand-InputSetData -TestData $FsCrawlJobTestData;
}

BeforeAll {
    #Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../bin/Debug/net461/win10-x64/FsInfoCat.psd1') -ErrorAction Stop;
}

Describe "<Description>" -ForEach @(
        @($TestContentInfo.SelectNodes('/Contents/InputSets/InputSet')) | ForEach-Object {
            @{ Description = '' + $_.Description; InputSet = $_ }
        }
    ) {
    BeforeAll {
        #[System.Xml.XmlElement]$InputSet = $_;
        $Roots = @($InputSet.SelectNodes('Roots/Root') | ForEach-Object {
            $TempDir = New-Item -Path ([System.IO.Path]::GetTempPath() | Join-Path -ChildPath "StartFsCrawlJobTest-$([Guid]::NewGuid().ToString('n'))") -ItemType Directory;
            $_.SelectNodes('TemplateRef') | ForEach-Object {
                $FileName = $TestContentInfo.SelectSingleNode("/Contents/Templates/Template[@ID='$($_.InnerText)']/@FileName").Value;
                $ZipFile = $DataDirectory | Join-Path -ChildPath ($FileName);
                Expand-Archive -LiteralPath $ZipFile -DestinationPath $TempDir -Force;
            }
            [PSCustomObject]@{
                RootDescription = '' + $_.Description;
                RootID = '' + $_.ID;
                Templates = ([System.Xml.XmlElement[]]@(
                    $_.SelectNodes('TemplateRef') | ForEach-Object {
                        $TestContentInfo.SelectSingleNode("/Contents/Templates/Template[@ID='$_']");
                    }
                ));
                TempDir = $TempDir;
            }
        });
    }
    It "<TestDescription> returns <Returns>" -ForEach @(
            @($InputSet.SelectNodes('Tests/Test')) | ForEach-Object {
                $s = "#$($_.ID): Start-FSCrawlJob";
                if ($null -ne $_.MaxDepth) { $s = "$s -MaxDepth $($_.MaxDepth)" }
                if ($null -ne $_.MaxItems) { $s = "$s -MaxItems $($_.MaxItems)" }
                @{
                    Test = $_;
                    Description = $s;
                    Returns = "#$($_.ID): " + ((@($_.SelectNodes('Expected')) | ForEach-Object { "{ Files: $($_.FileCount), Folders: $($_.FolderCount) }" }) -join '; ');
                }
            }
        ) {
        $FsCrawlJob = $null
        if ($null -ne $Test.MaxDepth) {
            $MaxDepth = [System.Xml.XmlConvert]::ToInt32($Test.MaxDepth);
            if ($null -ne $Test.MaxItems) {
                $MaxItems = [System.Xml.XmlConvert]::ToInt32($Test.MaxItems);
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob -MaxDepth $MaxDepth -MaxItems $MaxItems;
            } else {
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob -MaxDepth $MaxDepth;
            }
        } else {
            if ($null -ne $Test.MaxItems) {
                $MaxItems = [System.Xml.XmlConvert]::ToInt32($Test.MaxItems);
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob -MaxItems $MaxItems;
            } else {
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob;
            }
        }
        $FsCrawlJob | Should -Not -Be $null;
        $Index = 0;
        do {
            $FsHost = $FsCrawlJob | Receive-Job;
            $FsHost | Should -Not -Be $null;
            $FsHost | Should -BeOfType ([FsInfoCat.Models.Crawl.FSHost]);
            $Index++;
        } while ($FsCrawlJob.HasMoreData)
        $Index | Should -Be $Roots.Count;
    }
    AfterAll {
        $Roots | ForEach-Object { $_.TempDir | Remove-Item -Recurse -Force }
    }
}

