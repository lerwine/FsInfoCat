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
('ModelDefinitions', 'Sources', 'Source', 'LeadingTrivia', 'TrailingTrivia', 'UnknownSyntax', 'UnknownMemberDeclaration', 'Namespace', 'UnknownNamespaceDeclaration', 'Interface', 'Class', 'Struct', 'Enum',
        'Member', 'Property', 'UnknownPropertyDeclaration', 'Field', 'EventField', 'EventProperty', 'UnknownFieldDeclaration', 'UnknownBaseTypeDeclaration', 'UnknownTypeDeclaration', 'Method',
        'UnknownMethodDeclaration', 'Destructor', 'Constructor', 'Operator', 'ConversionOperator', 'Indexer', 'Delegate', 'GlobalStatement', 'IncompleteMember', 'AttributeList', 'Attribute',
        'Target') | ForEach-Object {
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

Function Import-ExternAliasDirectiveSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax]$ExternAliasDirective,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Parent
    )

    Process {
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax :
    Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode
  public Microsoft.CodeAnalysis.SyntaxToken ExternKeyword { get; }
  public Microsoft.CodeAnalysis.SyntaxToken AliasKeyword { get; }
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken get_ExternKeyword();
  public Microsoft.CodeAnalysis.SyntaxToken get_AliasKeyword();
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax Update(Microsoft.CodeAnalysis.SyntaxToken externKeyword, Microsoft.CodeAnalysis.SyntaxToken aliasKeyword, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithExternKeyword(Microsoft.CodeAnalysis.SyntaxToken externKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithAliasKeyword(Microsoft.CodeAnalysis.SyntaxToken aliasKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  #>
    }
}

Function Import-UsingDirectiveSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax]$UsingDirective,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Parent
    )

    Process {
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax :
    Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode
  public Microsoft.CodeAnalysis.SyntaxToken GlobalKeyword { get; }
  public Microsoft.CodeAnalysis.SyntaxToken UsingKeyword { get; }
  public Microsoft.CodeAnalysis.SyntaxToken StaticKeyword { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax Alias { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Name { get; }
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax Update(Microsoft.CodeAnalysis.SyntaxToken usingKeyword, Microsoft.CodeAnalysis.SyntaxToken staticKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax alias, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.SyntaxToken get_GlobalKeyword();
  public Microsoft.CodeAnalysis.SyntaxToken get_UsingKeyword();
  public Microsoft.CodeAnalysis.SyntaxToken get_StaticKeyword();
  public Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax get_Alias();
  public Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax get_Name();
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax Update(Microsoft.CodeAnalysis.SyntaxToken globalKeyword, Microsoft.CodeAnalysis.SyntaxToken usingKeyword, Microsoft.CodeAnalysis.SyntaxToken staticKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax alias, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithGlobalKeyword(Microsoft.CodeAnalysis.SyntaxToken globalKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithUsingKeyword(Microsoft.CodeAnalysis.SyntaxToken usingKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithStaticKeyword(Microsoft.CodeAnalysis.SyntaxToken staticKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithAlias(Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax alias);
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name);
  public Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  #>
    }
}

Function Import-AttributeSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax]$Attribute,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Parent
    )

    Process {
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax :
    Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode
  public Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Name { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax ArgumentList { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax get_Name();
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax get_ArgumentList();
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax argumentList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name);
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax WithArgumentList(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax argumentList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax AddArgumentListArguments(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax[] items);
        #>
    }
}

Function Import-AttributeTargetSpecifierSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax]$AttributeTargetSpecifier,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Parent,

        [string]$ElementName
    )

    Process {
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax :
    Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.SyntaxToken ColonToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.SyntaxToken get_ColonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax Update(Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.SyntaxToken colonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax WithColonToken(Microsoft.CodeAnalysis.SyntaxToken colonToken);
        #>
    }
}

Function Import-AttributeListSyntax {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax]$AttributeList,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Parent
    )

    Process {
        if ($AttributeList.Attributes.Count -gt 0) {
            $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AttributeList);
            $Parent.Add($Element);
            if ($null -ne $AttributeList.Target) { Import-AttributeTargetSpecifierSyntax -AttributeTargetSpecifier $AttributeList.Target -Parent $Element -ElementName $Script:ModelDefinitionNames.Target }
            $Script:ModelDefinitionNames.AttributeList | Import-AttributeSyntax -Parent $Element;
            Set-SyntaxNodeContents -SyntaxNode $AttributeList -Element $Element;
        } else {
            if ($null -ne $AttributeList.Target) {
                $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AttributeList);
                $Parent.Add($Element);
                Import-AttributeTargetSpecifierSyntax -AttributeTargetSpecifier $AttributeList.Target -Parent $Element -ElementName $Script:ModelDefinitionNames.Target;
                Set-SyntaxNodeContents -SyntaxNode $AttributeList -Element $Element;
            } else {
                if ($AttributeList.HasLeadingTrivia -or $AttributeList.HasTrailingTrivia) {
                    $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.AttributeList);
                    $Parent.Add($Element);
                    Set-SyntaxNodeContents -SyntaxNode $AttributeList -Element $Element;
                }
            }
        }
    }
}

Function Import-FieldDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]$FieldDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Field);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax declaration, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax WithDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax declaration);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax AddDeclarationVariables(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseFieldDeclarationSyntaxContents -BaseFieldDeclaration $FieldDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseFieldDeclarationSyntaxContents -BaseFieldDeclaration $FieldDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-EventFieldDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]$EventFieldDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.EventField);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken EventKeyword { get; }
  public Microsoft.CodeAnalysis.SyntaxToken get_EventKeyword();
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken eventKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax declaration, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax WithEventKeyword(Microsoft.CodeAnalysis.SyntaxToken eventKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax WithDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax declaration);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax AddDeclarationVariables(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseFieldDeclarationSyntaxContents -BaseFieldDeclaration $EventFieldDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseFieldDeclarationSyntaxContents -BaseFieldDeclaration $EventFieldDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-BaseFieldDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$BaseFieldDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        switch ($MemberDeclaration) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-FieldDeclarationSyntax -FieldDeclaration $BaseFieldDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-FieldDeclarationSyntax -FieldDeclaration $BaseFieldDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-FieldDeclarationSyntax -FieldDeclaration $BaseFieldDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-FieldDeclarationSyntax -FieldDeclaration $BaseFieldDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-EventFieldDeclarationSyntax -EventFieldDeclaration $BaseFieldDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-EventFieldDeclarationSyntax -EventFieldDeclaration $BaseFieldDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-EventFieldDeclarationSyntax -EventFieldDeclaration $BaseFieldDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-EventFieldDeclarationSyntax -EventFieldDeclaration $BaseFieldDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            default {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownFieldDeclaration);
                if ($PSCmdlet.ParameterSetName -eq 'Root') {
                    # TODO: Add to root
                    Set-BaseFieldDeclarationSyntaxContents -BaseFieldDeclaration $BaseFieldDeclaration -Element $XElement -FileId $FileId -IsUnknown;
                } else {
                    $Parent.Add($XElement);
                    Set-BaseFieldDeclarationSyntaxContents -BaseFieldDeclaration $BaseFieldDeclaration -Element $XElement -IsUnknown;
                }
                break;
            }
        }
    }
}

Function Set-BaseFieldDeclarationSyntaxContents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]$BaseFieldDeclaration,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [guid]$FileId,

        [switch]$IsUnknown
    )

    # TODO: Set contents
    <#
public class Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax Declaration { get; }
  public virtual Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax get_Declaration();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax WithDeclaration(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax declaration);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax AddDeclarationVariables(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax[] items);
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  #>
    if ($PSBoundParameters.ContainsKey('FileId')) {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseFieldDeclaration -Element $Element -FileId $FileId -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseFieldDeclaration -Element $Element -FileId $FileId;
        }
    } else {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseFieldDeclaration -Element $Element -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseFieldDeclaration -Element $Element;
        }
    }
}

Function Import-OperatorDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]$OperatorDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Operator);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ReturnType { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier { get; }
  public Microsoft.CodeAnalysis.SyntaxToken OperatorKeyword { get; }
  public Microsoft.CodeAnalysis.SyntaxToken OperatorToken { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax returnType, Microsoft.CodeAnalysis.SyntaxToken operatorKeyword, Microsoft.CodeAnalysis.SyntaxToken operatorToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax get_ReturnType();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax get_ExplicitInterfaceSpecifier();
  public Microsoft.CodeAnalysis.SyntaxToken get_OperatorKeyword();
  public Microsoft.CodeAnalysis.SyntaxToken get_OperatorToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax returnType, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken operatorKeyword, Microsoft.CodeAnalysis.SyntaxToken operatorToken, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithReturnType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax returnType);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithOperatorKeyword(Microsoft.CodeAnalysis.SyntaxToken operatorKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithOperatorToken(Microsoft.CodeAnalysis.SyntaxToken operatorToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithBody(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax AddBodyAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax AddBodyStatements(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $OperatorDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $OperatorDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-MethodDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]$MethodDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Method);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax
  public int Arity { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ReturnType { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier { get; }
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax TypeParameterList { get; }
  public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> ConstraintClauses { get; }
  public int get_Arity();
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax get_ReturnType();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax get_ExplicitInterfaceSpecifier();
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax get_TypeParameterList();
  public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> get_ConstraintClauses();
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax returnType, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax typeParameterList, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> constraintClauses, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithReturnType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax returnType);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax typeParameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithConstraintClauses(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> constraintClauses);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithBody(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddTypeParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddConstraintClauses(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddBodyAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax AddBodyStatements(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $MethodDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $MethodDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-DestructorDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]$DestructorDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Destructor);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken TildeToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken tildeToken, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.SyntaxToken get_TildeToken();
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken tildeToken, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithTildeToken(Microsoft.CodeAnalysis.SyntaxToken tildeToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithBody(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax AddBodyAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax AddBodyStatements(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $DestructorDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $DestructorDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-ConversionOperatorDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]$ConversionOperatorDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.ConversionOperator);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken ImplicitOrExplicitKeyword { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier { get; }
  public Microsoft.CodeAnalysis.SyntaxToken OperatorKeyword { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken implicitOrExplicitKeyword, Microsoft.CodeAnalysis.SyntaxToken operatorKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.SyntaxToken get_ImplicitOrExplicitKeyword();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax get_ExplicitInterfaceSpecifier();
  public Microsoft.CodeAnalysis.SyntaxToken get_OperatorKeyword();
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax get_Type();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken implicitOrExplicitKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken operatorKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithImplicitOrExplicitKeyword(Microsoft.CodeAnalysis.SyntaxToken implicitOrExplicitKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithOperatorKeyword(Microsoft.CodeAnalysis.SyntaxToken operatorKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithBody(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax AddBodyAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax AddBodyStatements(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $ConversionOperatorDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $ConversionOperatorDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-ConstructorDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax]$ConstructorDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Constructor);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax Initializer { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax initializer, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax get_Initializer();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax initializer, Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithInitializer(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax initializer);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithBody(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax AddBodyAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax AddBodyStatements(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $ConstructorDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $ConstructorDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-BaseMethodDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$BaseMethodDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        switch ($MemberDeclaration) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-ConstructorDeclarationSyntax -ConstructorDeclaration $BaseMethodDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-ConstructorDeclarationSyntax -ConstructorDeclaration $BaseMethodDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-ConstructorDeclarationSyntax -ConstructorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-ConstructorDeclarationSyntax -ConstructorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-ConversionOperatorDeclarationSyntax -ConversionOperatorDeclaration $BaseMethodDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-ConversionOperatorDeclarationSyntax -ConversionOperatorDeclaration $BaseMethodDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-ConversionOperatorDeclarationSyntax -ConversionOperatorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-ConversionOperatorDeclarationSyntax -ConversionOperatorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-DestructorDeclarationSyntax -DestructorDeclaration $BaseMethodDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-DestructorDeclarationSyntax -DestructorDeclaration $BaseMethodDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-DestructorDeclarationSyntax -DestructorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-DestructorDeclarationSyntax -DestructorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-MethodDeclarationSyntax -MethodDeclaration $BaseMethodDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-MethodDeclarationSyntax -MethodDeclaration $BaseMethodDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-MethodDeclarationSyntax -MethodDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-MethodDeclarationSyntax -MethodDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-OperatorDeclarationSyntax -OperatorDeclaration $BaseMethodDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-OperatorDeclarationSyntax -OperatorDeclaration $BaseMethodDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-OperatorDeclarationSyntax -OperatorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-OperatorDeclarationSyntax -OperatorDeclaration $BaseMethodDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            default {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownMethodDeclaration);
                if ($PSCmdlet.ParameterSetName -eq 'Root') {
                    # TODO: Add to root
                    Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $BaseMethodDeclaration -Element $XElement -FileId $FileId -IsUnknown;
                } else {
                    $Parent.Add($XElement);
                    Set-BaseMethodDeclarationSyntaxContents -BaseMethodDeclaration $BaseMethodDeclaration -Element $XElement -IsUnknown;
                }
                break;
            }
        }
    }
}

Function Set-BaseMethodDeclarationSyntaxContents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]$BaseMethodDeclaration,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [guid]$FileId,

        [switch]$IsUnknown
    )

    # TODO: Set contents
    <#
public class Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax ParameterList { get; }
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax Body { get; }
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody { get; }
  public virtual Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax get_ParameterList();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax get_Body();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithBody(Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax body);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddBodyAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddBodyStatements(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax get_ExpressionBody();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  #>
    if ($PSBoundParameters.ContainsKey('FileId')) {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseMethodDeclaration -Element $Element -FileId $FileId -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseMethodDeclaration -Element $Element -FileId $FileId;
        }
    } else {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseMethodDeclaration -Element $Element -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseMethodDeclaration -Element $Element;
        }
    }
}

Function Import-EventDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]$EventDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.EventProperty);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken EventKeyword { get; }
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken eventKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken eventKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.SyntaxToken get_EventKeyword();
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken eventKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithEventKeyword(Microsoft.CodeAnalysis.SyntaxToken eventKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithAccessorList(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax AddAccessorListAccessors(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $EventDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $EventDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-IndexerDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]$IndexerDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Indexer);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken Semicolon { get; }
  public Microsoft.CodeAnalysis.SyntaxToken ThisKeyword { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax ParameterList { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody { get; }
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken get_Semicolon();
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithSemicolon(Microsoft.CodeAnalysis.SyntaxToken semicolon);
  public Microsoft.CodeAnalysis.SyntaxToken get_ThisKeyword();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax get_ParameterList();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax get_ExpressionBody();
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken thisKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax parameterList, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithThisKeyword(Microsoft.CodeAnalysis.SyntaxToken thisKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithAccessorList(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax AddAccessorListAccessors(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $IndexerDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $IndexerDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-PropertyDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]$PropertyDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Property);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken Semicolon { get; }
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax ExpressionBody { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax Initializer { get; }
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken get_Semicolon();
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithSemicolon(Microsoft.CodeAnalysis.SyntaxToken semicolon);
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax get_ExpressionBody();
  public Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax get_Initializer();
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type, Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList, Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody, Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax initializer, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithAccessorList(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithExpressionBody(Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax expressionBody);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithInitializer(Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax initializer);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax AddAccessorListAccessors(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $PropertyDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $PropertyDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-BasePropertyDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$BasePropertyDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        switch ($MemberDeclaration) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-EventDeclarationSyntax -EventDeclaration $BasePropertyDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-EventDeclarationSyntax -EventDeclaration $BasePropertyDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-EventDeclarationSyntax -EventDeclaration $BasePropertyDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-EventDeclarationSyntax -EventDeclaration $BasePropertyDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-IndexerDeclarationSyntax -IndexerDeclaration $BasePropertyDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-IndexerDeclarationSyntax -IndexerDeclaration $BasePropertyDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-IndexerDeclarationSyntax -IndexerDeclaration $BasePropertyDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-IndexerDeclarationSyntax -IndexerDeclaration $BasePropertyDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-PropertyDeclarationSyntax -PropertyDeclaration $BasePropertyDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-PropertyDeclarationSyntax -PropertyDeclaration $BasePropertyDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-PropertyDeclarationSyntax -PropertyDeclaration $BasePropertyDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-PropertyDeclarationSyntax -PropertyDeclaration $BasePropertyDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            default {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownPropertyDeclaration);
                if ($PSCmdlet.ParameterSetName -eq 'Root') {
                    # TODO: Add to root
                    Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $BasePropertyDeclaration -Element $XElement -FileId $FileId -IsUnknown;
                } else {
                    $Parent.Add($XElement);
                    Set-BasePropertyDeclarationSyntaxContents -BasePropertyDeclaration $BasePropertyDeclaration -Element $XElement -IsUnknown;
                }
                break;
            }
        }
    }
}

Function Set-BasePropertyDeclarationSyntaxContents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]$BasePropertyDeclaration,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [guid]$FileId,

        [switch]$IsUnknown
    )

    # TODO: Set contents
    <#
public class Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type { get; }
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax ExplicitInterfaceSpecifier { get; }
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax AccessorList { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax get_Type();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type);
  public Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax get_ExplicitInterfaceSpecifier();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithExplicitInterfaceSpecifier(Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax explicitInterfaceSpecifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax get_AccessorList();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithAccessorList(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax accessorList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax AddAccessorListAccessors(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  #>
    if ($PSBoundParameters.ContainsKey('FileId')) {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BasePropertyDeclaration -Element $Element -FileId $FileId -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BasePropertyDeclaration -Element $Element -FileId $FileId;
        }
    } else {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BasePropertyDeclaration -Element $Element -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BasePropertyDeclaration -Element $Element;
        }
    }
}

Function Import-DelegateDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$DelegateDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Delegate);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public int Arity { get; }
  public Microsoft.CodeAnalysis.SyntaxToken DelegateKeyword { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax ReturnType { get; }
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax TypeParameterList { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax ParameterList { get; }
  public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> ConstraintClauses { get; }
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public int get_Arity();
  public Microsoft.CodeAnalysis.SyntaxToken get_DelegateKeyword();
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax get_ReturnType();
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax get_TypeParameterList();
  public Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax get_ParameterList();
  public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> get_ConstraintClauses();
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken delegateKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax returnType, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax typeParameterList, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> constraintClauses, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithDelegateKeyword(Microsoft.CodeAnalysis.SyntaxToken delegateKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithReturnType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax returnType);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithTypeParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax typeParameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithParameterList(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax parameterList);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithConstraintClauses(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax> constraintClauses);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddTypeParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddParameterListParameters(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax AddConstraintClauses(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $DelegateDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $DelegateDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-EnumMemberDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax]$EnumMemberDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Member);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken Identifier { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax EqualsValue { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax equalsValue);
  public Microsoft.CodeAnalysis.SyntaxToken get_Identifier();
  public Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax get_EqualsValue();
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken identifier, Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax equalsValue);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax WithIdentifier(Microsoft.CodeAnalysis.SyntaxToken identifier);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax WithEqualsValue(Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax equalsValue);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $EnumMemberDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $EnumMemberDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-GlobalStatementSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax]$GlobalStatement,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.GlobalStatement);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax Statement { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax Update(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax statement);
  public Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax get_Statement();
  public Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax statement);
  public Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax WithStatement(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax statement);
  public Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $GlobalStatement -Element $Element;
        } else {
            # TODO: Add to root
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $GlobalStatement -FileId $FileId -Element $Element;
        }
    }
}

Function Import-NamespaceDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$NamespaceDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Namespace);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken OpenBraceToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken CloseBraceToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxToken namespaceKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name, Microsoft.CodeAnalysis.SyntaxToken openBraceToken, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> externs, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> usings, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members, Microsoft.CodeAnalysis.SyntaxToken closeBraceToken, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.SyntaxToken get_OpenBraceToken();
  public Microsoft.CodeAnalysis.SyntaxToken get_CloseBraceToken();
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken namespaceKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name, Microsoft.CodeAnalysis.SyntaxToken openBraceToken, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> externs, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> usings, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members, Microsoft.CodeAnalysis.SyntaxToken closeBraceToken, Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithNamespaceKeyword(Microsoft.CodeAnalysis.SyntaxToken namespaceKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithOpenBraceToken(Microsoft.CodeAnalysis.SyntaxToken openBraceToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithExterns(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> externs);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithUsings(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> usings);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithCloseBraceToken(Microsoft.CodeAnalysis.SyntaxToken closeBraceToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax AddExterns(Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax AddUsings(Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespaceDeclaration $NamespaceDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespaceDeclaration $NamespaceDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-FileScopedNamespaceDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax]$FileScopedNamespaceDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.FileScopedNamespace);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax
  public Microsoft.CodeAnalysis.SyntaxToken SemicolonToken { get; }
  public Microsoft.CodeAnalysis.SyntaxToken get_SemicolonToken();
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.SyntaxToken namespaceKeyword, Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name, Microsoft.CodeAnalysis.SyntaxToken semicolonToken, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> externs, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> usings, Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithNamespaceKeyword(Microsoft.CodeAnalysis.SyntaxToken namespaceKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithSemicolonToken(Microsoft.CodeAnalysis.SyntaxToken semicolonToken);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithExterns(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> externs);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithUsings(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> usings);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax AddExterns(Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax AddUsings(Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespaceDeclaration $FileScopedNamespaceDeclaration -Element $Element;
        } else {
            # TODO: Add to root
            Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespaceDeclaration $FileScopedNamespaceDeclaration -FileId $FileId -Element $Element;
        }
    }
}

Function Import-BaseNamespaceDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$BaseNamespaceDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        switch ($MemberDeclaration) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-NamespaceDeclarationSyntax -NamespaceDeclaration $BaseNamespaceDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-NamespaceDeclarationSyntax -NamespaceDeclaration $BaseNamespaceDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-NamespaceDeclarationSyntax -NamespaceDeclaration $BaseNamespaceDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-NamespaceDeclarationSyntax -NamespaceDeclaration $BaseNamespaceDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FileScopedNamespaceDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-FileScopedNamespaceDeclarationSyntax -FileScopedNamespaceDeclaration $BaseNamespaceDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-FileScopedNamespaceDeclarationSyntax -FileScopedNamespaceDeclaration $BaseNamespaceDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-FileScopedNamespaceDeclarationSyntax -FileScopedNamespaceDeclaration $BaseNamespaceDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-FileScopedNamespaceDeclarationSyntax -FileScopedNamespaceDeclaration $BaseNamespaceDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            default {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownNamespaceDeclaration);
                if ($PSCmdlet.ParameterSetName -eq 'Root') {
                    # TODO: Add to root
                    Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespaceDeclaration $BaseNamespaceDeclaration -Element $XElement -FileId $FileId -IsUnknown;
                } else {
                    $Parent.Add($XElement);
                    Set-BaseNamespaceDeclarationSyntaxContents -BaseNamespaceDeclaration $BaseNamespaceDeclaration -Element $XElement -IsUnknown;
                }
                break;
            }
        }
    }
}

Function Set-BaseNamespaceDeclarationSyntaxContents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax]$BaseNamespaceDeclaration,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [guid]$FileId,

        [switch]$IsUnknown
    )

    # TODO: Set contents
    <#
public class Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public virtual Microsoft.CodeAnalysis.SyntaxToken NamespaceKeyword { get; }
  public virtual Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax Name { get; }
  public virtual Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> Externs { get; }
  public virtual Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> Usings { get; }
  public virtual Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> Members { get; }
  public Microsoft.CodeAnalysis.SyntaxToken get_NamespaceKeyword();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithNamespaceKeyword(Microsoft.CodeAnalysis.SyntaxToken namespaceKeyword);
  public Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax get_Name();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithName(Microsoft.CodeAnalysis.CSharp.Syntax.NameSyntax name);
  public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> get_Externs();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithExterns(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax> externs);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddExterns(Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax[] items);
  public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> get_Usings();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithUsings(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax> usings);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddUsings(Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax[] items);
  public Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> get_Members();
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithMembers(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax> members);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddMembers(Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  #>
    if ($PSBoundParameters.ContainsKey('FileId')) {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseNamespaceDeclaration -Element $Element -FileId $FileId -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseNamespaceDeclaration -Element $Element -FileId $FileId;
        }
    } else {
        if ($IsUnknown.IsPresent) {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseNamespaceDeclaration -Element $Element -IsUnknown;
        } else {
            Set-MemberDeclarationSyntaxContents -MemberDeclaration $BaseNamespaceDeclaration -Element $Element;
        }
    }
}

Function Import-IncompleteMemberSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax]$IncompleteMember,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [Guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        $Element = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.IncompleteMember);
        # TODO: Set contents
        <#
public sealed class Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax :
    Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax Type { get; }
  public Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax get_Type();
  public Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax Update(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists, Microsoft.CodeAnalysis.SyntaxTokenList modifiers, Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax WithAttributeLists(Microsoft.CodeAnalysis.SyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax> attributeLists);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax WithModifiers(Microsoft.CodeAnalysis.SyntaxTokenList modifiers);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax WithType(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax type);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax AddAttributeLists(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax[] items);
  public Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax AddModifiers(Microsoft.CodeAnalysis.SyntaxToken[] items);
  #>
        if ($PSCmdlet.ParameterSetName -eq 'Child') {
            $Parent.Add($Element);
            Set-MemberDeclarationSyntaxContents MemberDeclaration $IncompleteMember -Element $Element;
        } else {
            # TODO: Add to root
            Set-MemberDeclarationSyntaxContents MemberDeclaration $IncompleteMember -FileId $FileId -Element $Element;
        }
    }
}

Function Import-MemberDeclarationSyntax {
    [CmdletBinding(DefaultParameterSetName = 'Child')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$MemberDeclaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [ValidateScript({ $null -ne $_.Root -and $Root.Name.Equals($Script:ModelDefinitionNames.ModelDefinitions) })]
        [System.Xml.Linq.XDocument]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Root')]
        [guid]$FileId,

        [Parameter(Mandatory = $true, ParameterSetName = 'Child')]
        [System.Xml.Linq.XElement]$Parent,

        [switch]$PassThru
    )

    Process {
        switch ($MemberDeclaration) {
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-BaseFieldDeclarationSyntax -BaseFieldDeclaration $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-BaseFieldDeclarationSyntax -BaseFieldDeclaration $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-BaseFieldDeclarationSyntax -BaseFieldDeclaration $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-BaseFieldDeclarationSyntax -BaseFieldDeclaration $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-BaseMethodDeclarationSyntax -BaseMethodDeclaration $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-BaseMethodDeclarationSyntax -BaseMethodDeclaration $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-BaseMethodDeclarationSyntax -BaseMethodDeclaration $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-BaseMethodDeclarationSyntax -BaseMethodDeclaration $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-BasePropertyDeclarationSyntax -BasePropertyDeclaration $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-BasePropertyDeclarationSyntax -BasePropertyDeclaration $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-BasePropertyDeclarationSyntax -BasePropertyDeclaration $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-BasePropertyDeclarationSyntax -BasePropertyDeclaration $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-DelegateDeclarationSyntax -DelegateDeclaration $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-DelegateDeclarationSyntax -DelegateDeclaration $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-DelegateDeclarationSyntax -DelegateDeclaration $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-DelegateDeclarationSyntax -DelegateDeclaration $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-EnumMemberDeclarationSyntax -EnumMemberDeclaration $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-EnumMemberDeclarationSyntax -EnumMemberDeclaration $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-EnumMemberDeclarationSyntax -EnumMemberDeclaration $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-EnumMemberDeclarationSyntax -EnumMemberDeclaration $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-GlobalStatementSyntax -GlobalStatement $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-GlobalStatementSyntax -GlobalStatement $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-GlobalStatementSyntax -GlobalStatement $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-GlobalStatementSyntax -GlobalStatement $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-BaseNamespaceDeclarationSyntax -BaseNamespaceDeclaration $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-BaseNamespaceDeclarationSyntax -BaseNamespaceDeclaration $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-BaseNamespaceDeclarationSyntax -BaseNamespaceDeclaration $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-BaseNamespaceDeclarationSyntax -BaseNamespaceDeclaration $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-BaseTypeDeclarationSyntax -BaseTypeDeclaration $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-BaseTypeDeclarationSyntax -BaseTypeDeclaration $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-BaseTypeDeclarationSyntax -BaseTypeDeclaration $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-BaseTypeDeclarationSyntax -BaseTypeDeclaration $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax] } {
                if ($PSCmdlet.ParameterSetName -eq 'Child') {
                    if ($PassThru.IsPresent) {
                        Import-IncompleteMemberSyntax -IncompleteMember $MemberDeclaration -Parent $Parent -PassThru;
                    } else {
                        Import-IncompleteMemberSyntax -IncompleteMember $MemberDeclaration -Parent $Parent;
                    }
                } else {
                    if ($PassThru.IsPresent) {
                        Import-IncompleteMemberSyntax -IncompleteMember $MemberDeclaration -Document $Document -FileId $FileId -PassThru;
                    } else {
                        Import-IncompleteMemberSyntax -IncompleteMember $MemberDeclaration -Document $Document -FileId $FileId;
                    }
                }
                break;
            }
            default {
                $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownMemberDeclaration);
                if ($PSCmdlet.ParameterSetName -eq 'Root') {
                    # TODO: Add to root
                    Set-MemberDeclarationSyntaxContents -MemberDeclaration $MemberDeclaration -Element $XElement -FileId $FileId -IsUnknown;
                    $XElement.Add([System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('FileId'), $FileId));
                } else {
                    $Parent.Add($XElement);
                    Set-MemberDeclarationSyntaxContents -MemberDeclaration $MemberDeclaration -Element $XElement -IsUnknown;
                }
                break;
            }
        }
    }
}

Function Set-MemberDeclarationSyntaxContents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$MemberDeclaration,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Element,

        [Guid]$FileId,

        [switch]$IsUnknown
    )

    # TODO: Set contents
    <#
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
    if ($IsUnknown.IsPresent) {
        Set-SyntaxNodeContents -SyntaxNode $MemberDeclaration -Element $Element -IsUnknown;
    } else {
        Set-SyntaxNodeContents -SyntaxNode $MemberDeclaration -Element $Element;
    }
}

Function Import-SyntaxNode {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxNode]$SyntaxNode,

        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XElement]$Parent
    )

    switch ($SyntaxNode) {
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax] } { Import-ArgumentSyntax -Argument $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax] } { Import-ArrayRankSpecifierSyntax -ArrayRankSpecifier $SyntaxNode -Parent $Parent; break; }
    { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax] } { Import-AttributeSyntax -Attribute $SyntaxNode -Parent $Parent; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeTargetSpecifierSyntax] } { Import-AttributeTargetSpecifierSyntax -AttributeTargetSpecifier $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax] } { Import-CompilationUnitSyntax -CompilationUnit $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefParameterSyntax] } { Import-CrefParameterSyntax -CrefParameter $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax] } { Import-AccessorDeclarationSyntax -AccessorDeclaration $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructuredTriviaSyntax] } { Import-StructuredTriviaSyntax -StructuredTrivia $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax] } { Import-SubpatternSyntax -Subpattern $SyntaxNode -Parent $Parent; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax] } { Import-UsingDirectiveSyntax -UsingDirective $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeArgumentListSyntax] } { Import-TypeArgumentListSyntax -TypeArgumentList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax] } { Import-FunctionPointerParameterListSyntax -FunctionPointerParameterList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax] } { Import-FunctionPointerCallingConventionSyntax -FunctionPointerCallingConvention $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionListSyntax] } { Import-FunctionPointerUnmanagedCallingConventionListSyntax -FunctionPointerUnmanagedCallingConventionList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionSyntax] } { Import-FunctionPointerUnmanagedCallingConventionSyntax -FunctionPointerUnmanagedCallingConvention $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax] } { Import-TupleElementSyntax -TupleElement $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionOrPatternSyntax] } { Import-ExpressionOrPatternSyntax -ExpressionOrPattern $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseArgumentListSyntax] } { Import-BaseArgumentListSyntax -BaseArgumentList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseExpressionColonSyntax] } { Import-BaseExpressionColonSyntax -BaseExpressionColon $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax] } { Import-AnonymousObjectMemberDeclaratorSyntax -AnonymousObjectMemberDeclarator $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryClauseSyntax] } { Import-QueryClauseSyntax -QueryClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SelectOrGroupClauseSyntax] } { Import-SelectOrGroupClauseSyntax -SelectOrGroupClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryBodySyntax] } { Import-QueryBodySyntax -QueryBody $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.JoinIntoClauseSyntax] } { Import-JoinIntoClauseSyntax -JoinIntoClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax] } { Import-OrderingSyntax -Ordering $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.QueryContinuationSyntax] } { Import-QueryContinuationSyntax -QueryContinuation $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.WhenClauseSyntax] } { Import-WhenClauseSyntax -WhenClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PositionalPatternClauseSyntax] } { Import-PositionalPatternClauseSyntax -PositionalPatternClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyPatternClauseSyntax] } { Import-PropertyPatternClauseSyntax -PropertyPatternClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax] } { Import-InterpolatedStringContentSyntax -InterpolatedStringContent $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationAlignmentClauseSyntax] } { Import-InterpolationAlignmentClauseSyntax -InterpolationAlignmentClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationFormatClauseSyntax] } { Import-InterpolationFormatClauseSyntax -InterpolationFormatClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax] } { Import-StatementSyntax -Statement $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax] } { Import-VariableDeclarationSyntax -VariableDeclaration $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax] } { Import-VariableDeclaratorSyntax -VariableDeclarator $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax] } { Import-EqualsValueClauseSyntax -EqualsValueClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.VariableDesignationSyntax] } { Import-VariableDesignationSyntax -VariableDesignation $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ElseClauseSyntax] } { Import-ElseClauseSyntax -ElseClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax] } { Import-SwitchSectionSyntax -SwitchSection $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchLabelSyntax] } { Import-SwitchLabelSyntax -SwitchLabel $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionArmSyntax] } { Import-SwitchExpressionArmSyntax -SwitchExpressionArm $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax] } { Import-CatchClauseSyntax -CatchClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchDeclarationSyntax] } { Import-CatchDeclarationSyntax -CatchDeclaration $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CatchFilterClauseSyntax] } { Import-CatchFilterClauseSyntax -CatchFilterClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.FinallyClauseSyntax] } { Import-FinallyClauseSyntax -FinallyClause $SyntaxNode -Parent $Parent; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax] } { Import-ExternAliasDirectiveSyntax -ExternAliasDirective $SyntaxNode -Parent $Parent; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax] } { Import-MemberDeclarationSyntax -MemberDeclaration $SyntaxNode -Parent $Parent; break; }
        { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax] } { Import-AttributeListSyntax -AttributeList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax] } { Import-AttributeArgumentListSyntax -AttributeArgumentList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax] } { Import-AttributeArgumentSyntax -AttributeArgument $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax] } { Import-NameEqualsSyntax -NameEquals $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax] } { Import-TypeParameterListSyntax -TypeParameterList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax] } { Import-TypeParameterSyntax -TypeParameter $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax] } { Import-BaseListSyntax -BaseList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax] } { Import-BaseTypeSyntax -BaseType $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintClauseSyntax] } { Import-TypeParameterConstraintClauseSyntax -TypeParameterConstraintClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterConstraintSyntax] } { Import-TypeParameterConstraintSyntax -TypeParameterConstraint $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ExplicitInterfaceSpecifierSyntax] } { Import-ExplicitInterfaceSpecifierSyntax -ExplicitInterfaceSpecifier $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax] } { Import-ConstructorInitializerSyntax -ConstructorInitializer $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax] } { Import-ArrowExpressionClauseSyntax -ArrowExpressionClause $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.AccessorListSyntax] } { Import-AccessorListSyntax -AccessorList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax] } { Import-BaseParameterListSyntax -BaseParameterList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax] } { Import-BaseParameterSyntax -BaseParameter $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.CrefSyntax] } { Import-CrefSyntax -Cref $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseCrefParameterListSyntax] } { Import-BaseCrefParameterListSyntax -BaseCrefParameterList $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNodeSyntax] } { Import-XmlNodeSyntax -XmlNode $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementStartTagSyntax] } { Import-XmlElementStartTagSyntax -XmlElementStartTag $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlElementEndTagSyntax] } { Import-XmlElementEndTagSyntax -XmlElementEndTag $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlNameSyntax] } { Import-XmlNameSyntax -XmlName $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlPrefixSyntax] } { Import-XmlPrefixSyntax -XmlPrefix $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.XmlAttributeSyntax] } { Import-XmlAttributeSyntax -XmlAttribute $SyntaxNode -Parent $Parent; break; }
        # { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.LineDirectivePositionSyntax] } { Import-LineDirectivePositionSyntax -LineDirectivePosition $SyntaxNode -Parent $Parent; break; }
        default {
            $XElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.UnknownSyntax);
            $Parent.Add($XElement);
            Set-SyntaxNodeContents -SyntaxNode $SyntaxNode -Element $XElement -IsUnknown;
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
            $Nodes | Import-SyntaxNode -Parent $XElement;
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
            $Nodes | Import-SyntaxNode -Parent $XElement;
        }
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

    Process {
        if ($Content.Count -gt 0 -and $Content | Test-IsNotEmptyString) {
            [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree = $null;
            try { $SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText(($Content -join ([System.Environment]::NewLine)),
                [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $Path); }
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
                        $Id = [System.Xml.XmlConvert]::ToGuid($SourceElement.Attribute([System.Xml.Linq.XNamespace]::None.GetName('Id')).Value);
                        $XName = [System.Xml.Linq.XNamespace]::None.GetName('FileId');
                        @($Document.Descendants() | Where-Object {
                            $a = $_.Attribute($XName);
                            return $null -ne $a -and [System.Xml.XmlConvert]::ToGuid($a.Value) -eq $Id;
                        }) | ForEach-Object { $_.Remove() }
                        $SourceElement.Remove();
                    }
                    $Id = [Guid]::NewGuid();
                    $SourceElement = [System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Source,
                        [System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Path'), $Path),
                        [System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('Id'), $Id)
                    );
                    $XElement = $Document.Root.Element($Script:ModelDefinitionNames.Sources);
                    if ($null -eq $XElement) {
                        if ($null -eq $XElement) {
                            $Document.Root.Add([System.Xml.Linq.XElement]::new($Script:ModelDefinitionNames.Sources, $SourceElement));
                        } else {
                            $XElement.Add($SourceElement);
                        }
                    }
                    [Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax]$Syntax = $SyntaxTree.GetCompilationUnitRoot();
                    $Syntax.Externs | Import-ExternAliasDirectiveSyntax -Parent $SourceElement;
                    $Syntax.Usings | Import-UsingDirectiveSyntax -Parent $SourceElement;
                    $Syntax.AttributeLists | Import-AttributeListSyntax -Parent $SourceElement;
                    $Syntax.Members | Import-MemberDeclarationSyntax -Document $Document -FileId $Id;
                }
            }
        }
    }
}
