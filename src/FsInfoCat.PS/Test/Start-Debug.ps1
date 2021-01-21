$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
$ErrorActionPreference = [System.Management.Automation.ActionPreference]::Continue;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$DebugPreference = [System.Management.Automation.ActionPreference]::Ignore;
$VerbosePreference = [System.Management.Automation.ActionPreference]::Continue;
$PSModuleAutoLoadingPreference = [System.Management.Automation.ActionPreference]::Continue;
$ProgressPreference = [System.Management.Automation.ActionPreference]::Continue;

Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath '../../../Setup/bin/FsInfoCat') -ErrorAction Stop;
$FsCrawlJob = Start-FsCrawlJob -RootPath '../..' -ErrorAction Stop;
while ($FsCrawlJob.HasMoreData) {
    Receive-Job -Job $FsCrawlJob -Wait -ErrorAction Stop;
}

<#

$Error[0].Exception.StackTrace
$Error[0].Exception.FileName
$Error[0].Exception.FusionLog

#>

<#

Function Compare-PathStrings {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, Position = 0)]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$Value1,
        [Parameter(Mandatory = $true, Position = 1)]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$Value2,
        [switch]$AsFileName
    )

    if ($null -eq $Value1) {
        if ($null -eq $Value2) {
            return 0;
        }
        return -1;
    }
    if ($null -eq $Value2) {
        return 1;
    }
    if ($Value1.Length -eq 0) {
        if ($Value2.Length -eq 0) { return 0; }
        return -1;
    }
    if ($Value2.Length -eq 0) {
        return 1;
    }

    $a = $b = $null;
    if ($AsFileName.IsPresent) {
        $a = $Value1.Split('.');
        $b = $Value2.Split('.');
        for ($i = 0; $i -lt $a.Length -and $i -lt $b.Length; $i++) {
            $c = [System.StringComparer]::OrdinalIgnoreCase.Compare($a[$i], $b[$i]);
            if ($c -ne 0) { return $c }
            $c = [System.StringComparer]::InvariantCultureIgnoreCase.Compare($a[$i], $b[$i]);
            if ($c -ne 0) { return $c }
        }
    } else {
        $a = $Value1.Split(([char[]]@('/', '\', ':')));
        $b = $Value2.Split(([char[]]@('/', '\', ':')));
        for ($i = 0; $i -lt $a.Length -and $i -lt $b.Length; $i++) {
            $c = Compare-PathStrings -Value1 $a[$i] -Value2 $b[$i] -AsFileName;
            if ($c -ne 0) { return $c }
        }
        if ($b.Length -gt $a.Length) { return -1 }
    }
    return $a.Length - $b.Length;
}

enum ContentInfoElementType {
    Contents;
    Template;
    File;
    Folder;
    InputSet;
    Test;
    Root;
    TemplateRef;
}

class ContentElementInfo {
    static hidden [System.Collections.ObjectModel.ReadOnlyDictionary[string,ContentElementInfo]]$AllElements = $null;
    [string]$Name;
    [ContentInfoElementType]$Value;
    [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]$ParentElements;
    [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]$ChildElements;
    static [System.Collections.ObjectModel.ReadOnlyDictionary[string,ContentElementInfo]] GetDictionary() {
        if ($null -ne [ContentElementInfo]::AllElements) { return [ContentElementInfo]::AllElements }
        [System.Collections.Generic.Dictionary[string,ContentElementInfo]]$Dictionary = [System.Collections.Generic.Dictionary[string,ContentElementInfo]]::new([System.StringComparer]::InvariantCultureIgnoreCase);
        [ContentElementInfo]::AllElements = [System.Collections.ObjectModel.ReadOnlyDictionary[string,ContentElementInfo]]::new($Dictionary);
        [Enum]::GetValues([ContentInfoElementType]) | % { $Dictionary.Add($_, [ContentElementInfo]::new($_)) }
        $TemplateInfo = $Dictionary[[ContentInfoElementType]::Template];
        $InputSetInfo = $Dictionary[[ContentInfoElementType]::InputSet];
        $TemplateInfo.ParentElements = $InputSetInfo.ParentElements = [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]::new(([ContentElementInfo[]]@(
            $Dictionary[[ContentInfoElementType]::Contents]
        )));
        $FolderInfo = $Dictionary[[ContentInfoElementType]::Folder];
        $FileInfo = $Dictionary[[ContentInfoElementType]::File];
        $FolderInfo.ParentElements = $FileInfo.ParentElements = [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]::new(([ContentElementInfo[]]@(
            $Dictionary[[ContentInfoElementType]::Template],
            $Dictionary[[ContentInfoElementType]::Folder]
        )));
        $TestInfo = $Dictionary[[ContentInfoElementType]::Test];
        $TestInfo.ParentElements = [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]::new(([ContentElementInfo[]]@(
            $Dictionary[[ContentInfoElementType]::InputSet]
        )));
        $RootInfo = $Dictionary[[ContentInfoElementType]::Root];
        $RootInfo.ParentElements = [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]::new(([ContentElementInfo[]]@(
            $Dictionary[[ContentInfoElementType]::InputSet]
        )));
        $TemplateRefInfo = $Dictionary[[ContentInfoElementType]::TemplateRef];
        $TemplateRefInfo.ParentElements = [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]::new(([ContentElementInfo[]]@(
            $Dictionary[[ContentInfoElementType]::Root]
        )));

        foreach ($e in $Dictionary.Values | ? { $null -eq $_.ParentElements }) {
            if ($e.Value -ne [ContentInfoElementType]::Contents) {
                Write-Warning -Message "Item with value "$($e.Value)" is not the child of any element";
            }
            $e.ParentElements = [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]::new(([ContentElementInfo[]]@()));
        }
        foreach ($e in $Dictionary.Values) {
            [ContentElementInfo[]]$c = @($Dictionary.Values | ? { $null -ne ($_.ParentElements | ? { $_.Value -eq $e.Value } | select -First 1) });
            $e.ChildElements = [System.Collections.ObjectModel.ReadOnlyCollection[ContentElementInfo]]::new($c);
        }
        return [ContentElementInfo]::AllElements;
    }
    static [System.Nullable[ContentInfoElementType]] ToContentInfoElementType([string]$ElementName) {
        [ContentInfoElementType]$r = [ContentInfoElementType]::Contents;
        if ([Enum]::TryParse($ElementName, $false, [ref]$r)) { return $r }
        return $null;
    }
    static [string] ToElementName([ContentInfoElementType]$Type) {
        if ($null -eq $Type) { return $null; }
        return $Type.ToString('F');
    }
    ContentElementInfo([ContentInfoElementType]$Type) {
        $this.Value = $Type;
        $this.Name = $Type.ToString('F');
        if ($null -eq [ContentElementInfo]::AllElements) {
            [ContentElementInfo]::GetDictionary() | Out-Null;
        } else {
            $dict = [ContentElementInfo]::GetDictionary();
            if ($dict.ContainsKey($Type)) {
                $Source = $dict[$Type];
                $this.ParentElements = $Source.ParentElements;
                $this.ChildElements = $Source.ChildElements;
            }
        }
    }
    [bool] Equals([object]$obj) {
        return ($null -ne $obj -and ([object]::ReferenceEquals($this, $obj) -or ($obj -is [ContentElementInfo] -and $this.Value -eq $obj.Value)));
    }
}

Function Test-ContentInfoNode {
    [CmdletBinding(DefaultParameterSetName = 'Pipeline')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Pipeline')]
        [AllowNull()]
        [System.Xml.XmlNode]$InputNode,

        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Named')]
        [AllowNull()]
        [System.Xml.XmlNode[]]$Node,

        [ContentInfoElementType[]]$Type
    )

    Begin {
        #if ($null -eq $Script:__Test_ContentInfoElement_AllTypeNames) {
        #}
        [ContentElementInfo[]]$AcceptElementInfo = $null;
        $dict = [ContentElementInfo]::GetDictionary();
        if ($PSBoundParameters.ContainsKey('Type')) {
            [ContentElementInfo[]]$AcceptElementInfo = @($Type | % {
                $dict[$_];
            });
        } else {
            [ContentElementInfo[]]$AcceptElementInfo = @([ContentElementInfo]::GetDictionary().Values);
        }
    }

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'Named') {
            if ($null -eq $Node) {
                $false | write;
            } else {
                if ($Node.Length -gt 0) {
                    foreach ($n in $Node) {
                        if ($null -eq $n -or $n.PSBase.NodeType -ne [System.Xml.XmlNodeType]::Element -or $n.PSBase.NamespaceURI.Length -gt 0) {
                            $false | write;
                        } else {
                            [ContentElementInfo]$cei = $AcceptElementInfo | ? { $_.Name -ceq $n.PSBase.LocalName } | select -First 1;
                            if ($null -eq $cei) {
                                $false | Write;
                            } else {
                                ($null -eq $n.PSBase.ParentNode -or ($n.PSBase.ParentNode | Test-ContentInfoNode -Type ([ContentInfoElementType[]]($cei.ParentElements | % { $_.Value })))) | Write-Output;
                            }
                        }
                    }
                }
            }
        } else {
            if ($null -eq $InputNode -or $InputNode.PSBase.NodeType -ne [System.Xml.XmlNodeType]::Element -or $InputNode.PSBase.NamespaceURI.Length -gt 0) {
                $false | write;
            } else {
                [ContentElementInfo]$cei = $AcceptElementInfo | ? { $_.Name -ceq $InputNode.PSBase.LocalName } | select -First 1;
                if ($null -eq $cei) {
                    $false | Write;
                } else {
                    ($null -eq $InputNode.PSBase.ParentNode -or $InputNode.PSBase.ParentNode -is [System.Xml.XmlDocument] -or ($InputNode.PSBase.ParentNode | Test-ContentInfoNode -Type ([ContentInfoElementType[]]($cei.ParentElements | % { $_.Value })))) | Write-Output;
                }
            }
        }
    }
}

Function Get-ContentInfoElement {
    [CmdletBinding(DefaultParameterSetName = 'Pipeline')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Pipeline')]
        [AllowNull()]
        [System.Xml.XmlNode]$InputNode,

        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Named')]
        [AllowNull()]
        [System.Xml.XmlNode[]]$Node,

        [ContentInfoElementType[]]$Type,

        [switch]$Parent
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'Named') {
            if ($null -ne $Node -and $Node.Length -gt 0) {
                if ($Parent.IsPresent) {
                    if ($PSBoundParameters.ContainsKey('Type')) {
                        foreach ($n in $Node) {
                            if ($null -ne $n) {
                                $r = $n.ParentNode;
                                while ($null -ne $r -and -not $r | Test-ContentInfoNode -Type $Type) { $r = $r.ParentNode }
                                if ($null -ne $r) { $r | write }
                            }
                        }
                    } else {
                        foreach ($n in $Node) {
                            if ($null -ne $n) {
                                $r = $n.ParentNode;
                                while ($null -ne $r -and -not $r | Test-ContentInfoNode) { $r = $r.ParentNode }
                                if ($null -ne $r) { $r | write }
                            }
                        }
                    }
                } else {
                    if ($PSBoundParameters.ContainsKey('Type')) {
                        foreach ($n in $Node) {
                            $r = $n;
                            while ($null -ne $r -and -not $r | Test-ContentInfoNode -Type $Type) { $r = $r.ParentNode }
                            if ($null -ne $r) { $r | write }
                        }
                    } else {
                        foreach ($n in $Node) {
                            $r = $n;
                            while ($null -ne $r -and -not $r | Test-ContentInfoNode) { $r = $r.ParentNode }
                            if ($null -ne $r) { $r | write }
                        }
                    }
                }
            }
        } else { if ($null -ne $InputNode) {
            $r = $null;
            if ($Parent.IsPresent) { $r = $InputNode.ParentNode } else { $r = $InputNode }
            if ($PSBoundParameters.ContainsKey('Type')) {
                while ($null -ne $r -and -not $r | Test-ContentInfoNode -Type $Type) { $r = $r.ParentNode }
            } else {
                while ($null -ne $r -and -not $r | Test-ContentInfoNode) { $r = $r.ParentNode }
            }
            if ($null -ne $r) { $r | write }
        } }
    }
}

Function Get-XPath {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ $_.PSBase.NodeType -eq [System.Xml.XmlNodeType]::Attribute -or $_.PSBase.NodeType -eq [System.Xml.XmlNodeType]::Element -or $_.PSBase.NodeType -eq [System.Xml.XmlNodeType]::Document })]
        [System.Xml.XmlNode]$XmlNode,
        [switch]$Full
    )

    Process {
        switch ($XmlNode.PSBase.NodeType) {
            { $_ -eq [System.Xml.XmlNodeType]::Document } {
                '/' | write;
                break;
            }
            { $_ -eq [System.Xml.XmlNodeType]::Attribute } {
                if ($null -eq $XmlNode.PSBase.ParentNode) {
                    "@$($XmlNode.PSBase.LocalName)" | write;
                } else {
                    if ($Full.IsPresent) {
                        "$(Get-XPath -XmlNode $XmlNode.PSBase.ParentNode -Full)/@$($XmlNode.PSBase.LocalName)" | write;
                    } else {
                        "$(Get-XPath -XmlNode $XmlNode.PSBase.ParentNode)/@$($XmlNode.PSBase.LocalName)" | write;
                    }
                }
                break;
            }
            default {
                $XPath = $XmlNode.PSBase.LocalName;
                if ($null -ne $XmlNode.PSBase.ParentNode) {
                    if ($XmlNode.PSBase.ParentNode -is [System.Xml.XmlDocument]) {
                        if ($Full.IsPresent) { $XPath = "/$XPath"; }
                    } else {
                        if ($Full.IsPresent) {
                            $XPath = "$(Get-XPath -XmlNode $XmlNode.PSBase.ParentNode -Full)/$XPath";
                        } else {
                            if ($XmlNode.PSBase.ParentNode.PSBase.LocalName -eq 'Template' -or $XmlNode.PSBase.ParentNode.PSBase.LocalName -eq 'Folder') {
                                $XPath = "$(Get-XPath -XmlNode $XmlNode.PSBase.ParentNode)/$XPath";
                            }
                        }
                    }
                }
                $t = [ContentElementInfo]::ToContentInfoElementType($XmlNode.PSBase.LocalName);
                if ($null -ne $t -and ($XmlNode | Test-ContentInfoNode -Type $t)) {
                    switch ($t) {
                        'Template' {
                            "$XPath[@FileName=`"$($XmlNode.FileName)`"]" | write;
                            break;
                        }
                        'File' {
                            "$XPath[@Name=`"$($XmlNode.Name)`"]" | write;
                            break;
                        }
                        'Folder' {
                            "$XPath[@Name=`"$($XmlNode.Name)`"]" | write;
                            break;
                        }
                        'InputSet' {
                            "$XPath[@ID=`"$($XmlNode.Description)`"]" | write;
                            break;
                        }
                        'Root' {
                            "$XPath[@ID=`"$($XmlNode.Description)`"]" | write;
                            break;
                        }
                        'TemplateRef' {
                            "$XPath[.=`"$($XmlNode.PSBase.InnerText)`"]" | write;
                            break;
                        }
                        'Test' {
                            "$XPath[@ID=`"$($XmlNode.Description)`"]" | write;
                            break;
                        }
                        default {
                            if ($null -ne $XmlNode.PSBase.ParentNode -and $XmlNode.PSBase.ParentNode -isnot [System.Xml.XmlDocument]) {
                                $i = $XmlNode.PSBase.SelectNodes("preceding-sibling::$($XmlNode.PSBase.LocalName)").Count;
                                "$XPath[$i]" | write;
                            } else {
                                $XPath | write;
                            }
                            break;
                        }
                    }
                } else {
                    if ($null -ne $XmlNode.PSBase.ParentNode -and $XmlNode.PSBase.ParentNode -isnot [System.Xml.XmlDocument]) {
                        $i = $XmlNode.PSBase.SelectNodes("preceding-sibling::$($XmlNode.PSBase.LocalName)").Count;
                        "$XPath[$i]" | write;
                    } else {
                        $XPath | write;
                    }
                }
                break;
            }
        }
    }
}

Function Get-RelativePath {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ $_ | Test-ContentInfoNode -Type File, Folder })]
        [System.Xml.XmlElement]$XmlElement
    )

    Process {
        if ((Test-ContentInfoNode -Node $XmlElement.PSBase.ParentNode -Type Folder)) {
            "$(Get-RelativePath -XmlElement $XmlElement.PSBase.ParentNode)/$($XmlElement.Name)" | write;
        } else {
            "$($XmlElement.Name)" | write;
        }
    }
}

class PathComparable : IComparable {
    [System.Xml.XmlNode]$Node;
    [string]$Path;
    PathComparable([System.Xml.XmlNode]$Node, $Path) { $this.Path = $Path; $this.Node = $Node }
    [int] CompareTo([object]$Other) {
        if ($null -eq $Other) { return 1 }
        if ($Other -isnot [PathComparable]) { return -1 }
        [string[]]$a = $this.Path.Split('/');
        [string[]]$b = $Other.Path.Split('/');
        $e = $a.Length - 1;
        if ($b.Length -lt $a.Length) { $e = $b.Length - 1 }
        for ($i = 0; $i -lt $e; $i++) {
            [string[]]$x = $a[$i].Split('.');
            [string[]]$y = $b[$i].Split('.');
            $z = $x.Length - 1;
            if ($y.Length -lt $x.Length) { $z = $y.Length }
            for ($n = 0; $n -lt $z; $n++) {
                $r = [StringComparer]::OrdinalIgnoreCase.Compare($x[$n], $y[$n]);
                if ($r -ne 0) {
                    Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ $i, $n" -ForegroundColor Cyan
                    return $r;
                }
                $r = [StringComparer]::InvariantCultureIgnoreCase.Compare($x[$n], $y[$n]);
                if ($r -ne 0) {
                    Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ $i, $n" -ForegroundColor Cyan
                    return $r;
                }
            }
            $r = $x.Length - $y.Length;
            if ($r -ne 0) {
                Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ $i (length)" -ForegroundColor Cyan
                return $r;
            }
            $r = [StringComparer]::OrdinalIgnoreCase.Compare($x[$z], $y[$z]);
            if ($r -ne 0) {
                Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ $i" -ForegroundColor Cyan
                return $r;
            }
            $r = [StringComparer]::InvariantCultureIgnoreCase.Compare($x[$z], $y[$z]);
            if ($r -ne 0) {
                Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ $i" -ForegroundColor Cyan
                return $r;
            }
        }
        $r = $a.Length - $b.Length;
        if ($r -ne 0) { return $r }
        [string[]]$x = $a[$e].Split('.');
        [string[]]$y = $b[$e].Split('.');
        $z = $x.Length - 1;
        if ($y.Length -lt $x.Length) { $z = $y.Length }
        for ($n = 0; $n -lt $z; $n++) {
            $r = [StringComparer]::OrdinalIgnoreCase.Compare($x[$n], $y[$n]);
            if ($r -ne 0) {
                Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ *, $n" -ForegroundColor Cyan
                return $r;
            }
            $r = [StringComparer]::InvariantCultureIgnoreCase.Compare($x[$n], $y[$n]);
            if ($r -ne 0) {
                Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ *, $n" -ForegroundColor Cyan
                return $r;
            }
        }
        $r = $x.Length - $y.Length;
        if ($r -ne 0) {
            Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ * (length)" -ForegroundColor Cyan
            return $r;
        }
        $r = [StringComparer]::OrdinalIgnoreCase.Compare($x[$z], $y[$z]);
        if ($r -eq 0) { $r = [StringComparer]::InvariantCultureIgnoreCase.Compare($x[$z], $y[$z]) }
        Write-Host -Object "$r = '$($this.Path)' => '$($Other.Path)' @ *" -ForegroundColor Cyan
        return $r;
    }
}

$Path = $PSScriptRoot | Join-Path -ChildPath 'Data\DirectoryContentTemplates\ContentInfo.xml';
[Xml]$ContentInfo = '<Contents />';
$ContentInfo.Load($Path);

$IdMap = @{};
foreach ($InputSetElement in @($ContentInfo.SelectNodes('/Contents/InputSet'))) {
    $InputSetElement.Description | Write-Host -ForegroundColor Cyan;
    $TestElement = $InputSetElement.AppendChild($ContentInfo.CreateElement('Test'));
    $MaxDepth = 0;
    $MaxItems = 1;
    foreach ($RootElement in @($InputSetElement.SelectNodes('Root'))) {
        $Key = '' + $RootElement.Description;
        $ID = $IdMap.Count;
        if ($IdMap.ContainsKey($Key)) { $ID = $IdMap[$Key] } else { $IdMap[$Key] = $ID }
        $RootElement.Attributes.Append($ContentInfo.CreateAttribute('ID')).Value = $ID;
        [string[]]$Names = @(@($RootElement.SelectNodes('TemplateRef')) | % { $_.InnerText });
        $ExpectedElement = $TestElement.AppendChild($ContentInfo.CreateElement('Expected'));
        $ExpectedElement.Attributes.Append($ContentInfo.CreateAttribute('RootID')).Value = $ID;
        $FileCount = 0;
        $FolderCount = 0;
        $Names | % {
            $e = $ContentInfo.SelectSingleNode("/Contents/Template[@FileName='$_']");
            $m = [int]::Parse($e.MaxDepth);
            if ($m -gt $MaxDepth) { $MaxDepth = $m }
            (@(@($e.SelectNodes('.//File|.//Folder')) | % {
                if ($_.LocalName -eq 'File') { $FileCount++ } else { $FolderCount++ }
                $x = $ContentInfo.CreateElement("$($_.LocalName)Ref");
                $p = Get-RelativePath -XmlElement $_ -ErrorAction Stop;
                $x.Attributes.Append($ContentInfo.CreateAttribute('Path')).Value = $p;
                $x.InnerText = Get-XPath -XmlNode $_ -ErrorAction Stop;
                [PathComparable]::new($x, $p) | Write-Output;
            }) | sort) | % {
                $ExpectedElement.AppendChild($_.Node) | Out-Null;
            }
        }
        $ExpectedElement.Attributes.Append($ContentInfo.CreateAttribute('FileCount')).Value = $FileCount;
        $ExpectedElement.Attributes.Append($ContentInfo.CreateAttribute('FolderCount')).Value = $FolderCount;
        $FileCount += $FolderCount;
        if ($FileCount -gt $MaxItems) { $MaxItems = $FileCount }
    }
    $t = $InputSetElement.AppendChild($TestElement.CloneNode($true));
    $t.Attributes.Append($ContentInfo.CreateAttribute('MaxDepth')).Value = $MaxDepth;
    $c = $InputSetElement.AppendChild($TestElement.CloneNode($true));
    $c.Attributes.Append($ContentInfo.CreateAttribute('MaxItems')).Value = $MaxItems;
    $t = $InputSetElement.AppendChild($t.CloneNode($true));
    $t.Attributes.Append($ContentInfo.CreateAttribute('MaxItems')).Value = $MaxItems;
}


<#

foreach ($InputSetElement in @($ContentInfo.SelectNodes('/Contents/InputSet'))) {
    foreach ($TestElement in @($InputSetElement.SelectNodes('Test'))) {
        $m = $TestElement.SelectNodes('ExpectedFolder|ExpectedFile').Count;
        $t = $InputSetElement.AppendChild($TestElement.CloneNode($true));
        $t.Attributes.Append($ContentInfo.CreateAttribute('MaxDepth')).Value = (@($TestElement.SelectNodes('ExpectedFolder|ExpectedFile')) | % {
            $v = $ContentInfo.PSBase.SelectSingleNode("/Contents/$($_.InnerText)").Depth;
            #Write-Host -Object "$($_.InnerText) = '$v'" -ForegroundColor Cyan;
            [int]::Parse($v);
        } | sort -Descending) | select -First 1;
        $e = $InputSetElement.AppendChild($TestElement.CloneNode($true));
        $e.Attributes.Append($ContentInfo.CreateAttribute('MaxItems')).Value = $m;
        $t = $InputSetElement.AppendChild($t.CloneNode($true));
        $t.Attributes.Append($ContentInfo.CreateAttribute('MaxItems')).Value = $m;
    }
}

<#
[System.Xml.XmlWriterSettings]$XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
$XmlWriterSettings.Indent =  $true;
$XmlWriterSettings.OmitXmlDeclaration = $true;
$XmlWriterSettings.Encoding = [System.Text.UTF8Encoding]::new($false, $true);
$XmlWriter = [System.Xml.XmlWriter]::Create($Path, $XmlWriterSettings);
try {
    $ContentInfo.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
#>
