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
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
}

Describe ""
Describe "<Description>" -ForEach @(
        @($TestContentInfo.SelectNodes('/Contents/InputSets/InputSet')) | ForEach-Object {
            # Emit hash that will become variables
            @{ Description = '' + $_.Description; InputSet = $_ }
        }
    ) {
    BeforeAll {
        # Unzip contents from specified /Contents/InputSets/InputSet/Roots/Root elements from ContentInfo.xml
        $Roots = @($InputSet.SelectNodes('Roots/Root') | ForEach-Object {
            # Create a temp directory where test folder structures will be extracted.
            $TempDir = New-Item -Path ([System.IO.Path]::GetTempPath() | Join-Path -ChildPath "StartFsCrawlJobTest-$([Guid]::NewGuid().ToString('n'))") -ItemType Directory;
            # /Contents/InputSets/InputSet/Roots/Root/TemplateRef elements contain a reference to the template to be extracted.
            $_.SelectNodes('TemplateRef') | ForEach-Object {
                # Get name of ZIP file to extract from /Contents/Templates/Template[@ID=<guid>]/@FileName
                $FileName = $TestContentInfo.SelectSingleNode("/Contents/Templates/Template[@ID='$($_.InnerText)']/@FileName").Value;
                # Construct full path to ZIP file and extract it to the temp dir
                $ZipFile = $DataDirectory | Join-Path -ChildPath ($FileName);
                Expand-Archive -LiteralPath $ZipFile -DestinationPath $TempDir -Force;
            }
            # Emit a custom object to describe the current test params
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
                $Description = "#$($_.ID): Start-FSCrawlJob";
                if ($null -ne $_.MaxDepth) { $Description = "$Description -MaxDepth $($_.MaxDepth)" }
                if ($null -ne $_.MaxItems) { $Description = "$Description -MaxItems $($_.MaxItems)" }
                # Emit hash that will become variables
                @{
                    Test = $_;
                    Description = $Description;
                    Returns = "#$($_.ID): " + ((@($_.SelectNodes('Expected')) | ForEach-Object { "{ Files: $($_.FileCount), Folders: $($_.FolderCount) }" }) -join '; ');
                }
            }
        ) {
        $FsCrawlJob = $null;
        $MachineIdentifier = Get-LocalMachineIdentifier;
        $MachineIdentifier | Should -Not -BeNullOrEmpty;
        if ($null -ne $Test.MaxDepth) {
            $MaxDepth = [System.Xml.XmlConvert]::ToInt32($Test.MaxDepth);
            if ($null -ne $Test.MaxItems) {
                $MaxItems = [System.Xml.XmlConvert]::ToInt32($Test.MaxItems);
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob -MachineIdentifier $MachineIdentifier -MaxDepth $MaxDepth -MaxItems $MaxItems;
            } else {
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob -MachineIdentifier $MachineIdentifier -MaxDepth $MaxDepth;
            }
        } else {
            if ($null -ne $Test.MaxItems) {
                $MaxItems = [System.Xml.XmlConvert]::ToInt32($Test.MaxItems);
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob -MachineIdentifier $MachineIdentifier -MaxItems $MaxItems;
            } else {
                $FsCrawlJob = @($Roots | Select-Object -ExpandProperty 'TempDir') | Start-FsCrawlJob -MachineIdentifier $MachineIdentifier;
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

