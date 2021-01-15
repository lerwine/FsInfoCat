BeforeAll {
    Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
    [Xml]$ContentInfo = '<Contents />';
    $Path = $PSScriptRoot | Join-Path -ChildPath "Data\DirectoryContentTemplates\ContentInfo.xml";
    $ContentInfo.Load($Path);
    $SetDepthAttributes = {
        ([System.Xml.XmlElement]$ParentFolder, [int]$Depth) = $args;
        @($ParentFolder.SelectNodes('File')) | ForEach-Object {
            $_.PSBase.Attributes.Append($ParentFolder.OwnerDocument.CreateAttribute('Depth')).Value = $Depth.ToString();
        }
        $Depth++;
        @($ParentFolder.SelectNodes('Folder')) | ForEach-Object {
            $_.PSBase.Attributes.Append($ParentFolder.OwnerDocument.CreateAttribute('Depth')).Value = $Depth.ToString();
            $SetDepthAttributes.Invoke($_, $Depth);
        }
    }
    @($ContentInfo.SelectNodes('/Contents/RootPath')) | ForEach-Object {
        $SetDepthAttributes.Invoke($_, $Depth);
        New-Object -TypeName 'System.Management.Automation.PSObject' -Property @{
            Template = $_.PSBase.Attributes['Template'].Value;
            Items = 
        };
    }
    @($ContentInfo.SelectNodes('/ContentInfo/RootPath'))
    [string[]]$TemplateNames = @($ContentInfo.SelectNodes('/ContentInfo/RootPath/@Template')) | ForEach-Object { $_.Value };
    if ($TemplateNames.Length -eq 0) { Write-Error -Message 'Unable to parse template names' -Category ReadError -ErrorId 'NoTemplateNameFound' -TargetObject $Path -ErrorAction Stop }
    
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

