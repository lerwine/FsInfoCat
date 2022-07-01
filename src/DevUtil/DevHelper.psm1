Set-Variable -Name 'XamlNamespaces' -Option Constant -Scope 'Script' -Value ([PSCustomObject]@{
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation";
    x="http://schemas.microsoft.com/winfx/2006/xaml";
    mc="http://schemas.openxmlformats.org/markup-compatibility/2006";
    d="http://schemas.microsoft.com/expression/blend/2008";
});

Function Test-XmlName {
    [CmdletBinding(DefaultParameterSetName = 'Name')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$Name,

        [Parameter(ParameterSetName = 'Name')]
        [switch]$CName,

        [Parameter(Mandatory = $true, ParameterSetName = 'NCName')]
        [switch]$NCName
    )

    Process {
        if ([string]::IsNullOrEmpty($Name) -or -not [System.Xml.XmlConvert]::IsStartNCNameChar($Name[0])) {
            $false | Write-Output;
        } else {
            $c = $Name.IndexOf(':');
            if ($c -lt 0) {
                $IsValid = $true;
                for ($i = 1; $i -lt $Name.Length; $i++) {
                    if (-not [System.Xml.XmlConvert]::IsNcNameChar($Name[$i])) {
                        $IsValid = $false;
                        break;
                    }
                }
                $IsValid | Write-Output;
            } else {
                if ($NCName.IsPresent -or $c -eq 0 -or $c -eq $Name.Length - 1 -or -not [System.Xml.XmlConvert]::IsStartNCNameChar($Name[$c + 1])) {
                    $false | Write-Output;
                } else {
                    $IsValid = $true;
                    for ($i = 1; $i -lt $c; $i++) {
                        if (-not [System.Xml.XmlConvert]::IsNcNameChar($Name[$i])) {
                            $IsValid = $false;
                            break;
                        }
                    }
                    if ($IsValid) {
                        for ($i = $c + 2; $i -lt $Name.Length; $i++) {
                            if (-not [System.Xml.XmlConvert]::IsNcNameChar($Name[$i])) {
                                $IsValid = $false;
                                break;
                            }
                        }
                    }
                    $IsValid | Write-Output;
                }
            }
        }
    }
}

Function Get-PreviousElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )
    for ($n = $Node.PreviousSibling; $null -ne $n; $n = $n.PreviousSibling) {
        if ($n -is [System.Xml.XmlElement]) {
            Write-Output -InputObject $n -NoEnumerate;
            break;
        }
    }
}

Function Get-NextElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )
    for ($n = $Node.NextSibling; $null -ne $n; $n = $n.NextSibling) {
        if ($n -is [System.Xml.XmlElement]) {
            Write-Output -InputObject $n -NoEnumerate;
            break;
        }
    }
}

Function Get-PrecedingWhiteSpace {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )

    $XmlCharacterData = $null;
    for ($n = $Node.PreviousSibling; $null -ne $n -and $n -isnot [System.Xml.XmlElement]; $n = $n.PreviousSibling) {
        if ($n -is [System.Xml.XmlCharacterData]) {
            $XmlCharacterData = $n;
            break;
        }
    }
    if ($null -ne $XmlCharacterData) {
        $Text = $XmlCharacterData.InnerText;
        if ($Text.Length -eq 0) {
            (Get-PrecedingWhiteSpace -Node $XmlCharacterData) | Write-Output;
        } else {
            if ($XmlCharacterData -is [System.Xml.XmlWhitespace]) {
                $ws = Get-PrecedingWhiteSpace -Node $XmlCharacterData;
                if ($null -eq $ws) {
                    $Text | Write-Output;
                } else {
                    "$ws$Text" | Write-Output;
                }
            } else {
                $StartIndex = 0;
                for ($c = $Text.Length - 1; $c -ge 0; $c--) {
                    if (-not [System.Xml.XmlConvert]::IsWhitespaceChar($Text[$c])) {
                        $StartIndex = $c + 1;
                        break;
                    }
                }
                if ($StartIndex -lt $Text.Length) {
                    if ($StartIndex -gt 0) {
                        $Text.Substring($StartIndex) | Write-Output;
                    } else {
                        $ws = Get-PrecedingWhiteSpace -Node $XmlCharacterData;
                        if ($null -eq $ws) {
                            $Text | Write-Output;
                        } else {
                            "$ws$Text" | Write-Output;
                        }
                    }
                }
            }
        }
    }
}

Function Get-FollowingWhiteSpace {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Node
    )

    $XmlCharacterData = $null;
    for ($n = $Node.NextSibling; $null -ne $n -and $n -isnot [System.Xml.XmlElement]; $n = $n.NextSibling) {
        if ($n -is [System.Xml.XmlCharacterData]) {
            $XmlCharacterData = $n;
            break;
        }
    }
    if ($null -ne $XmlCharacterData) {
        $Text = $XmlCharacterData.InnerText;
        if ($Text.Length -eq 0) {
            (Get-FollowingWhiteSpace -Node $XmlCharacterData) | Write-Output;
        } else {
            if ($XmlCharacterData -is [System.Xml.XmlWhitespace]) {
                $ws = Get-FollowingWhiteSpace -Node $XmlCharacterData;
                if ($null -eq $ws) {
                    $Text | Write-Output;
                } else {
                    "$Text$ws" | Write-Output;
                }
            } else {
                $Length = $Text.Length;
                for ($c = 0; $c -lt $Length; $c++) {
                    if (-not [System.Xml.XmlConvert]::IsWhitespaceChar($Text[$c])) {
                        $Length = $c;
                        break;
                    }
                }
                if ($Length -gt 0) {
                    if ($Length -lt $Text.Length) {
                        $Text.Substring(0, $Length) | Write-Output;
                    } else {
                        $ws = Get-FollowingWhiteSpace -Node $XmlCharacterData;
                        if ($null -eq $ws) {
                            $Text | Write-Output;
                        } else {
                            "$Text$ws" | Write-Output;
                        }
                    }
                }
            }
        }
    }
}

Function Add-XmlElement {
    [CmdletBinding(DefaultParameterSetName = 'Last')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'ByIndex')]
        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Last')]
        [System.Xml.XmlElement]$Parent,

        [Parameter(Mandatory = $true, ParameterSetName = 'ByIndex')]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$Index,

        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [switch]$First,

        [Parameter(ParameterSetName = 'Last')]
        [switch]$Last,

        [Parameter(Mandatory = $true, ParameterSetName = 'Before')]
        [ValidateScript({ $null -ne $_.ParentNode })]
        [System.Xml.XmlElement]$Before,

        [Parameter(Mandatory = $true, ParameterSetName = 'After')]
        [ValidateScript({ $null -ne $_.ParentNode })]
        [System.Xml.XmlElement]$After,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$LocalName,

        [ValidateScript({ [string]::IsNullOrEmpty($_) -or ($_ | Test-XmlName -NCName) })]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Prefix,

        [ValidateScript({ [string]::IsNullOrEmpty($_) -or [Uri]::IsWellFormedUriString($_, [UriKind]::Absolute)})]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$NamespaceURI
    )

    $ns = $NamespaceURI;
    $p = $Prefix;
    $XmlElement = $null;
    switch ($PSCmdlet.ParameterSetName) {
        'Before' {
            if ($PSBoundParameters.ContainsKey('NamespaceURI')) {
                if (-not $PSBoundParameters.ContainsKey('Prefix')) {
                    if ($ns -ceq $Before.NamespaceURI) {
                        $p = $Before.Prefix;
                    } else {
                        $p = $Before.GetPrefixOfNamespace($ns);
                        if ([string]::IsNullOrEmpty($p) -and $Before.GetNamespaceOfPrefix($p) -cne $ns) {
                            $i = 0;
                            do {
                                $i++;
                                $p = "ns$i";
                            } while (-not [string]::IsNullOrEmpty($Before.GetNamespaceOfPrefix($p)));
                        }
                    }
                }
            } else {
                $ns = $Before.NamespaceURI;
                if (-not $PSBoundParameters.ContainsKey('Prefix')) { $p = $Before.Prefix; }
            }
            if ($null -eq $ns) { $ns = '' }
            if ($null -eq $p) { $p = '' }
            $OwnerDocument = $Before.OwnerDocument;
            $XmlElement = $Before.ParentNode.InsertBefore($OwnerDocument.CreateElement($p, $LocalName, $ns), $Before);
            $Ws = Get-PrecedingWhiteSpace -Node $XmlElement;
            if ($null -eq $ws) {
                $ws = Get-FollowingWhiteSpace -Node $Before;
                if ($null -ne $ws) {
                    if ($null -eq (Get-NextElement -Node $Before)) { $ws = "$ws    " }
                    $Before.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $Before) | Out-Null;
                    $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                }
            } else {
                $Before.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $Before) | Out-Null;
            }
            break;
        }
        'After' {
            $OwnerDocument = $After.OwnerDocument;
            if ($PSBoundParameters.ContainsKey('NamespaceURI')) {
                if (-not $PSBoundParameters.ContainsKey('Prefix')) {
                    if ($ns -ceq $After.NamespaceURI) {
                        $p = $After.Prefix;
                    } else {
                        $p = $After.GetPrefixOfNamespace($ns);
                        if ([string]::IsNullOrEmpty($p) -and $After.GetNamespaceOfPrefix($p) -cne $ns) {
                            $i = 0;
                            do {
                                $i++;
                                $p = "ns$i";
                            } while (-not [string]::IsNullOrEmpty($After.GetNamespaceOfPrefix($p)));
                        }
                    }
                }
            } else {
                $ns = $After.NamespaceURI;
                if (-not $PSBoundParameters.ContainsKey('Prefix')) { $p = $After.Prefix; }
            }
            if ($null -eq $ns) { $ns = '' }
            if ($null -eq $p) { $p = '' }
            $XmlElement = $After.ParentNode.InsertAfter($OwnerDocument.CreateElement($p, $LocalName, $ns), $After);
            $ws = Get-PrecedingWhiteSpace -Node $After;
            if ($null -eq $ws) {
                $ws = Get-FollowingWhiteSpace -Node $XmlElement;
                if ($null -ne $ws) {
                    if ($null -eq (Get-NextElement -Node $XmlElement)) { $ws = "    $ws" }
                    $After.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $After) | Out-Null;
                    $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                }
            } else {
                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
            }
            break;
        }
        default {
            $OwnerDocument = $Parent.OwnerDocument;
            if ($PSBoundParameters.ContainsKey('NamespaceURI')) {
                if (-not $PSBoundParameters.ContainsKey('Prefix')) {
                    if ($ns -ceq $Parent.NamespaceURI) {
                        $p = $Parent.Prefix;
                    } else {
                        $p = $Parent.GetPrefixOfNamespace($ns);
                        if ([string]::IsNullOrEmpty($p) -and $Parent.GetNamespaceOfPrefix($p) -cne $ns) {
                            $i = 0;
                            do {
                                $i++;
                                $p = "ns$i";
                            } while (-not [string]::IsNullOrEmpty($Parent.GetNamespaceOfPrefix($p)));
                        }
                    }
                }
            } else {
                $ns = $Parent.NamespaceURI;
                if (-not $PSBoundParameters.ContainsKey('Prefix')) { $p = $Parent.Prefix; }
            }
            if ($null -eq $ns) { $ns = '' }
            if ($null -eq $p) { $p = '' }
            if ($First.IsPresent) {
                $XmlElement = $Parent.PrependChild($OwnerDocument.CreateElement($p, $LocalName, $ns));
                $NextElement = Get-NextElement -Node $XmlElement;
                if ($null -eq $NextElement) {
                    $ws = Get-PrecedingWhiteSpace -Node $Parent;
                    if ($null -ne $ws) {
                        $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace("$ws    "), $XmlElement) | Out-Null;
                    }
                } else {
                    $Ws = Get-PrecedingWhiteSpace -Node $XmlElement;
                    if ($null -eq $ws) {
                        $ws = Get-FollowingWhiteSpace -Node $NextElement;
                        if ($null -ne $ws) {
                            if ($null -eq (Get-NextElement -Node $NextElement)) { $ws = "$ws    " }
                            $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                            $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                        }
                    } else {
                        $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                    }
                }
            } else {
                if ($PSBoundParameters.ContainsKey('Index')) {
                    $NextElement = $Parent.SelectSingleNode("*[$($Index + 1)]");
                    if ($null -eq $NextElement) {
                        $XmlElement = $Parent.AppendChild($OwnerDocument.CreateElement($p, $LocalName, $ns));
                        $PreviousElement = Get-PreviousElement -Node $XmlElement;
                        if ($null -eq $PreviousElement) {
                            $ws = Get-PrecedingWhiteSpace -Node $Parent;
                            if ($null -ne $ws) {
                                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace("$ws    "), $XmlElement) | Out-Null;
                            }
                        } else {
                            $ws = Get-PrecedingWhiteSpace -Node $PreviousElement;
                            if ($null -ne $ws) {
                                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                            }
                        }
                    } else {
                        $XmlElement = $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateElement($p, $LocalName, $ns), $NextElement);
                        $Ws = Get-PrecedingWhiteSpace -Node $XmlElement;
                        if ($null -eq $ws) {
                            $ws = Get-FollowingWhiteSpace -Node $NextElement;
                            if ($null -ne $ws) {
                                if ($null -eq (Get-NextElement -Node $NextElement)) { $ws = "$ws    " }
                                $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                                $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                            }
                        } else {
                            $NextElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $NextElement) | Out-Null;
                        }
                    }
                } else {
                    $XmlElement = $Parent.AppendChild($OwnerDocument.CreateElement($p, $LocalName, $ns));
                    $PreviousElement = Get-PreviousElement -Node $XmlElement;
                    if ($null -eq $PreviousElement) {
                        $ws = Get-PrecedingWhiteSpace -Node $Parent;
                        if ($null -ne $ws) {
                            $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace("$ws    "), $XmlElement) | Out-Null;
                        }
                    } else {
                        $ws = Get-PrecedingWhiteSpace -Node $PreviousElement;
                        if ($null -ne $ws) {
                            $XmlElement.ParentNode.InsertBefore($OwnerDocument.CreateWhitespace($ws), $XmlElement) | Out-Null;
                        }
                    }
                }
            }
            break;
        }
    }

    Write-Output -InputObject $XmlElement -NoEnumerate;
}

Function Test-XamlNode {
    [CmdletBinding(DefaultParameterSetName = 'Presentation')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [System.Xml.XmlNode]$Node,

        [Parameter(ParameterSetName = 'Presentation')]
        [switch]$Presentation,

        [Parameter(Mandatory = $true, ParameterSetName = 'Xaml')]
        [switch]$Xaml,

        [string]$LocalName
    )

    Process {
        if ($null -eq $Node -or -not ($Node -is [System.Xml.XmlElement] -or $Node -is [System.Xml.XmlAttribute])) {
            $false | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('LocalName') -and $Node.LocalName -cne $LocalName) {
                $false | Write-Output;
            } else {
                if ($Xaml.IsPresent) {
                    ($Node.NamespaceURI -ceq $Script:XamlNamespaces.x) | Write-Output;
                } else {
                    if ($Presentation.IsPresent) {
                        ($Node.NamespaceURI -ceq $Script:XamlNamespaces.xmlns) | Write-Output;
                    } else {
                        ($Node.NamespaceURI -ceq $Script:XamlNamespaces.xmlns -or $Node.NamespaceURI -ceq $Script:XamlNamespaces.x) | Write-Output;
                    }
                }
            }
        }
    }
}

Function Add-XamlGridRow {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Index')]
        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Last')]
        [ValidateScript({ ($_ | Test-XamlNode -LocalName 'Grid' -Presentation) -or ($_ | Test-XamlNode -LocalName 'Grid.RowDefinitions' -Presentation) })]
        [System.Xml.XmlElement]$GridElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'Before')]
        [ValidateScript({ ($_ | Test-XamlNode -LocalName 'RowDefinition' -Presentation) -and ($_.ParentNode | Test-XamlNode -LocalName 'Grid.RowDefinitions' -Presentation) })]
        [System.Xml.XmlElement]$Before,

        [Parameter(Mandatory = $true, ParameterSetName = 'After')]
        [ValidateScript({ ($_ | Test-XamlNode -LocalName 'RowDefinition' -Presentation) -and ($_.ParentNode | Test-XamlNode -LocalName 'Grid.RowDefinitions' -Presentation) })]
        [System.Xml.XmlElement]$After,

        [Parameter(Mandatory = $true, ParameterSetName = 'Index')]
        [ValidateRange(1, [int]::MaxValue)]
        [int]$RowIndex,

        [ValidateRange(1, [int]::MaxValue)]
        [int]$RowCount = 1,

        [Parameter(Mandatory = $true, ParameterSetName = 'First')]
        [switch]$First,

        [Parameter(ParameterSetName = 'Last')]
        [switch]$Last,

        [ValidatePattern('^(Auto|(\d+(\.\d+)?)\*?|\*)$')]
        [string]$Height,

        [switch]$PassThru
    )
    $RowDefinitionElements = [System.Collections.ObjectModel.Collection[System.Xml.XmlElement]]::new();
    $InsertedIndex = $RowIndex;
    $ParentElement = $GridElement;
    $nsmgr = $null;
    switch ($PSBoundParameters.ParameterSetName) {
        'Before' {
            $ParentElement = $Before.ParentNode;
            $RowDefinition = Add-XmlElement -Before $Before -LocalName 'RowDefinition';
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($Before.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $Before.NamespaceURI);
            $InsertedIndex = $RowDefinition.SelectNodes("preceding-sibling::p:RowDefinition", $nsmgr).Count;
            break;
        }
        'After' {
            $ParentElement = $After.ParentNode;
            $RowDefinition = Add-XmlElement After $After -LocalName 'RowDefinition';
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($After.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $After.NamespaceURI);
            $InsertedIndex = $RowDefinition.SelectNodes("preceding-sibling::p:RowDefinition", $nsmgr).Count;
            break;
        }
        'First' {
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($GridElement.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $GridElement.NamespaceURI);
            if ($GridElement.LocalName -ne 'Grid.RowDefinitions') {
                $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.RowDefinitions', $nsmgr);
                if ($null -eq $DefinitionsElement) {
                    $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.ColumnDefinitions', $nsmgr);
                    if ($null -eq $DefinitionsElement) {
                        $DefinitionsElement = Add-XmlElement -Parent $GridElement -First -LocalName 'Grid.RowDefinitions';
                    } else {
                        $DefinitionsElement = Add-XmlElement Before $DefinitionsElement -First -LocalName 'Grid.RowDefinitions';
                    }
                    $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition';
                } else {
                    $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition' -First;
                }
            } else {
                $ParentElement = $GridElement.ParentNode;
                $RowDefinition = Add-XmlElement -Parent $GridElement -LocalName 'RowDefinition' -First;
            }
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            $InsertedIndex = 0;
            break;
        }
        'Last' {
            $DefinitionsElement = $GridElement;
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($GridElement.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $GridElement.NamespaceURI);
            if ($DefinitionsElement.LocalName -ne 'Grid.RowDefinitions') {
                $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.RowDefinitions', $nsmgr);
                if ($null -eq $DefinitionsElement) {
                    $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.ColumnDefinitions', $nsmgr);
                    if ($null -eq $DefinitionsElement) {
                        $DefinitionsElement = Add-XmlElement -Parent $GridElement -First -LocalName 'Grid.RowDefinitions';
                    } else {
                        $DefinitionsElement = Add-XmlElement Before $DefinitionsElement -First -LocalName 'Grid.RowDefinitions';
                    }
                    $InsertedIndex = 0;
                } else {
                    $InsertedIndex = $DefinitionsElement.SelectNodes('p:RowDefinition', $nsmgr).Count;
                }
            } else {
                $ParentElement = $GridElement.ParentNode;
                $InsertedIndex = $GridElement.SelectNodes('p:RowDefinition', $nsmgr).Count;
            }
            $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition';
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            break;
        }
        default {
            $DefinitionsElement = $GridElement;
            $nsmgr = [System.Xml.XmlNamespaceManager]::new($GridElement.OwnerDocument.NameTable);
            $nsmgr.AddNamespace('p', $GridElement.NamespaceURI);
            $XmlNodeList = $null;
            if ($DefinitionsElement.LocalName -ne 'Grid.RowDefinitions') {
                $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.RowDefinitions', $nsmgr);
                if ($null -eq $DefinitionsElement) {
                    $DefinitionsElement = $GridElement.SelectSingleNode('p:Grid.ColumnDefinitions', $nsmgr);
                    if ($null -eq $DefinitionsElement) {
                        $DefinitionsElement = Add-XmlElement -Parent $GridElement -First -LocalName 'Grid.RowDefinitions';
                    } else {
                        $DefinitionsElement = Add-XmlElement Before $DefinitionsElement -First -LocalName 'Grid.RowDefinitions';
                    }
                }
                $XmlNodeList = $DefinitionsElement.SelectNodes('p:RowDefinition', $nsmgr);
            } else {
                $ParentElement = $GridElement.ParentNode;
                $XmlNodeList = $GridElement.SelectNodes('p:RowDefinition', $nsmgr);
            }
            if ($XmlNodeList.Count -gt $InsertedIndex) {
                $RowDefinition = Add-XmlElement -Before $XmlNodeList[$InsertedIndex] -LocalName 'RowDefinition';
            } else {
                for ($i = $XmlNodeList.Count; $i -lt $InsertedIndex; $i++) {
                    (Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition') | Out-Null;
                }
                $RowDefinition = Add-XmlElement -Parent $DefinitionsElement -LocalName 'RowDefinition';
            }
            $RowDefinitionElements.Add($RowDefinition);
            for ($r = 1; $r -lt $RowCount; $r++) {
                $RowDefinitionElements.Add((Add-XmlElement -After $RowDefinition -LocalName 'RowDefinition'));
            }
            break;
        }
    }
    $OwnerDocument = $ParentElement.OwnerDocument;
    if ($PSBoundParameters.ContainsKey('Height')) {
        foreach ($Rd in $RowDefinitionElements) {
            $rd.Attributes.Append($OwnerDocument.CreateAttribute('Height')).Value = $Height;
        }
    }
    if ($InsertedIndex -gt 0) {
        foreach ($XmlAttribute in @($ParentElement.SelectNodes('p:*/@Grid.Row', $nsmgr))) {
            $i = 0;
            try { $i = [System.Xml.XmlConvert]::ToInt32($XmlAttribute.Value) } catch { $i = 0 }
            if ($i -ge $InsertedIndex) {
                $XmlAttribute.Value = [System.Xml.XmlConvert]::ToString($i + $RowCount);
            }
        }
    } else {
        foreach ($Element in @($ParentElement.SelectNodes('p:*', $nsmgr))) {
            $XmlAttribute = $Element.SelectSingleNode('@Grid.Row');
            if ($null -eq $XmlAttribute) {
                if (-not $Element.LocalName.Contains('.')) {
                    $Element.Attributes.Append($OwnerDocument.CreateAttribute('Grid.Row')).Value = "$RowCount";
                }
            } else {
                try {
                    $XmlAttribute.Value = [System.Xml.XmlConvert]::ToString([System.Xml.XmlConvert]::ToInt32($XmlAttribute.Value) + $RowCount);
                } catch {
                    $XmlAttribute.Value = "$RowCount";
                }
            }
        }
    }
    if ($PassThru.IsPresent) { RowDefinitionElements | Write-Output }
}

Function Select-XElement {
    [CmdletBinding(DefaultParameterSetName = 'Elements')]
    [OutputType([System.Xml.Linq.XAttribute])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XContainer]$Parent,

        [Parameter(ParameterSetName = 'Elements')]
        [Parameter(ParameterSetName = 'AttributeEqualsString')]
        [Parameter(ParameterSetName = 'ChildElementEqualsString')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTime')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTime')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'AttributeEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTimeOffset')]
        [Parameter(ParameterSetName = 'AttributeEqualsGuid')]
        [Parameter(ParameterSetName = 'ChildElementEqualsGuid')]
        [Parameter(ParameterSetName = 'AttributeEqualsBoolean')]
        [Parameter(ParameterSetName = 'ChildElementEqualsBoolean')]
        [Parameter(ParameterSetName = 'AttributeEqualsChar')]
        [Parameter(ParameterSetName = 'ChildElementEqualsChar')]
        [Parameter(ParameterSetName = 'AttributeEqualsDecimal')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDecimal')]
        [Parameter(ParameterSetName = 'AttributeEqualsSByte')]
        [Parameter(ParameterSetName = 'ChildElementEqualsSByte')]
        [Parameter(ParameterSetName = 'AttributeEqualsInt16')]
        [Parameter(ParameterSetName = 'ChildElementEqualsInt16')]
        [Parameter(ParameterSetName = 'AttributeEqualsInt32')]
        [Parameter(ParameterSetName = 'ChildElementEqualsInt32')]
        [Parameter(ParameterSetName = 'AttributeEqualsInt64')]
        [Parameter(ParameterSetName = 'ChildElementEqualsInt64')]
        [Parameter(ParameterSetName = 'AttributeEqualsByte')]
        [Parameter(ParameterSetName = 'ChildElementEqualsByte')]
        [Parameter(ParameterSetName = 'AttributeEqualsUInt16')]
        [Parameter(ParameterSetName = 'ChildElementEqualsUInt16')]
        [Parameter(ParameterSetName = 'AttributeEqualsUInt32')]
        [Parameter(ParameterSetName = 'ChildElementEqualsUInt32')]
        [Parameter(ParameterSetName = 'AttributeEqualsUInt64')]
        [Parameter(ParameterSetName = 'ChildElementEqualsUInt64')]
        [Parameter(ParameterSetName = 'AttributeEqualsSingle')]
        [Parameter(ParameterSetName = 'ChildElementEqualsSingle')]
        [Parameter(ParameterSetName = 'AttributeEqualsDouble')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDouble')]
        [Parameter(ParameterSetName = 'AttributeEqualsTimeSpan')]
        [Parameter(ParameterSetName = 'ChildElementEqualsTimeSpan')]
        [System.Xml.Linq.XName]$ElementName,

        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.Xml.Linq.XNamespace]$Namespace,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEquals')]
        [System.Xml.Linq.XName]$AttributeName,

        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEquals')]
        [System.Xml.Linq.XName]$ChildElementName,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsString')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsString')]
        [string]$Equals,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDateTime')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDateTime')]
        [string]$EqualsDateTime,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsBoolean')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsBoolean')]
        [string]$EqualsBoolean,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDecimal')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDecimal')]
        [string]$EqualsDecimal,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsSByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsSByte')]
        [string]$EqualsSByte,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt16')]
        [string]$EqualsInt16,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt32')]
        [string]$EqualsInt32,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt64')]
        [string]$EqualsInt64,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsByte')]
        [string]$EqualsByte,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt16')]
        [string]$EqualsUInt16,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt32')]
        [string]$EqualsUInt32,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt64')]
        [string]$EqualsUInt64,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsSingle')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsSingle')]
        [string]$EqualsSingle,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDouble')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDouble')]
        [string]$EqualsDouble,

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsTimeSpan')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsTimeSpan')]
        [string]$EqualsTimeSpan,

        [Parameter(ParameterSetName = 'AttributeEqualsString')]
        [Parameter(ParameterSetName = 'ChildElementEqualsString')]
        [System.StringComparison]$Comparison = [System.StringComparison]::CurrentCulture,

        [Parameter(ParameterSetName = 'AttributeEqualsDateTime')]
        [Parameter(ParameterSetName = 'ChildElementEqualsDateTime')]
        [System.Xml.XmlDateTimeSerializationMode]$DateTimeSerializationMode = [System.Xml.XmlDateTimeSerializationMode]::RoundtripKind,

        [Parameter(ParameterSetName = 'ChildElementEqualsString')]
        [switch]$Trim
    )

    Begin {
        [System.StringComparer]$Comparer = $null;
        if (('AttributeEqualsString', 'ChildElementEqualsString') -contains $PSCmdlet.ParameterSetName) {
            switch ($Comparison) {
                CurrentCultureIgnoreCase {
                    $Comparer = [System.StringComparer]::CurrentCultureIgnoreCase;
                    break;
                }
                InvariantCulture {
                    $Comparer = [System.StringComparer]::InvariantCulture;
                    break;
                }
                InvariantCultureIgnoreCase {
                    $Comparer = [System.StringComparer]::InvariantCultureIgnoreCase;
                    break;
                }
                Ordinal {
                    $Comparer = [System.StringComparer]::Ordinal;
                    break;
                }
                OrdinalIgnoreCase {
                    $Comparer = [System.StringComparer]::OrdinalIgnoreCase;
                    break;
                }
                default {
                    $Comparer = [System.StringComparer]::CurrentCulture;
                    break;
                }
            }
        }
    }

    Process {
        switch ($PSCmdlet.ParameterSetName) {
            'Namespace' {
                $Parent.Elements() | Where-Object { $_.Name.Namespace -eq $Namespace } | Write-Output;
                break;
            }
            'AttributeEqualsString' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        return $null -ne $a -and $Comparer.Equals($a.Value, $Equals);
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        return $null -ne $a -and $Comparer.Equals($a.Value, $Equals);
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsString' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    if ($Trim.IsPresent) {
                        $Parent.Elements($ElementName) | Where-Object {
                            $e = $_.Element($ChildElementName);
                            return $null -ne $e -and (-not $e.IsEmpty) -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    } else {
                        $Parent.Elements($ElementName) | Where-Object {
                            $e = $_.Element($ChildElementName);
                            if ($null -eq $e -or $e.IsEmpty) { return $false }
                            $t = $e.Value.Trim();
                            return $t.Length -gt 0 -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    }
                } else {
                    if ($Trim.IsPresent) {
                        $Parent.Elements() | Where-Object {
                            $e = $_.Element($ChildElementName);
                            if ($null -eq $e -or $e.IsEmpty) { return $false }
                            $t = $e.Value.Trim();
                            return $t.Length -gt 0 -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    } else {
                        $Parent.Elements() | Where-Object {
                            $e = $_.Element($ChildElementName);
                            return $null -ne $e -and (-not $e.IsEmpty) -and $Comparer.Equals($e.Value, $Equals);
                        } | Write-Output;
                    }
                }
                break;
            }
            'AttributeEqualsDateTime' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDateTime($a.Value, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDateTime($a.Value, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsDateTime' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDateTime($t, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDateTime($t, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsGuid' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToGuid($a.Value);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToGuid($a.Value);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsGuid' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToGuid($t);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToGuid($t);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsBoolean' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToBoolean($a.Value);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToBoolean($a.Value);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsBoolean' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToBoolean($t);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToBoolean($t);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsDecimal' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDecimal($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDecimal($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsDecimal' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDecimal($t);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDecimal($t);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsSByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsSByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSByte($t);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSByte($t);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt16($t);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt16($t);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt32($t);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt32($t);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt64($t);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToInt64($t);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsByte' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToByte($t);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToByte($t);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsUInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsUInt16' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt16($t);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt16($t);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsUInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsUInt32' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt32($t);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt32($t);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsUInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsUInt64' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt64($t);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToUInt64($t);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsSingle' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSingle($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSingle($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsSingle' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSingle($t);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToSingle($t);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsDouble' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDouble($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDouble($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsDouble' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDouble($t);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToDouble($t);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                }
                break;
            }
            'AttributeEqualsTimeSpan' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToTimeSpan($a.Value);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToTimeSpan($a.Value);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                }
                break;
            }
            'ChildElementEqualsTimeSpan' {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToTimeSpan($t);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [XLinqHelper]::ToTimeSpan($t);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                }
                break;
            }
            default {
                if ($PSBoundParameters.ContainsKey('ElementName')) {
                    $Parent.Elements($ElementName) | Write-Output;
                } else {
                    $Parent.Elements() | Write-Output;
                }
                break;
            }
        }
    }
}

Function Select-XAttribute {
    [CmdletBinding(DefaultParameterSetName = 'Name')]
    [OutputType([System.Xml.Linq.XAttribute])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Element,

        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [System.Xml.Linq.XNamespace]$Namespace,

        [Parameter(ParameterSetName = 'Name')]
        [System.Xml.Linq.XName]$Name
    )

    Process {
        if ($PSCmdlet.ParameterSetName -eq 'Namespace') {
            $Element.Attributes() | Where-Object { $_.Name.Namespace -eq $Namespace } | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('Name')) {
                $a = $Element.Attribute($Name);
                if ($null -ne $a) { $a | Write-Output }
            } else {
                $Element.Attributes() | Write-Output;
            }
        }
    }
}

Function New-XsdRestrictedSimpleType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyNCName($_) | Out-Null })]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyName($_) | Out-Null })]
        [string]$Base
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('restriction'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('base'), $Base)) }

    Process { $XElement.Add($Content) }

    End { [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('restriction'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name), $XElement) | Write-Output }
}

Function New-XsdAttributeGroup {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyNCName($_) | Out-Null })]
        [string]$Name
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('attributeGroup'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name)) }

    Process { $XElement.Add($Content) }

    End { $XElement | Write-Output }
}

Function New-XsdGroup {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyNCName($_) | Out-Null })]
        [string]$Name
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('group'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name)) }

    Process { $XElement.Add($Content) }

    End { $XElement | Write-Output }
}

Function New-XsdAttribute {
    [CmdletBinding(DefaultParameterSetName = 'Optional')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyNCName($_) | Out-Null })]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Ref')]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyName($_) | Out-Null })]
        [string]$Ref,

        [Parameter(Mandatory = $true, ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyName($_) | Out-Null })]
        [string]$Type,

        [Parameter(ParameterSetName = 'Optional')]
        [string]$DefaultValue,

        [Parameter(ParameterSetName = 'Optional')]
        [switch]$Optional,

        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [switch]$Required
    )

    if ($Required.IsPresent) {
        [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('attribute'),
            [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
            [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
            [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('use'), 'required')
        ) | Write-Output;
    } else {
        if ($PSCmdlet.ParameterSetName -eq 'Ref') {
            [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('attributeGroup'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('ref'), $Ref)) | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('DefaultValue')) {
                [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('attribute'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('use'), 'optional'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('default'), $DefaultValue)
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('attribute'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('use'), 'optional')
                ) | Write-Output;
            }
        }
    }
}

Function New-XsdComplexType {
    [CmdletBinding()]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyNCName($_) | Out-Null })]
        [string]$Name,

        [ValidateScript({ [System.Xml.XmlConvert]::VerifyName($_) | Out-Null })]
        [string]$Extends,

        [switch]$Abstract,

        [switch]$Mixed
    )

    Begin {
        [System.Xml.Linq.XElement]$ParentElement = $null;
        [System.Xml.Linq.XElement]$ComplexTypeElement = $null;
        if ($PSBoundParameters.ContainsKey('Extends')) {
            $ParentElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('extension'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('base'), $Extends));
            if ($Abstract.IsPresent) {
                if ($Mixed.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('abstract'), 'true'),
                        [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexContent'),
                            [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('mixed'), 'true'),
                            $ParentElement
                        )
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('abstract'), 'true'),
                        [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexContent'), $ParentElement)
                    );
                }
            } else {
                if ($Mixed.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexContent'),
                            [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('mixed'), 'true'),
                            $ParentElement
                        )
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexContent'), $ParentElement)
                    );
                }
            }
        } else {
            if ($Mixed.IsPresent) {
                if ($Abstract.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('abstract'), 'true'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('mixed'), 'true')
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('mixed'), 'true')
                    );
                }
            } else {
                if ($Abstract.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('abstract'), 'true')
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('complexType'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name));
                }
            }
            $ParentElement = $ComplexTypeElement;
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $ParentElement.Add($_) } }

    End { $ComplexTypeElement | Write-Output }
}

Function New-XsdSequence {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    Begin {
        [System.Xml.Linq.XElement]$XElement = $null;
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('sequence'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('sequence'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('sequence'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('sequence'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('sequence'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('sequence'));
                }
            }
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $XElement.Add($_) } }

    End { $XElement | Write-Output }
}

Function New-XsdChoice {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    Begin {
        [System.Xml.Linq.XElement]$XElement = $null;
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('choice'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('choice'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('choice'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('choice'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('choice'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('choice'));
                }
            }
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $XElement.Add($_) } }

    End { $XElement | Write-Output }
}

Function New-XsdAll {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    Begin {
        [System.Xml.Linq.XElement]$XElement = $null;
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('all'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('all'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('all'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('all'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('all'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('all'));
                }
            }
        }
    }

    Process { if ($PSBoundParameters.ContainsKey('Content')) { $XElement.Add($_) } }

    End { $XElement | Write-Output }
}

Function New-XsdElement {
    [CmdletBinding(DefaultParameterSetName = 'MaxExplicit')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'MaxExplicit')]
        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyNCName($_) | Out-Null })]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Group')]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyName($_) | Out-Null })]
        [string]$Group,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxExplicit')]
        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [ValidateScript({ [System.Xml.XmlConvert]::VerifyName($_) | Out-Null })]
        [string]$Type,

        [ValidateRange(0, [int]::MaxValue)]
        [int]$MinOccurs,

        [ValidateRange(1, [int]::MaxValue, ParameterSetName = 'MaxExplicit')]
        [int]$MaxOccurs,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [switch]$MaxUnbounded
    )

    if ($PSCmdlet.ParameterSetName -eq 'Group') {
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('group'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('ref'), $Group),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded')
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('group'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('ref'), $Group),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded')
                ) | Write-Output;
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('group'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('ref'), $Group),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('group'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('ref'), $Group),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs)
                    ) | Write-Output;
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('group'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('ref'), $Group),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('group'), [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('ref'), $Group)) | Write-Output;
                }
            }
        }
    } else {
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('element'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded')
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('element'),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
                    [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), 'unbounded')
                ) | Write-Output;
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('element'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('element'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('maxOccurs'), $MaxOccurs)
                    ) | Write-Output;
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('element'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('minOccurs'), $MinOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([XLinqHelper]::GetXsdName('element'),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('name'), $Name),
                        [System.Xml.Linq.XAttribute]::new([XLinqHelper]::GetName('type'), $Type)
                    ) | Write-Output;
                }
            }
        }
    }
}

Function Get-FsInfoCatProjectPath {
    [CmdletBinding(DefaultParameterSetName = 'Base')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Base')]
        [switch]$Base,

        [Parameter(Mandatory = $true, ParameterSetName = 'Local')]
        [switch]$Local,

        [Parameter(Mandatory = $true, ParameterSetName = 'Upstream')]
        [switch]$Upstream
    )

    switch ($PSCmdlet.ParameterSetName) {
        'Local' {
            $PSScriptRoot | Join-Path -ChildPath ($MyInvocation.MyCommand.Module.PrivateData['FsInfoCatLocalProjectPath']);
            break;
        }
        'Upstream' {
            $PSScriptRoot | Join-Path -ChildPath ($MyInvocation.MyCommand.Module.PrivateData['FsInfoCatUpstreamProjectPath']);
            break;
        }
        default {
            $PSScriptRoot | Join-Path -ChildPath ($MyInvocation.MyCommand.Module.PrivateData['FsInfoCatProjectPath']);
            break;
        }
    }
}

Set-Variable -Name 'ModelDefinitionNames' -Option Constant -Scope 'Script' -Value ([System.Management.Automation.PSObject]@{
    _ns = [System.Xml.Linq.XNamespace]::Get('http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xs')
});
('ModelDefinitions', 'Sources', 'Source', 'LeadingTrivia', 'TrailingTrivia', 'UnknownSyntax', 'Type', 'UnknownTypeSyntax', 'Name', 'UnknownNameSyntax', 'AliasQualifiedName', 'QualifiedName', 'SimpleName',
        'UnknownSimpleNameSyntax', 'GenericName', 'IdentifierName', 'RefType', 'PredefinedType', 'ArrayType', 'PointerType', 'FunctionPointerType', 'NullableType', 'TupleType', 'OmittedTypeArgument', 'Member',
        'UnknownMemberDeclarationSyntax', 'BaseField', 'UnknownBaseFieldDeclarationSyntax', 'Field', 'EventField', 'BaseMethod', 'UnknownBaseMethodDeclarationSyntax', 'Constructor', 'ConversionOperator',
        'Destructor', 'Method', 'Operator', 'BaseProperty', 'UnknownBasePropertyDeclarationSyntax', 'Event', 'Indexer', 'Property', 'Delegate', 'EnumMember', 'GlobalStatement', 'BaseNamespace',
        'UnknownBaseNamespaceDeclarationSyntax', 'Namespace', 'FileScopedNamespace', 'BaseType', 'UnknownBaseTypeDeclarationSyntax', 'Type', 'UnknownTypeDeclarationSyntax', 'Record', 'Class', 'Struct', 'Interface',
        'Enum', 'IncompleteMember', 'UsingDirective', 'ExternAliasDirective', 'AttributeTargetSpecifier', 'Attribute', 'BaseParameter', 'UnknownBaseParameterSyntax', 'Parameter', 'FunctionPointerParameter',
        'BaseParameterList', 'UnknownBaseParameterListSyntax', 'ParameterList', 'BracketedParameterList', 'ArrayRankSpecifier', 'TupleElement', 'TypeParameter', 'TypeParameterConstraint',
        'UnknownTypeParameterConstraintSyntax', 'ClassOrStructConstraint', 'ConstructorConstraint', 'TypeConstraint', 'DefaultConstraint', 'TypeParameterList', 'TypeParameterConstraintClause', 'Argument',
        'ArgumentList', 'BaseType', 'UnknownBaseTypeSyntax', 'SimpleBaseType', 'PrimaryConstructorBaseType', 'BaseList', 'AttributeArgumentList', 'AttributeArgument', 'BaseArgumentList',
        'UnknownBaseArgumentListSyntax', 'ArgumentList', 'BracketedArgumentList') | ForEach-Object {
    $Script:ModelDefinitionNames | Add-Member -MemberType NoteProperty -Name $_ -Value $Script:ModelDefinitionNames._ns.GetName($_);
}

Function ConvertTo-CsTypeName {
    [CmdletBinding()]
    [OutputType([string])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [string[]]$UsingNamespace
    )

    Process {
        switch ($Type) {
            { $_.IsArray } {
                $r = $Type.GetArrayRank();
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    if ($r -lt 2) {
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)[]";
                    } else {
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)[$([string]::new(([char]','), $r - 1))]";
                    }
                } else {
                    if ($r -lt 2) {
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType())[]";
                    } else {
                        "$(ConvertTo-CsTypeName -Type $Type.GetElementType())[$([string]::new(([char]','), $r - 1))]";
                    }
                }
                break;
            }
            { $_.IsByRef } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)&";
                } else {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType())&";
                }
                break;
            }
            { $_.IsPointer } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)*";
                } else {
                    "$(ConvertTo-CsTypeName -Type $Type.GetElementType())*";
                }
                break;
            }
            { $_ -eq [System.Byte] } { 'byte' | Write-Output; break; }
            { $_ -eq [System.SByte] } { 'sbyte' | Write-Output; break; }
            { $_ -eq [System.Char] } { 'char' | Write-Output; break; }
            { $_ -eq [System.Int16] } { 'short' | Write-Output; break; }
            { $_ -eq [System.Int32] } { 'int' | Write-Output; break; }
            { $_ -eq [System.Int64] } { 'long' | Write-Output; break; }
            { $_ -eq [System.UInt16] } { 'ushort' | Write-Output; break; }
            { $_ -eq [System.UInt32] } { 'uint' | Write-Output; break; }
            { $_ -eq [System.UInt64] } { 'ulong' | Write-Output; break; }
            { $_ -eq [System.Single] } { 'float' | Write-Output; break; }
            { $_ -eq [System.Double] } { 'double' | Write-Output; break; }
            { $_ -eq [System.Decimal] } { 'decimal' | Write-Output; break; }
            { $_ -eq [System.String] } { 'string' | Write-Output; break; }
            { $_ -eq [System.Void] } { 'void' | Write-Output; break; }
            { $_.IsValueType -and $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq [System.Nullable`1] } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    "$(ConvertTo-CsTypeName -Type ([System.Nullable]::GetUnderlyingType($Type)) -UsingNamespace $UsingNamespace)?";
                } else {
                    "$(ConvertTo-CsTypeName -Type ([System.Nullable]::GetUnderlyingType($Type)))?";
                }
                break;
            }
            default {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    (ConvertTo-NameSyntax $Type.DeclaringType -UsingNamespace $UsingNamespace).ToString() | Write-Output;
                } else {
                    (ConvertTo-NameSyntax $Type.DeclaringType).ToString() | Write-Output;
                }
                break;
            }
        }
    }
}

Function Test-TypeAssignableTo {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [Type]$BaseType,

        [switch]$IncludeGenericArguments
    )

    Process {
        if ($BaseType.IsAssignableFrom($Type)) {
            $true | Write-Output;
        } else {
            if ($Type.IsConstructedGenericType -and $IncludeGenericArguments.IsPresent) {
                $Result = $false;
                foreach ($g in $Type.GetGenericArguments()) {
                    if (Test-TypeAssignableTo -Type $g -BaseType $BaseType) {
                        $Result = $true;
                        break;
                    }
                }
                $Result | Write-Output;
            } else {
                $false | Write-Output;
            }
        }
    }
}

Function Test-TypeEquals {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [Type]$BaseType,

        [switch]$IncludeGenericArguments
    )

    Process {
        if ($BaseType -eq $Type) {
            $true | Write-Output;
        } else {
            if ($Type.IsConstructedGenericType -and $IncludeGenericArguments.IsPresent) {
                $Result = $false;
                foreach ($g in $Type.GetGenericArguments()) {
                    if (Test-TypeEquals -Type $g -BaseType $BaseType) {
                        $Result = $true;
                        break;
                    }
                }
                $Result | Write-Output;
            } else {
                $false | Write-Output;
            }
        }
    }
}

Function Test-TypeExtends {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [Type]$BaseType,

        [switch]$Directly,

        [switch]$IncludeGenericArguments
    )

    Process {
        if ($Type -eq $BaseType) {
            $false | Write-Output;
        } else {
            if ($BaseType.IsAssignableFrom($Type)) {
                if ($Directly.IsPresent) {
                    if ($Type.BaseType -ne $BaseType) {
                            if ($BaseType.IsInterface) {
                                $AllInterfaces = $Type.GetInterfaces();
                                $TypeExtends = $false;
                                foreach ($Interface in $AllInterfaces) {
                                    if ($Interface -eq $BaseType) {
                                        $TypeExtends = $true;
                                        foreach ($i in $AllInterfaces) {
                                            if ($i -ne $Interface -and $Interface.IsAssignableFrom($i)) {
                                                $TypeExtends = $false;
                                                break;
                                            }
                                        }
                                        break;
                                    }
                                }
                                $TypeExtends | Write-Output;
                            } else {
                                $false | Write-Output;
                            }
                    } else {
                        $true | Write-Output;
                    }
                } else {
                    $true | Write-Output;
                }
            } else {
                if ($IncludeGenericArguments.IsPresent -and $Type.IsConstructedGenericType) {
                    $Result = $false;
                    if ($Directly.IsPresent) {
                        foreach ($g in $Type.GetGenericArguments()) {
                            if (Test-TypeExtends -Type $g -BaseType $BaseType -Directly) {
                                $Result = $true;
                                break;
                            }
                        }
                    } else {
                        foreach ($g in $Type.GetGenericArguments()) {
                            if (Test-TypeExtends -Type $g -BaseType $BaseType) {
                                $Result = $true;
                                break;
                            }
                        }
                    }
                    $Result | Write-Output;
                } else {
                    $false | Write-Output;
                }
            }
        }
    }
}

Function Get-ExtendingTypes {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [System.Reflection.Assembly[]]$Assembly,

        [switch]$Directly
    )

    Begin {
        [System.Reflection.Assembly[]]$AllAssemblies = $Assembly;
        if (-not $PSBoundParameters.ContainsKey('Assembly')) {
            [System.Reflection.Assembly[]]$AllAssemblies = [System.AppDomain]::CurrentDomain.GetAssemblies();
        }
    }

    Process {
        if (-not $Type.IsSealed) {
            if ($Directly.IsPresent) {
                $AllAssemblies | ForEach-Object { $_.GetTypes() } | Where-Object { $_ | Test-TypeExtends -BaseType $Type -Directly }
            } else {
                $AllAssemblies | ForEach-Object { $_.GetTypes() } | Where-Object { $_ | Test-TypeExtends -BaseType $Type }
            }
        }
    }
}

Function Get-PropertiesReferencingType {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [System.Reflection.Assembly[]]$Assembly,

        [switch]$Directly,

        [switch]$IncludeGenericArguments
    )

    Begin {
        $PropertyCollection = @();
        if ($PSBoundParameters.ContainsKey('Assembly')) {
            $PropertyCollection = @($Assembly | ForEach-Object { $_.GetTypes() | ForEach-Object { $_.GetProperties() | Where-Object { $_.ReflectedType -eq $_.DeclaringType } } });
        }
    }

    Process {
        $AllProperties = $PropertyCollection;
        if ($PSBoundParameters.ContainsKey('Assembly')) {
            if ($Assembly -notcontains $Type.Assembly) {
                $AllProperties = @($Type.Assembly.GetTypes() | ForEach-Object {$_.GetProperties() | Where-Object { $_.ReflectedType -eq $_.DeclaringType } }) + $PropertyCollection;
            }
        } else {
            $AllProperties = @($Type.Assembly.GetTypes() | ForEach-Object { $_.GetProperties() | Where-Object { $_.ReflectedType -eq $_.DeclaringType } });
        }
        if ($Directly.IsPresent) {
            if ($IncludeGenericArguments.IsPresent) {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeEquals -BaseType $Type -IncludeGenericArguments }
            } else {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeEquals -BaseType $Type }
            }
        } else {
            if ($IncludeGenericArguments.IsPresent) {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeAssignableTo -BaseType $Type -IncludeGenericArguments }
            } else {
                $AllProperties | Where-Object { $_.PropertyType | Test-TypeAssignableTo -BaseType $Type }
            }
        }
    }
}

Function Get-BaseTypes {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [switch]$AllAncestors
    )

    Process {
        if ($AllAncestors.IsPresent) {
            for ($BaseType = $Type.BaseType; $null -ne $Type.BaseType; $BaseType = $BaseType.BaseType) {
                $BaseType | Write-Output;
            }
            $Type.GetInterfaces() | Write-Output;
        } else {
            if ($null -ne $Type.BaseType) { $Type.BaseType | Write-Output }
            $AllInterfaces = $Type.GetInterfaces();
            foreach ($Interface in $AllInterfaces) {
                $IsDirect = $true;
                foreach ($i in $AllInterfaces) {
                    if ($i -ne $Interface -and $Interface.IsAssignableFrom($i)) {
                        $IsDirect = $false;
                        break;
                    }
                }
                if ($IsDirect) { $Interface | Write-Output }
            }
        }
    }
}

Function ConvertTo-TypeSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [string[]]$UsingNamespace
    )

    Process {
        switch ($Type) {
            { $_.IsArray } {
                $NameSyntax = $null;
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    $NameSyntax = ConvertTo-TypeSyntax -Type $Type.GetElementType() -UsingNamespace $UsingNamespace;
                } else {
                    $NameSyntax = ConvertTo-TypeSyntax -Type $Type.GetElementType();
                }
                $r = $Type.GetArrayRank();
                if ($r -lt 2) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$NameSyntax[]")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$$NameSyntax[$([string]::new(([char]','), $r - 1))]")) -NoEnumerate;
                }
                break;
            }
            { $_.IsByRef } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)&")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType())&")) -NoEnumerate;
                }
                break;
            }
            { $_.IsPointer } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType() -UsingNamespace $UsingNamespace)*")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type $Type.GetElementType())*")) -NoEnumerate;
                }
                break;
            }
            { $_ -eq [System.Byte] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('byte') | Write-Output; break; }
            { $_ -eq [System.SByte] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('sbyte') | Write-Output; break; }
            { $_ -eq [System.Char] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('char') | Write-Output; break; }
            { $_ -eq [System.Int16] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('short') | Write-Output; break; }
            { $_ -eq [System.Int32] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('int') | Write-Output; break; }
            { $_ -eq [System.Int64] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('long') | Write-Output; break; }
            { $_ -eq [System.UInt16] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('ushort') | Write-Output; break; }
            { $_ -eq [System.UInt32] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('uint') | Write-Output; break; }
            { $_ -eq [System.UInt64] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('ulong') | Write-Output; break; }
            { $_ -eq [System.Single] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('float') | Write-Output; break; }
            { $_ -eq [System.Double] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('double') | Write-Output; break; }
            { $_ -eq [System.Decimal] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('decimal') | Write-Output; break; }
            { $_ -eq [System.String] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('string') | Write-Output; break; }
            { $_ -eq [System.Void] } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName('void') | Write-Output; break; }
            { $_.IsGenericParameter } { [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName($Type.Name) | Write-Output; break; }
            { $_.IsValueType -and $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq [System.Nullable`1] } {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type ([System.Nullable]::GetUnderlyingType($Type)) -UsingNamespace $UsingNamespace)?")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$(ConvertTo-TypeSyntax -Type ([System.Nullable]::GetUnderlyingType($Type)))?")) -NoEnumerate;
                }
                break;
            }
            default {
                $LeftSyntax = $null;
                if ($Type.IsNested) {
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        $LeftSyntax = ConvertTo-TypeSyntax -Type $Type.DeclaringType -UsingNamespace $UsingNamespace;
                    } else {
                        $LeftSyntax = ConvertTo-TypeSyntax -Type $Type.DeclaringType;
                    }
                } else {
                    if (-not ([string]::IsNullOrEmpty($Type.Namespace) -or ($PSBoundParameters.ContainsKey('UsingNamespace') -and $UsingNamespace -ccontains $Type.Namespace))) {
                        $LeftSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseName($Type.Namespace);
                    }
                }
                $RightSyntax = $null;
                $n = $Type.Name;
                if ($Type.IsConstructedGenericType) {
                    $n = $n.Substring(0, $n.IndexOf('`'));
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$n<$((($Type.GetGenericArguments() | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace) | ForEach-Object { $_.ToString() }) -join ',')>");
                    } else {
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$n<$((($Type.GetGenericArguments() | ConvertTo-TypeSyntax) | ForEach-Object { $_.ToString() }) -join ',')>");
                    }
                } else {
                    if ($Type.IsGenericTypeDefinition) {
                        $n = $n.Substring(0, $n.IndexOf('`'));
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName("$n$($n.Substring(0, $n.IndexOf('`'))){$($Type.GetGenericArguments() -join ', ')}");
                    } else {
                        $RightSyntax = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseTypeName($n) | Write-Output;
                    }
                }
                if ($null -eq $LeftSyntax) {
                    Write-Output -InputObject $RightSyntax -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::QualifiedName($LeftSyntax, $RightSyntax)) -NoEnumerate;
                }
                break;
            }
        }
    }
}

Function ConvertTo-PropertyDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.PropertyInfo]$Property,

        [string[]]$UsingNamespace
    )

    Process {
        [System.Reflection.ParameterInfo[]]$IndexParameters = $Property.GetIndexParameters();
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$PropertyTypeSyntax = $null;
        $Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $PropertyTypeSyntax = ConvertTo-TypeSyntax -Type $Property.PropertyType -UsingNamespace $UsingNamespace;
            if ($IndexParameters.Length -gt 0) {
                $Code = "$PropertyTypeSyntax this[$(($IndexParameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace ) $($_.Name)" }) -join ', ')]"
            } else {
                $Code = "$PropertyTypeSyntax $($Property.Name)";
            }
        } else {
            $PropertyTypeSyntax = ConvertTo-TypeSyntax -Type $Property.PropertyType;
            if ($IndexParameters.Length -gt 0) {
                $Code = "$PropertyTypeSyntax this[$(($IndexParameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType ) $($_.Name)" }) -join ', ')]"
            } else {
                $Code = "$PropertyTypeSyntax $($Property.Name)";
            }
        }
        if ($Property.DeclaringType.IsInterface) {
            if ($Property.CanWrite) {
                if ($Property.CanRead) {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code get; set; }")) -NoEnumerate;
                } else {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { set; }")) -NoEnumerate;
                }
            } else {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { get; }")) -NoEnumerate;
            }
        } else {
            [System.Reflection.MethodInfo]$GetAccessor = $Property.GetGetMethod();
            [System.Reflection.MethodInfo]$SetAccessor = $Property.GetSetMethod();
            if ($null -ne $GetAccessor) {
                if ($GetAccessor.IsStatic) { $Code = "static $Code" }
                if ($GetAccessor.IsVirtual) {
                    $Code = "virtual $Code";
                } else {
                    if ($GetAccessor.IsAbstract) { $code = "abstract $Code" }
                }
                if ($null -eq $SetAccessor) {
                    switch ($GetAccessor) {
                        { $_.IsPublic } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsPrivate } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsAssembly } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsFamily } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; }")) -NoEnumerate;
                            break;
                        }
                        default {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; }")) -NoEnumerate;
                            break;
                        }
                    }
                } else {
                    switch ($GetAccessor) {
                        { $_.IsPublic } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; internal set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; protected set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { get; protected internal set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        { $_.IsPrivate } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { private get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        { $_.IsAssembly } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        { $_.IsFamily } {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { protected get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; internal set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { protected get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                        default {
                            switch ($SetAccessor) {
                                { $_.IsPublic } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { protected internal get; set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsPrivate } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsAssembly } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; internal set; }")) -NoEnumerate;
                                    break;
                                }
                                { $_.IsFamily } {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; protected set; }")) -NoEnumerate;
                                    break;
                                }
                                default {
                                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { get; set; }")) -NoEnumerate;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            } else {
                if ($null -ne $SetAccessor) {
                    if ($SetAccessor.IsStatic) { $Code = "static $Code" }
                    if ($SetAccessor.IsVirtual) {
                        $Code = "virtual $Code";
                    } else {
                        if ($SetAccessor.IsAbstract) { $code = "abstract $Code" }
                    }
                    switch ($SetAccessor) {
                        { $_.IsPublic } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsPrivate } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsAssembly } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        { $_.IsFamily } {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code { set; }")) -NoEnumerate;
                            break;
                        }
                        default {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code { set; }")) -NoEnumerate;
                            break;
                        }
                    }
                } else {
                    if ($Property.CanWrite) {
                        if ($Property.CanRead) {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { get; set; }")) -NoEnumerate;
                        } else {
                            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { set; }")) -NoEnumerate;
                        }
                    } else {
                        Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code { get; }")) -NoEnumerate;
                    }
                }
            }
        }
    }
}

Function ConvertTo-EventDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.EventInfo]$Event,

        [string[]]$UsingNamespace
    )

    Process {
        $Event.EventHandlerType;
        [string]$Code = $null;
        $Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = "event $(ConvertTo-TypeSyntax -Type $Event.EventHandlerType -UsingNamespace $UsingNamespace) $($Event.Name)";
        } else {
            $Code = "event $(ConvertTo-TypeSyntax -Type $Event.EventHandlerType) $($Event.Name)";
        }
        if ($Property.DeclaringType.IsInterface) {
            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code;")) -NoEnumerate;
        } else {
            if ($Event.RaiseMethod.IsStatic) { $Code = "static $Code" }
            if ($Event.RaiseMethod.IsVirtual) {
                $Code = "virtual $Code";
            } else {
                if ($GetAccessor.IsAbstract) { $code = "abstract $Code" }
            }
            switch ($Event.RaiseMethod) {
                { $_.IsPublic } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code")) -NoEnumerate;
                    break;
                }
                { $_.IsPrivate } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code")) -NoEnumerate;
                    break;
                }
                { $_.IsAssembly } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code")) -NoEnumerate;
                    break;
                }
                { $_.IsFamily } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code")) -NoEnumerate;
                    break;
                }
                default {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code")) -NoEnumerate;
                    break;
                }
            }
        }
    }
}

Function ConvertTo-FieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.FieldInfo]$Field,

        [string[]]$UsingNamespace
    )

    Process {
        [string]$Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = "$(ConvertTo-TypeSyntax -Type $Property.FieldType -UsingNamespace $UsingNamespace)) $($Field.Name)";
        } else {
            $Code = "$(ConvertTo-TypeSyntax -Type $Property.FieldType)) $($Field.Name)";
        }
        if ($Field.IsLiteral) {
            if ($Field.IsStatic) { $Code = "const $Code" }
        } else {
            if ($Field.IsInitOnly) { $Code = "readonly $Code" }
            if ($Field.IsStatic) { $Code = "static $Code" }
        }
        switch ($Field) {
            { $_.IsPublic } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code;")) -NoEnumerate;
                break;
            }
            { $_.IsPrivate } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code;")) -NoEnumerate;
                break;
            }
            { $_.IsAssembly } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code;")) -NoEnumerate;
                break;
            }
            { $_.IsFamily } {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code;")) -NoEnumerate;
                break;
            }
            default {
                Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code;")) -NoEnumerate;
                break;
            }
        }
    }
}

Function ConvertTo-MethodDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.MethodInfo]$Method,

        [string[]]$UsingNamespace
    )

    Process {
        $Code = $Method.Name;
        [System.Reflection.ParameterInfo[]]$Parameters = $Method.GetParameters();
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = "$(ConvertTo-TypeSyntax -Type $Method.ReturnType -UsingNamespace $UsingNamespace) $Code";
            if ($Method.IsConstructedGenericMethod) {
                $Code = "$($Code.Substring(0, $Code.LastIndexOf('`')))<$((($Method.GetGenericArguments() | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace) | ForEach-Object { $_.ToString() }) -join ',')>";
            } else {
                if ($Method.IsGenericMethodDefinition) {
                    $Code = "$($Code.Substring(0, $Code.LastIndexOf('`')))<$(($Method.GetGenericArguments() | ForEach-Object { $_.Name }) -join ',')>";
                }
            }
            if ($Parameters.Length -eq 0) {
                $Code += "()";
            } else {
                $Code = "$Code($(($Parameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace ) $($_.Name)" }) -join ', '))";
            }
        } else {
            $Code = "$(ConvertTo-TypeSyntax -Type $Method.ReturnType) $Code";
            if ($Method.IsConstructedGenericMethod) {
                $i = $Code.LastIndexOf('`');
                if ($i -gt 0) { $Code = $Code.Substring(0, $i) }
                $Code = "$Code<$((($Method.GetGenericArguments() | ConvertTo-TypeSyntax) | ForEach-Object { $_.ToString() }) -join ',')>";
            } else {
                if ($Method.IsGenericMethodDefinition) {
                    $i = $Code.LastIndexOf('`');
                    if ($i -gt 0) { $Code = $Code.Substring(0, $i) }
                    $Code = "$Code<$(($Method.GetGenericArguments() | ForEach-Object { $_.Name }) -join ',')>";
                }
            }
            if ($Parameters.Length -eq 0) {
                $Code += "()";
            } else {
                $Code = "$Code($(($Parameters | ForEach-Object { "$(ConvertTo-TypeSyntax -Type $_.ParameterType ) $($_.Name)" }) -join ', '))";
            }
        }
        if ($Method.DeclaringType.IsInterface) {
            Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("$Code;")) -NoEnumerate;
        } else {
            if ($SetAccessor.IsStatic) { $Code = "static $Code" }
            switch ($Method) {
                { $_.IsPublic } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("public $Code;")) -NoEnumerate;
                    break;
                }
                { $_.IsPrivate } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("private $Code;")) -NoEnumerate;
                    break;
                }
                { $_.IsAssembly } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("internal $Code;")) -NoEnumerate;
                    break;
                }
                { $_.IsFamily } {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected $Code;")) -NoEnumerate;
                    break;
                }
                default {
                    Write-Output -InputObject ([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseMemberDeclaration("protected internal $Code;")) -NoEnumerate;
                    break;
                }
            }
        }
    }
}

Function ConvertTo-ConstructorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Reflection.ConstructorInfo]$Constructor,

        [string[]]$UsingNamespace
    )

    Process {
        $Name = $Constructor.DeclaringType.Name;
        if ($Constructor.DeclaringType.IsGenericType) { $Name = $Name.Substring(0, $Name.LastIndexOf('`')) }
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax] $Result = [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ConstructorDeclaration($Name);
        switch ($Constructor) {
            { $_.IsPublic } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('public ')));
                break;
            }
            { $_.IsPrivate } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('private ')));
                break;
            }
            { $_.IsAssembly } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('internal ')));
                break;
            }
            { $_.IsFamily } {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('protected ')));
                break;
            }
            default {
                $Result = $Result.WithParameterList([Microsoft.CodeAnalysis.SyntaxTokenList]::new([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('protected '), [Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseToken('internal ')));
                break;
            }
        }

        [System.Reflection.ParameterInfo[]]$Parameters = $Constructor.GetParameters();
        if ($Parameters.Length -gt 0) {
            if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                Write-Output -InputObject $Result.WithParameterList([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseParameterList("($(($Parameters | ForEach-Object {
                    if ($_.IsOut) {
                        "out $(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name)" | Write-Output;
                    } else {
                        if ($_.IsRetval) {
                            "ref $(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name)" | Write-Output;
                        } else {
                            if ($_.IsOptional) {
                                "$c$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name) = " | Write-Output;
                                if ($null -eq $_.DefaultValue) {
                                    "($c)default" | Write-Output;
                                } else {
                                    if ($_.DefaultValue -is [string] -or $_.DefaultValue -is [char] -or $_.DefaultValue -is [double] -or $_.DefaultValue -is [float] -or $_.DefaultValue -is [decimal]) {
                                        "$c$([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::Literal($_.DefaultValue))" | Write-Output;
                                    } else {
                                        if ($_.DefaultValue -is [type]) {
                                            "$($c)typeof($(ConvertTo-TypeSyntax -Type $_.DefaultValue -UsingNamespace $UsingNamespace))" | Write-Output;
                                        } else {
                                            "$c$($_.DefaultValue)" | Write-Output;
                                        }
                                    }
                                }
                            } else {
                                "$(ConvertTo-TypeSyntax -Type $_.ParameterType -UsingNamespace $UsingNamespace) $($_.Name)" | Write-Output;
                            }
                        }
                    }
                }) -join ', '))")) -NoEnumerate;
            } else {
                Write-Output -InputObject $Result.WithParameterList([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::ParseParameterList("($(($Parameters | ForEach-Object {
                    if ($_.IsOut) {
                        "out $(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name)" | Write-Output;
                    } else {
                        if ($_.IsRetval) {
                            "ref $(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name)" | Write-Output;
                        } else {
                            if ($_.IsOptional) {
                                "$c$(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name) = " | Write-Output;
                                if ($null -eq $_.DefaultValue) {
                                    "($c)default" | Write-Output;
                                } else {
                                    if ($_.DefaultValue -is [string] -or $_.DefaultValue -is [char] -or $_.DefaultValue -is [double] -or $_.DefaultValue -is [float] -or $_.DefaultValue -is [decimal]) {
                                        "$c$([Microsoft.CodeAnalysis.CSharp.SyntaxFactory]::Literal($_.DefaultValue))" | Write-Output;
                                    } else {
                                        if ($_.DefaultValue -is [type]) {
                                            "$($c)typeof($(ConvertTo-TypeSyntax -Type $_.DefaultValue))" | Write-Output;
                                        } else {
                                            "$c$($_.DefaultValue)" | Write-Output;
                                        }
                                    }
                                }
                            } else {
                                "$(ConvertTo-TypeSyntax -Type $_.ParameterType) $($_.Name)" | Write-Output;
                            }
                        }
                    }
                }) -join ', '))")) -NoEnumerate;
            }
        } else {
            Write-Output -InputObject $Result -NoEnumerate;
        }
    }
}

Function Get-UnderlyingType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type
    )

    Process {
        if ($Type.IsArray -or $Type.IsPointer -or $Type.IsByRef) {
            (Get-UnderlyingType -Type $Type.GetElementType()) | Write-Output;
        } else {
            if ($Type.IsEnum) {
                $Type.GetEnumUnderlyingType() | Write-Output;
            } else {
                if ($Type.IsConstructedGenericType) {
                    if ($Type.GetGenericTypeDefinition() -eq [System.Nullable`1]) {
                        [System.Nullable]::GetUnderlyingType($Type) | Write-Output;
                    } else {
                        if ($Type.GetGenericTypeDefinition() -eq [System.Collections.Generic.IEnumerable`1]) {
                            $Type.GetGenericArguments() | Write-Output;
                        } else {
                            @($Type.GetInterfaces() | Where-Object {
                                $_.IsConstructedGenericType -and $_.GetGenericTypeDefinition() -eq [System.Collections.Generic.IEnumerable`1] -and -not $_.GetGenericArguments()[0].IsGenericMethodParameter;
                            }) | Write-Output;
                        }
                    }
                }
            }
        }
    }
}

Function Get-TypeMemberInfo {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [string[]]$UsingNamespace,

        [switch]$IgnoreInherited
    )

    Process {
        $UnderlyingType = @(Get-UnderlyingType -Type $Type);
        $Code = $null;
        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
            $Code = (ConvertTo-TypeSyntax -Type $Type -UsingNamespace $UsingNamespace).ToString();
        } else {
            $Code = (ConvertTo-TypeSyntax -Type $Type).ToString();
        }
        if ($Type.IsPointer -or $Type.IsByRef -or $Type.IsArray) {
            $ElementType = $UnderlyingType[0];
            if ($ElementType.IsPublic) {
                "public $Code" | Write-Output;
            } else {
                $Code | Write-Output;
            }
            if ($ElementType -ne [string] -and $ElementType -ne [object] -and $ElementType -ne [decimal] -and -not $ElementType.IsPrimitive) {
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    if ($IgnoreInherited.IsPresent) {
                        (Get-TypeMemberInfo -Type $ElementType -UsingNamespace $UsingNamespace -IgnoreInherited) | ForEach-Object { "  $_" | Write-Output }
                    } else {
                        (Get-TypeMemberInfo -Type $ElementType -UsingNamespace $UsingNamespace) | ForEach-Object { "  $_" | Write-Output }
                    }
                } else {
                    if ($IgnoreInherited.IsPresent) {
                        (Get-TypeMemberInfo -Type $ElementType -IgnoreInherited) | ForEach-Object { "  $_" | Write-Output }
                    } else {
                        (Get-TypeMemberInfo -Type $ElementType) | ForEach-Object { "  $_" | Write-Output }
                    }
                    (Get-TypeMemberInfo -Type $ElementType) | ForEach-Object { "  $_" | Write-Output }
                }
            }
        } else {
            if ($Type.IsEnum) {
                [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax[]]$Fields = @();
                if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                    $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax -UsingNamespace $UsingNamespace);
                    if ($Type.IsPublic) {
                        "public enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0] -UsingNamespace $UsingNamespace)" | Write-Output;
                    } else {
                        "enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0] -UsingNamespace $UsingNamespace)" | Write-Output;
                    }
                } else {
                    $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                    if ($Type.IsPublic) {
                        "public enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0])" | Write-Output;
                    } else {
                        "enum $Code : $(ConvertTo-TypeSyntax -Type $UnderlyingType[0])" | Write-Output;
                    }
                }
                $Fields | ForEach-Object { "  $_" | Write-Output }
            } else {
                if ($UnderlyingType.Count -gt 0) {
                    $UnderlyingType = @($UnderlyingType | Where-Object {
                        $ElementType -ne [string] -and $ElementType -ne [object] -and $ElementType -ne [decimal] -and -not $ElementType.IsPrimitive
                    });
                }
                if ($UnderlyingType.Count -eq 0) {
                    [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax[]]$Interfaces = @();
                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax[]]$Methods = @();
                    [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax[]]$Properties = @();
                    [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax[]]$Events = @();
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        if ($IgnoreInherited.IsPresent) {
                            $BaseNames = @($Type.BaseType.GetInterfaces() | ForEach-Object { $_.FullName });
                            $Interfaces = @($Type.GetInterfaces() | Where-Object { $BaseNames -cnotcontains $_.FullName } | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace);
                            $BaseNames = @($Type.BaseType.GetMethods() | ForEach-Object { $_.ToString() });
                            $Methods = @($Type.GetMethods() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-MethodDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $BaseNames = @($Type.BaseType.GetProperties() | ForEach-Object { $_.ToString() });
                            $Properties = @($Type.GetProperties() | Where-Object {
                                if ($BaseNames -ccontains $_.ToString()) { return $false }
                                $M = $_.GetGetMethod();
                                if ($null -eq $M) {
                                    $M = $_.GetSetMethod();
                                    return $null -eq $M -or $M.DeclaringType -eq $Type;
                                }
                                return $M.DeclaringType -eq $Type
                            } | ConvertTo-PropertyDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $BaseNames = @($Type.BaseType.GetEvents() | ForEach-Object { $_.ToString() });
                            $Events = @($Type.GetEvents() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-EventDeclarationSyntax -UsingNamespace $UsingNamespace);
                        } else {
                            $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $Interfaces = @($Type.GetInterfaces() | ConvertTo-TypeSyntax -UsingNamespace $UsingNamespace);
                            $Methods = @($Type.GetMethods() | ConvertTo-MethodDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $Properties = @($Type.GetProperties() | ConvertTo-PropertyDeclarationSyntax -UsingNamespace $UsingNamespace);
                            $Events = @($Type.GetEvents() | ConvertTo-EventDeclarationSyntax -UsingNamespace $UsingNamespace);
                        }
                    } else {
                        if ($IgnoreInherited.IsPresent) {
                            $BaseNames = @($Type.BaseType.GetInterfaces() | ForEach-Object { $_.FullName });
                            $Interfaces = @($Type.GetInterfaces() | Where-Object { $BaseNames -cnotcontains $_.FullName } | ConvertTo-TypeSyntax);
                            $BaseNames = @($Type.BaseType.GetMethods() | ForEach-Object { $_.ToString() });
                            $Methods = @($Type.GetMethods() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-MethodDeclarationSyntax);
                            $BaseNames = @($Type.BaseType.GetProperties() | ForEach-Object { $_.ToString() });
                            $Properties = @($Type.GetProperties() | Where-Object {
                                if ($BaseNames -ccontains $_.ToString()) { return $false }
                                $M = $_.GetGetMethod();
                                if ($null -eq $M) {
                                    $M = $_.GetSetMethod();
                                    return $null -eq $M -or $M.DeclaringType -eq $Type;
                                }
                                return $M.DeclaringType -eq $Type
                            } | ConvertTo-PropertyDeclarationSyntax);
                            $BaseNames = @($Type.BaseType.GetEvents() | ForEach-Object { $_.ToString() });
                            $Events = @($Type.GetEvents() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.ToString() } | ConvertTo-EventDeclarationSyntax);
                        } else {
                            $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                            $Interfaces = @($Type.GetInterfaces() | ConvertTo-TypeSyntax);
                            $Methods = @($Type.GetMethods() | ConvertTo-MethodDeclarationSyntax);
                            $Properties = @($Type.GetProperties() | ConvertTo-PropertyDeclarationSyntax);
                            $Events = @($Type.GetEvents() | ConvertTo-EventDeclarationSyntax);
                        }
                    }
                    if ($Type.IsInterface) {
                        if ($Interfaces.Count -gt 0) {
                            if ($Type.IsPublic) {
                                "public interface $Code :" | Write-Output;
                            } else {
                                "interface $Code :" | Write-Output;
                            }
                            ($Interfaces | Select-Object -SkipLast 1) | ForEach-Object { "    $_," | Write-Output }
                            "    $($Interfaces[$Interfaces.Count - 1])" | Write-Output;
                        } else {
                            if ($Type.IsPublic) {
                                "public interface $Code" | Write-Output;
                            } else {
                                "interface $Code" | Write-Output;
                            }
                        }
                        $Events | ForEach-Object { "  $_" | Write-Output }
                        $Properties | ForEach-Object { "  $_" | Write-Output }
                        $Methods | ForEach-Object { "  $_" | Write-Output }
                    } else {
                        [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax[]]$Fields = @();
                        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax[]]$Constructors = @();
                        if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                            if ($IgnoreInherited.IsPresent) {
                                $BaseNames = @($Type.BaseType.GetFields() | ForEach-Object { $_.Name });
                                $Fields = @($Type.GetFields() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.Name } | ConvertTo-FieldDeclarationSyntax -UsingNamespace $UsingNamespace);
                                $Constructors = @($Type.GetConstructors() | Where-Object { $_.DeclaringType -eq $Type } | ConvertTo-ConstructorDeclarationSyntax -UsingNamespace $UsingNamespace);
                            } else {
                                $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                                $Constructors = @($Type.GetConstructors() | ConvertTo-ConstructorDeclarationSyntax);
                            }
                        } else {
                            if ($IgnoreInherited.IsPresent) {
                                $BaseNames = @($Type.BaseType.GetFields() | ForEach-Object { $_.Name });
                                $Fields = @($Type.GetFields() | Where-Object { $_.DeclaringType -eq $Type -and $BaseNames -cnotcontains $_.Name } | ConvertTo-FieldDeclarationSyntax);
                                $Constructors = @($Type.GetConstructors() | Where-Object { $_.DeclaringType -eq $Type } | ConvertTo-ConstructorDeclarationSyntax);
                            } else {
                                $Fields = @($Type.GetFields() | ConvertTo-FieldDeclarationSyntax);
                                $Constructors = @($Type.GetConstructors() | ConvertTo-ConstructorDeclarationSyntax);
                            }
                        }
                        if ($Type.IsValueType) {
                            if ($Interfaces.Count -gt 0) {
                                if ($Type.IsPublic) {
                                    "public struct $Code :" | Write-Output;
                                } else {
                                    "struct $Code :" | Write-Output;
                                }
                                ($Interfaces | Select-Object -SkipLast 1) | ForEach-Object { "    $_," | Write-Output }
                                "    $($Interfaces[$Interfaces.Count - 1])" | Write-Output;
                            } else {
                                if ($Type.IsPublic) {
                                    "public struct $Code" | Write-Output;
                                } else {
                                    "struct $Code" | Write-Output;
                                }
                            }
                        } else {
                            if ($Type.IsClass) {
                                if ($Type.IsSealed) {
                                    if ($Type.IsPublic) {
                                        $Code = "public sealed class $Code";
                                    } else {
                                        $Code = "sealed class $Code" | Write-Output;
                                    }
                                } else {
                                    if ($Type.IsPublic) {
                                        $Code = "public class $Code";
                                    } else {
                                        $Code = "class $Code";
                                    }
                                }
                            }
                            $BaseType = $Type.BaseType;
                            if ($Interfaces.Count -gt 0) {
                                "$Code :" | Write-Output;
                                if ($null -ne $BaseType) {
                                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType -UsingNamespace $UsingNamespace)," | Write-Output;
                                    } else {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType)," | Write-Output;
                                    }
                                }
                                ($Interfaces | Select-Object -SkipLast 1) | ForEach-Object { "    $_," | Write-Output }
                                "    $($Interfaces[$Interfaces.Count - 1])" | Write-Output;
                            } else {
                                if ($null -ne $BaseType) {
                                    "$Code :" | Write-Output;
                                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType -UsingNamespace $UsingNamespace)" | Write-Output;
                                    } else {
                                        "    $(ConvertTo-TypeSyntax -Type $BaseType)" | Write-Output;
                                    }
                                } else {
                                    $Code | Write-Output;
                                }
                            }
                        }
                    }
                    $Fields | ForEach-Object { "  $_" | Write-Output }
                    $Events | ForEach-Object { "  $_" | Write-Output }
                    $Properties | ForEach-Object { "  $_" | Write-Output }
                    $Constructors | ForEach-Object { "  $_" | Write-Output }
                    $Methods | ForEach-Object { "  $_" | Write-Output }
                } else {
                    if ($Type.IsInterface) {
                        if ($Type.IsPublic) {
                            "public interface $Code" | Write-Output;
                        } else {
                            "interface $Code" | Write-Output;
                        }
                    } else {
                        if ($Type.IsValueType) {
                            if ($Type.IsPublic) {
                                "public struct $Code" | Write-Output;
                            } else {
                                "struct $Code" | Write-Output;
                            }
                        } else {
                            if ($Type.IsClass) {
                                if ($Type.IsSealed) {
                                    if ($Type.IsPublic) {
                                        "public sealed class $Code" | Write-Output;
                                    } else {
                                        "sealed class $Code" | Write-Output;
                                    }
                                } else {
                                    if ($Type.IsPublic) {
                                        "public class $Code" | Write-Output;
                                    } else {
                                        "class $Code" | Write-Output;
                                    }
                                }
                            } else {
                                if ($Type.IsPublic) {
                                    "public $Code" | Write-Output;
                                } else {
                                    $Code | Write-Output;
                                }
                            }
                        }
                    }
                    if ($PSBoundParameters.ContainsKey('UsingNamespace')) {
                        if ($IgnoreInherited.IsPresent) {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ -UsingNamespace $UsingNamespace -IgnoreInherited } | ForEach-Object { "  $_" | Write-Output }
                        } else {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ -UsingNamespace $UsingNamespace } | ForEach-Object { "  $_" | Write-Output }
                        }
                    } else {
                        if ($IgnoreInherited.IsPresent) {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ -IgnoreInherited } | ForEach-Object { "  $_" | Write-Output }
                        } else {
                            $UnderlyingType | ForEach-Object { Get-TypeMemberInfo -Type $_ } | ForEach-Object { "  $_" | Write-Output }
                        }
                    }
                }
            }
        }
    }
}

Function Get-ExceptionObject {
    [CmdletBinding(DefaultParameterSetName = 'WcPath')]
    [OutputType([System.Xml.Linq.XDocument])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyCollection()]
        [object[]]$InputObject
    )

    Process {
        if ($null -ne $InputObject -and $InputObject.Length -gt 0) {
            foreach ($obj in $InputObject) {
                if ($null -ne $obj) {
                    if ($obj -is [System.Management.Automation.ErrorRecord]) {
                        Get-ExceptionObject -InputObject $obj.Exception;
                    } else {
                        if ($obj -is [System.Exception]) {
                            $e = $Obj;
                            if (($obj -is [System.Management.Automation.MethodInvocationException] -or $obj -is [System.Management.Automation.GetValueInvocationException] `
                                    -or $obj -is [System.Management.Automation.SetValueInvocationException]) -and $null -ne $obj.InnerException) {
                                $e = $obj.InnerException;
                            }
                            if ($e -is [System.AggregateException] -and $e.InnerExceptions.Count -eq 1) {
                                Write-Output -InputObject $e.InnerException -NoEnumerate;
                            } else {
                                Write-Output -InputObject $e -NoEnumerate;
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Optimize-PathString {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string[]]$Path
    )

    Process {
        $Path | ForEach-Object {
            $Parent = $_ | Split-Path -Parent;
            if ([string]::IsNullOrEmpty($Parent)) {
                $_ | Write-Output;
            } else {
                $Leaf = $_ | Split-Path -Leaf;
                if ([string]::IsNullOrEmpty($Leaf)) {
                    ($Parent | Optimize-PathInfo) | Write-Output;
                } else {
                    ($Parent | Join-Path -ChildPath $Leaf) | Write-Output;
                }
            }
        }
    }
}

Function Test-IsNotEmptyString {
    [CmdletBinding()]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString()]
        [AllowNull()]
        [string]$Value
    )

    Process {
        if (-not [string]::IsNullOrWhiteSpace($Value)) {
            $true | Write-Output;
            continue;
        }
    }

    End { $false | Write-Output }
}

Function Test-ContainsPath {
    [CmdletBinding()]
    [OutputType([bool])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Management.Automation.PathInfo]$InputPath,

        [Parameter(Mandatory = $true)]
        [System.Management.Automation.PathInfo]$Target
    )


    Process {
        if ($InputPath.Provider -eq $Target.Provider) {
            if ($InputPath.Path -ieq $Target.Path) {
                $true | Write-Output;
                continue;
            }
            if ($InputPath.Path.Length -lt $Target.Path.Length) {
                $p
            }
        }
        if (-not [string]::IsNullOrWhiteSpace($Value)) {
            $true | Write-Output;
            continue;
        }
    }

    End { $false | Write-Output }
}

class ModelDefinitionsTypeMapper : DevUtil.TypeMapper {
    hidden static [string]$PREFIX = 'md';
    hidden static [string]$XMLNS = 'http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd';
    hidden static [System.Xml.Linq.XNamespace]$NAMESPACE = [System.Xml.Linq.XNamespace]::Get([ModelDefinitionsTypeMapper]::XMLNS);
    ModelDefinitionsTypeMapper() : base([ModelDefinitionsTypeMapper]::PREFIX, [ModelDefinitionsTypeMapper]::XMLNS) { }
    static [System.Xml.Linq.XName] GetXName([string]$NCName) {
        if ([string]::IsNullOrEmpty($NCName)) { throw [System.ArgumentException]::new("Name cannot be empty", "NCName") }
        return [ModelDefinitionsTypeMapper]::NAMESPACE.GetName([System.Xml.XmlConvert]::VerifyNCName($NCName));
    }
    hidden static [Type] GetElementType([Type]$ConstructedGenericType)
    {
        $a = $ConstructedGenericType.GetGenericArguments();
        if ($a.Length -eq 1) {
            $i = [System.Collections.Generic.IEnumerable`1].MakeGenericType($a[0]);
            if ($i -ceq $ConstructedGenericType -or $ConstructedGenericType.GetInterfaces() -ccontains $i) { return $a[0] }
        }
        return $null;
    }
    hidden static [bool] HasElementType([Type]$ConstructedGenericType)
    {
        $a = $ConstructedGenericType.GetGenericArguments();
        $i = [System.Collections.Generic.IEnumerable`1].MakeGenericType($a[0]);
        return $a.Length -eq 1 -and ($i -ceq $ConstructedGenericType -or $ConstructedGenericType.GetInterfaces() -ccontains $i);
    }
    hidden static [string] GetNCName([Type]$Type) {
        if ($Type -eq [Guid]) { return "GuidType" }
        if ($Type.IsEnum) { return "$($Type.Name)Type" }
        if ($Type -eq [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) { return 'SyntaxNodeType' }
        if ([Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($Type)) { return "$($Type.Name -replace 'Syntax$', '')Type" }
        return $null;
    }
    [string] GetNCNameOrNull([Type]$Type) {
        if ($null -eq $Type -or $Type.IsPointer -or $Type.IsByRef) { return $null }
        if ($Type.IsConstructedGenericType) {
            $ElementType = [ModelDefinitionsTypeMapper]::GetElementType($Type);
            if ($null -ne $ElementType -and -not $ElementType.IsGenericType) {
                $ElementName = [ModelDefinitionsTypeMapper]::GetNCName($ElementType);
                if ($null -ne $ElementName) { return "$($ElementName)List" }
            }
        } else {
            if (-not $Type.IsGenericType) { return [ModelDefinitionsTypeMapper]::GetNCName($Type) }
        }
        return $null;
    }
    [bool] CanMapToXsdType([Type]$Type, [string]$NCName) {
        if ($null -eq $Type) { throw [System.ArgumentNullException]::new('Type') }
        if ([string]::IsNullOrEmpty($NCName)) { return $false }
        return $this.GetNCNameOrNull($Type) -ceq $NCName;
    }
    [bool] IsMappedType([Type]$Type) {
        if ($null -eq $Type -or $Type.IsPointer -or $Type.IsByRef) { return $false }
        if ($Type.IsConstructedGenericType) {
            if ($null -eq ($Type = [ModelDefinitionsTypeMapper]::GetElementType($Type))) { return $false }
        }
        return (-not $Type.IsGenericType) -and ($Type -eq [Guid] -or $Type.IsEnum -or [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode].IsAssignableFrom($Type));
    }
}

Function New-ModelDefinitionDocument {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XDocument])]
    Param()
    Write-Output -InputObject ([System.Xml.Linq.XDocument]::new([System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ModelDefinitions))) -NoEnumerate;
}

Function Get-ModelDefinitionSourceElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string[]]$Path,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$Document
    )

    Begin {
        $PathXName = [System.Xml.Linq.XNamespace]::None.GetName('Path');
        $SourceElements = @(
            $Document.Root.Elements([System.Xml.Linq.XName]::Get($Script:ModelDefinitionNames.Sources)) | ForEach-Object {
                $_.Elements($Script:ModelDefinitionNames.Source) | Write-Output;
            }
        ) | Group-Object -Property @{ Expression = { $_.Attribute($PathXName).Value } } -AsHashTable;
        if ($SourceElements.Count -eq 0) { continue; }
    }
    Process {
        $Path | ForEach-Object {
            $e = $SourceElements[$_];
            if ($null -ne $e) { Write-Output -InputObject $e -NoEnumerate }
        }
    }
}

Function Import-DocumentationCommentTriviaSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax]$DocumentationCommentTrivia
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.DocumentationComment);
        $e = $DocumentationCommentTrivia.Content.Count - 1;
        if ($e -gt 0) {
            $StringBuilder = [System.Text.StringBuilder]::new();
            for ($i = 0; $i -lt $e; $i++) {
                $StringBuilder.AppendLine($DocumentationCommentTrivia.Content[$i].ToString()) | Out-Null;
            }
            $Text = $StringBuilder.Append($DocumentationCommentTrivia.Content[$e]).ToString();
            [System.Xml.Linq.XNode[]]$Nodes;
            try {
                $Nodes = @([System.Xml.Linq.XElement]::Parse("<doc>$Text</doc>").Nodes());
            } catch {
                $Nodes = @([System.Xml.Linq.XCData]::new($Text));
            }
            $Nodes | ForEach-Object { $Element.Add($_) }
        }
        Write-Output -InputObject $Element -NoEnumerate;
    }
}

Function Import-StructuredTriviaSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax]$StructuredTrivia
    )

    Process {
        switch ($StructuredTrivia) {
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DirectiveTriviaSyntax] } { (Import-DirectiveTriviaSyntax -Argument $DirectiveTrivia) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SkippedTokensTriviaSyntax] } { (Import-SkippedTokensTriviaSyntax -Argument $SkippedTokensTrivia) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax] } { (Import-DocumentationCommentTriviaSyntax -Argument $DocumentationCommentTrivia) | Write-Output; break; }
            default {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownStructuredTriviaSyntax);
                Set-SyntaxNodeContents -SyntaxNode $SyntaxNode -Element $XElement -IsUnknown;
                Write-Output -InputObject $XElement -NoEnumerate;
                break;
            }
        }
    }
}

Function Import-AttributeListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax]$AttributeList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AttributeList);
        Set-SyntaxNodeContents -SyntaxNode $AttributeList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax :
                    Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenBracketToken
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax Target
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax] Attributes
            Microsoft.CodeAnalysis.SyntaxToken CloseBracketToken

            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax WithOpenBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax WithTarget(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax WithAttributes(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax WithCloseBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax AddAttributes(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax[])
        #>
    }
}

Function Import-AliasQualifiedNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax]$AliasQualifiedName
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AliasQualifiedName);
        Set-NameSyntaxContents -Name $AliasQualifiedName -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax Alias
            Microsoft.CodeAnalysis.SyntaxToken ColonColonToken
            Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax Name

            Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax WithAlias(Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax WithColonColonToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax)
        #>
    }
}

Function Import-QualifiedNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax]$QualifiedName
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.QualifiedName);
        Set-NameSyntaxContents -Name $QualifiedName -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Left
            Microsoft.CodeAnalysis.SyntaxToken DotToken
            Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax Right

            Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax WithLeft(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax WithDotToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax WithRight(Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax)
        #>
    }
}

Function Import-GenericNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax]$GenericName
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.GenericName);
        Set-SimpleNameSyntaxContents -SimpleName $GenericName -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Boolean IsUnboundGenericName
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax TypeArgumentList

            Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax WithTypeArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax AddTypeArgumentListArguments(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax[])
        #>
    }
}

Function Import-IdentifierNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax]$IdentifierName
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.IdentifierName);
        Set-SimpleNameSyntaxContents -SimpleName $IdentifierName -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax Update(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-SimpleNameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax]$SimpleName
    )

    Process {
        switch ($SimpleName) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax] } { (Import-GenericNameSyntax -GenericName $SimpleName) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax] } { (Import-IdentifierNameSyntax -IdentifierName $SimpleName) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownSimpleNameSyntax);
                Set-SimpleNameSyntaxContents -SimpleName $SimpleName -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-SimpleNameSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax]$SimpleName,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-NameSyntaxContents -Name $SimpleName -Element $Element -IsUnknown;
    } else {
        Set-NameSyntaxContents -Name $SimpleName -Element $Element;
    }

    <#
        BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax :
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax,
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SyntaxToken Identifier

        Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
    #>
}

Function Import-NameSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax]$Name
    )

    Process {
        switch ($Name) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax] } { (Import-AliasQualifiedNameSyntax -AliasQualifiedName $Name) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax] } { (Import-QualifiedNameSyntax -QualifiedName $Name) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleNameSyntax] } { (Import-SimpleNameSyntax -SimpleName $Name) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownNameSyntax);
                Set-NameSyntaxContents -Name $Name -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-NameSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax]$Name,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-TypeSyntaxContents -Type $Name -Element $Element -IsUnknown;
    } else {
        Set-TypeSyntaxContents -Type $Name -Element $Element;
    }

    <#
        BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Int32 Arity
    #>
}

Function Import-RefTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax]$RefType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.RefType);
        Set-TypeSyntaxContents -Type $RefType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken RefKeyword
            Microsoft.CodeAnalysis.SyntaxToken ReadOnlyKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type

            Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax WithRefKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax WithReadOnlyKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
        #>
    }
}

Function Import-PredefinedTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax]$PredefinedType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.PredefinedType);
        Set-TypeSyntaxContents -Type $PredefinedType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Keyword

            Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax Update(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax WithKeyword(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-ArrayTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax]$ArrayType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ArrayType);
        Set-TypeSyntaxContents -Type $ArrayType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ElementType
            Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax] RankSpecifiers

            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax WithElementType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax WithRankSpecifiers(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax AddRankSpecifiers(Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax[])
        #>
    }
}

Function Import-PointerTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax]$PointerType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.PointerType);
        Set-TypeSyntaxContents -Type $PointerType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ElementType
            Microsoft.CodeAnalysis.SyntaxToken AsteriskToken

            Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax WithElementType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax WithAsteriskToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-FunctionPointerTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax]$FunctionPointerType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.FunctionPointerType);
        Set-TypeSyntaxContents -Type $FunctionPointerType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken DelegateKeyword
            Microsoft.CodeAnalysis.SyntaxToken AsteriskToken
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax CallingConvention
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax ParameterList

            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax WithDelegateKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax WithAsteriskToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax WithCallingConvention(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax[])
        #>
    }
}

Function Import-NullableTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax]$NullableType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.NullableType);
        Set-TypeSyntaxContents -Type $NullableType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ElementType
            Microsoft.CodeAnalysis.SyntaxToken QuestionToken

            Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax WithElementType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax WithQuestionToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-TupleTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax]$TupleType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.TupleType);
        Set-TypeSyntaxContents -Type $TupleType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenParenToken
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax] Elements
            Microsoft.CodeAnalysis.SyntaxToken CloseParenToken

            Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax WithOpenParenToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax WithElements(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax WithCloseParenToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax AddElements(Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax[])
        #>
    }
}

Function Import-OmittedTypeArgumentSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax]$OmittedTypeArgument
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.OmittedTypeArgument);
        Set-TypeSyntaxContents -Type $OmittedTypeArgument -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OmittedTypeArgumentToken

            TResult Accept[TResult](Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1[TResult])
            Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax Update(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax WithOmittedTypeArgumentToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-TypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type
    )

    Process {
        switch ($Type) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax] } { (Import-NameSyntax -Name $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax] } { (Import-RefTypeSyntax -RefType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax] } { (Import-PredefinedTypeSyntax -PredefinedType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax] } { (Import-ArrayTypeSyntax -ArrayType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax] } { (Import-PointerTypeSyntax -PointerType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax] } { (Import-FunctionPointerTypeSyntax -FunctionPointerType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax] } { (Import-NullableTypeSyntax -NullableType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax] } { (Import-TupleTypeSyntax -TupleType $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax] } { (Import-OmittedTypeArgumentSyntax -OmittedTypeArgument $Type) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownTypeSyntax);
                Set-TypeSyntaxContents -Type $Type -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-TypeSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax]$Type,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $Type -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $Type -Element $Element;
    }

    <#
        BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax :
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax,
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Boolean IsVar
        Boolean IsUnmanaged
        Boolean IsNotNull
        Boolean IsNint
        Boolean IsNuint
    #>
}

Function Import-FieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]$Field
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Field);
        Set-BaseFieldDeclarationSyntaxContents -BaseField $Field -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax, Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-EventFieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]$EventField
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.EventField);
        Set-BaseFieldDeclarationSyntaxContents -BaseField $EventField -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken EventKeyword

            Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax WithEventKeyword(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BaseFieldDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$BaseField
    )

    Process {
        switch ($BaseField) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax] } { (Import-FieldDeclarationSyntax -Field $BaseField) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax] } { (Import-EventFieldDeclarationSyntax -EventField $BaseField) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseFieldDeclarationSyntax);
                Set-BaseFieldDeclarationSyntaxContents -BaseField $BaseField -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseFieldDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$BaseField,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseField -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseField -Element $Element;
    }

    <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax Declaration
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax WithDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax AddDeclarationVariables(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax[])
            Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
    #>
}

Function Import-ConstructorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax]$Constructor
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Constructor);
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Constructor -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax Initializer

            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithInitializer(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax)
        #>
    }
}

Function Import-ConversionOperatorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]$ConversionOperator
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ConversionOperator);
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $ConversionOperator -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken ImplicitOrExplicitKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            Microsoft.CodeAnalysis.SyntaxToken OperatorKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type

            Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithImplicitOrExplicitKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithOperatorKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
        #>
    }
}

Function Import-DestructorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]$Destructor
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Destructor);
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Destructor -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken TildeToken
            Microsoft.CodeAnalysis.SyntaxToken Identifier

            Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithTildeToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-MethodDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]$Method
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Method);
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Method -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Int32 Arity
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ReturnType
            Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax TypeParameterList
            Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax] ConstraintClauses

            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax], Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithReturnType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithConstraintClauses(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddTypeParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[])
            Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddConstraintClauses(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax[])
        #>
    }
}

Function Import-OperatorDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]$Operator
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Operator);
        Set-BaseMethodDeclarationSyntaxContents -BaseMethod $Operator -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ReturnType
            Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
            Microsoft.CodeAnalysis.SyntaxToken OperatorKeyword
            Microsoft.CodeAnalysis.SyntaxToken OperatorToken

            Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithReturnType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithOperatorKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithOperatorToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BaseMethodDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$BaseMethod
    )

    Process {
        switch ($BaseMethod) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax] } { (Import-ConstructorDeclarationSyntax -Constructor $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] } { (Import-ConversionOperatorDeclarationSyntax -ConversionOperator $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax] } { (Import-DestructorDeclarationSyntax -Destructor $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } { (Import-MethodDeclarationSyntax -Method $BaseMethod) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] } { (Import-OperatorDeclarationSyntax -Operator $BaseMethod) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseMethodDeclarationSyntax);
                Set-BaseMethodDeclarationSyntaxContents -BaseMethod $BaseMethod -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseMethodDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$BaseMethod,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseMethod -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseMethod -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax ParameterList
        Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax Body
        Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody
        Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

        Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithBody(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddBodyAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddBodyStatements(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
    #>
}

Function Import-EventDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]$Event
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Event);
        Set-BasePropertyDeclarationSyntaxContents -BaseProperty $Event -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken EventKeyword
            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithEventKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-IndexerDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]$Indexer
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Indexer);
        Set-BasePropertyDeclarationSyntaxContents -BaseProperty $Indexer -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Semicolon
            Microsoft.CodeAnalysis.SyntaxToken ThisKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax ParameterList
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithSemicolon(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithThisKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[])
        #>
    }
}

Function Import-PropertyDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]$Property
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Property);
        Set-BasePropertyDeclarationSyntaxContents -BaseProperty $Property -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Semicolon
            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody
            Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax Initializer
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithSemicolon(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithInitializer(Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BasePropertyDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$BaseProperty
    )

    Process {
        switch ($BaseProperty) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] } { (Import-EventDeclarationSyntax -Event $BaseProperty) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } { (Import-IndexerDeclarationSyntax -Indexer $BaseProperty) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } { (Import-PropertyDeclarationSyntax -Property $BaseProperty) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBasePropertyDeclarationSyntax);
                Set-BasePropertyDeclarationSyntaxContents -BaseProperty $BaseProperty -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BasePropertyDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$BaseProperty,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseProperty -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseProperty -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type
        Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier
        Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax AccessorList

        Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithAccessorList(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax AddAccessorListAccessors(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax[])
    #>
}

Function Import-DelegateDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Delegate
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Delegate);
        Set-MemberDeclarationSyntaxContents -Member $Delegate -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Int32 Arity
            Microsoft.CodeAnalysis.SyntaxToken DelegateKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ReturnType
            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax TypeParameterList
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax ParameterList
            Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax] ConstraintClauses
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithDelegateKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithReturnType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithConstraintClauses(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddTypeParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[])
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[])
            Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddConstraintClauses(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax[])
        #>
    }
}

Function Import-EnumMemberDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax]$EnumMember
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.EnumMember);
        Set-MemberDeclarationSyntaxContents -Member $EnumMember -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax EqualsValue

            Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax WithEqualsValue(Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax)
        #>
    }
}

Function Import-GlobalStatementSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax]$GlobalStatement
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.GlobalStatement);
        Set-MemberDeclarationSyntaxContents -Member $GlobalStatement -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax Statement

            Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax WithStatement(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax)
        #>
    }
}

Function Import-NamespaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$Namespace
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Namespace);
        Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespace $Namespace -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenBraceToken
            Microsoft.CodeAnalysis.SyntaxToken CloseBraceToken
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax], Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax], Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax], Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax], Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithOpenBraceToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithCloseBraceToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-FileScopedNamespaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax]$FileScopedNamespace
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.FileScopedNamespace);
        Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespace $FileScopedNamespace -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax], Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax], Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BaseNamespaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$BaseNamespace
    )

    Process {
        switch ($BaseNamespace) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } { (Import-NamespaceDeclarationSyntax -Namespace $BaseNamespace) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax] } { (Import-FileScopedNamespaceDeclarationSyntax -FileScopedNamespace $BaseNamespace) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseNamespaceDeclarationSyntax);
                Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespace $BaseNamespace -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseNamespaceDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$BaseNamespace,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseNamespace -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseNamespace -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SyntaxToken NamespaceKeyword
        Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Name
        Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax] Externs
        Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax] Usings
        Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax] Members

        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithNamespaceKeyword(Microsoft.CodeAnalysis.SyntaxToken)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithExterns(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddExterns(Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithUsings(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddUsings(Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax[])
    #>
}

Function Import-RecordDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]$Record
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Record);
        Set-TypeDeclarationSyntaxContents -Type $Record -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken ClassOrStructKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax ParameterList

            Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax WithClassOrStructKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[])
        #>
    }
}

Function Import-ClassDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$Class
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Class);
        Set-TypeDeclarationSyntaxContents -Type $Class -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-StructDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]$Struct
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Struct);
        Set-TypeDeclarationSyntaxContents -Type $Struct -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-InterfaceDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$Interface
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Interface);
        Set-TypeDeclarationSyntaxContents -Type $Interface -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)

        #>
    }
}

Function Import-TypeDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]$Type
    )

    Process {
        switch ($Type) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] } { (Import-RecordDeclarationSyntax -Record $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] } { (Import-ClassDeclarationSyntax -Class $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] } { (Import-StructDeclarationSyntax -Struct $Type) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] } { (Import-InterfaceDeclarationSyntax -Interface $Type) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownTypeDeclarationSyntax);
                Set-TypeDeclarationSyntaxContents -Type $Type -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-TypeDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]$Type,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-BaseTypeDeclarationSyntaxContents -BaseType $Type -Element $Element -IsUnknown;
    } else {
        Set-BaseTypeDeclarationSyntaxContents -BaseType $Type -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax :
            Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Int32 Arity
        Microsoft.CodeAnalysis.SyntaxToken Keyword
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax TypeParameterList
        Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax] ConstraintClauses
        Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax] Members

        Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithKeyword(Microsoft.CodeAnalysis.SyntaxToken)
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddTypeParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithConstraintClauses(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddConstraintClauses(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax[])
    #>
}

Function Import-EnumDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$Enum
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Enum);
        Set-BaseTypeDeclarationSyntaxContents -BaseType $Enum -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken EnumKeyword
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax] Members

            Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax WithEnumKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax[])
        #>
    }
}

Function Import-BaseTypeDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]$BaseType
    )

    Process {
        switch ($BaseType) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax] } { (Import-TypeDeclarationSyntax -Type $BaseType) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] } { (Import-EnumDeclarationSyntax -Enum $BaseType) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseTypeDeclarationSyntax);
                Set-BaseTypeDeclarationSyntaxContents -BaseType $BaseType -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseTypeDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]$BaseType,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-MemberDeclarationSyntaxContents -Member $BaseType -Element $Element -IsUnknown;
    } else {
        Set-MemberDeclarationSyntaxContents -Member $BaseType -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SyntaxToken Identifier
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax BaseList
        Microsoft.CodeAnalysis.SyntaxToken OpenBraceToken
        Microsoft.CodeAnalysis.SyntaxToken CloseBraceToken
        Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithBaseList(Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax AddBaseListTypes(Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithOpenBraceToken(Microsoft.CodeAnalysis.SyntaxToken)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithCloseBraceToken(Microsoft.CodeAnalysis.SyntaxToken)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
    #>
}

Function Import-IncompleteMemberSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax]$IncompleteMember
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.IncompleteMember);
        Set-MemberDeclarationSyntaxContents -Member $IncompleteMember -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type

            Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
        #>
    }
}

Function Import-MemberDeclarationSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Member
    )

    Process {
        switch ($Member) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax] } { (Import-BaseFieldDeclarationSyntax -BaseField $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] } { (Import-BaseMethodDeclarationSyntax -BaseMethod $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] } { (Import-BasePropertyDeclarationSyntax -BaseProperty $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax] } { (Import-DelegateDeclarationSyntax -Delegate $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax] } { (Import-EnumMemberDeclarationSyntax -EnumMember $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax] } { (Import-GlobalStatementSyntax -GlobalStatement $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } { (Import-BaseNamespaceDeclarationSyntax -BaseNamespace $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] } { (Import-BaseTypeDeclarationSyntax -BaseType $Member) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax] } { (Import-IncompleteMemberSyntax -IncompleteMember $Member) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownMemberDeclarationSyntax);
                Set-MemberDeclarationSyntaxContents -Member $Member -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-MemberDeclarationSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Member,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $Member -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $Member -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax] AttributeLists
        Microsoft.CodeAnalysis.SyntaxTokenList Modifiers

        Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList)
        Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[])
    #>
}

Function Import-UsingDirectiveSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax]$UsingDirective
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UsingDirective);
        Set-SyntaxNodeContents -SyntaxNode $UsingDirective -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken GlobalKeyword
            Microsoft.CodeAnalysis.SyntaxToken UsingKeyword
            Microsoft.CodeAnalysis.SyntaxToken StaticKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax Alias
            Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Name
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithGlobalKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithUsingKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithStaticKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithAlias(Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-ExternAliasDirectiveSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax]$ExternAliasDirective
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ExternAliasDirective);
        Set-SyntaxNodeContents -SyntaxNode $ExternAliasDirective -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken ExternKeyword
            Microsoft.CodeAnalysis.SyntaxToken AliasKeyword
            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.SyntaxToken SemicolonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithExternKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithAliasKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-AttributeTargetSpecifierSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax]$AttributeTargetSpecifier
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AttributeTargetSpecifier);
        Set-SyntaxNodeContents -SyntaxNode $AttributeTargetSpecifier -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.SyntaxToken ColonToken

            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax WithColonToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-AttributeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax]$Attribute
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Attribute);
        Set-SyntaxNodeContents -SyntaxNode $Attribute -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Name
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax ArgumentList

            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax WithArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax AddArgumentListArguments(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax[])
        #>
    }
}

Function Import-AttributeArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax]$AttributeArgumentList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AttributeArgumentList);
        Set-SyntaxNodeContents -SyntaxNode $AttributeArgumentList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenParenToken
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax] Arguments
            Microsoft.CodeAnalysis.SyntaxToken CloseParenToken

            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax WithOpenParenToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax WithArguments(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax WithCloseParenToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax AddArguments(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax[])
        #>
    }
}

Function Import-AttributeArgumentSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax]$AttributeArgument
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AttributeArgument);
        Set-SyntaxNodeContents -SyntaxNode $AttributeArgument -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax NameEquals
            Microsoft.CodeAnalysis.CSharp.Syntax.NameColonSyntax NameColon
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Expression

            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.NameColonSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax WithNameEquals(Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax WithNameColon(Microsoft.CodeAnalysis.CSharp.Syntax.NameColonSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax WithExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax)
        #>
    }
}

Function Import-BaseListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax]$BaseList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.BaseList);
        Set-SyntaxNodeContents -SyntaxNode $BaseList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken ColonToken
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax] Types

            Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax WithColonToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax WithTypes(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax AddTypes(Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax[])
        #>
    }
}

Function Import-SimpleBaseTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax]$SimpleBaseType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.SimpleBaseType);
        Set-BaseTypeSyntaxContents -BaseType $SimpleBaseType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
        #>
    }
}

Function Import-PrimaryConstructorBaseTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax]$PrimaryConstructorBaseType
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.PrimaryConstructorBaseType);
        Set-BaseTypeSyntaxContents -BaseType $PrimaryConstructorBaseType -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax ArgumentList

            Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax WithArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax AddArgumentListArguments(Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax[])
        #>
    }
}

Function Import-BaseTypeSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax]$BaseType
    )

    Process {
        switch ($BaseType) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax] } { (Import-SimpleBaseTypeSyntax -SimpleBaseType $BaseType) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax] } { (Import-PrimaryConstructorBaseTypeSyntax -PrimaryConstructorBaseType $BaseType) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseTypeSyntax);
                Set-BaseTypeSyntaxContents -BaseType $BaseType -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseTypeSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax]$BaseType,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $BaseType -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $BaseType -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type

        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
    #>
}

Function Import-ArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax]$ArgumentList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ArgumentList);
        Set-BaseArgumentListSyntaxContents -BaseArgumentList $ArgumentList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenParenToken
            Microsoft.CodeAnalysis.SyntaxToken CloseParenToken

            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax WithOpenParenToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax WithCloseParenToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BracketedArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax]$BracketedArgumentList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.BracketedArgumentList);
        Set-BaseArgumentListSyntaxContents -BaseArgumentList $BracketedArgumentList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType: Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax :
                Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax,
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenBracketToken
            Microsoft.CodeAnalysis.SyntaxToken CloseBracketToken

            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax WithOpenBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax WithCloseBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BaseArgumentListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax]$BaseArgumentList
    )

    Process {
        switch ($BaseArgumentList) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentListSyntax] } { (Import-ArgumentListSyntax -ArgumentList $BaseArgumentList) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax] } { (Import-BracketedArgumentListSyntax -BracketedArgumentList $BaseArgumentList) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseArgumentListSyntax);
                Set-BaseArgumentListSyntaxContents -BaseArgumentList $BaseArgumentList -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseArgumentListSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax]$BaseArgumentList,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $BaseArgumentList -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $BaseArgumentList -Element $Element;
    }

    <#
        BaseType: Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
            Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax] Arguments

        Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax WithArguments(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax AddArguments(Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax[])
    #>
}

# '

Function Import-ArgumentSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax]$Argument
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Argument);
        Set-SyntaxNodeContents -SyntaxNode $Argument -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken RefOrOutKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.NameColonSyntax NameColon
            Microsoft.CodeAnalysis.SyntaxToken RefKindKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax Expression

            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax WithRefOrOutKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.NameColonSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax WithNameColon(Microsoft.CodeAnalysis.CSharp.Syntax.NameColonSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax WithRefKindKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax WithExpression(Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax)
        #>
    }
}

Function Import-TypeParameterListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax]$TypeParameterList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.TypeParameterList);
        Set-SyntaxNodeContents -SyntaxNode $TypeParameterList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken LessThanToken
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax] Parameters
            Microsoft.CodeAnalysis.SyntaxToken GreaterThanToken

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax WithLessThanToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax WithParameters(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax WithGreaterThanToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax AddParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[])
        #>
    }
}

Function Import-TypeParameterConstraintClauseSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax]$TypeParameterConstraintClause
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.TypeParameterConstraintClause);
        Set-SyntaxNodeContents -SyntaxNode $TypeParameterConstraintClause -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken WhereKeyword
            Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax Name
            Microsoft.CodeAnalysis.SyntaxToken ColonToken
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax] Constraints

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax WithWhereKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax WithColonToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax WithConstraints(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax AddConstraints(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax[])
        #>
    }
}

Function Import-ClassOrStructConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax]$ClassOrStructConstraint
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ClassOrStructConstraint);
        Set-SyntaxNodeContents -SyntaxNode $ClassOrStructConstraint -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken ClassOrStructKeyword
            Microsoft.CodeAnalysis.SyntaxToken QuestionToken

            Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax Update(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax WithClassOrStructKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax WithQuestionToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-ConstructorConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]$ConstructorConstraint
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ConstructorConstraint);
        Set-SyntaxNodeContents -SyntaxNode $ConstructorConstraint -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken NewKeyword
            Microsoft.CodeAnalysis.SyntaxToken OpenParenToken
            Microsoft.CodeAnalysis.SyntaxToken CloseParenToken

            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax WithNewKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax WithOpenParenToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax WithCloseParenToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-TypeConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax]$TypeConstraint
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.TypeConstraint);
        Set-SyntaxNodeContents -SyntaxNode $TypeConstraint -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
        #>
    }
}

Function Import-DefaultConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultConstraintSyntax]$DefaultConstraint
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.DefaultConstraint);
        Set-SyntaxNodeContents -SyntaxNode $DefaultConstraint -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken DefaultKeyword

            Microsoft.CodeAnalysis.CSharp.Syntax.DefaultConstraintSyntax Update(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.DefaultConstraintSyntax WithDefaultKeyword(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-TypeParameterConstraintSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax]$TypeParameterConstraint
    )

    Process {
        switch ($TypeParameterConstraint) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax] } { (Import-ClassOrStructConstraintSyntax -ClassOrStructConstraint $TypeParameterConstraint) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax] } { (Import-ConstructorConstraintSyntax -ConstructorConstraint $TypeParameterConstraint) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax] } { (Import-TypeConstraintSyntax -TypeConstraint $TypeParameterConstraint) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultConstraintSyntax] } { (Import-DefaultConstraintSyntax -DefaultConstraint $TypeParameterConstraint) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownTypeParameterConstraintSyntax);
                Set-TypeParameterConstraintSyntaxContents -TypeParameterConstraint $TypeParameterConstraint -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Import-TypeParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax]$TypeParameter
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.TypeParameter);
        Set-SyntaxNodeContents -SyntaxNode $TypeParameter -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax] AttributeLists
            Microsoft.CodeAnalysis.SyntaxToken VarianceKeyword
            Microsoft.CodeAnalysis.SyntaxToken Identifier

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax WithVarianceKeyword(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[])
        #>
    }
}

Function Import-TupleElementSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax]$TupleElement
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.TupleElement);
        Set-SyntaxNodeContents -SyntaxNode $TupleElement -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type
            Microsoft.CodeAnalysis.SyntaxToken Identifier

            Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-ArrayRankSpecifierSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax]$ArrayRankSpecifier
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ArrayRankSpecifier);
        Set-SyntaxNodeContents -SyntaxNode $ArrayRankSpecifier -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
                Microsoft.CodeAnalysis.SyntaxNode

            Int32 Rank
            Microsoft.CodeAnalysis.SyntaxToken OpenBracketToken
            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax] Sizes
            Microsoft.CodeAnalysis.SyntaxToken CloseBracketToken

            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax WithOpenBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax WithSizes(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax WithCloseBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax AddSizes(Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax[])
        #>
    }
}

Function Import-ParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax]$Parameter
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Parameter);
        Set-BaseParameterSyntaxContents -BaseParameter $Parameter -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken Identifier
            Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax Default

            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax, Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax)
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax WithDefault(Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax)
        #>
    }
}

Function Import-FunctionPointerParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax]$FunctionPointerParameter
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.FunctionPointerParameter);
        Set-BaseParameterSyntaxContents -BaseParameter $FunctionPointerParameter -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax Update(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax], Microsoft.CodeAnalysis.SyntaxTokenList, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
        #>
    }
}

Function Import-BaseParameterSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax]$BaseParameter
    )

    Process {
        switch ($BaseParameter) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax] } { (Import-ParameterSyntax -Parameter $BaseParameter) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax] } { (Import-FunctionPointerParameterSyntax -FunctionPointerParameter $BaseParameter) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseParameterSyntax);
                Set-BaseParameterSyntaxContents -BaseParameter $BaseParameter -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseParameterSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax]$BaseParameter,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $BaseParameter -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $BaseParameter -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
            Microsoft.CodeAnalysis.SyntaxNode

        Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax] AttributeLists
        Microsoft.CodeAnalysis.SyntaxTokenList Modifiers
        Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type

        Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList)
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[])
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax)
    #>
}

Function Import-ParameterListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax]$ParameterList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ParameterList);
        Set-BaseParameterListSyntaxContents -BaseParameterList $ParameterList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenParenToken
            Microsoft.CodeAnalysis.SyntaxToken CloseParenToken

            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax WithOpenParenToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax WithCloseParenToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BracketedParameterListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax]$BracketedParameterList
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.BracketedParameterList);
        Set-BaseParameterListSyntaxContents -BaseParameterList $BracketedParameterList -Element $Element;
        Write-Output -InputObject $Element -NoEnumerate;
        <#
            BaseType Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax :
                Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode,
                Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SyntaxToken OpenBracketToken
            Microsoft.CodeAnalysis.SyntaxToken CloseBracketToken

            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax Update(Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax], Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax WithOpenBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax WithCloseBracketToken(Microsoft.CodeAnalysis.SyntaxToken)
        #>
    }
}

Function Import-BaseParameterListSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax]$BaseParameterList
    )

    Process {
        switch ($BaseParameterList) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax] } { (Import-ParameterListSyntax -ParameterList $BaseParameterList) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax] } { (Import-BracketedParameterListSyntax -BracketedParameterList $BaseParameterList) | Write-Output; break; }
            default {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownBaseParameterListSyntax);
                Set-BaseParameterListSyntaxContents -BaseParameterList $BaseParameterList -Element $Element -IsUnknown;
                Write-Output -InputObject $Element -NoEnumerate;
                break;
            }
        }
    }
}

Function Set-BaseParameterListSyntaxContents {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax]$BaseParameterList,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $BaseParameterList -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $BaseParameterList -Element $Element;
    }

    <#
        BaseType Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode :
            Microsoft.CodeAnalysis.SyntaxNode

            Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax] Parameters

            Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax WithParameters(Microsoft.CodeAnalysis.SeparatedSyntaxList`1[Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax])
            Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax AddParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[])
    #>
}

Function Import-ExpressionSyntax {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax]$Expression
    )

    Process {
        switch ($Expression) {
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousFunctionExpressionSyntax] } { (Import-AnonymousFunctionExpressionSyntax -AnonymousFunctionExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StackAllocArrayCreationExpressionSyntax] } { (Import-StackAllocArrayCreationExpressionSyntax -StackAllocArrayCreationExpression $Expression) | Write-Output; break; }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax] } { (Import-TypeSyntax -Type $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax] } { (Import-ParenthesizedExpressionSyntax -ParenthesizedExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleExpressionSyntax] } { (Import-TupleExpressionSyntax -TupleExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PrefixUnaryExpressionSyntax] } { (Import-PrefixUnaryExpressionSyntax -PrefixUnaryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax] } { (Import-AwaitExpressionSyntax -AwaitExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PostfixUnaryExpressionSyntax] } { (Import-PostfixUnaryExpressionSyntax -PostfixUnaryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax] } { (Import-MemberAccessExpressionSyntax -MemberAccessExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalAccessExpressionSyntax] } { (Import-ConditionalAccessExpressionSyntax -ConditionalAccessExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberBindingExpressionSyntax] } { (Import-MemberBindingExpressionSyntax -MemberBindingExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElementBindingExpressionSyntax] } { (Import-ElementBindingExpressionSyntax -ElementBindingExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RangeExpressionSyntax] } { (Import-RangeExpressionSyntax -RangeExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitElementAccessSyntax] } { (Import-ImplicitElementAccessSyntax -ImplicitElementAccess $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax] } { (Import-BinaryExpressionSyntax -BinaryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax] } { (Import-AssignmentExpressionSyntax -AssignmentExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalExpressionSyntax] } { (Import-ConditionalExpressionSyntax -ConditionalExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InstanceExpressionSyntax] } { (Import-InstanceExpressionSyntax -InstanceExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.LiteralExpressionSyntax] } { (Import-LiteralExpressionSyntax -LiteralExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MakeRefExpressionSyntax] } { (Import-MakeRefExpressionSyntax -MakeRefExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeExpressionSyntax] } { (Import-RefTypeExpressionSyntax -RefTypeExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefValueExpressionSyntax] } { (Import-RefValueExpressionSyntax -RefValueExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CheckedExpressionSyntax] } { (Import-CheckedExpressionSyntax -CheckedExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DefaultExpressionSyntax] } { (Import-DefaultExpressionSyntax -DefaultExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeOfExpressionSyntax] } { (Import-TypeOfExpressionSyntax -TypeOfExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SizeOfExpressionSyntax] } { (Import-SizeOfExpressionSyntax -SizeOfExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax] } { (Import-InvocationExpressionSyntax -InvocationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax] } { (Import-ElementAccessExpressionSyntax -ElementAccessExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationExpressionSyntax] } { (Import-DeclarationExpressionSyntax -DeclarationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax] } { (Import-CastExpressionSyntax -CastExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RefExpressionSyntax] } { (Import-RefExpressionSyntax -RefExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InitializerExpressionSyntax] } { (Import-InitializerExpressionSyntax -InitializerExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseObjectCreationExpressionSyntax] } { (Import-BaseObjectCreationExpressionSyntax -BaseObjectCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.WithExpressionSyntax] } { (Import-WithExpressionSyntax -WithExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectCreationExpressionSyntax] } { (Import-AnonymousObjectCreationExpressionSyntax -AnonymousObjectCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayCreationExpressionSyntax] } { (Import-ArrayCreationExpressionSyntax -ArrayCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitArrayCreationExpressionSyntax] } { (Import-ImplicitArrayCreationExpressionSyntax -ImplicitArrayCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitStackAllocArrayCreationExpressionSyntax] } { (Import-ImplicitStackAllocArrayCreationExpressionSyntax -ImplicitStackAllocArrayCreationExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax] } { (Import-QueryExpressionSyntax -QueryExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OmittedArraySizeExpressionSyntax] } { (Import-OmittedArraySizeExpressionSyntax -OmittedArraySizeExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringExpressionSyntax] } { (Import-InterpolatedStringExpressionSyntax -InterpolatedStringExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IsPatternExpressionSyntax] } { (Import-IsPatternExpressionSyntax -IsPatternExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ThrowExpressionSyntax] } { (Import-ThrowExpressionSyntax -ThrowExpression $Expression) | Write-Output; break; }
            # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionSyntax] } { (Import-SwitchExpressionSyntax -SwitchExpression $Expression) | Write-Output; break; }
            default {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownExpressionSyntax);
                Set-SyntaxNodeContents -SyntaxNode $Expression -Element $XElement -IsUnknown;
                Write-Output -InputObject $XElement -NoEnumerate;
                break;
            }
        }
    }
}

Function Import-SyntaxNode {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XElement])]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]$SyntaxNode
    )
    [System.Xml.Linq.XElement]$XElement = $null;
    switch ($SyntaxNode) {
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax] } { (Import-ArgumentSyntax -Argument $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax] } { (Import-ArrayRankSpecifierSyntax -ArrayRankSpecifier $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax] } { (Import-AttributeSyntax -Attribute $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax] } { (Import-AttributeTargetSpecifierSyntax -AttributeTargetSpecifier $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax] } { (Import-CompilationUnitSyntax -CompilationUnit $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax] } { (Import-CrefParameterSyntax -CrefParameter $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax] } { (Import-AccessorDeclarationSyntax -AccessorDeclaration $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax] } { (Import-StructuredTriviaSyntax -StructuredTrivia $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax] } { (Import-SubpatternSyntax -Subpattern $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax] } { (Import-UsingDirectiveSyntax -UsingDirective $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax] } { (Import-TypeArgumentListSyntax -TypeArgumentList $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax] } { (Import-FunctionPointerParameterListSyntax -FunctionPointerParameterList $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax] } { (Import-FunctionPointerCallingConventionSyntax -FunctionPointerCallingConvention $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionListSyntax] } { (Import-FunctionPointerUnmanagedCallingConventionListSyntax -FunctionPointerUnmanagedCallingConventionList $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionSyntax] } { (Import-FunctionPointerUnmanagedCallingConventionSyntax -FunctionPointerUnmanagedCallingConvention $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax] } { (Import-TupleElementSyntax -TupleElement $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax] } { (Import-ExpressionOrPatternSyntax -ExpressionOrPattern $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax] } { (Import-BaseArgumentListSyntax -BaseArgumentList $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseExpressionColonSyntax] } { (Import-BaseExpressionColonSyntax -BaseExpressionColon $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax] } { (Import-AnonymousObjectMemberDeclaratorSyntax -AnonymousObjectMemberDeclarator $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryClauseSyntax] } { (Import-QueryClauseSyntax -QueryClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SelectOrGroupClauseSyntax] } { (Import-SelectOrGroupClauseSyntax -SelectOrGroupClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryBodySyntax] } { (Import-QueryBodySyntax -QueryBody $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.JoinIntoClauseSyntax] } { (Import-JoinIntoClauseSyntax -JoinIntoClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax] } { (Import-OrderingSyntax -Ordering $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryContinuationSyntax] } { (Import-QueryContinuationSyntax -QueryContinuation $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.WhenClauseSyntax] } { (Import-WhenClauseSyntax -WhenClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PositionalPatternClauseSyntax] } { (Import-PositionalPatternClauseSyntax -PositionalPatternClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyPatternClauseSyntax] } { (Import-PropertyPatternClauseSyntax -PropertyPatternClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax] } { (Import-InterpolatedStringContentSyntax -InterpolatedStringContent $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationAlignmentClauseSyntax] } { (Import-InterpolationAlignmentClauseSyntax -InterpolationAlignmentClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationFormatClauseSyntax] } { (Import-InterpolationFormatClauseSyntax -InterpolationFormatClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax] } { (Import-StatementSyntax -Statement $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax] } { (Import-VariableDeclarationSyntax -VariableDeclaration $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax] } { (Import-VariableDeclaratorSyntax -VariableDeclarator $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax] } { (Import-EqualsValueClauseSyntax -EqualsValueClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDesignationSyntax] } { (Import-VariableDesignationSyntax -VariableDesignation $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElseClauseSyntax] } { (Import-ElseClauseSyntax -ElseClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax] } { (Import-SwitchSectionSyntax -SwitchSection $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchLabelSyntax] } { (Import-SwitchLabelSyntax -SwitchLabel $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionArmSyntax] } { (Import-SwitchExpressionArmSyntax -SwitchExpressionArm $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax] } { (Import-CatchClauseSyntax -CatchClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchDeclarationSyntax] } { (Import-CatchDeclarationSyntax -CatchDeclaration $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchFilterClauseSyntax] } { (Import-CatchFilterClauseSyntax -CatchFilterClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FinallyClauseSyntax] } { (Import-FinallyClauseSyntax -FinallyClause $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax] } { (Import-ExternAliasDirectiveSyntax -ExternAliasDirective $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax] } { (Import-MemberDeclarationSyntax -MemberDeclaration $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax] } { (Import-AttributeListSyntax -AttributeList $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax] } { (Import-AttributeArgumentListSyntax -AttributeArgumentList $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax] } { (Import-AttributeArgumentSyntax -AttributeArgument $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax] } { (Import-NameEqualsSyntax -NameEquals $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax] } { (Import-TypeParameterListSyntax -TypeParameterList $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax] } { (Import-TypeParameterSyntax -TypeParameter $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax] } { (Import-BaseListSyntax -BaseList $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax] } { (Import-BaseTypeSyntax -BaseType $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax] } { (Import-TypeParameterConstraintClauseSyntax -TypeParameterConstraintClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax] } { (Import-TypeParameterConstraintSyntax -TypeParameterConstraint $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax] } { (Import-ExplicitInterfaceSpecifierSyntax -ExplicitInterfaceSpecifier $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax] } { (Import-ConstructorInitializerSyntax -ConstructorInitializer $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax] } { (Import-ArrowExpressionClauseSyntax -ArrowExpressionClause $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax] } { (Import-AccessorListSyntax -AccessorList $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax] } { (Import-BaseParameterListSyntax -BaseParameterList $SyntaxNode) | Write-Output; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax] } { (Import-BaseParameterSyntax -BaseParameter $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefSyntax] } { (Import-CrefSyntax -Cref $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseCrefParameterListSyntax] } { (Import-BaseCrefParameterListSyntax -BaseCrefParameterList $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax] } { (Import-XmlNodeSyntax -XmlNode $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementStartTagSyntax] } { (Import-XmlElementStartTagSyntax -XmlElementStartTag $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementEndTagSyntax] } { (Import-XmlElementEndTagSyntax -XmlElementEndTag $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameSyntax] } { (Import-XmlNameSyntax -XmlName $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlPrefixSyntax] } { (Import-XmlPrefixSyntax -XmlPrefix $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax] } { (Import-XmlAttributeSyntax -XmlAttribute $SyntaxNode) | Write-Output; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.LineDirectivePositionSyntax] } { (Import-LineDirectivePositionSyntax -LineDirectivePosition $SyntaxNode) | Write-Output; break; }
        default {
            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownSyntax);
            Set-SyntaxNodeContents -SyntaxNode $SyntaxNode -Element $XElement -IsUnknown;
            Write-Output -InputObject $XElement -NoEnumerate;
            break;
        }
    }
}

Function Set-SyntaxNodeContents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]$SyntaxNode,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [switch]$IsUnknown
    )

    if ($IsUnknown.IsPresent) {
        $Element.Add([System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Kind'), [Enum]::GetName([Microsoft.CodeAnalysis.CSharp.SyntaxKind], $SyntaxNode.Kind())));
        $Element.Add([System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('RawKind'), $SyntaxNode.RawKind));
        $Element.Add([System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('TypeName'), (ConvertTo-TypeSyntax -Type $SyntaxNode.GetType()).ToString()));
    }
    if ($SyntaxNode.IsMissing) { $Element.Add([System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('IsMissing'), $true)) }
    if ($SyntaxNode.HasLeadingTrivia) {
        $Nodes = @($SyntaxNode.GetLeadingTrivia() | ForEach-Object {
            if ($_.HasStructure) {
                $n = $_.GetStructure();
                if ($n -is [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
                    Write-Output -InputObject $n -NoEnumerate;
                }
            }
        });
        if ($Nodes.Count -gt 0) {
            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.LeadingTrivia);
            $Element.Add($XElement);
            $Nodes | Import-SyntaxNode | ForEach-Object { $XElement.Add($_) }
        }
    }
    if ($SyntaxNode.HasTrailingTrivia) {
        $Nodes = @($SyntaxNode.GetTrailingTrivia() | Where-Object {
            if ($_.HasStructure) {
                $n = $_.GetStructure();
                if ($n -is [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]) {
                    Write-Output -InputObject $n -NoEnumerate;
                }
            }
        });
        if ($Nodes.Count -gt 0) {
            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.TrailingTrivia);
            $Element.Add($XElement);
            $Nodes | Import-SyntaxNode | ForEach-Object { $XElement.Add($_) }
        }
    }
}

Function Import-ModelDefinition {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$InterfaceDeclaration,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$Document
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Interface);
        # TODO: Set contents
        Set-TypeDeclarationSyntaxContents -TypeDeclaration $InterfaceDeclaration -Element $Element;
        <#
            public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax :
                    Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken keyword, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax typeParameterList, Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax baseList, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> constraintClauses, Microsoft.CodeAnalysis.SyntaxToken openBraceToken, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members, Microsoft.CodeAnalysis.SyntaxToken closeBraceToken, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithKeyword(Microsoft.CodeAnalysis.SyntaxToken keyword);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax typeParameterList);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithBaseList(Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax baseList);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithConstraintClauses(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> constraintClauses);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithOpenBraceToken(Microsoft.CodeAnalysis.SyntaxToken openBraceToken);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithCloseBraceToken(Microsoft.CodeAnalysis.SyntaxToken closeBraceToken);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax AddTypeParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax AddBaseListTypes(Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax AddConstraintClauses(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax[] items);
            public class Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax :
                    Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax
                public int Arity { get; }
                public virtual Microsoft.CodeAnalysis.SyntaxToken Keyword { get; }
                public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax TypeParameterList { get; }
                public virtual Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> ConstraintClauses { get; }
                public virtual Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> Members { get; }
                public int get_Arity();
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
                public Microsoft.CodeAnalysis.SyntaxToken get_Keyword();
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithKeyword(Microsoft.CodeAnalysis.SyntaxToken keyword);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax get_TypeParameterList();
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax typeParameterList);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddTypeParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[] items);
                public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> get_ConstraintClauses();
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithConstraintClauses(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> constraintClauses);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddConstraintClauses(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax[] items);
                public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> get_Members();
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithBaseList(Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax baseList);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithOpenBraceToken(Microsoft.CodeAnalysis.SyntaxToken openBraceToken);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithCloseBraceToken(Microsoft.CodeAnalysis.SyntaxToken closeBraceToken);
                public Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
            public class Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax :
                    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
                public virtual Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
                public virtual Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax BaseList { get; }
                public virtual Microsoft.CodeAnalysis.SyntaxToken OpenBraceToken { get; }
                public virtual Microsoft.CodeAnalysis.SyntaxToken CloseBraceToken { get; }
                public virtual Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
                public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax get_BaseList();
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithBaseList(Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax baseList);
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax AddBaseListTypes(Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax[] items);
                public Microsoft.CodeAnalysis.SyntaxToken get_OpenBraceToken();
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithOpenBraceToken(Microsoft.CodeAnalysis.SyntaxToken openBraceToken);
                public Microsoft.CodeAnalysis.SyntaxToken get_CloseBraceToken();
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithCloseBraceToken(Microsoft.CodeAnalysis.SyntaxToken closeBraceToken);
                public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
                public Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
            public class Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax :
                    Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode
                public virtual Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> AttributeLists { get; }
                public virtual Microsoft.CodeAnalysis.SyntaxTokenList Modifiers { get; }
                public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> get_AttributeLists();
                public Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
                public Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
                public Microsoft.CodeAnalysis.SyntaxTokenList get_Modifiers();
                public Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
                public Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
        #>
    }
}

Function Import-SourceFile {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string[]]$CodeLines,

        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document
    )

    Begin {
        $Writer = [System.IO.StringWriter]::new();
    }

    Process {
        $CodeLines | ForEach-Object { $Writer.WriteLine($_) }
    }

    End {
        $Text = $Writer.ToString();
        if ($Text.TrimEnd().Length -gt 0) {
            [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = $null;
            try { $SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($Text, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $Path); }
            catch {
                $e = Get-ExceptionObject -InputObject $_;
                Write-Error -Exception $e -Message 'Failed to read source code' -Category ParserError -ErrorId 'Import-SourceFile::ParseException' -TargetObject $CodeLines `
                    -CategoryReason '[Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText threw an exception' -CategoryActivity 'CSharpSyntaxTree.ParseText' `
                    -CategoryTargetName 'CodeLines';
            }
            if ($null -ne $SyntaxTree) {
                if ($SyntaxTree.HasCompilationUnitRoot) {
                    $SourceElement = Get-ModelDefinitionSourceElement -Path $Path -Document $Document;
                    if ($null -ne $SourceElement) {
                        throw 'Source element already exists';
                    }
                    $Id = [Guid]::NewGuid();
                    $SourceElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Source,
                        [System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Path'), $Path),
                        [System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Id'), $Id)
                    );
                    $XElement = $Document.Root.Element($Script:ModelDefinitionNames.Sources);
                    if ($null -eq $XElement) {
                        if ($null -eq $XElement) {
                            $Document.Root.AddFirst([System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Sources, $SourceElement));
                        } else {
                            $XElement.Add($SourceElement);
                        }
                    }
                    [Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax]$Syntax = $SyntaxTree.GetCompilationUnitRoot();
                    $Syntax.Externs | Import-ExternAliasDirectiveSyntax | ForEach-Object { $SourceElement.Add($_) }
                    $Syntax.Usings | Import-UsingDirectiveSyntax | ForEach-Object { $SourceElement.Add($_) }
                    $Syntax.AttributeLists | Import-AttributeListSyntax | ForEach-Object { $SourceElement.Add($_) }
                    foreach ($MemberSyntax in $Syntax.Members) {
                        switch ($MemberSyntax) {
                            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } {
                                $Namespace = $MemberSyntax.Name.ToString();
                                $RootNamespaceElement = [DevUtil.ExtensionMethods]::FindFirstMatchingAttribute($Document.Root, $Script:ModelDefinitionNames.Namespace,
                                    [System.Xml.Linq.XNamespace]::None.GetName('Name'), $Namespace);
                                $NamespaceId = [Guid]::Empty;
                                if ($null -eq $RootNamespaceElement) {
                                    $NamespaceId = [Guid]::NewGuid();
                                    $RootNamespaceElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Namespace,
                                        [System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Name'), $Namespace),
                                        [System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Id'), $NamespaceId));
                                    $Document.Root.Add($RootNamespaceElement);
                                } else {
                                    $NamespaceId = [System.Xml.XmlConvert]::ToGuid($RootNamespaceElement.Attribute().Value);
                                }
                                [System.Xml.Linq.XElement]$FileNamespaceElement = $null;
                                switch ($MemberDeclaration) {
                                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } {
                                        $FileNamespaceElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Namespace);
                                        $SourceElement.Add($FileNamespaceElement);
                                        Set-MemberDeclarationSyntaxContents -MemberDeclaration $MemberSyntax -Element $FileNamespaceElement;
                                        break;
                                    }
                                    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax] } {
                                        $FileNamespaceElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.FileScopedNamespace);
                                        $SourceElement.Add($FileNamespaceElement);
                                        Set-MemberDeclarationSyntaxContents -MemberDeclaration $MemberSyntax -Element $FileNamespaceElement;
                                        break;
                                    }
                                    default {
                                        $FileNamespaceElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownNamespaceDeclaration);
                                        $SourceElement.Add($FileNamespaceElement);
                                        Set-MemberDeclarationSyntaxContents -MemberDeclaration $MemberSyntax -Element $FileNamespaceElement -IsUnknown;
                                        break;
                                    }
                                }
                                $FileNamespaceElement.Add([System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Id'), $NamespaceId));
                                $MemberSyntax.Externs | Import-ExternAliasDirectiveSyntax | ForEach-Object { $FileNamespaceElement.Add($_) }
                                $MemberSyntax.Usings | Import-UsingDirectiveSyntax | ForEach-Object { $FileNamespaceElement.Add($_) }
                                $MemberSyntax.AttributeLists | Import-AttributeListSyntax | ForEach-Object { $FileNamespaceElement.Add($_) }
                                foreach ($NsMember in $MemberSyntax.Members) {
                                    if ($NsMember -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]) {
                                        Import-ModelDefinition -InterfaceDeclaration $MemberSyntax -Document $Document -FileId $Id -NamepaceId $NamespaceId;
                                    } else {
                                        $NsElement.Add((Import-MemberDeclarationSyntax -MemberDeclaration $NsMember));
                                    }
                                }
                                break;
                            }
                            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] } {
                                Import-ModelDefinition -InterfaceDeclaration $MemberSyntax -Document $Document -FileId $Id;
                                break;
                            }
                            default {
                                $SourceElement.Add((Import-MemberDeclarationSyntax -MemberDeclaration $MemberSyntax));
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
