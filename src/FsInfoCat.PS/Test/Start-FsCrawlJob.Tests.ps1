Param(
    [Parameter(Mandatory = $true)]
    [string]$FsInfoCatModulePath
)

BeforeAll {
    Import-Module $FsInfoCatModulePath -ErrorAction Stop;
}

Describe "Start-FsCrawlJob -Name <Name> -RootPath <Description> -MaxDepth <MaxDepth> -MaxItems <MaxItems>" -ForEach @(
    @{
        Name = '';
        Description = '';
        MaxDepth = 0;
        MaxItems = 0;
        FolderTemplate = '';
    },
    @{
        Name = '';
        Description = '';
        MaxDepth = 0;
        MaxItems = 0;
        FolderTemplate = '';
    }
) {
    It "Returns <xxx>"  {
        $Job = Start-FsCrawlJob -Name $Name -RootPath $RootPath -MaxDepth $MaxDepth -MaxItems $MaxItems;
        throw 'Not implemented';
        # Wait-Job -Job $Job -Timeout 10000;
    }
}

