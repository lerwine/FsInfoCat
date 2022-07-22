#region Xml Functions

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

#endregion

#region XLinq Functions

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

        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsString')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDateTime')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsBoolean')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDecimal')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsSByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsUInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsSingle')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsDouble')]
        [Parameter(Mandatory = $true, ParameterSetName = 'AttributeEqualsTimeSpan')]
        [System.Xml.Linq.XName]$AttributeName,

        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsString')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDateTime')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsBoolean')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDecimal')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsSByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsByte')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt16')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt32')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsUInt64')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsSingle')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsDouble')]
        [Parameter(Mandatory = $true, ParameterSetName = 'ChildElementEqualsTimeSpan')]
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
                        $v = [DevUtil.XLinqHelper]::ToDateTime($a.Value, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDateTime($a.Value, $DateTimeSerializationMode);
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
                        $v = [DevUtil.XLinqHelper]::ToDateTime($t, $DateTimeSerializationMode);
                        return $null -ne $v -and $v -eq $EqualsDateTime;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDateTime($t, $DateTimeSerializationMode);
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
                        $v = [DevUtil.XLinqHelper]::ToGuid($a.Value);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToGuid($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToGuid($t);
                        return $null -ne $v -and $v -eq $EqualsGuid;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToGuid($t);
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
                        $v = [DevUtil.XLinqHelper]::ToBoolean($a.Value);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToBoolean($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToBoolean($t);
                        return $null -ne $v -and $v -eq $EqualsBoolean;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToBoolean($t);
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
                        $v = [DevUtil.XLinqHelper]::ToDecimal($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDecimal($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToDecimal($t);
                        return $null -ne $v -and $v -eq $EqualsDecimal;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDecimal($t);
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
                        $v = [DevUtil.XLinqHelper]::ToSByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSByte($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToSByte($t);
                        return $null -ne $v -and $v -eq $EqualsSByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSByte($t);
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
                        $v = [DevUtil.XLinqHelper]::ToInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt16($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToInt16($t);
                        return $null -ne $v -and $v -eq $EqualsInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt16($t);
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
                        $v = [DevUtil.XLinqHelper]::ToInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt32($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToInt32($t);
                        return $null -ne $v -and $v -eq $EqualsInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt32($t);
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
                        $v = [DevUtil.XLinqHelper]::ToInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt64($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToInt64($t);
                        return $null -ne $v -and $v -eq $EqualsInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToInt64($t);
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
                        $v = [DevUtil.XLinqHelper]::ToByte($a.Value);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToByte($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToByte($t);
                        return $null -ne $v -and $v -eq $EqualsByte;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToByte($t);
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
                        $v = [DevUtil.XLinqHelper]::ToUInt16($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt16($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToUInt16($t);
                        return $null -ne $v -and $v -eq $EqualsUInt16;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt16($t);
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
                        $v = [DevUtil.XLinqHelper]::ToUInt32($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt32($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToUInt32($t);
                        return $null -ne $v -and $v -eq $EqualsUInt32;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt32($t);
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
                        $v = [DevUtil.XLinqHelper]::ToUInt64($a.Value);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt64($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToUInt64($t);
                        return $null -ne $v -and $v -eq $EqualsUInt64;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToUInt64($t);
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
                        $v = [DevUtil.XLinqHelper]::ToSingle($a.Value);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSingle($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToSingle($t);
                        return $null -ne $v -and $v -eq $EqualsSingle;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToSingle($t);
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
                        $v = [DevUtil.XLinqHelper]::ToDouble($a.Value);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDouble($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToDouble($t);
                        return $null -ne $v -and $v -eq $EqualsDouble;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToDouble($t);
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
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($a.Value);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $a = $_.Attribute($AttributeName);
                        if ($null -eq $a) { return $false }
                        if ($a.Value.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($a.Value);
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
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($t);
                        return $null -ne $v -and $v -eq $EqualsTimeSpan;
                    } | Write-Output;
                } else {
                    $Parent.Elements() | Where-Object {
                        $e = $_.Element($ChildElementName);
                        if ($null -eq $e -or $e.IsEmpty) { return $false }
                        $t = $e.Value.Trim();
                        if ($t.Length -eq 0) { return $false; }
                        $v = [DevUtil.XLinqHelper]::ToTimeSpan($t);
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


#endregion

#region XSD Functions

Function New-XsdRestrictedSimpleType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Base
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('restriction'), [DevUtil.XLinqHelper]::NewAttribute('base', $Base)) }

    Process { $XElement.Add($Content) }

    End { [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('restriction'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name), $XElement) | Write-Output }
}

Function New-XsdAttributeGroup {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attributeGroup'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name)) }

    Process { $XElement.Add($Content) }

    End { $XElement | Write-Output }
}

Function New-XsdGroup {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$Content,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name
    )

    Begin { $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name)) }

    Process { $XElement.Add($Content) }

    End { $XElement | Write-Output }
}

Function New-XsdAttribute {
    [CmdletBinding(DefaultParameterSetName = 'Optional')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Ref')]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Ref,

        [Parameter(Mandatory = $true, ParameterSetName = 'Optional')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Type,

        [Parameter(ParameterSetName = 'Optional')]
        [string]$DefaultValue,

        [Parameter(ParameterSetName = 'Optional')]
        [switch]$Optional,

        [Parameter(Mandatory = $true, ParameterSetName = 'Required')]
        [switch]$Required
    )

    if ($Required.IsPresent) {
        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attribute'),
            [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
            [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
            [DevUtil.XLinqHelper]::NewAttribute('use', 'required')
        ) | Write-Output;
    } else {
        if ($PSCmdlet.ParameterSetName -eq 'Ref') {
            [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attributeGroup'), [DevUtil.XLinqHelper]::NewAttribute('ref', $Ref)) | Write-Output;
        } else {
            if ($PSBoundParameters.ContainsKey('DefaultValue')) {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attribute'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('use', 'optional'),
                    [DevUtil.XLinqHelper]::NewAttribute('default', $DefaultValue)
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('attribute'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('use', 'optional')
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
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Extends,

        [switch]$Abstract,

        [switch]$Mixed
    )

    Begin {
        [System.Xml.Linq.XElement]$ParentElement = $null;
        [System.Xml.Linq.XElement]$ComplexTypeElement = $null;
        if ($PSBoundParameters.ContainsKey('Extends')) {
            $ParentElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('extension'), [DevUtil.XLinqHelper]::NewAttribute('base', $Extends));
            if ($Abstract.IsPresent) {
                if ($Mixed.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true'),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'),
                            [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true'),
                            $ParentElement
                        )
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true'),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'), $ParentElement)
                    );
                }
            } else {
                if ($Mixed.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'),
                            [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true'),
                            $ParentElement
                        )
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexContent'), $ParentElement)
                    );
                }
            }
        } else {
            if ($Mixed.IsPresent) {
                if ($Abstract.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true'),
                        [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true')
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('mixed', 'true')
                    );
                }
            } else {
                if ($Abstract.IsPresent) {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('abstract', 'true')
                    );
                } else {
                    $ComplexTypeElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('complexType'), [DevUtil.XLinqHelper]::NewAttribute('name', $Name));
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
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'), [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('sequence'));
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
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'), [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('choice'));
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
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                );
            } else {
                $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded'));
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    );
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'), [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs));
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'), [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs));
                } else {
                    $XElement = [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('all'));
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
        [ValidateScript({ $_ | Test-XmlName -NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Group')]
        [ValidateScript({ $_ | Test-XmlName })]
        [string]$Group,

        [Parameter(Mandatory = $true, ParameterSetName = 'MaxExplicit')]
        [Parameter(Mandatory = $true, ParameterSetName = 'MaxUnbounded')]
        [ValidateScript({ $_ | Test-XmlName })]
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
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                    [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                    [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                        [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                        [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'),
                        [DevUtil.XLinqHelper]::NewAttribute('ref', $Group),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('group'), [DevUtil.XLinqHelper]::NewAttribute('ref', $Group)) | Write-Output;
                }
            }
        }
    } else {
        if ($MaxUnbounded.IsPresent) {
            if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            } else {
                [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                    [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                    [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                    [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', 'unbounded')
                ) | Write-Output;
            }
        } else {
            if ($PSBoundParameters.ContainsKey('MaxOccurs')) {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                        [DevUtil.XLinqHelper]::NewAttribute('maxOccurs', $MaxOccurs)
                    ) | Write-Output;
                }
            } else {
                if ($PSBoundParameters.ContainsKey('MinOccurs')) {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type),
                        [DevUtil.XLinqHelper]::NewAttribute('minOccurs', $MinOccurs)
                    ) | Write-Output;
                } else {
                    [System.Xml.Linq.XElement]::new([DevUtil.XLinqHelper]::GetXsdName('element'),
                        [DevUtil.XLinqHelper]::NewAttribute('name', $Name),
                        [DevUtil.XLinqHelper]::NewAttribute('type', $Type)
                    ) | Write-Output;
                }
            }
        }
    }
}

#endregion

#region Type Functions

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

#endregion

#region TypeSyntax Conversion Functions

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
            if ($Method.IsStatic) { $Code = "static $Code" }
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

#endregion

#region Miscellaneous Functions

Function Get-ErrorDetails {
    [CmdletBinding(DefaultParameterSetName = 'Any')]
    [OutputType([System.Xml.Linq.XDocument])]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ParameterSetName = 'Any')]
        [object]$ErrorObject,

        [Parameter(Mandatory = $true, ParameterSetName = 'ErrorRecord')]
        [System.Management.Automation.ErrorRecord]$ErrorRecord,

        [Parameter(Mandatory = $true, ParameterSetName = 'Exception')]
        [System.Exception]$Exception,

        [Parameter(ParameterSetName = 'Exception')]
        [switch]$NoRecurse
    )

    switch ($PSCmdlet.ParameterSetName) {
        'ErrorRecord' {
            $ErrorRecord.InvocationInfo.PositionMessage;
            if ($null -ne $ErrorRecord.ErrorDetails) {
                if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.ErrorDetails.Message)) {
                    "  Message: $($ErrorRecord.ErrorDetails.Message)" | Write-Output;
                }
                if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.ErrorDetails.RecommendedAction)) {
                    "  RecommendedAction: $($ErrorRecord.ErrorDetails.RecommendedAction)" | Write-Output;
                }
            }
            "  Category: $($ErrorRecord.CategoryInfo.Category)" | Write-Output;
            "    [Message]: $($ErrorRecord.CategoryInfo.GetMessage())" | Write-Output;
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.Activity)) {
                "    Activity: $($ErrorRecord.CategoryInfo.Activity)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.Reason)) {
                "    Reason: $($ErrorRecord.CategoryInfo.Reason)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.TargetName)) {
                "    TargetName: $($ErrorRecord.CategoryInfo.TargetName)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.CategoryInfo.TargetType)) {
                "    TargetType: $($ErrorRecord.CategoryInfo.TargetType)" | Write-Output;
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorRecord.FullyQualifiedErrorId)) {
                "    ErrorId: $($ErrorRecord.FullyQualifiedErrorId)" | Write-Output;
            }

            $InnerErrorRecord = $ErrorRecord.Exception.ErrorRecord;
            if ($null -eq $InnerErrorRecord) {
                (Get-ErrorDetails -Exception $ErrorRecord.Exception) | ForEach-Object { "  $_" }
            } else {
                if ($null -eq $ErrorRecord.Exception.InnerException -or -not [object]::ReferenceEquals($InnerErrorRecord.Exception, $ErrorRecord.Exception.InnerException)) {
                    (Get-ErrorDetails -Exception $ErrorRecord.Exception) | ForEach-Object { "  $_" }
                } else {
                    (Get-ErrorDetails -Exception $ErrorRecord.Exception -NoRecurse) | ForEach-Object { "  $_" }
                }
                (Get-ErrorDetails -ErrorRecord $InnerErrorRecord) | ForEach-Object { "  $_" }
            }
            break;
        }
        'Exception' {
            $Exception.GetType().FullName | Write-Output;
            if (-not [string]::IsNullOrWhiteSpace($Exception.Message)) {
                "  Message: $($Exception.Message)" | Write-Output;
            }
            if (-not $NoRecurse.IsPresent) {
                if ($Exception -is [System.AggregateException]) {
                    $Exception.InnerExceptions | ForEach-Object {
                        "  InnerException:" | Write-Output;
                        (Get-ErrorDetails -Exception $_) | ForEach-Object { "    $_" }
                    }
                } else {
                    if ($null -ne $Exception.InnerException) {
                        "  InnerException:" | Write-Output;
                        (Get-ErrorDetails -Exception $Exception.InnerException) | ForEach-Object { "    $_" }
                    }
                }
            }
            foreach ($P in @($Exception | Get-Member -MemberType Properties)) {
                switch ($P.Name) {
                    'Message' { break; }
                    'InnerExceptions' { break; }
                    'InnerException' { break; }
                    'StackTrace' { break; }
                    'Data' { break; }
                    default {
                        $Text = ('' + $Exception.$($P.Name)).Trim();
                        if ($Text.Length -gt 0) {
                            $Lines = @(('' + $Exception.$($P.Name)) -split '\r\n?|\n');
                            "  $($P.Name): $($Lines[0])" | Write-Output;
                            ($Lines | Select-Object -Skip 1) | ForEach-Object { "  $_" } | Write-Output;
                        }
                        break;
                    }
                }
            }
            break;
        }
        default {
            if ($ErrorObject -is [System.Management.Automation.ErrorRecord]) {
                Get-ErrorDetails -ErrorRecord $ErrorObject;
            } else {
                if ($ErrorObject -is [System.Exception]) {
                    Get-ErrorDetails -Exception $Exception;
                } else {
                    $ErrorObject.GetType().FullName | Write-Output;
                    "  $ErrorObject" | Write-Output;
                }
            }
            break;
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

    Begin { $IsNotEmptyString = $false }
    Process {
        if (-not ($IsNotEmptyString -or [string]::IsNullOrWhiteSpace($Value))) {
            $IsNotEmptyString = $true;
        }
    }

    End { $IsNotEmptyString | Write-Output }
}

#endregion

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

Set-Variable -Name 'ModelDefinitionsNS' -Option Constant -Scope 'Script' -Value ([System.Xml.Linq.XNamespace]::Get('http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xs'));

Function New-ModelDefinitionDocument {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XDocument])]
    Param()
    Write-Output -InputObject ([System.Xml.Linq.XDocument]::new([DevUtil.XLinqHelper]::NewMdElement('ModelDefinitions'))) -NoEnumerate;
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
        $PathXName = [DevUtil.XLinqHelper]::GetName('Path');
        $SourceElements = @(
            $Document.Root.Elements([System.Xml.Linq.XName]::Get([DevUtil.XLinqHelper]::GetMdName('Sources'))) | ForEach-Object {
                $_.Elements([DevUtil.XLinqHelper]::GetMdName('Source')) | Write-Output;
            }
        ) | Group-Object -Property @{ Expression = { $_.Attribute($PathXName).Value } } -AsHashTable;
    }
    Process {
        if ($SourceElements.Count -gt 0) {
            $Path | ForEach-Object {
                $e = $SourceElements[$_];
                if ($null -ne $e) { Write-Output -InputObject $e -NoEnumerate }
            }
        }
    }
}

Function Convert-TypeReferenceToString {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.Linq.XElement]$XElement
    )

    Process {
        switch ($XElement.Name.LocalName) {
            'Predefined' {
                $XElement.Attribute('Type').Value | Write-Output;
                break;
            }
            'Nullable' {
                $XElement.Elements() | Convert-TypeReferenceToString | ForEach-Object { "$_?" }
                break;
            }
            'Array' {
                $a = $XElement.Attribute('Rank');
                $rank = 1;
                if ($null -ne $a -and [int]::TryParse($a.Value, [ref]$rank) -and $rank -gt 1) {
                    $XElement.Elements() | Convert-TypeReferenceToString | ForEach-Object { "$_[$([string]::new(',', $rank - 1))]" }
                } else {
                    $XElement.Elements() | Convert-TypeReferenceToString | ForEach-Object { "$_[]" }
                }
                break;
            }
            'Argument' {
                $XElement.Attribute('Name').Value | Write-Output;
                break;
            }
            default {
                $XElement.Attribute('Ref').Value | Write-Output;
                break;
            }
        }
    }
}

Function Get-MemberReferenceDictionary {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$XDocument
    )

    $Dictionary = [System.Collections.Generic.Dictionary[string,System.Xml.Linq.XElement]]::new();
    $Namespace = $XDocument.Root.Name.Namespace;
    $RootSelector = [DevUtil.XElementSelector]::new($XDocument.Root);
    $AttributesSelector = $RootSelector.Elements($Namespace.GetName('ByteEnum')).Attributes('ID');
    ('SByteEnum', 'ShortEnum', 'UShortEnum', 'IntEnum', 'UIntEnum', 'LongEnum', 'ULongEnum') | ForEach-Object {
        $AttributesSelector = $AttributesSelector.Concat($RootSelector.Elements($Namespace.GetName($_)).Attributes('ID').GetItems());
    }
    foreach ($XAttribute in $AttributesSelector.GetItems()) {
        if ($Dictionary.ContainsKey($XAttribute.Value)) {
            $E = $Dictionary[$XAttribute.Value];
            Write-Warning -Message "Enum ID `"$($XAttribute.Value)`" at line $($XAttribute.LineNumber), column $($XAttribute.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
        } else {
            $Dictionary.Add($XAttribute.Value, $XAttribute.Parent);
            foreach ($a in [DevUtil.XElementSelector]::new($XAttribute.Parent).Elements($Namespace.GetName('Field')).Attributes('Name').GetItems()) {
                $Key = "$($XAttribute.Value).$($a.Value)";
                if ($Dictionary.ContainsKey($Key)) {
                    $E = $Dictionary[$Key];
                    Write-Warning -Message "Enum Field ID `"$Key`" at line $($a.LineNumber), column $($a.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
                } else {
                    $Dictionary.Add($Key, $a.Parent);
                }
            }
        }
    }
    foreach ($XAttribute in $RootSelector.Elements($Namespace.GetName('Model')).Attributes('ID').GetItems()) {
        if ($Dictionary.ContainsKey($XAttribute.Value)) {
            $E = $Dictionary[$XAttribute.Value];
            Write-Warning -Message "Model ID `"$($XAttribute.Value)`" at line $($XAttribute.LineNumber), column $($XAttribute.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
        } else {
            $Dictionary.Add($XAttribute.Value, $XAttribute.Parent);
            foreach ($a in [DevUtil.XElementSelector]::new($XAttribute.Parent).Elements($Namespace.GetName('Property')).Attributes('Name').GetItems()) {
                $Key = "$($XAttribute.Value).$($a.Value)";
                if ($Dictionary.ContainsKey($Key)) {
                    $E = $Dictionary[$Key];
                    Write-Warning -Message "Property ID `"$Key`" at line $($a.LineNumber), column $($a.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
                } else {
                    $Dictionary.Add($Key, $a.Parent);
                }
            }
            foreach ($a in [DevUtil.XElementSelector]::new($XAttribute.Parent).Elements($Namespace.GetName('Method')).Attributes('Name').GetItems()) {
                $Key = "$($XAttribute.Value).$($a.Value)";
                $ArgNames = @([DevUtil.XElementSelector]::new($XAttribute.Parent).Elements($Namespace.GetName('TypeParameter')).Attributes('Name').GetItems() | ForEach-Object {
                    $_.Value;
                });
                if ($ArgNames.Count -gt 0) { $Key = "$Key{$($GenericArgNames -join ',')}" }
                $ArgNames = @([DevUtil.XElementSelector]::new($XAttribute.Parent).Elements($Namespace.GetName('TypeParameter')).Attributes('Name').GetItems() | ForEach-Object {
                    $_.Value;
                });
                $Key = "$Key($(([DevUtil.XElementSelector]::new($XAttribute.Parent).Elements($Namespace.GetName('Parameter')).Elements().GetItems() | Convert-TypeReferenceToString) -join ','))";
                if ($Dictionary.ContainsKey($Key)) {
                    $E = $Dictionary[$Key];
                    Write-Warning -Message "Method ID `"$Key`" at line $($a.Parent.LineNumber), column $($a.Parent.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
                } else {
                    $Dictionary.Add($Key, $a.Parent);
                }
            }
        }
    }
    foreach ($XAttribute in $RootSelector.Elements($Namespace.GetName('StaticClass')).Attributes('ID').GetItems()) {
        if ($Dictionary.ContainsKey($XAttribute.Value)) {
            $E = $Dictionary[$XAttribute.Value];
            Write-Warning -Message "Class ID `"$($XAttribute.Value)`" at line $($XAttribute.LineNumber), column $($XAttribute.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
        } else {
            $Dictionary.Add($XAttribute.Value, $XAttribute.Parent);
        }
    }
    $ClrSelector = $RootSelector.Elements($Namespace.GetName('CLR'));
    $AttributesSelector = $ClrSelector.Elements($Namespace.GetName('ByteEnum')).Attributes('ID');
    ('SByteEnum', 'ShortEnum', 'UShortEnum', 'IntEnum', 'UIntEnum', 'LongEnum', 'ULongEnum') | ForEach-Object {
        $AttributesSelector = $AttributesSelector.Concat($ClrSelector.Elements($Namespace.GetName($_)).Attributes('ID').GetItems());
    }
    foreach ($XAttribute in $AttributesSelector.GetItems()) {
        if ($Dictionary.ContainsKey($XAttribute.Value)) {
            $E = $Dictionary[$XAttribute.Value];
            Write-Warning -Message "Enum ID `"$($XAttribute.Value)`" at line $($XAttribute.LineNumber), column $($XAttribute.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
        } else {
            $Dictionary.Add($XAttribute.Value, $XAttribute.Parent);
        }
    }
    $AttributesSelector = $ClrSelector.Elements($Namespace.GetName('Interface')).Attributes('ID');
    $AttributesSelector = $AttributesSelector.Concat($ClrSelector.Elements($Namespace.GetName('Struct')).Attributes('ID').GetItems());
    $AttributesSelector = $AttributesSelector.Concat($ClrSelector.Elements($Namespace.GetName('Class')).Attributes('ID').GetItems());
    foreach ($XAttribute in $AttributesSelector.GetItems()) {
        if ($Dictionary.ContainsKey($XAttribute.Value)) {
            $E = $Dictionary[$XAttribute.Value];
            Write-Warning -Message "Type ID at line $($XAttribute.LineNumber), column $($XAttribute.LinePosition) is a duplicate of the identifier for the definition at $($E.LineNumber), column $($E.LinePosition)";
        } else {
            $Dictionary.Add($XAttribute.Value, $XAttribute.Parent);
        }
    }
    Write-Output -InputObject $Dictionary -NoEnumerate;
}
