BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
    [Xml]$ContentInfo = '<Contents />';
    $ContentInfo.Load(($PSScriptRoot | Join-Path -ChildPath "Data\DirectoryContentTemplates\ContentInfo.xml"));
}

Describe "Start-FsCrawlJob -Name <Name> -RootPath <Description> -MaxDepth <MaxDepth> -MaxItems <MaxItems>" -ForEach @(
    @{
        Name = '';
        Description = '';
        MaxDepth = 0;
        MaxItems = 0;
        FolderTemplate = @('SingleFile');
    },
    @{
        Name = '';
        Description = '';
        MaxDepth = 0;
        MaxItems = 0;
        FolderTemplate = @('MultiFile');
    },
    @{
        Name = '';
        Description = '';
        MaxDepth = 0;
        MaxItems = 0;
        FolderTemplate = @('MultiFile', 'ThreeDeep');
    },
    @{
        Name = '';
        Description = '';
        MaxDepth = 0;
        MaxItems = 0;
        FolderTemplate = @('SingleFile', 'ThreeDeep', 'SevenDeep');
    }
) {
    BeforeAll {
        $TempPath = [System.IO.Path]::GetTempFileName();
        [System.IO.File]::Delete($TempPath);
        [System.IO.Directory]::CreateDirectory($TempPath);
        $_.FolderTemplate | ForEach-Object { Expand-Archive -Path ($PSScriptRoot | Join-Path -ChildPath "Data\DirectoryContentTemplates\$_.zip") -DestinationPath $TempPath -Force }
    }
    AfterAll {
        [System.IO.Directory]::Delete($TempPath, $true);
    }
    It "<Name>: <Description>"  {
        $Job = Start-FsCrawlJob -Name $Name -RootPath $RootPath -MaxDepth $MaxDepth -MaxItems $MaxItems;
        throw 'Not implemented';
        # Wait-Job -Job $Job -Timeout 10000;
    }
}

